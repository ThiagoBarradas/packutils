using System;
using System.Reflection;

namespace PackUtils
{
    public static class AssemblyUtility
    {
        private static void InvokeMethodWithGeneric(this object instance, string method, Type type, object[] parameters = null)
        {
            MethodInfo baseMethod = instance.GetType().GetMethod(method);
            MethodInfo methodWithGeneric = baseMethod.MakeGenericMethod(type);
            methodWithGeneric.Invoke(instance, parameters);
        }

        private static object CreateInstanceWithGeneric(Type baseType, Type genericType, object[] parameters = null)
        {
            Type[] typeArgs = { genericType };
            var makeme = baseType.MakeGenericType(typeArgs);

            return Activator.CreateInstance(makeme, parameters);
        }
    }
}
