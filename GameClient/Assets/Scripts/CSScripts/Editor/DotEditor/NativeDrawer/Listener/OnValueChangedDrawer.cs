using DotEngine.NativeDrawer.Listener;
using System.Reflection;

namespace DotEditor.NativeDrawer.Listener
{
    [AttrBinder(typeof(OnValueChangedAttribute))]
    public class OnValueChangedDrawer : ListenerDrawer
    {
        public OnValueChangedDrawer(object target, ListenerAttribute attr) : base(target, attr)
        {
        }

        public override void Execute()
        {
            if (Target == null)
            {
                return;
            }
            var attr = GetAttr<OnValueChangedAttribute>();

            MethodInfo methodInfo = Target.GetType().GetMethod(attr.MethodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            if (methodInfo != null)
            {
                methodInfo.Invoke(Target,null);
            }
        }
    }
}
