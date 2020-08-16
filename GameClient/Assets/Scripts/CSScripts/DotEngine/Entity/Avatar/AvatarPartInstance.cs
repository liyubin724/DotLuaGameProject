using UnityEngine;

namespace DotEngine.Entity.Avatar
{
    public class AvatarPartInstance
    {
        public string partName = string.Empty;
        public SkinnedMeshRenderer[] renderers = new SkinnedMeshRenderer[0];
        public GameObject[] gameObjects = new GameObject[0];
    }
}
