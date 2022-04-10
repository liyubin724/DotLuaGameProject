using System;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Config.WDB
{
    public abstract class WDBCellValidation
    {
        public string[] Values { get; private set; }

        public virtual void SetRule(string[] values)
        {
            Values = values;
        }

        protected WDBField GetField(WDBContext context)
        {
            return context.Get<WDBField>(WDBContextKey.CURRENT_FIELD_NAME);
        }

        protected WDBCell GetCell(WDBContext context)
        {
            return context.Get<WDBCell>(WDBContextKey.CURRENT_CELL_NAME);
        }

        public abstract void Verify(WDBContext context);
    }

    public static class WDBCellValidationFactory
    {
        private static Dictionary<string, Type> fieldTypeDic = new Dictionary<string, Type>();
        static WDBCellValidationFactory()
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

        public static WDBCellValidation CreateValidation(string validationName)
        {
            if (fieldTypeDic.TryGetValue(validationName, out var type))
            {
                return Activator.CreateInstance(type) as WDBCellValidation;
            }
            return null;
        }
    }
}
