using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class LineSetting
    {
        public static LineSetting Setting = null;

        public const int MIN_TRACKLINE_HEIGHT = 30;
        public const int MAX_TRACKLINE_HEIGHT = 120;

        public const float MIN_TIME_STEP = 0.01f;
        public const float MAX_TIME_STEP = 1.0f;
        
        public const int MIN_WIDTH_FOR_SECOND = 200;
        public const int MAX_WIDTH_FOR_SECOND = 400;

        private int tracklineHeight = 60;
        public int TracklineHeight
        {
            get
            {
                return tracklineHeight;
            }
            set
            {
                tracklineHeight = value;
                if(tracklineHeight>MAX_TRACKLINE_HEIGHT)
                {
                    tracklineHeight = MAX_TRACKLINE_HEIGHT;
                }else if(tracklineHeight<MIN_TRACKLINE_HEIGHT)
                {
                    tracklineHeight = MIN_TRACKLINE_HEIGHT;
                }
            }
        }
        private float timeStep = 0.05f;
        public float TimeStep
        {
            get
            {
                return timeStep;
            }
            set
            {
                timeStep = value;
                if(timeStep>MAX_TIME_STEP)
                {
                    timeStep = MAX_TIME_STEP;
                }else if(timeStep<MIN_TIME_STEP)
                {
                    timeStep = MIN_TIME_STEP;
                }
            }
        }
        public float ZoomTimeStep { get; set; } = 0.01f;
        private int widthForSecond = 200;
        public int WidthForSecond
        {
            get { return widthForSecond; }
            set
            {
                widthForSecond = value;
                if(widthForSecond > MAX_WIDTH_FOR_SECOND)
                {
                    widthForSecond = MAX_WIDTH_FOR_SECOND;
                }else if(widthForSecond < MIN_WIDTH_FOR_SECOND)
                {
                    widthForSecond = MIN_WIDTH_FOR_SECOND;
                }
            }
        }

        public float TimeStepWidth { get => TimeStep * WidthForSecond; }
        public Vector2 ScrollPos = Vector2.zero;
        public float ScrollPosX { get => ScrollPos.x; }
        public float ScrollPosY { get => ScrollPos.y; }

        public string CopiedActionData = string.Empty;

        public int MaxActionIndex = 0;
        public int GetActionIndex()
        {
            return ++MaxActionIndex;
        }
    }
}
