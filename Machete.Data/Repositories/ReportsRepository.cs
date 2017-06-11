using Machete.Data.Helpers;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Machete.Data
{
    public interface IReportsRepository : IRepository<ReportDefinition>
    {
        List<SimpleDataRow> getSimpleAggregate(int id, DateTime beginDate, DateTime endDate);
        List<dynamic> getDynamicQuery(int id, DTO.SearchOptions o);
        List<ReportDefinition> getList();
        List<QueryMetadata> getColumns(string tableName);
        DataTable getDataTable(string query, DTO.SearchOptions o);
    }
    public class ReportsRepository : RepositoryBase<ReportDefinition>, IReportsRepository
    {


        public ReportsRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {}

        public List<SimpleDataRow> getSimpleAggregate(int id, DateTime beginDate, DateTime endDate)
        {
            var rdef = dbset.Single(a => a.ID == id);
            return db.Get().Database.SqlQuery<SimpleDataRow>(rdef.sqlquery,
                new SqlParameter { ParameterName = "beginDate", Value = beginDate },
                new SqlParameter { ParameterName = "endDate", Value = endDate })
                .ToList();
        }

        public List<dynamic> getDynamicQuery(int id, DTO.SearchOptions o)
        {
            var rdef = dbset.Single(a => a.ID == id);
            var meta = SqlServerUtils.getMetadata(DataContext, rdef.sqlquery);
            var queryType = buildQueryType(meta);
            Task<List<object>> raw = db.Get().Database.SqlQuery(
                queryType, 
                rdef.sqlquery,
                new SqlParameter { ParameterName = "beginDate", Value = o.beginDate },
                new SqlParameter { ParameterName = "endDate", Value = o.endDate },
                new SqlParameter { ParameterName = "dwccardnum", Value = o.dwccardnum }).ToListAsync();

            // TODO catch exception and handle here
            raw.Wait();
            var results = raw.Result;
            return results;

        }

        public DataTable getDataTable(string query, DTO.SearchOptions o)
        {
            // https://stackoverflow.com/documentation/epplus/8223/filling-the-document-with-data
            DataTable dt = new DataTable();
            var cnxn = DataContext.Database.Connection.ConnectionString;
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, cnxn))
            {
                adapter.Fill(dt);
            }
            return dt;
        }

        public Type buildQueryType(List<QueryMetadata> columns)
        {
            TypeBuilder builder = CreateTypeBuilder(
                "MyDynamicAssembly", "MyModule", "dynamicQueryType");
            foreach (var c in columns)
            {
                if (c.system_type_name == null) throw new ArgumentNullException("getQueryTyped received null for column name");
                var ttype = getTypeof(c.system_type_name);
                CreateAutoImplementedProperty(builder, c.name, ttype);
            }
            return builder.CreateType();
        }

        public Type getTypeof(string sqlType)
        {
            if (sqlType.ToUpper().Substring(0, 3) == "BIT") return typeof(bool);
            if (sqlType.ToUpper().Substring(0, 3) == "DAT") return typeof(DateTime);
            if (sqlType.ToUpper().Substring(0, 3) == "FLO") return typeof(double);
            if (sqlType.ToUpper().Substring(0, 3) == "INT") return typeof(int);
            if (sqlType.ToUpper().Substring(0, 3) == "NVA") return typeof(string);
            if (sqlType.ToUpper().Substring(0, 3) == "REA") return typeof(Single);
            if (sqlType.ToUpper().Substring(0, 4) == "VARC") return typeof(string);
            if (sqlType.ToUpper().Substring(0, 3) == "MON") return typeof(decimal);
            if (sqlType.ToUpper().Substring(0, 4) == "NULL") return null;

            if (sqlType.ToUpper().Substring(0, 4) == "VARB") return null; // not implementing varbinary

            return null;
        }

        public List<ReportDefinition> getList()
        {
            return db.Get().ReportDefinitions.AsEnumerable().ToList();
        }

        public List<QueryMetadata> getColumns(string tableName)
        {
            if (tableName == null) throw new ArgumentNullException("tableName can't be null");
            return SqlServerUtils.getMetadata(DataContext, "select top 0 * from " + tableName);
        }

        #region IL voodoo

        //
        // https://www.codeproject.com/Articles/206416/Use-dynamic-type-in-Entity-Framework-SqlQuery
        public static TypeBuilder CreateTypeBuilder(
                        string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName),
                                       AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }
        public static void CreateAutoImplementedProperty(
            TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
                string.Concat(PrivateFieldPrefix, propertyName),
                              propertyType, FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
                propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(GetterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
                string.Concat(SetterPrefix, propertyName),
                propertyMethodAttributes, null, new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }
        #endregion
    }

    public class QueryMetadata
    {
        public string name { get; set; }
        public bool? is_nullable { get; set; }
        public string system_type_name { get; set; }
        public bool include { get; set; } /// default value for the UI
    }
  
}
