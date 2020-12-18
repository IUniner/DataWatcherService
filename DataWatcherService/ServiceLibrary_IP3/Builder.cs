using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ServiceLibrary_IP3
{
    public class CBuilder
    {
        private static readonly ModuleBuilder _moduleBuilder;
        private readonly TypeBuilder _typeBuilder;
        private readonly Dictionary<string, object> _getValueByName = new Dictionary<string, object>();
        static CBuilder()
        {
            // Create an assembly.            
            var assemblyName = new AssemblyName
            {
                Name = "DynamicAssembly"
            };
            var assemblyBuilder =
                           AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            // Create a dynamic module in Dynamic Assembly.
            _moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
        }
        public CBuilder(string className)
        {
            // Define a public class in the assembly.
            _typeBuilder = _moduleBuilder.DefineType(className, TypeAttributes.Public);
        }
        public void AddField(Type fieldType, string fieldName, object value)
        {
            // Define a public static field   
            _typeBuilder.DefineField(fieldName,
                fieldType, FieldAttributes.Public | FieldAttributes.Static);
            _getValueByName[fieldName] = value;
        }
        public Type CreateClass()
        {
            var type = _typeBuilder.CreateTypeInfo();
            foreach (var field in type.GetFields())
            {
                field.SetValue(null, _getValueByName[field.Name]);
            }
            return type;
        }
    }
}
