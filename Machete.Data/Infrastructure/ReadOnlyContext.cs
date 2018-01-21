using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Machete.Data.Infrastructure
{
    public interface IReadOnlyContext : IDatabaseFactory
    {
        List<string> ExecuteSql(MacheteContext context, string query);
    }
    public class ReadOnlyContext : Disposable, IReadOnlyContext
    {
        private string connectionString;
        private BindingFlags bindFlags;
        private FieldInfo field;

        // this class cannot be instantiated without a connection string
        // the purpose is to make the person who implements it think about
        // which connection string should be used (i.e., the readonly one).
        // not validating the connection string here, because this class
        // should have no knowledge of connection strings.
        public ReadOnlyContext(string connectionString)
        {
            this.bindFlags = BindingFlags.Instance
                           | BindingFlags.Public
                           | BindingFlags.NonPublic
                           | BindingFlags.Static;
            this.field = typeof(SqlConnection).GetField("ObjectID", bindFlags);
            this.connectionString = connectionString;
        }

        // we don't want the base get() because it allows you to pass in
        // nothing and get MacheteConnection(), which uses "macheteContext"
        public MacheteContext Get()
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(paramName: "connectionString", message: "An instance of MacheteContext was requested but no connection string was provided.");
            }
            else
            {
                return new MacheteContext(connectionString);
            }
        }

        public List<string> ExecuteSql(MacheteContext context, string query)
        {
            var errors = new List<string>();
            var connection = (context as System.Data.Entity.DbContext).Database.Connection;

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
