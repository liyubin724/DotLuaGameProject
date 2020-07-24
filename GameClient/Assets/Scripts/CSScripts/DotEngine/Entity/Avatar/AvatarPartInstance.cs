using UnityEngine;

namespace DotEngine.Entity.Avatar
{
    public class AvatarPartInstance
    {
        public AvatarPartType partType = AvatarPartType.Feet;
        public SkinnedMeshRenderer[] renderers = new SkinnedMeshRenderer[0];
        public GameObject[] gameObjects = new GameObject[0];
    }
}
