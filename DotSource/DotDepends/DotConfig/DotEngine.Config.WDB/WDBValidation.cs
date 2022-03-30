using System;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Config.WDB
{
    public abstract class WDBValidation
    {
        public string[] Values { get; private set; }

        public virtual void SetRule(string[] values)
        {
            Values = values;
        }

        public abstract void Verify(WDBContext context);

        protected void AddErrorMessage(WDBContext context, string errorMsg)
        {
            List<string> errors = context.Get<List<string>>(WDBContextKey.ERROR_NAME);
            if(errors == null)
            {
                errors = new List<string>();
                context.Add(WDBContextKey.ERROR_NAME, errors);
            }
            errors.Add(errorMsg);
        }
    }

    public static class WDBValidationFactory
    {
        private static Dictionary<string, Type> fieldTypeDic = new Dictionary<string, Type>();
        static WDBValidationFactory()
        {
            var assembly = typeof(WDBField).Assembly;
            var types = (from type in assembly.GetTypes()
                         where type.IsClass && !type.IsAbstract
                         let attrs = type.GetCustomAttributes(typeof(CustomValidationAttribute), false)
                         where attrs != null && attrs.Length > 0
                         let validationName = (attrs[0] as CustomValidationAttribute).Name
                         select (validationName, type)).ToArray();
            if (types != null && types.Length > 0)
            {
                foreach (var t in types)
                {
                    fieldTypeDic.Add(t.validationName, t.type);
                }
            }
        }

        public static WDBValidation CreateValidation(string validationName)
        {
            if (fieldTypeDic.TryGetValue(validationName, out var type))
            {
                return Activator.CreateInstance(type) as WDBValidation;
            }
            return null;
        }
    }
}
