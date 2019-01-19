using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Machete.Data.DTO;
using Machete.Data.Dynamic;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Microsoft.Extensions.Configuration;

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
        private string connectionString { get; }

        public ReportsRepository(IDatabaseFactory databaseFactory, IConfiguration configuration) : base(databaseFactory)
        {
            connectionString = configuration.GetConnectionString("ReadOnlyConnection");
        }


        public List<dynamic> getDynamicQuery(int id, SearchOptions o)
        {
            ReportDefinition report = dbset.Single(a => a.ID == id); // TODO move to ADO
            List<QueryMetadata> meta = MacheteAdoContext.getMetadata(report.sqlquery, connectionString);
            Type queryType = ILVoodoo.buildQueryType(meta);
            MethodInfo method = Type.GetType("Machete.Data.MacheteAdoContext")
                .GetMethod("SqlQuery", new[] { typeof(string), typeof(string), typeof(SqlParameter[]) });
            MethodInfo man = method.MakeGenericMethod(queryType);

            dynamic dynamicQuery = man.Invoke(null, new object[] {
                    report.sqlquery,
                    connectionString,
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
            return dbFactory.Get().ReportDefinitions.AsEnumerable().ToList(); // TODO move to ADO
        }

        public List<QueryMetadata> getColumns(string tableName)
        {
            return MacheteAdoContext.getMetadata($"select top 0 * from {tableName}", connectionString);
        }

        public DataTable getDataTable(string query)
        {
            // https://stackoverflow.com/documentation/epplus/8223/filling-the-document-with-data
            MacheteAdoContext.Fill(query, connectionString, out var dataTable);
            return dataTable;
        }

        public List<string> validate(string query)
        {
            return MacheteAdoContext.ValidateQuery(query, connectionString).ToList();
        }
    }
}
