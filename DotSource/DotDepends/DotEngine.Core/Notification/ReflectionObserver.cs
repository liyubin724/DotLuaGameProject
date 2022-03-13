using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotEngine.Notification
{
    public class ReflectionObserver : IObserver
    {
        private Dictionary<string, MethodInfo> notificationDic = new Dictionary<string, MethodInfo>();
        public void HandleNotification(string name, object body = null)
        {
            if(notificationDic.TryGetValue(name,out var methodInfo))
            {
                methodInfo.Invoke(this,new object[] { name, body });
            }
        }

        public string[] ListInterestNotification()
        {
            MethodInfo[] methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach(var method in methods)
            {
                var attr = method.GetCustomAttribute<CustomNotificationHandlerAttribute>();
                if(attr!=null)
                {
                    var parameters = method.GetParameters();
                    if(parameters == null || parameters.Length != 2 || parameters[0].ParameterType != typeof(string))
                    {
                        throw new Exception("The parameters is not match the attribute!");
                    }
                    else
                    {
                        notificationDic.Add(attr.Name, method);
                    }
                }
            }
            return notificationDic.Keys.ToArray();
        }
    }
}
