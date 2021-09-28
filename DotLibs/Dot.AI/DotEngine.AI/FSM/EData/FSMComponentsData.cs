using System.Collections.Generic;
using System.Linq;

namespace DotEngine.AI.FSM.Editor
{
    public class FSMComponentsData
    {
        public List<ComponentData> components = new List<ComponentData>();

        public ComponentData GetComponent(string componentName)
        {
            return (from component in components where component.name == componentName select component).First();
        }

        public ComponentFieldData GetField(string componentName, string fieldName)
        {
            return (from component in components
                    where component.name == componentName
                    from field in component.fields
                    where field.functionName == fieldName
                    select field).First();
        }
    }

    public class ComponentData
    {
        public string name;
        public string displayName;
        public string scriptPath;

        public List<ComponentFieldData> fields = new List<ComponentFieldData>();

        public ComponentFieldData GetField(string fieldName)
        {
            return (from field in fields where field.functionName == fieldName select field).First();
        }
    }

    public enum ValueType
    {
        Number = 0,
        String,
        Bool,
        Table
    }

    public class ComponentFieldData
    {
        public string functionName;
        public string displayName;
        public ValueType valueType = ValueType.Number;
    }
}
