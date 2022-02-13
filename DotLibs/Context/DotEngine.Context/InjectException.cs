using System;

namespace DotEngine.Injection
{
    public class InjectException : Exception
    {
        public InjectException(string messageFormat, params object[] values)
            : base(string.Format(messageFormat, values))
        {
        }
    }

    public class InjectContextKeyRepeatException : InjectException
    {
        public InjectContextKeyRepeatException(object key)
            : base("The key ({0}) has been added!", key)
        {
        }
    }

    public class InjectContextKeyNotFoundException : InjectException
    {
        public InjectContextKeyNotFoundException(object key)
            : base("The key ({0}) was not found!", key)
        {
        }
    }

    public class InjectContextValueCastFailedException : InjectException
    {
        public InjectContextValueCastFailedException(object key, Type valueType, Type targetType)
            : base("The key ({0}) is typeof {1},can't cast to be {2}", key, valueType, targetType)
        {
        }
    }

    public class InjectValueNotFoundException : InjectException
    {
        public InjectValueNotFoundException(object key) : base("The value of the key ({0}) is null!", key)
        {
        }
    }
}
