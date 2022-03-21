using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.UIElements
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class QAttribute : Attribute
    {
        public string Name { get; set; }
        public string[] Classes { get; set; }

        public QAttribute() { }
        public QAttribute(string name, params string[] classes) =>
            (Name, Classes) = (name, classes);
    }
}
