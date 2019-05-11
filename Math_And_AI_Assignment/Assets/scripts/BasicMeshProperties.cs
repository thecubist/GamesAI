using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeshProperties : MonoBehaviour
{
    [Header("Base properties")]
    public string objectType;
    public Material[] materials = new Material[5];
    MeshRenderer mesh;
    bool checkForCollisions;

    [Header("Pathfinder properties")]
    public int numberOfEnemies;

    private int[] totalPathCost;
    private int[] actualPathCost;
    private int[] heuristicCostEstimate;
    private double[] visualHeuristicPath;
    // Use this for initialization
    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
        totalPathCost = new int[numberOfEnemies];
        actualPathCost = new int[numberOfEnemies];
        heuristicCostEstimate = new int[numberOfEnemies];
        visualHeuristicPath = new double[numberOfEnemies];
    }

    public int getFTotalPathCost(int index)
    {
        return totalPathCost[index];
    }

    public void setFTotalPathCost(int index, int value)
    {
        totalPathCost[index] = value;
    }

    public int getGActualPathCost(int index)
    {
        return actualPathCost[index];
    }

    public void setGActualPathCost(int index, int value)
    {
        actualPathCost[index] = value;
    }

    public int getHHeuristicCostEstimate(int index)
    {
        return heuristicCostEstimate[index];
    }

    public void setHHeuristicCostEstimate(int index, int value)
    {
        heuristicCostEstimate[index] = value;
    }

    public double getVisualHeuristic(int index)
    {
        return visualHeuristicPath[index];
    }

    public Vector3 getPosition()
    {
        return GetComponentInParent<Transform>().position;
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
            mesh.transform.localScale = new Vector3(1f, 2f, 1f);
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
