using System;
using System.Reflection;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class NObjectDrawer : NInstanceDrawer
    {
        public NObjectDrawer(SystemObject target):base(target)
        {
        }

        protected override void InitDrawers()
        {
            Type[] allTypes = NDrawerUtility.GetAllBaseTypes(Target.GetType());
            if (allTypes != null && allTypes.Length > 0)
            {
                foreach (var type in allTypes)
                {
                    if(IsShowInherit)
                    {
                        childDrawers.Add(new NHeadDrawer()
                        {
                            Header = type.Name,
                        });
                    }

                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    foreach (var field in fields)
                    {
                        if(NDrawerUtility.IsTypeSupported(field.FieldType))
                        {
                            NFieldDrawer fieldDrawer = new NFieldDrawer(field, Target);
                            childDrawers.Add(fieldDrawer);
                        }else
                        {
                            childDrawers.Add(new UnsupportedTypeDrawer()
                            {
                                Label = field.Name,
                                TargetType = field.FieldType,
                            });
                        }
                    }

                    if(IsShowInherit)
                    {
                        childDrawers.Add(new NHorizontalLineDrawer());
                    }
                }
            }
        } 
    }
}