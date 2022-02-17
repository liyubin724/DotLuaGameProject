using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Notify
{
    public sealed class DispatchManager
    {
        private static DispatchManager manager = null;

        public static DispatchManager GetInstance()
        {
            return manager;
        }

        public static DispatchManager CreateMgr()
        {
            if(manager == null)
            {
                manager = new DispatchManager();
                manager.OnInitilize();
            }
            return manager;
        }

        public static void DestroyMgr()
        {
            if(manager!=null)
            {
                manager.OnDestroy();
                manager = null;
            }
        }

        private readonly Dictionary<string, List<Action<INotification>>> observerDic = new Dictionary<string, List<Action<INotification>>>();

        private DispatchManager()
        {

        }

        public void OnInitilize()
        {
        }

        public void OnDestroy()
        {
            observerDic.Clear();
        }

        public void RegisterObserver(string notificationName,Action<INotification> notifyHanlder)
        {

        }

        public void UnregisterObserver(string notificationName,Action<INotification> notifyHandler)
        {

        }

        public void UnregisterObserver(string notificationName)
        {

        }

        public void Notify(string notificationName,object body = null)
        {

        }

        public void Notify(INotification notification)
        {

        }

        public void ClearAll()
        {

        }
    }
}
