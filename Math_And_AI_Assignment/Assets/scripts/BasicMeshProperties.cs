using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeshProperties : MonoBehaviour
{
    public string objectType;
    public Material[] materials = new Material[5];
    MeshRenderer mesh;
    bool checkForCollisions; 

    // Use this for initialization
    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (checkForCollisions)
        {
            boxToPointCollision();
        }

        if (objectType.Equals("floor"))
        {

        }
        else if (objectType.Equals("wall"))
        {

        }
        else if (objectType.Equals("regenPoint"))
        {

        }
        else if (objectType.Equals("ammoPoint"))
        {

        }
        else if (objectType.Equals("controlPoint"))
        {

        }
    }

    bool boxToPointCollision()
    {
        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("BulletPlayer");
        GameObject[] npcBullets = GameObject.FindGameObjectsWithTag("BulletPlayer");

        foreach (GameObject bullet in playerBullets)
        {
            //if(bullet.transform.position.x )
        }

        return false;
    }
    public void changeType(int type)
    {
        if (mesh == null)
        {
            Start();
        }

        if (type == 0)
        {
            mesh.material = materials[1];
            objectType = "wall";

        }
        else
        {
            mesh.material = materials[0];
            objectType = "floor";

        }
    }

    public void setType(string type)
    {
        if (mesh == null)
        {
            Start();
        }

        if (type.Equals("floor"))
        {
            mesh.material = materials[0];
            mesh.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (type.Equals("wall"))
        {
            mesh.material = materials[1];
            mesh.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (type.Equals("regenPoint"))
        {
            mesh.material = materials[2];
            mesh.transform.localScale = new Vector3(1f, 100f, 1f);
        }
        else if (type.Equals("ammoPoint"))
        {
            mesh.material = materials[3];
            mesh.transform.localScale = new Vector3(1f, 100f, 1f);
        }
        else if (type.Equals("controlPoint"))
        {
            mesh.material = materials[4];
            mesh.transform.localScale = new Vector3(1f, 100f, 1f);
        }
        else if (type.Equals("borderWall"))
        {
            mesh.material = materials[5];
            mesh.transform.localScale = new Vector3(1f, 2f, 1f);
        }
        else
        {
            Debug.LogError("ERROR: unknown object type assigned");
        }

        objectType = type;
    }
}
