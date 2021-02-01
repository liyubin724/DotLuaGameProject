﻿namespace DotEngine.GUIExt.NativeDrawer
{
    public class VerificationIfAttribute : VerificationAttribute
    {
        public string MemberName { get; private set; }

        public VerificationIfAttribute(string memberName, string invalidMsg) : base(invalidMsg)
        {
            MemberName = memberName;
        }
    }
}
