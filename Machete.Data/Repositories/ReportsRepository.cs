using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Machete.Data.DTO;
using Machete.Data.Dynamic;
using Machete.Data.Infrastructure;
using Machete.Data.Tenancy;
using Machete.Domain;

namespace Machete.Data.Repositories
{   
    public interface IReportsRepository : IRepository<ReportDefinition>
    {
        List<dynamic> getDynamicQuery(int id, SearchOptions o);
        List<ReportDefinition> getList();
        List<QueryMetadata> getColumns(string tableName);
        DataTable getDataTable(string query);
        List<string> validate(string query);
    }

    public class ReportsRepository : RepositoryBase<ReportDefinition>, IReportsRepository
    {
        private readonly string _readonlyConnectionString;

        /// <summary>
        /// A repository wrapping DbSet{ReportDefinition} and providing for direct queries against the database using
        /// the MacheteAdoContext with a readonly connection string. Secure code reviews required for any changes to
        /// this class.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="tenantService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReportsRepository(IDatabaseFactory factory, ITenantService tenantService) : base(factory)
        {
            var currentTenant = tenantService.GetCurrentTenant();
            _readonlyConnectionString = currentTenant.ReadOnlyConnectionString;
            
            if (_readonlyConnectionString.IsNullOrEmpty()) throw new ArgumentNullException(
                $"ReportsRepository requires valid currentTenant.ReadOnlyConnectionString; was: {currentTenant.ReadOnlyConnectionString ?? "null"}"
            );
        }

        public List<dynamic> getDynamicQuery(int id, SearchOptions o)
        {
            ReportDefinition report = dbset.Single(a => a.ID == id);
            List<QueryMetadata> meta = MacheteAdoContext.getMetadata(report.sqlquery, _readonlyConnectionString);
            Type queryType = ILVoodoo.buildQueryType(meta);
            MethodInfo method = Type.GetType("Machete.Data.MacheteAdoContext")
                .GetMethod("SqlQuery", new[] { typeof(string), typeof(string), typeof(SqlParameter[]) });
            MethodInfo man = method.MakeGenericMethod(queryType);

            dynamic dynamicQuery = man.Invoke(null, new object[] {
                    report.sqlquery,
                    _readonlyConnectionString,
                    new[] {
                        new SqlParameter { ParameterName = "beginDate", Value = o.beginDate },
                        new SqlParameter { ParameterName = "endDate", Value = o.endDate },
                        new SqlParameter { ParameterName = "dwccardnum", Value = o.dwccardnum }
                    }
                });

            var dynamicList = new List<dynamic>();
            foreach (var row in dynamicQuery) {
                dynamicList.Add(row);
            }

            return dynamicList;
        }

        public List<ReportDefinition> getList()
        {
            return dbFactory.Get().ReportDefinitions.AsEnumerable().ToList();
        }

        public List<QueryMetadata> getColumns(string tableName)
        {
            return MacheteAdoContext.getMetadata($"select top 0 * from {tableName}", _readonlyConnectionString);
        }

        public DataTable getDataTable(string query)
        {
            // https://stackoverflow.com/documentation/epplus/8223/filling-the-document-with-data
            return MacheteAdoContext.Fill(query, _readonlyConnectionString);
        }

        public List<string> validate(string query)
        {
            return MacheteAdoContext.ValidateQuery(query, _readonlyConnectionString).ToList();
        }
    }
}
