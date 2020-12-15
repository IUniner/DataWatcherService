using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ServiceLibrary_IP3
{
    public class Builder
    {
        private static ModuleBuilder _moduleBuilder;
        private TypeBuilder _typeBuilder;
        private Dictionary<string, object> _getValueByName = new Dictionary<string, object>();
        static Builder()
        {
            // Create an assembly.            
            var assemblyName = new AssemblyName();
            assemblyName.Name = "DynamicAssembly";
            var assemblyBuilder =
                           AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            // Create a dynamic module in Dynamic Assembly.
            _moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
        }
        public Builder(string className)
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
            var type = _typeBuilder.CreateType();
            foreach (var field in type.GetFields())
            {
                field.SetValue(null, _getValueByName[field.Name]);
            }
            return type;
        }
    }
}
