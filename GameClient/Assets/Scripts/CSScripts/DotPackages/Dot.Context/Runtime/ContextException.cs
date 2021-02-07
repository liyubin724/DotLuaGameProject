using System;

namespace DotEngine.Context
{
    public class ContextException : Exception
    {
        public ContextException(string message) : base(message)
        {
        }
    }

    public class ContextKeyRepeatException:ContextException
    {
        public object Key { get; private set; }

        public ContextKeyRepeatException(object key) : base($"The key (${key}) has been added!")
        {
            Key = key;
        }
    }

    public class ContextKeyNotFoundException:ContextException
    {
        public object Key { get; private set; }

        public ContextKeyNotFoundException(object key) : base($"The key (${key}) was not found!")
        {
            Key = key;
        }
    }

    public class ContextValueNullException:ContextException
    {
        public object Key { get; private set; }

        public ContextValueNullException(object key) : base($"The value of the key (${key}) is null!")
        {
            Key = key;
        }
    }
}
