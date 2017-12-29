using System;
using System.Data.SqlClient;
using System.Reflection;

namespace Machete.Data.Infrastructure
{
    public interface IReadOnlyContext : IDatabaseFactory {
       string[] ExecuteSql();
    }
    public class ReadOnlyContext : Disposable, IReadOnlyContext
    {
        private string connectionString;
        private BindingFlags bindFlags;
        private FieldInfo field;
        private MacheteContext macheteContext;

        // this class cannot be instantiated without a connection string
        // the purpose is to avoid getting an instance of "macheteContext"
        public ReadOnlyContext(string connectionString) {
            this.bindFlags = BindingFlags.Instance 
                           | BindingFlags.Public
                           | BindingFlags.NonPublic
                           | BindingFlags.Static;
            this.field = typeof(SqlConnection).GetField("ObjectID", bindFlags);
            this.connectionString = connectionString;
        }

        // we don't want the base get() because it allows you to pass in
        // nothing and get MacheteConnection(), which uses "macheteContext"
        public MacheteContext Get() {
            if (String.IsNullOrEmpty(connectionString)) {
                throw new ArgumentNullException(paramName: "connectionString", message: "An instance of MacheteContext was requested but no connection string was provided.");
            } else {
                // intentionally not using the C# 6 stuff...it is nowhere else in Machete that I've seen
                return (macheteContext == null) ? new MacheteContext(connectionString) : macheteContext;
            }
        }

        public string[] ExecuteSql()
        {
            throw new NotImplementedException();
        }
    }
}
