using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Machete.Data.Dynamic;
using Newtonsoft.Json;

namespace Machete.Data
{
    public static class MacheteAdoContext
    {
        private static string escapeQueryText(string query)
        {
            try {
                var removedDates = Regex.Replace(query, @"@\w+[Dd]ate", "'1/1/2016'", RegexOptions.None);
                var escapedQuery = Regex.Replace(removedDates, @"@dwccardnum", "0", RegexOptions.None);
                return escapedQuery;
            } catch (RegexMatchTimeoutException) {
                return query;
            }
        }

        public static int Fill(string query, string connectionString, out DataTable dataTable)
        {
            dataTable = new DataTable();
            using (var adapter = new SqlDataAdapter(query, connectionString)) {
                return adapter.Fill(dataTable);
            }
        }
        
        public static List<QueryMetadata> getMetadata(string fromQuery, string connectionString)
        {
            var param = new SqlParameter("@query", escapeQueryText(fromQuery));
            var queryResult = SqlQuery<QueryMetadata>(
                // https://docs.microsoft.com/en-us/sql/relational-databases/system-dynamic-management-views/sys-dm-exec-describe-first-result-set-transact-sql
                // http://stackoverflow.com/questions/13766564/finding-number-of-columns-returned-by-a-query
                @"SELECT
                    name, is_nullable, system_type_name
                FROM
                    sys.dm_exec_describe_first_result_set(@query, NULL, 0);",
                connectionString,
                param);
            return queryResult.ToList();
        }

        // used for report initialization
        public static string getUIColumnsJson(string query, string connectionString)
        {
            var cols = getMetadata(query, connectionString);
            var result = cols.Select(a => 
                new {
                    field = a.name,
                    header = a.name,
                    visible = a.name != "id"
                });
            return JsonConvert.SerializeObject(result);
        }

        public static IEnumerable<T> SqlQuery<T>(string query, string connectionString, params SqlParameter[] sqlParameters) where T : class
        {
            using (var connection = new SqlConnection(connectionString)) 
            {
                var type = typeof(T);
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var result = new List<T>();
                
                var cmd = new SqlCommand(query, connection) { CommandType = CommandType.Text };

                foreach (var parameter in sqlParameters) {
                    cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.SqlValue);                    
                }
                
                connection.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    var instance = (T)Activator.CreateInstance(type);
                    foreach (var property in properties) {
                        if (property == null) continue;
                        if (!property.CanWrite) continue;
                        try {
                            var value = reader[property.Name];
                            property.SetValue(instance, value);
                        } catch (IndexOutOfRangeException) { /*sorry*/ }
                    }
                    result.Add(instance);
                }

                return result;
            }
        }

        public static IEnumerable<string> ValidateQuery(string query, string connectionString)
        {
            var errors = new List<string>();
            var connection = new SqlConnection(connectionString);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "sp_executesql";
                command.CommandType = CommandType.StoredProcedure;
                var param = command.CreateParameter();
                param.ParameterName = "@statement";
                param.Value = query;
                command.Parameters.Add(param);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    for (var i = 0; i < ex.Errors.Count; i++)
                    {
                        // just messages for now; more available: https://stackoverflow.com/a/5842100/2496266
                        errors.Add(ex.Errors[i].Message);
                    }
                }

            }
            return errors;
        }
    }
}
