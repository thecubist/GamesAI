using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfSpaceTest : MonoBehaviour
{
    public Transform testPoint;
	// Use this for initialization
	void Start ()
    {
		
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
        float result = Vector3.Dot(temp, Normal);

        if (result > 0)
            Debug.Log("in front");
        else if (result < 0)
            Debug.Log("behind");
        else
            Debug.Log("on plane");
	}
}
