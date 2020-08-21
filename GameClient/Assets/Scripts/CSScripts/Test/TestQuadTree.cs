using DotEngine.World.QT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TestCubeQuadObject : IQuadObject
{
    public Rect Bounds { get; private set; }
    private GameObject cubeGO;

    public event BoundsChanged BoundsChangedHandler;

    private Transform parentTran = null;
    public TestCubeQuadObject(Transform t)
    {
        parentTran = t;
    }

    public void DoHidden()
    {
        if(cubeGO!=null)
        {
            GameObject.Destroy(cubeGO);
        }
    }

    public void DoLOD(int level)
    {

    }

    public void DoShow()
    {
        var prefab = Resources.Load<GameObject>("Cube");
        cubeGO = GameObject.Instantiate(prefab);
        cubeGO.transform.SetParent(parentTran, false);
        Vector3 pos = new Vector3(Random.Range(-25f, 25f), 0f, Random.Range(-25f, 25f));
        cubeGO.transform.localPosition = pos;
        float sacle = Random.Range(0.1f, 0.2f);
        cubeGO.transform.localScale = new Vector3(sacle, sacle, sacle);

        Bounds = new Rect(pos.x, pos.z, sacle, sacle);
    }
}

public class TestQuadTree : MonoBehaviour
{
    private QuadTree quadTree = new QuadTree();
    private List<TestCubeQuadObject> objs = new List<TestCubeQuadObject>();
    
    void Start()
    {
        Application.targetFrameRate = 30;
        quadTree.SetData(10, 3, new Rect(-30f, -30f, 60, 60));
    }
    private void OnGUI()
    {
        if (GUILayout.Button("CLICK"))
        {
            TestCubeQuadObject obj = new TestCubeQuadObject(transform.parent);
            obj.DoShow();
            Profiler.BeginSample("QuadTree.InsertObject");
            quadTree.InsertObject(obj);
            Profiler.EndSample();
        }

        if (GUILayout.Button("Random 1000"))
        {
            for (int i = 0; i < 1000; ++i)
            {
                TestCubeQuadObject obj = new TestCubeQuadObject(transform.parent);
                obj.DoShow();
                quadTree.InsertObject(obj);

                objs.Add(obj);
            }
        }

        if(GUILayout.Button("Delete "))
        {
            int rCount = Random.Range(10, 100);
            for(int i =0;i<rCount;i++)
            {
                if(objs.Count == 0)
                {
                    break;
                }
                int rIndex = Random.Range(0, objs.Count - 1);
                TestCubeQuadObject obj = objs[rIndex];
                objs.RemoveAt(rIndex);
                quadTree.RemoveObject(obj);
                obj.DoHidden();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-50, 0.1f, 0f), new Vector3(50, 0.1f, 0f));
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(0, 0.1f, -50f), new Vector3(0, 0.1f, 50f));

            List<QuadNode> nodes = new List<QuadNode>(quadTree.GetNodes());
            Gizmos.color = Color.white;
            foreach (var node in nodes)
            {
                Vector2 center = node.Bounds.center;
                Vector2 size = node.Bounds.size;
                Gizmos.DrawWireCube(new Vector3(center.x, 0.0f, center.y), new Vector3(size.x, 1.0f, size.y));
            }
        }
    }
}
