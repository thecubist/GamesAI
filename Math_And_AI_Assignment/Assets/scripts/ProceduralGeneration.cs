using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public bool simpleInstance = true;
    public Vector3 tileCount = new Vector3(1, 1, 1);
    public Vector3 tilePosMult = new Vector3(0,0,0);
    public GameObject mesh;

    private static bool DEVMODE = true;
	void Start()
    {
        regenerateMap();

        clusterPass();
        Debug.Log("pass1 complete");
        clusterPass();
        Debug.Log("pass2 complete");
    }


    void regenerateMap()
    {
        Vector3 instancePos; //used for defining where the next mesh is generated
        float tempY;
        int randomNumberHolder = 0;

        if (tileCount.x < 1 || tileCount.y < 1 || tileCount.z < 1)
        {
            Debug.LogWarning("WARNING: tileCounts were setup below usable values");
            tileCount = new Vector3(1, 1, 1);
        }

        if (tilePosMult.x < 1 || tilePosMult.y < 1 || tilePosMult.z < 1)
        {
            Debug.LogWarning("WARNING: tileOffset were setup below usable values");
            tilePosMult = new Vector3(1, 1, 1);
        }

        //int meshCount = (int)tileCount.x * (int)tileCount.y * (int)tileCount.z;

        //generate the tiles
        for (int i = 0; i < tileCount.x; i++)
        {
            for (int j = 0; j < tileCount.y; j++)
            {
                for (int k = 0; k < tileCount.z; k++)
                {
                    if (simpleInstance)
                    {
                        tempY = Mathf.PerlinNoise(i, k);
                        //instancePos = new Vector3(i * tilePosMult.x, j * tilePosMult.y, k * tilePosMult.z);

                        instancePos = new Vector3(i * tilePosMult.x, (j + 1) * tempY, k * tilePosMult.z);
                        Instantiate(mesh, instancePos, Quaternion.identity);
                        //Debug.Log(instancePos);
                    }
                    else
                    {
                        float yPosition = j * tilePosMult.y;
                        randomNumberHolder = Random.Range(0, 8);

                        //if (randomNumberHolder == 0)
                        //{
                        //    yPosition = yPosition + 1;
                        //}

                        instancePos = new Vector3(i * tilePosMult.x, yPosition, k * tilePosMult.z);

                        Instantiate(mesh, instancePos, Quaternion.identity);
                        
                        mesh.GetComponent<BasicMeshProperties>().changeType(randomNumberHolder);

                    }
                }
            }
        }
    }

    void clusterPass()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("ProceduralTile");
        GameObject[,] grid = new GameObject[100,100];
        int wallCount = 0;
        //int loopCount = 0;
        //Vector2 dataPackingLocation = new Vector2(0, 0);
        foreach (GameObject tile in tiles)
        {
            grid[(int)tile.GetComponent<Transform>().localPosition.x, (int)tile.GetComponent<Transform>().localPosition.z] = tile;
        }

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                wallCount = 0;

                try
                {
                    if (grid[i, j + 1].GetComponent<BasicMeshProperties>().objectType.Equals("wall"))
                        wallCount++;
                }
                catch (System.Exception e) { }


                try
                {
                    if (grid[i, j - 1].GetComponent<BasicMeshProperties>().objectType.Equals("wall"))
                        wallCount++;
                }
                catch (System.Exception e) { }

                try
                {
                    if (grid[i + 1, j].GetComponent<BasicMeshProperties>().objectType.Equals("wall"))
                        wallCount++;
                }
                catch (System.Exception e) { }

                try
                {
                    if (grid[i - 1, j].GetComponent<BasicMeshProperties>().objectType.Equals("wall"))
                        wallCount++;
                }
                catch (System.Exception e) { }

                if (wallCount > 1)
                {
                    grid[i, j].GetComponent<BasicMeshProperties>().objectType = "wall";
                    grid[i, j].GetComponent<MeshRenderer>().material.color = Color.green;
                    grid[i, j].GetComponent<Transform>().position = new Vector3(grid[i, j].GetComponent<Transform>().position.x, 1, grid[i, j].GetComponent<Transform>().position.z);
                }
                else if(wallCount == 0)
                {
                    grid[i, j].GetComponent<BasicMeshProperties>().objectType = "floor";
                    grid[i, j].GetComponent<MeshRenderer>().material.color = Color.white;
                    grid[i, j].GetComponent<Transform>().position = new Vector3(grid[i, j].GetComponent<Transform>().position.x, 0, grid[i, j].GetComponent<Transform>().position.z);
                }
            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (DEVMODE)
        {
            if (Input.GetKeyDown("]"))
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag("ProceduralTile");

                foreach (GameObject item in objects)
                {
                    Destroy(item);
                }

                regenerateMap();
            }

            if (Input.GetKeyDown("["))
            {
                clusterPass();
                Debug.Log("pass complete");
            }
        }
	}
}
