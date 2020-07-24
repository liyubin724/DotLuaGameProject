using System;

namespace DotEngine.BehaviourLine.Action
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ActionNameAttribute : Attribute
    {
        public string BriefName { get; set; } = string.Empty;
        public string DetailName { get; set; } = string.Empty;

        public ActionNameAttribute()
        {

        }
    }
}
