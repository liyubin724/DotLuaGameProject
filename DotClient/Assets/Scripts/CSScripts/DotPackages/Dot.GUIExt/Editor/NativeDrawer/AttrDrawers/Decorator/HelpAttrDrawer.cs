using DotEngine.GUIExt.NativeDrawer;
using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(HelpAttribute))]
    public class HelpAttrDrawer : DecoratorAttrDrawer
    {
        public override void OnGUILayout()
        {
            HelpAttribute attr = GetAttr<HelpAttribute>();
            MessageType messageType = MessageType.None;
            if (attr.MessageType == HelpMessageType.Warning)
            {
                messageType = MessageType.Warning;
            }
            else if (attr.MessageType == HelpMessageType.Error)
            {
                messageType = MessageType.Error;
            }
            else if (attr.MessageType == HelpMessageType.Info)
            {
                messageType = MessageType.Info;
            }

            EditorGUILayout.HelpBox(attr.Text, messageType);
        }
    }
}
