using UnityEngine;

namespace DotEngine.Core.EGizmos
{
    public class PointGizmos : MonoBehaviour
    {
        public enum PointShape
        {
            Sphere = 0,
            Cube,
        }

        public Transform targetTran;

        [Space]
        public PointShape shape = PointShape.Sphere;
        public Color color = Color.red;
        public bool isOnlyWire = false;
        public float size = 1.0f;

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if(targetTran == null)
            {
                targetTran = transform;
            }

            Color cachedColor = Gizmos.color;
            Gizmos.color = color;
            if(shape == PointShape.Sphere)
            {
                if (isOnlyWire)
                {
                    Gizmos.DrawWireSphere(targetTran.position, size);
                }
                else
                {
                    Gizmos.DrawSphere(targetTran.position, size);
                }
            }
            else if(shape == PointShape.Cube)
            {
                if(isOnlyWire)
                {
                    Gizmos.DrawWireCube(targetTran.position, Vector3.one * size);
                }else
                {
                    Gizmos.DrawCube(targetTran.position, Vector3.one * size);
                }
            }

            Gizmos.color = cachedColor;
        }

#endif
    }
}
