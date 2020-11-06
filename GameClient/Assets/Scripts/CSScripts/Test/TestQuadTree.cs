using DotEngine;
using DotEngine.Generic;
using DotEngine.Log;
using DotEngine.World.QT;
using UnityEngine;

public class TestCubeQuadObject : MonoBehaviour,IQuadObject
{
    public int UniqueID { get; set; }

    public AABB2D Bounds { get; set; }

    public float Speed { get; set; } = 5.0f;
 
    public event System.Action<QuadObjectBoundChangeEventArgs> OnBoundsChanged;

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

            OnBoundsChanged.Invoke(new QuadObjectBoundChangeEventArgs(this, oldBounds, newBounds));

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
        GetComponent<MeshRenderer>().material.color = Color.blue;
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
        float randomExtentsX = Random.Range(0.1f, 1);
        float randomExtentsY = Random.Range(0.1f, 1);


        AABB2D bounds = new AABB2D(new Vector2(randomCenterX, randomCenterY), new Vector2(randomExtentsX, randomExtentsY));

        GameObject gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        TestCubeQuadObject qObject = gObj.AddComponent<TestCubeQuadObject>();
        qObject.UniqueID = idCreator.NextID;
        qObject.Bounds = bounds;
        gObj.name = "Cube " + qObject.UniqueID;
        gObj.transform.localScale = new Vector3(bounds.Size.x, 1, bounds.Size.y);
        gObj.transform.position = new Vector3(bounds.Center.x, 0, bounds.Center.y);

        qObject.OnEnterView();

        return qObject;
    }

    private void OnGUI()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        if(GUILayout.Button("CLICK"))
        {
            for(int i =0;i<40;++i)
            {
                TestCubeQuadObject quadObject = CreateObject();
                tree.InsertObject(quadObject);

                float value = Random.Range(0.0f, 1.0f);
                if (value > 0.5f)
                {
                    quadObject.RandomMove();
                }
            }

        }
        if(Input.GetMouseButtonUp(0))
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
