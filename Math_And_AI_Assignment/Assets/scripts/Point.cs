using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public struct moveableDirections { public bool left; public bool right; public bool front; public bool back; };

    public int cost_G { get; set; }
    public int cost_H { get; set; }
    public int cost_T { get; set; }
    public Point[] neighbours { get; set; }
    public moveableDirections moveLocations;
    public Point cameFrom { get; set; }
    public float Xpos { get; set; }
    public float Ypos { get; set; }
    public float Zpos { get; set; }
    public bool visited { get; set; }

    // Use this for initialization
    void Start ()
    {
        neighbours = new Point[4];
        moveLocations.front = false;
        moveLocations.back = false;
        moveLocations.left = false;
        moveLocations.right = false;
        visited = false;
    }

    public Vector3 getPosition()
    {
        return new Vector3(Xpos,Ypos,Zpos);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
