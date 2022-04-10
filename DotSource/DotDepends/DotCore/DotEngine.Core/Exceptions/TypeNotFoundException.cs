using System;

namespace DotEngine.Core.Exceptions
{
    public class TypeNotFoundException : Exception
    {
        public TypeNotFoundException(string typeName) : base($"The type({typeName}) is not found.")
        {
        }
    }
}
