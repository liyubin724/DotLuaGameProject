using UnityEngine;

namespace DotEngine.UI
{
    public enum UIAtlasImageAnimationMode
    {
        Once,
        Loop,
        Pingpong,
    }

    [AddComponentMenu("DotEngine/UI/Atlas Image Animation", 13)]
    [ExecuteInEditMode]
    public class UIAtlasImageAnimation : UIAtlasImage
    {
        public bool isSetNativeSize = false;
        public int frameRate = 8;
        public bool autoPlayOnAwake = true;
        public UIAtlasImageAnimationMode playMode = UIAtlasImageAnimationMode.Loop;
        public string spriteNamePrefix = "";
        public int spriteIndex = 0;
        public int spriteStartIndex = 0;
        public int spriteEndIndex = 0;

        private float frameTime = 0.0f;
        private float elapseTime = 0.0f;
        private bool isPlaying = false;
        private bool isForward = true;
        protected override void Awake()
        {
            base.Awake();
            if (frameRate != 0)
            {
                frameTime = 1.0f / frameRate;
            }

            if (Application.isPlaying && autoPlayOnAwake)
            {
                isPlaying = true;
                ChangeAnimation();
            }
        }

        private void Update()
        {
            if (isPlaying && frameRate > 0)
            {
                elapseTime += Time.unscaledDeltaTime;
                if (elapseTime >= frameTime)
                {
                    if (isForward)
                    {
                        ++spriteIndex;
                    }
                    else
                    {
                        --spriteIndex;
                    }

                    if (spriteIndex > spriteEndIndex)
                    {
                        if (playMode == UIAtlasImageAnimationMode.Once)
                        {
                            isPlaying = false;
                        }
                        else if (playMode == UIAtlasImageAnimationMode.Loop)
                        {
                            spriteIndex = spriteStartIndex;
                        }
                        else if (playMode == UIAtlasImageAnimationMode.Pingpong)
                        {
                            spriteIndex = spriteEndIndex;
                            isForward = false;
                        }
                    }
                    else if (spriteIndex < spriteStartIndex && playMode == UIAtlasImageAnimationMode.Pingpong)
                    {
                        spriteIndex = spriteStartIndex;
                        isForward = true;
                    }
                    else
                    {
                        isPlaying = false;
                    }
                    ChangeAnimation();
                    elapseTime = 0;
                }
            }
        }

        public void ChangeAnimation()
        {
            if (spriteIndex > spriteEndIndex)
            {
                spriteIndex = spriteEndIndex;
            }
            if (spriteIndex < spriteStartIndex)
            {
                spriteIndex = spriteStartIndex;
            }
            string spriteName = $"{spriteNamePrefix}{spriteIndex}";
            if (SpriteName != spriteName)
            {
                SpriteName = spriteName;
                if (isSetNativeSize)
                {
                    SetNativeSize();
                }
            }
        }

        public void Play()
        {
            isPlaying = true;
        }

        public void PlayAt(int index)
        {
            isPlaying = true;
            spriteIndex = index;

            ChangeAnimation();
        }

        public void Stop()
        {
            isPlaying = false;
            elapseTime = 0f;
        }

        public void StopAt(int index)
        {
            isPlaying = false;
            spriteIndex = index;
            elapseTime = 0f;

            ChangeAnimation();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            ChangeAnimation();
            base.OnValidate();
        }
#endif
    }
}
