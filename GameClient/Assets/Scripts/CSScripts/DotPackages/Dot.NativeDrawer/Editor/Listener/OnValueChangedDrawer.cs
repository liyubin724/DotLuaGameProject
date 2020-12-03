using DotEngine.NativeDrawer.Listener;
using System.Reflection;

namespace DotEditor.NativeDrawer.Listener
{
    [Binder(typeof(OnValueChangedAttribute))]
    public class OnValueChangedDrawer : ListenerDrawer
    {
        public override void Execute()
        {
            var attr = GetAttr<OnValueChangedAttribute>();
            if(!string.IsNullOrEmpty(attr.CallbackMethodName))
            {
                MethodInfo methodInfo = Property.TargetType.GetMethod(attr.CallbackMethodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (methodInfo != null)
                {
                    methodInfo.Invoke(Property.Target, null);
                }
            }
        }
    }
}
