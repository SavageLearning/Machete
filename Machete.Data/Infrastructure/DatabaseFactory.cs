#region COPYRIGHT
// File:     DatabaseFactory.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Data
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Machete.Data.Infrastructure
{
    //
    //
    public interface IDatabaseFactory : IDisposable
    {
        MacheteContext Get();
        //void Set(MacheteContext context);
    }
    //
    //
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        string connString;
        private MacheteContext macheteContext;
        private BindingFlags bindFlags = BindingFlags.Instance |
             BindingFlags.Public |
             BindingFlags.NonPublic |
             BindingFlags.Static;
        private FieldInfo field;
        public DatabaseFactory() 
        {
            field = typeof(SqlConnection).GetField("ObjectID", bindFlags);
        }

        public DatabaseFactory(string connString)
        {
            field = typeof(SqlConnection).GetField("ObjectID", bindFlags);
            this.connString = connString;
        }

        public MacheteContext Get()
        {
            if (macheteContext == null) 
            {
                if (connString == null)
                {
                    macheteContext = new MacheteContext();
                }
                else
                {
                    macheteContext = new MacheteContext(connString);
                }
            }
            log_connection_count("DatabaseFactory.Get");
            return macheteContext;
        }

        private void log_connection_count(string prefix)
        {
            var sb = new StringBuilder();
            var conn1 = (macheteContext as System.Data.Entity.DbContext).Database.Connection;
            var objid1 = field.GetValue(conn1);
            sb.AppendFormat("-----------{0} # [{1}], Conn: {2}",
                prefix,
                objid1.ToString(),
                connString);
            Debug.WriteLine(sb.ToString());
        }
        //public void Set(MacheteContext context)
        //{
        //    dataContext = context;
        //}
        protected override void DisposeCore()
        {
            if (macheteContext != null)
            {

                log_connection_count("DatabaseFactory.DisposeCore");
                macheteContext.Dispose();
                macheteContext = null;
            }
        }
    }
}
