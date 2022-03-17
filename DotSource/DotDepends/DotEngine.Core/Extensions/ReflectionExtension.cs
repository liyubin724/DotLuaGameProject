using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Core.Extensions
{
    public static class ReflectionExtension
    {
        public static FieldInfo[] GetCustomAttributeFields<T>(this Type type, BindingFlags flags) where T : Attribute
        {
            return (from field in GetFields()
                    let attr = field.GetCustomAttribute<T>()
                    where attr != null
                    select field).ToArray();

            IEnumerable<FieldInfo> GetFields()
            {
                foreach (var field in type.GetFields(flags))
                {
                    yield return field;
                }
            }
        }

        public static PropertyInfo[] GetCustomAttributeProperties<T>(this Type type, BindingFlags flags) where T : Attribute
        {
            return (from property in GetProperties()
                    let attr = property.GetCustomAttribute<T>()
                    where attr != null
                    select property).ToArray();

            IEnumerable<PropertyInfo> GetProperties()
            {
                foreach (var property in type.GetProperties(flags))
                {
                    yield return property;
                }
            }
        }

        /// <summary>
        /// 用于检查函数的参数是否符合指定的类型
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="paramTypes"></param>
        /// <returns></returns>
        public static bool HasParameter(this MethodInfo methodInfo, Type[] paramTypes)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length != paramTypes.Length)
            {
                return false;
            }
            for (int i = 0; i < parameters.Length; ++i)
            {
                if (parameters[i].ParameterType != paramTypes[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
