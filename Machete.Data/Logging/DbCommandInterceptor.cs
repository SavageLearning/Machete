using System.Data.Common;
using System.Data.SqlClient;

// TODO write interceptor https://github.com/aspnet/EntityFrameworkCore/issues/12024#issuecomment-389584810
namespace Machete.Data.Logging
{
    public class DbCommandInterceptor
    {
        public virtual void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            //throw new System.NotImplementedException();
        }

        public virtual void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            //throw new System.NotImplementedException();
        }

        public virtual void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            //throw new System.NotImplementedException();
        }
    }

    public class DbCommandInterceptionContext<T>
    {
        public SqlException Exception { get; set; }
    }
}