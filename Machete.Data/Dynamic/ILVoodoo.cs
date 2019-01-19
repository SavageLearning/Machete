using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Machete.Data.Dynamic {
    public static class ILVoodoo {
        public static Type buildQueryType(List<QueryMetadata> columns)
        {
            TypeBuilder builder = CreateTypeBuilder("MyDynamicAssembly", "MyModule", "dynamicQueryType");
            foreach (QueryMetadata c in columns)
            {
                if (c.system_type_name == null) throw new ArgumentNullException("getQueryTyped received null for column name");
                Type ttype = getTypeof(c.system_type_name);
                CreateAutoImplementedProperty(builder, c.name, ttype);
            }
            return builder.CreateType();
        }

        public static Type getTypeof(string sqlType)
        {
            switch (sqlType.ToUpper().Substring(0, 3)) {
                case "BIT":
                    return typeof(bool);
                case "DAT":
                    return typeof(DateTime);
                case "FLO":
                    return typeof(double);
                case "INT":
                    return typeof(int);
                case "NVA":
                    return typeof(string);
                case "REA":
                    return typeof(Single);
                case "MON":
                    return typeof(decimal);
            }
            switch (sqlType.ToUpper().Substring(0, 4)) {
                case "VARC":
                    return typeof(string);
                case "VARB":
                case "NULL":
                    return null; // not implementing varbinary
                default:
                    return null;
            }
        }

        // https://www.codeproject.com/Articles/206416/Use-dynamic-type-in-Entity-Framework-SqlQuery
        public static TypeBuilder CreateTypeBuilder(
            string assemblyName, string moduleName, string typeName)
        {
            //TypeBuilder typeBuilder = AppDomain
            //    .CurrentDomain
            TypeBuilder typeBuilder = AssemblyBuilder
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
    }
}