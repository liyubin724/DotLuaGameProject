using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGeometryUtility : MonoBehaviour
{
    void Start()
    {
        Bounds bounds = new Bounds(Vector3.zero, new Vector3(1,0,1));

        Bounds tBounds = new Bounds(Vector3.zero, new Vector3(0.5f, 0, 0.5f));

        if(bounds.Intersects(tBounds))
        {
            Debug.Log("SSSSSSSSSSSSSSSSS");
        }

        // Calculate the planes from the main camera's view frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Create a "Plane" GameObject aligned to each of the calculated planes
        for (int i = 0; i < 6; ++i)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            p.name = "Plane " + i.ToString();
            p.transform.position = -planes[i].normal * planes[i].distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
        }
    }
}
