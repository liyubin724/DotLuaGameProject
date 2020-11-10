using DotEngine.Generic;
using DotEngine.Log;
using DotEngine.Pool;
using DotEngine.World.QT;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestCubeQuadObject : MonoBehaviour,IQuadObject
{
    public int UniqueID { get; set; }

    public AABB2D Bounds { get; set; }

    public float Speed { get; set; } = 5.0f;
    public bool IsBoundsChangeable { get; set; } = false;

    public event System.Action<IQuadObject, AABB2D, AABB2D> OnBoundsChanged;

    void Update()
    {
        if(m_IsMoving)
        {
            Vector3 dir = (m_TargetPosition - transform.position).normalized;
            Vector3 deltaDistance = dir * Speed * Time.deltaTime;

            AABB2D oldBounds = Bounds;
            Vector2 centerPosition = Bounds.Center + new Vector2(deltaDistance.x,deltaDistance.z);
            AABB2D newBounds = new AABB2D(centerPosition, Bounds.Extents);

            transform.position = new Vector3(newBounds.Center.x, 0, newBounds.Center.y);

            Bounds = newBounds;

            OnBoundsChanged.Invoke(this, oldBounds, newBounds);

            if(Vector3.Distance(transform.position,m_TargetPosition)< 0.1f)
            {
                RandomMove();
            }
        }
    }

    private bool m_IsMoving = false;
    private Vector3 m_TargetPosition;

    

    public void StartMoveTo(Vector3 targetPosition)
    {
        m_IsMoving = true;
        m_TargetPosition = targetPosition;
    }

    public void RandomMove()
    {
        float targetPositionX = Random.Range(30, 360);
        float targetPositionY = Random.Range(30, 360);

        Speed = Random.Range(9.0f, 30.0f);

        StartMoveTo(new Vector3(targetPositionX, 0, targetPositionY));
    }

    public void OnEnterView()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void OnExitView()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}

public class TestQuadTree : MonoBehaviour
{
    private QuadTree tree = null;
    private QuadTreeGizmosDrawer drawer = null;

    private UniqueIntID idCreator = new UniqueIntID();
    private void Start()
    {

        LogUtil.SetLogger(new UnityLogger());

        tree = new QuadTree(7, 3, new AABB2D(0, 0, 400, 400));

        drawer = QuadTreeGizmosDrawer.DrawGizmos(tree);
    }

    private TestCubeQuadObject CreateObject()
    {
        float randomCenterX = Random.Range(21, 370);
        float randomCenterY = Random.Range(21, 370);
        float randomExtents = Random.Range(0.3f, 2);


        AABB2D bounds = new AABB2D(new Vector2(randomCenterX, randomCenterY), new Vector2(randomExtents, randomExtents));

        GameObject gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        TestCubeQuadObject qObject = gObj.AddComponent<TestCubeQuadObject>();
        qObject.UniqueID = idCreator.NextID;
        qObject.Bounds = bounds;
        gObj.name = "Cube " + qObject.UniqueID;
        gObj.transform.localScale = new Vector3(bounds.Size.x, 1, bounds.Size.y);
        gObj.transform.position = new Vector3(bounds.Center.x, 0, bounds.Center.y);

        //qObject.OnEnterView();

        return qObject;
    }

    private void OnDrawGizmos()
    {
        if(isMouseDown)
        {
            QuadTreeUtil.DrawGizmoAABBBorder(showBounds, Color.yellow);
        }
    }

    private bool isMouseDown = false;
    private AABB2D showBounds;
    private List<IQuadObject> selectedObjects = new List<IQuadObject>();
    private List<IQuadObject> tempObjects = new List<IQuadObject>();
    private void OnGUI()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        if(GUILayout.Button("CLICK"))
        {
            for(int i =0;i<10;++i)
            {
                TestCubeQuadObject quadObject = CreateObject();
                float value = Random.Range(0.0f, 1.0f);
                if (value > 0.5f)
                {
                    quadObject.IsBoundsChangeable = true;
                    quadObject.RandomMove();
                }
                tree.InsertObject(quadObject);
            }
        }

        if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            isMouseDown = true;
        }
        if(isMouseDown )
        {
            Vector3 wPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            showBounds = new AABB2D(new Vector2(wPosition.x, wPosition.z), new Vector2(50, 50));

            List<IQuadObject> list1 = ListPool<IQuadObject>.Get();
            if(selectedObjects!=null)
            {
                list1.AddRange(selectedObjects);
            }

            tempObjects.Clear();
            tree.QueryIntersectsObjects(showBounds,ref tempObjects);
            List<IQuadObject> list2 = ListPool<IQuadObject>.Get();
            list2.AddRange(tempObjects);

            var dis1 = list1.Except(list2).ToArray();
            foreach(var obj in dis1)
            {
                obj.OnExitView();
            }

            var dis2 = list2.Except(list1).ToArray();
            foreach (var obj in dis2)
            {
                obj.OnEnterView();
            }

            selectedObjects.Clear();
            selectedObjects.AddRange(tempObjects);

            ListPool<IQuadObject>.Release(list1);
            ListPool<IQuadObject>.Release(list2);
        }
        if(isMouseDown && Event.current.type == EventType.MouseUp)
        {
            isMouseDown = false;
        }
        
        if(Input.GetKeyUp(KeyCode.Delete))
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, float.MaxValue))
            {
                var qObject = hitInfo.collider.gameObject.GetComponent<TestCubeQuadObject>();
                if(qObject!=null)
                {
                    tree.RemoveObject(qObject);

                    Destroy(qObject.gameObject);
                }
            }
        }
    }
}
