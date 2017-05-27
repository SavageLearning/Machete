using AutoMapper;
using Machete.Data;
using Machete.Data.Helpers;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service.DTO.Reports;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Machete.Service.DTO;

namespace Machete.Service
{
    public interface IReportsV2Service : IService<ReportDefinition>
    {
        List<dynamic> getQuery(DTO.SearchOptions o);
        List<ReportDefinition> getList();
        ReportDefinition Get(string idOrName);
        List<QueryMetadata> getColumns(string tableName);
        DataTable getDataTable(string query, DTO.SearchOptions o);
        void getXlsxFile(DTO.SearchOptions o, ref byte[] bytes);
    }

    public class ReportsV2Service : ServiceBase<ReportDefinition>, IReportsV2Service
    {
        private readonly IReportsRepository repo;
        private readonly IMapper map;
        public ReportsV2Service(IReportsRepository repo, IUnitOfWork unitOfWork, IMapper map) : base(repo, unitOfWork)
        {
            this.repo = repo;
            this.map = map;
        }

        public List<dynamic> getQuery(DTO.SearchOptions o)
        {
            // if name, get id for report definition
            int id = 0;
            if (!Int32.TryParse(o.idOrName, out id))
            {
                id = repo.GetMany(r => string.Equals(r.name, o.idOrName, StringComparison.OrdinalIgnoreCase)).First().ID;
            }

            var oo = map.Map<DTO.SearchOptions, Data.DTO.SearchOptions>(o);
            return repo.getDynamicQuery(id, oo);
        }

        public List<ReportDefinition> getList()
        {
            return repo.getList();
        }

        public ReportDefinition Get(string idOrName)
        {
            int id = 0;
            Domain.ReportDefinition result;
            // accept vanityname or ID
            if (Int32.TryParse(idOrName, out id))
            {
                result = repo.GetById(id);
            }
            else
            {
                result = repo.GetMany(r => string.Equals(r.name, idOrName, StringComparison.OrdinalIgnoreCase)).First();
            }
            return result;
        }

        public List<QueryMetadata> getColumns(string tableName)
        {
            var result = repo.getColumns(tableName);
            result.ForEach(c => c.include = true);
            return result;
        }

        public DataTable getDataTable(string query, DTO.SearchOptions o)
        {
            var oo = map.Map<DTO.SearchOptions, Data.DTO.SearchOptions>(o);
            return repo.getDataTable(query, oo);
        }


        public void getXlsxFile(DTO.SearchOptions o, ref byte[] bytes)
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
                        .Append(" > ")
                        .Append(((DateTime)o.beginDate).ToShortDateString());
                    firstWhere = false;
                }
                if (o.endDate != null)
                {
                    if (!firstWhere) { query.Append(" AND "); }
                    query.Append(o.exportFilterField)
                        .Append(" < ")
                        .Append(((DateTime)o.endDate).ToShortDateString());
                }
            }
            var oo = map.Map<DTO.SearchOptions, Data.DTO.SearchOptions>(o);
            var tbl = repo.getDataTable(query.ToString(), oo);


            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(o.name);
                ws.Cells["A1"].LoadFromDataTable(tbl, true);
                bytes = pck.GetAsByteArray();
            }
        }

        public string sanitizeColumnName(string col)
        {
            return "[" + col + "]";
        }


    }
}
