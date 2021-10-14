using DotEngine.Core.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Assets
{
    public enum LoaderMode
    {
        Database = 0,
        Bundle = 1,
        Resource = 2,
    }

    public class AssetManager : IUpdate
    {
        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            
        }
    }
}
