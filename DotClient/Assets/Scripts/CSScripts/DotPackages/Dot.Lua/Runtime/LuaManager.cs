﻿using DotEngine.Core;
using DotEngine.Core.Update;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Lua
{
    public class LuaManager : Singleton<LuaManager>, IUpdate, IFixedUpdate, ILateUpdate
    {
        private string defaultBridgeName = "Default";
        private Dictionary<string, LuaBridger> bridgerDic = new Dictionary<string, LuaBridger>();

        public bool HasBridger(string name)
        {
            return bridgerDic.ContainsKey(name);
        }

        public LuaBridger GetBridger(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = defaultBridgeName;
            }
            return bridgerDic[name];
        }

        public LuaBridger CreateBridger(string name, string initScriptPath, bool asDefault = false)
        {
            if (bridgerDic.TryGetValue(name, out var bridger))
            {
                return bridger;
            }
            bridger = new LuaBridger(initScriptPath);
            bridger.DoStart();
            bridgerDic.Add(name, bridger);

            if (asDefault)
            {
                defaultBridgeName = name;
            }

            return bridger;
        }

        public void DisposeBridger(string name)
        {
            if (!bridgerDic.TryGetValue(name, out var bridger))
            {
                Debug.LogWarning("the bridger is not found");
                return;
            }
            bridger.DoDestroy();
            bridgerDic.Remove(name);
        }

        public void DoFixedUpdate(float deltaTime, float unscaleDeltaTime)
        {
            foreach (var kvp in bridgerDic)
            {
                kvp.Value.DoFixedUpdate(deltaTime, unscaleDeltaTime);
            }
        }

        public void DoLateUpdate(float deltaTime, float unscaleDeltaTime)
        {
            foreach (var kvp in bridgerDic)
            {
                kvp.Value.DoLateUpdate(deltaTime, unscaleDeltaTime);
            }
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            foreach (var kvp in bridgerDic)
            {
                kvp.Value.DoUpdate(deltaTime, unscaleDeltaTime);
            }
        }

        protected override void OnInit()
        {
            base.OnInit();
            UpdateManager.GetInstance().AddUpdater(this);
            UpdateManager.GetInstance().AddLateUpdater(this);
            UpdateManager.GetInstance().AddFixedUpdater(this);
        }

        protected override void OnDestroy()
        {
            UpdateManager.GetInstance().RemoveUpdater(this);
            UpdateManager.GetInstance().RemoveLateUpdater(this);
            UpdateManager.GetInstance().RemoveFixedUpdater(this);

            foreach (var kvp in bridgerDic)
            {
                kvp.Value.DoDestroy();
            }
            foreach (var kvp in bridgerDic)
            {
                kvp.Value.Dispose();
            }
            bridgerDic = null;

            base.OnDestroy();
        }

    }
}