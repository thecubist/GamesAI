using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollisionHelper : MonoBehaviour
{
    public enum collisionType// your custom enumeration
    {
        box_to_box = 0,
        box_to_point = 1,
        sphere_to_sphere = 2,
        sphere_to_point = 3
    };
    
    public collisionType collisionTypeSelected = collisionType.box_to_box;

    [Header("Generic Variables")]
    public GameObject Object1;
    public GameObject Object2;

    [Header("Box specific variables")]
    private Vector3 minPoint1;
    private Vector3 maxPoint1;
    private Vector3 minPoint2;
    private Vector3 maxPoint2;
    private struct AABBVecCollided { public bool x; public bool y; public bool z; };
    private AABBVecCollided AABBCheck;

    [Header("Sphere to sphere specific variables")]
    public float sphere1Radius = 1.0f;
    public float sphere2Radius = 1.0f;

    // Use this for initialization
    void Start ()
    {
        if (collisionTypeSelected.Equals(collisionType.box_to_point))
        {
            minPoint1 = GetComponent<BoxCollider>().bounds.min;
            maxPoint1 = GetComponent<BoxCollider>().bounds.max;

            AABBCheck.x = false;
            AABBCheck.y = false;
            AABBCheck.z = false;
        }
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
        //------------------------------------------------ box to point -------------------------------------------------//
        if (collisionTypeSelected.Equals(collisionType.box_to_point))
        {
            Vector3 temp = Object1.transform.position - PointOnPlane;

            //Debug.Log("\n minpoint is: " + minPoint);
            //Debug.Log("\n maxpoint is: " + maxPoint);
            //Debug.Log("\n check point is: " + temp);

            if (temp.x >= minPoint1.x && temp.x <= maxPoint1.x)
            {
                Debug.Log("in line in X");
                AABBCheck.x = true;
            }
            else { AABBCheck.x = false; }

            if (temp.y >= minPoint1.y && temp.y <= maxPoint1.y)
            {
                Debug.Log("in line at Y");
                AABBCheck.y = true;
            }
            else { AABBCheck.y = false; }

            if (temp.z >= minPoint1.z && temp.z <= maxPoint1.z)
            {
                Debug.Log("in line at Z");
                AABBCheck.z = true;
            }
            else { AABBCheck.z = false; }

            if (AABBCheck.x && AABBCheck.y && AABBCheck.z)
            {
                Debug.Log("Colliding");
            }
        }
        //------------------------------------------------ sphere to point -------------------------------------------------//
        else if (collisionTypeSelected.Equals(collisionType.sphere_to_point))
        {
            Vector3 sphereCentre = Object1.transform.position;
            Vector3 point = Object2.transform.position;
            double distance = pythag(sphereCentre, point);

            Debug.Log(sphereCentre);
            Debug.Log(point);

            if (distance < sphere1Radius)
            {
                Object1.GetComponent<Renderer>().material.color = Color.red;
                Debug.Log("Sphere to point Colliding");
            }
            else { Object1.GetComponent<Renderer>().material.color = Color.white; }
        }
        //------------------------------------------------ sphere to sphere -------------------------------------------------//
        else if (collisionTypeSelected.Equals(collisionType.sphere_to_sphere))
        {
            Vector3 sphereCentre1 = Object1.transform.position;
            Vector3 sphereCentre2 = Object2.transform.position;
            double distance = pythag(sphereCentre1, sphereCentre2);

            if (distance < (sphere1Radius + sphere2Radius))
            {
                Object1.GetComponent<Renderer>().material.color = Color.red;
                Debug.Log("Sphere to sphere Colliding");
            }
            else { Object1.GetComponent<Renderer>().material.color = Color.white; }
        }

        /*float result = Vector3.Dot(temp, Normal);

if (result > 0)
    Debug.Log("in front");
else if (result < 0)
    Debug.Log("behind");
else
    Debug.Log("on plane");*/
    }

    double pythag(Vector3 vector1, Vector3 vector2)
    {
        /*
		Theory for collision detection was based off of the following extract from
		http://www.bbc.co.uk/schools/gcsebitesize/maths/geometry/pythagoras3drev1.shtml
		First use Pythagoras' theorem in triangle ABC to find length AC.
		AC2 = 62 + 22
		AC = sqrt -> 40
		You do not need to find the root as we will need to square it in the following step. Next we use Pythagoras' theorem in triangle ACF to find length AF.
		AF2 = AC2 + CF2
		AF2 = 40 + 32
		AF = sqrt -> 49
		AF = 7
		*/

        double distance, acs, afs, x, y, z;

        x = vector1.x - vector2.x;
        y = vector1.y - vector2.y;
        z = vector1.z - vector2.z;

        acs = (x * x) + (z * z);
        afs = (acs + (y * y));
        distance = Math.Sqrt(afs);

        return distance;
    }
}
