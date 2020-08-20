using DotEngine.World.QT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubeQuadObject : IQuadObject
{
    public Rect Bounds { get; private set; }
    private GameObject cubeGO;

    public event QuadObjectBoundsChanged BoundsChanged;

    private Transform parentTran = null;
    public TestCubeQuadObject(Transform t)
    {
        parentTran = t;
    }

    public void DoHidden()
    {
        
    }

    public void DoLOD(int level)
    {
        
    }

    public void DoShow()
    {
        var prefab = Resources.Load<GameObject>("Cube");
        cubeGO = GameObject.Instantiate(prefab);
        cubeGO.transform.SetParent(parentTran, false);
        Vector3 pos = new Vector3(Random.Range(1f, 20f), 0f, Random.Range(1f, 20f));
        cubeGO.transform.localPosition = pos;
        float sacle = Random.Range(0.1f, 1.0f);
        cubeGO.transform.localScale = new Vector3(sacle, sacle, sacle);

        Bounds = new Rect(pos.x, pos.z, sacle, sacle);
    }
}

public class TestQuadTree : MonoBehaviour
{
    private QuadTree quadTree = new QuadTree();
    // Start is called before the first frame update
    void Start()
    {
        quadTree.SetData(7, 3, new Rect(-25f, -25f, 50, 50));
    }
    private void OnGUI()
    {
        if (GUILayout.Button("CLICK"))
        {
            IQuadObject obj = new TestCubeQuadObject(transform.parent);
            obj.DoShow();
            quadTree.InsertObject(obj);
        }

        if(GUILayout.Button("Random 1000"))
        {
            for(int i=0;i<1000;++i)
            {
                IQuadObject obj = new TestCubeQuadObject(transform.parent);
                obj.DoShow();
                quadTree.InsertObject(obj);
            }    
        }    
    }

    private void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-50, 0.1f, 0f), new Vector3(50, 0.1f,0f));
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(0, 0.1f, -50f), new Vector3(0, 0.1f, 50f));

            List<QuadNode> nodes = new List<QuadNode>();
            FindNodes(quadTree.RootNode, nodes);

            Gizmos.color = Color.white;
            foreach (var node in nodes)
            {
                Vector2 center = node.Bounds.center;
                Vector2 size = node.Bounds.size;
                Gizmos.DrawWireCube(new Vector3(center.x, 0.0f, center.y), new Vector3(size.x, 1.0f, size.y));
            }
        }
        

    }

    private void FindNodes(QuadNode node ,List<QuadNode> results)
    {
        results.Add(node);

        if(!node.IsLeaf)
        {
            foreach (var childNode in node.ChildNodes)
            {
                FindNodes(childNode, results);
            }
        }
    }

}
