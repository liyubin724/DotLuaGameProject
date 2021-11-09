using DotEngine.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace DotEditor.Net
{
    public class EditorNetwork
    {
        private static ClientNetwork clientNetwork = null;
        public static ClientNetwork GetInstance()
        {
            if(clientNetwork == null)
            {
                clientNetwork = new ClientNetwork(new MessageEncoder(), new MessageDecoder());
                EditorApplication.update += () =>
                {
                    clientNetwork.DoUpdate(Time.deltaTime, Time.unscaledDeltaTime);
                };
            }

            return clientNetwork;
        }
    }
}

