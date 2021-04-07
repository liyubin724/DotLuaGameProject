using System;

namespace DotEngine.Context
{
    public class ContextException : Exception
    {
        public ContextException(string messageFormat,params object[] values) 
            : base(string.Format(messageFormat,values))
        {
        }
    }

    public class ContextKeyRepeatException:ContextException
    {
        public ContextKeyRepeatException(object key) 
            : base("The key ({0}) has been added!",key)
        {
        }
    }

    public class ContextKeyNotFoundException:ContextException
    {
        public ContextKeyNotFoundException(object key) 
            : base("The key ({0}) was not found!",key)
        {
        }
    }

    public class ContextValueCastFailedException:ContextException
    {
        public ContextValueCastFailedException(object key,Type valueType,Type targetType)
            :base("The key ({0}) is typeof {1},can't cast to be {2}",key,valueType,targetType)
        {
        }
    }

    public class ContextValueNullException:ContextException
    {
        public ContextValueNullException(object key) : base("The value of the key ({0}) is null!",key)
        {
        }
    }
}
