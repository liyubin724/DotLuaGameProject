using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    public enum HelpMessageType
    {
        None,
        Info,
        Warning,
        Error,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HelpAttribute : DecoratorAttribute
    {
        public string Text { get; private set; }
        public HelpMessageType MessageType { get; private set; }

        public HelpAttribute(string text, HelpMessageType messageType = HelpMessageType.None)
        {
            Text = text;
            MessageType = messageType;
        }
    }
}
