using System;
using System.Reflection;

namespace DotEngine.Config
{
    public static class AssemblyUtility
    {
        public static Type GetTypeByName(string name, bool ingnoreCase = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (ingnoreCase)
            {
                name = name.ToLower();
            }

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    string typeName = ingnoreCase ? type.Name.ToLower() : type.Name;
                    if (typeName == name)
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }
}
