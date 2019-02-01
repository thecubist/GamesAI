using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB : MonoBehaviour
{
    public Transform testPoint;
    private Vector3 minPoint, maxPoint;
	
    // Use this for initialization
	void Start ()
    {
        minPoint = GetComponent<BoxCollider>().bounds.min;
        maxPoint = GetComponent<BoxCollider>().bounds.max;

    }
	
    //returns the 
    Vector3 Normal
    {
        get
        {
            return transform.up;
        }
    }

    //gets the current position of the object the script is attached to
    Vector3 PointOnPlane
    {
        get
        {
            return transform.position; 
        }
    }
	// Update is called once per frame
	void Update ()
    {
        Vector3 temp = testPoint.position - PointOnPlane;

        Debug.Log("\n minpoint is: " + minPoint);
        Debug.Log("\n maxpoint is: " + maxPoint);
        Debug.Log("\n check point is: " + temp);

        if (temp.x >= minPoint.x && temp.x <= maxPoint.x)
        {
            Debug.Log("in line in X");
            if (temp.y >= minPoint.y && temp.y <= maxPoint.y)
            {
                Debug.Log("in line at Y");
                if (temp.z >= minPoint.z && temp.z <= maxPoint.z)
                {
                    Debug.Log("in line at Z");
                }
            }
        }
        /*float result = Vector3.Dot(temp, Normal);

        if (result > 0)
            Debug.Log("in front");
        else if (result < 0)
            Debug.Log("behind");
        else
            Debug.Log("on plane");*/
    }
}
