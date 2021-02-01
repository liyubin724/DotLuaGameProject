using DotEngine.NativeDrawer.Decorator;
using UnityEditor;

namespace DotEditor.NativeDrawer.Decorator
{
    [Binder(typeof(HelpAttribute))]
    public class HelpDrawer : DecoratorDrawer
    {
        public override void OnGUILayout()
        {
            if(!DrawerSetting.IsShowHelp)
            {
                return;
            }

            HelpAttribute attr = GetAttr<HelpAttribute>();
            MessageType messageType = MessageType.None;
            if(attr.MessageType == HelpMessageType.Warning)
            {
                messageType = MessageType.Warning;
            }else if(attr.MessageType == HelpMessageType.Error)
            {
                messageType = MessageType.Error;
            }else if(attr.MessageType == HelpMessageType.Info)
            {
                messageType = MessageType.Info;
            }

            EditorGUILayout.HelpBox(attr.Text, messageType);
        }
    }
}
