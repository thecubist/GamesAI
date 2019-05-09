using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public int cost_G { get; set; }
    public int cost_H { get; set; }
    public int cost_T { get; set; }
    public Point[] neighbours { get; set; }
    public Point cameFrom { get; set; }
    public float Xpos { get; set; }
    public float Ypos { get; set; }
    public float Zpos { get; set; }


    // Use this for initialization
    void Start ()
    {
        neighbours = new Point[4];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
