using System.Data.Common;
using System.Diagnostics;

namespace Machete.Data.Logging
{
    public class MacheteInterceptorLogging : DbCommandInterceptor
    {
        private ILogger _logger = new Logger();

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            Stopwatch timespan = Stopwatch.StartNew();
            base.ScalarExecuting(command, interceptionContext);
            timespan.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQL Database", "MacheteInterceptor.ScalarExecuting", timespan.Elapsed, "Command: {0}: ", command.CommandText);
            }
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Stopwatch timespan = Stopwatch.StartNew();
            base.NonQueryExecuting(command, interceptionContext);
            timespan.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQL Database", "MacheteInterceptor.NonQueryExecuting", timespan.Elapsed, "Command: {0}: ", command.CommandText);
            }
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            Stopwatch timespan = Stopwatch.StartNew();
            base.ReaderExecuting(command, interceptionContext);
            timespan.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQL Database", "MacheteInterceptor.ReaderExecuting", timespan.Elapsed, "Command: {0}: ", command.CommandText);
            }
        }
    }
}
