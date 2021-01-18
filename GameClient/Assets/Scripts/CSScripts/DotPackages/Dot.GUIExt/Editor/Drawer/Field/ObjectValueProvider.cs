using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.Field
{
    public class ObjectValueProvider
    {
        public SystemObject Target { get; private set; }

        private FieldValueProvider[] fields = null;
        private FieldValueProvider[] Fields
        {
            get
            {

                return fields;
            }
        }

        public ObjectValueProvider(SystemObject target)
        {
            Target = target;
        }
    }
}
