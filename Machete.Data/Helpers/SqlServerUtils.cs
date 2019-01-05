using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Machete.Data.Helpers
{
    public static class SqlServerUtils
    {
        public static string escapeQueryText(string query)
        {
            string escapedQuery;
            string removedDates;
            try
            {
                removedDates = Regex.Replace(query,
                      @"@\w+[Dd]ate",
                      "'1/1/2016'", RegexOptions.None);
                escapedQuery = Regex.Replace(removedDates,
                      @"@dwccardnum",
                      "0", RegexOptions.None);
            }
            catch (RegexMatchTimeoutException)
            {
                escapedQuery = query;
            }
            return escapedQuery;
        }

        public static List<QueryMetadata> getMetadata(MacheteContext context, string fromQuery)
        {
            var param = new SqlParameter("@query", escapeQueryText(fromQuery));
            
            var queryResult = context.Query<QueryMetadata>().FromSql(
                // https://docs.microsoft.com/en-us/sql/relational-databases/system-dynamic-management-views/sys-dm-exec-describe-first-result-set-transact-sql
                // http://stackoverflow.com/questions/13766564/finding-number-of-columns-returned-by-a-query
                @"SELECT
                    name, is_nullable, system_type_name
                FROM
                    sys.dm_exec_describe_first_result_set(@query, NULL, 0);",
                param);
            return queryResult.ToList();
        }
        // used for report initialization
        public static string getUIColumnsJson(MacheteContext context, string query)
        {
            var cols = SqlServerUtils.getMetadata(context, query);
            var result = cols.Select(a => 
                new {
                    field = a.name,
                    header = a.name,
                    visible = a.name == "id" ? false : true
                });
            return JsonConvert.SerializeObject(result);
        }
    }
}
