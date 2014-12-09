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
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
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
        private MacheteContext dataContext;
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
            var sb = new StringBuilder();
            if (dataContext == null) 
            {
                if (connString == null)
                {
                    dataContext = new MacheteContext();
                }
                else
                {
                    dataContext = new MacheteContext(connString);
                }
            }
            var conn1 = (dataContext as System.Data.Entity.DbContext).Database.Connection;
            var objid1 = field.GetValue(conn1);
            sb.AppendFormat("DatabaseFactory SqlConnection # [{0}], Conn: {1}", 
                objid1.ToString(),
                connString);
            Debug.WriteLine(sb.ToString());
            return dataContext;
        }
        //public void Set(MacheteContext context)
        //{
        //    dataContext = context;
        //}
        protected override void DisposeCore()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
                dataContext = null;
            }
        }
    }
}
