﻿using DotEngine.Framework.Updater;

namespace DotEngine.Framework.Services
{
    public delegate void FixedUpdateHandler(float deltaTime,float unscaleDeltaTime);

    public interface IFixedUpdateService : IFixedUpdate, IService
    {
        event FixedUpdateHandler Handler;
    }
}
