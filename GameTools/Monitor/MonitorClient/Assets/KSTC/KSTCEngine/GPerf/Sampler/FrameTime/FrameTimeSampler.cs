using UnityEngine;
using ProfilerRecorder = UnityEngine.Profiling.Recorder;

namespace KSTCEngine.GPerf.Sampler
{
    public class FrameTimeRecord:Record
    {
        public float PlayerLoopTime { get; set; } = 0L;

        public float RenderingTime { get; set; } = 0L;
        public float ScriptTime { get; set; } = 0L;
        public float PhysicsTime { get; set; } = 0L;
        public float AnimationTime { get; set; } = 0L;

        public float CPUFrameTime { get; set; } = 0.0f;
        public float GPUFrameTime { get; set; } = 0.0f;
    }

    public class FrameTimeSampler : GPerfSampler<FrameTimeRecord>
    {
        FrameTiming[] mFrameTimings;

        string[] mPlayerLoopSamplerNames =
        {
            "PlayerLoop",
        };

        string[] mRenderingSamplerNames =
        {
            "PreLateUpdate.EndGraphicsJobsAfterScriptUpdate",
            "PreLateUpdate.ParticleSystemBeginUpdateAll",

            "PostLateUpdate.UpdateCustomRenderTextures",

            "PostLateUpdate.UpdateAllRenerers",
            "PostLateUpdate.UpdateAllSkinnedMeshes",
            "PostLateUpdate.FinishFrameRendering",
        };

        string[] mScriptsSamplerNames =
        {
            "Update.ScriptRunBehaviourUpdate",
            "PreLateUpdate.ScriptRunBehaviourLateUpdate",
            "FixedUpdate.ScriptRunBehaviourFixedUpdate",
            "Update.ScriptRunDelayedDynamicFrameRate",
        };

        string[] mPhysicsSamplerNames =
        {
            "FixedUpdate.PhysocsFixedUpdate",
        };

        string[] mAnimationSamplerNames = {
            "PreLateUpdate.DirectorUpdateAnimationBegin",
            "PreLateUpdate.DirectorUpdateAnimationEnd",
        };

        ProfilerRecorder[] mPlayerLoopRecorders;
        ProfilerRecorder[] mRenderingRecorders;
        ProfilerRecorder[] mScriptsSamplerRecorders;
        ProfilerRecorder[] mPhysicsSamplerRecorders;
        ProfilerRecorder[] mAnimationSamplerRecorders;

        public FrameTimeSampler()
        {
            MetricType = SamplerMetricType.FrameTime;
            FreqType = SamplerFreqType.Interval;
        }

        protected override void OnStart()
        {
            mFrameTimings = new FrameTiming[2];
            if(Debug.isDebugBuild)
            {
                RecorerdInit(in mPlayerLoopSamplerNames, out mPlayerLoopRecorders);
                RecorerdInit(in mRenderingSamplerNames, out mRenderingRecorders);
                RecorerdInit(in mScriptsSamplerNames, out mScriptsSamplerRecorders);
                RecorerdInit(in mPhysicsSamplerNames, out mPhysicsSamplerRecorders);
                RecorerdInit(in mAnimationSamplerNames, out mAnimationSamplerRecorders);
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (Debug.isDebugBuild)
            {
                FrameTimingManager.CaptureFrameTimings();
                FrameTimingManager.GetLatestTimings((uint)mFrameTimings.Length, mFrameTimings);
            }
        }

        protected override void OnSample()
        {
            if(Debug.isDebugBuild)
            {
                record.PlayerLoopTime = GetRecordersTime(mPlayerLoopRecorders);

                record.RenderingTime = GetRecordersTime(mRenderingRecorders);
                record.ScriptTime = GetRecordersTime(mScriptsSamplerRecorders);
                record.PhysicsTime = GetRecordersTime(mPhysicsSamplerRecorders);
                record.AnimationTime = GetRecordersTime(mAnimationSamplerRecorders);

                record.CPUFrameTime = (float)mFrameTimings[0].cpuFrameTime;
                record.GPUFrameTime = (float)mFrameTimings[0].gpuFrameTime;
            }
        }

        private void RecorerdInit(in string[] samplerNames, out ProfilerRecorder[] recorders)
        {
            recorders = new ProfilerRecorder[samplerNames.Length];
            for (var i = 0; i < recorders.Length; i++)
            {
                recorders[i] = ProfilerRecorder.Get(samplerNames[i]);
            }
        }

        private float GetRecordersTime(ProfilerRecorder[] recorders)
        {
            long t = 0;
            foreach (var recorder in recorders)
            {
                t += recorder.elapsedNanoseconds;
            }
            return t / 1000000.0f;
        }
    }
}
