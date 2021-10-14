﻿using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public abstract class AAsyncOperation : AOperation
    {
        protected AsyncOperation operation = null;
        public override bool IsFinished
        {
            get
            {
                if(isRunning && operation!=null)
                {
                    return operation.isDone;
                }
                return false;
            }
        }

        public override float Progress
        {
            get
            {
                if(isRunning && operation !=null)
                {
                    return operation.progress;
                }
                return 0.0f;
            }
        }

        public override UnityObject GetAsset()
        {
            if(!IsFinished)
            {
                Debug.LogError("The asyncOperation has finished.");
                return null;
            }
            return GetFromOperation();
        }


        public override void DoStart()
        {
            base.DoStart();

            operation = CreateOperation(assetPath);
            operation.completed += OnComplete;
        }

        private void OnComplete(AsyncOperation ao)
        {
            operation.completed -= OnComplete;
        }

        public override void DoEnd()
        {
            DestroyOperation();

            operation = null;
            base.DoEnd();
        }

        protected abstract AsyncOperation CreateOperation(string path);
        protected abstract void DestroyOperation();
        protected abstract UnityObject GetFromOperation();
    }
}
