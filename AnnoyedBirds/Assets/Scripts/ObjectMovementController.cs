using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovementController : MonoBehaviour
{
    public float moveSpeed = 1;
    public GameObject self; 
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown("w"))
        {
            Vector3 pos = transform.position;
            pos.z += moveSpeed;
            transform.position = pos;//.MoveTowards(pos, new Quaternion(0, 0, 0, 0));
        }

        if (Input.GetKeyDown("s"))
        {
            Vector3 pos = transform.position;
            pos.z -= moveSpeed;
            transform.position = pos;//.MoveTowards(pos, new Quaternion(0, 0, 0, 0));
        }

        if (Input.GetKeyDown("a"))
        {
            Vector3 pos = transform.position;
            pos.x -= moveSpeed;
            transform.position = pos;//.MoveTowards(pos, new Quaternion(0, 0, 0, 0));
        }

        if (Input.GetKeyDown("d"))
        {
            Vector3 pos = transform.position;
            pos.x += moveSpeed;
            transform.position = pos;//.MoveTowards(pos, new Quaternion(0, 0, 0, 0));
        }

        if (Input.GetKeyDown("up"))
        {
            Vector3 pos = transform.position;
            pos.y += moveSpeed;
            transform.position = pos;
        }

        if (Input.GetKeyDown("down"))
        {
            Vector3 pos = transform.position;
            pos.y -= moveSpeed;
            transform.position = pos;//.MoveTowards(pos, new Quaternion(0, 0, 0, 0));
        }
    }
}
