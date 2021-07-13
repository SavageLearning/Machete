using AutoMapper;
using Machete.Service.Infrastructure;
using Machete.Domain;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Machete.Service.DTO;
using Machete.Service.Dynamic;
using Machete.Service.Tenancy;

namespace Machete.Service
{
    public interface IReportsV2Service : IService<ReportDefinition>
    {
        List<dynamic> GetQuery(SearchOptions o);
        List<ReportDefinition> GetList();
        ReportDefinition Get(string idOrName);
        List<QueryMetadata> GetColumns(string tableName);
        void GetXlsxFile(DTO.SearchOptions o, ref byte[] bytes);
        List<string> ValidateQuery(string query);
        
        List<dynamic> GetDynamicQuery(int id, SearchOptions o);
        DataTable GetDataTable(string query);
        List<string> Validate(string query);
        bool Exists(string id);
    }

    public class ReportsV2Service : ServiceBase2<ReportDefinition>, IReportsV2Service
    {
        private readonly string _readonlyConnectionString;
        public ReportsV2Service(IDatabaseFactory db, ITenantService tenantService, IMapper map) : base(db, map)
        {
            var currentTenant = tenantService.GetCurrentTenant();
            _readonlyConnectionString = currentTenant.ReadOnlyConnectionString;
            
            if (String.IsNullOrEmpty(_readonlyConnectionString)) throw new ArgumentNullException(
                $"ReportsRepository requires valid currentTenant.ReadOnlyConnectionString; was: {currentTenant.ReadOnlyConnectionString ?? "null"}"
            );
        }

        public List<dynamic> GetQuery(DTO.SearchOptions o)
        {
            // if name, get id for report definition
            if (!Int32.TryParse(o.idOrName, out var id))
            {
                id = GetMany(r => string.Equals(r.name, o.idOrName, StringComparison.OrdinalIgnoreCase)).First().ID;
            }

            var oo = map.Map<DTO.SearchOptions, SearchOptions>(o);
            return GetDynamicQuery(id, oo);
        }

        public List<ReportDefinition> GetList()
        {
            return db.ReportDefinitions.AsEnumerable().ToList();
        }

        public ReportDefinition Get(string idOrName)
        {
            int id = 0;
            ReportDefinition result;
            // accept vanityname or ID
            if (Int32.TryParse(idOrName, out id))
            {
                result = dbset.Find(id);
            }
            else
            {
                result = GetMany(r => string.Equals(r.name, idOrName, StringComparison.OrdinalIgnoreCase)).First();
            }
            return result;
        }

        public bool Exists(string id) => dbset.Count(r => r.name == id) >= 1;

        public List<QueryMetadata> GetColumns(string tableName)
        {
            var result = MacheteAdoContext.getMetadata($"select top 0 * from {tableName}", _readonlyConnectionString);
            result.ForEach(c => c.include = true);
            return result;
        }

        public void GetXlsxFile(DTO.SearchOptions o, ref byte[] bytes)
        {
            var oo = map.Map<DTO.SearchOptions, SearchOptions>(o);
            var exportQuery = BuildExportQuery(o);
            var tbl = GetDataTable(exportQuery);

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(o.name);
                ws.Cells["A1"].LoadFromDataTable(tbl, true);
                bytes = pck.GetAsByteArray();
            }
        }

        private string BuildExportQuery(DTO.SearchOptions o)
        {
            bool firstSelect = true;
            bool firstWhere = true;
            StringBuilder query = new StringBuilder();
            //
            query.Append("SELECT ");
            foreach (var d in o.exportIncludeOptions)
            {
                if (d.Value == "false") continue;
                if (!firstSelect) query.Append(", ");
                query.Append(sanitizeColumnName(d.Key));
                firstSelect = false;
            }

            if (query.Length == 7) // just select
                query.Append("*");
            
            //
            query.Append(" FROM ").Append(o.name);
            //
            if (o.exportFilterField != null
                &&
                    (o.beginDate != null ||
                     o.endDate != null
                    ))
            {
                query.Append(" WHERE ");
                if (o.beginDate != null)
                {
                    query.Append(o.exportFilterField)
                        .Append(" > '")
                        .Append(((DateTime)o.beginDate).ToShortDateString())
                        .Append("' ");
                    firstWhere = false;
                }
                if (o.endDate != null)
                {
                    if (!firstWhere) { query.Append(" AND "); }
                    query.Append(o.exportFilterField)
                        .Append(" < '")
                        .Append(((DateTime)o.endDate).ToShortDateString())
                        .Append("' ");
                }
            }
            return query.ToString();
        }

        public string sanitizeColumnName(string col)
        {
            return "[" + col + "]";
        }

        public List<string> ValidateQuery(string query)
        {
            return Validate(query).ToList();
        }
        
        public List<dynamic> GetDynamicQuery(int id, SearchOptions o)
        {
            ReportDefinition report = dbset.Single(a => a.ID == id);
            List<QueryMetadata> meta = MacheteAdoContext.getMetadata(report.sqlquery, _readonlyConnectionString);
            Type queryType = ILVoodoo.buildQueryType(meta);
            MethodInfo method = Type.GetType("Machete.Service.MacheteAdoContext")
                .GetMethod("SqlQuery", new[] { typeof(string), typeof(string), typeof(SqlParameter[]) });
            MethodInfo man = method.MakeGenericMethod(queryType);
            var blarg = new List<SqlParameter>()
            {
                new SqlParameter {ParameterName = "beginDate", Value = o.beginDate},
                new SqlParameter {ParameterName = "endDate", Value = o.endDate}
            };
            if (o.dwccardnum != null)
            {
                blarg.Add(new SqlParameter {ParameterName = "dwccardnum", Value = o.dwccardnum});
            }
            dynamic dynamicQuery = man.Invoke(null, new object[] {
                report.sqlquery,
                _readonlyConnectionString,
                blarg.ToArray<SqlParameter>()
            });

            var dynamicList = new List<dynamic>();
            foreach (var row in dynamicQuery) {
                dynamicList.Add(row);
            }

            return dynamicList;
        }
        public DataTable GetDataTable(string query)
        {
            // https://stackoverflow.com/documentation/epplus/8223/filling-the-document-with-data
            return MacheteAdoContext.Fill(query, _readonlyConnectionString);
        }

        public List<string> Validate(string query)
        {

            string vars = @"DECLARE @beginDate Date
SET @beginDate = '2021-01-01'
DECLARE @endDate Date
SET @endDate = '2022-01-01'
DECLARE @dwccardnum int
SET @dwccardnum = 10000
";
            string fullQuery = string.Concat(vars, query);
            var result = MacheteAdoContext.ValidateQuery(fullQuery, _readonlyConnectionString).ToList();
            return result;
        }
    }
}
