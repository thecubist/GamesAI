using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public bool simpleInstance = true;
    public Vector3 tileCount = new Vector3(1, 1, 1);
    public Vector3 tilePosMult = new Vector3(0,0,0);
    public GameObject mesh;
    public int clusterPassIterations = 2;
    public GameObject player;
    public bool DEVMODE = true;

    public Material floorMaterial;
    public Material wallMaterial;

	void Awake()
    {
        RegenerateMap();
        for (int i = 0; i < clusterPassIterations; i++)
        {
            clusterPass();
        }
        MakeBoundingWalls();
    }

    void Wait(int seconds)
    {
        System.DateTime unpauseTime = System.DateTime.Now.AddSeconds(seconds);
        while (System.DateTime.Now < unpauseTime)
        {
            Debug.Log("waiting");
        }
    }

    void RegenerateMap()
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

                        instancePos = new Vector3(i * tilePosMult.x, (j + 1) * tempY, k * tilePosMult.z);
                        Instantiate(mesh, instancePos, Quaternion.identity);
                    }
                    else
                    {
                        float yPosition = j * tilePosMult.y;
                        randomNumberHolder = Random.Range(0, 8);

                        instancePos = new Vector3(i * tilePosMult.x, yPosition, k * tilePosMult.z);

                        Instantiate(mesh, instancePos, Quaternion.identity);
                        
                        mesh.GetComponent<BasicMeshProperties>().changeType(randomNumberHolder);

                    }
                }
            }
        }
    }

    GameObject[,] MakeGridArray()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("ProceduralTile");
        GameObject[,] grid = new GameObject[(int)tileCount.x, (int)tileCount.z];

        foreach (GameObject tile in tiles)
        {
            grid[(int)tile.GetComponent<Transform>().localPosition.x, (int)tile.GetComponent<Transform>().localPosition.z] = tile;
        }

        return grid;
    }

    void MakeBoundingWalls()
    {
        GameObject[,] grid = MakeGridArray();

        for (int i = 0; i < tileCount.x; i++)
        {
            for (int j = 0; j < tileCount.z; j++)
            {
                if (i == 0 || i == (tileCount.x - 1) || j == 0 || j == (tileCount.z - 1))
                {
                    grid[i, j].GetComponent<BasicMeshProperties>().objectType = "wall";
                    grid[i, j].GetComponent<MeshRenderer>().material = wallMaterial;
                    grid[i, j].GetComponent<Transform>().position = new Vector3(grid[i, j].GetComponent<Transform>().position.x, 1, grid[i, j].GetComponent<Transform>().position.z);
                }
            }
        }
    }
    void clusterPass()
    {
        GameObject[,] grid = MakeGridArray();
        int wallCount = 0;

        for (int i = 0; i < tileCount.x; i++)
        {
            for (int j = 0; j < tileCount.z; j++)
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
                    grid[i, j].GetComponent<MeshRenderer>().material = wallMaterial;
                    grid[i, j].GetComponent<Transform>().position = new Vector3(grid[i, j].GetComponent<Transform>().position.x, 1, grid[i, j].GetComponent<Transform>().position.z);
                }
                else if (wallCount == 0)
                {
                    grid[i, j].GetComponent<BasicMeshProperties>().objectType = "floor";
                    grid[i, j].GetComponent<MeshRenderer>().material = floorMaterial;
                    grid[i, j].GetComponent<Transform>().position = new Vector3(grid[i, j].GetComponent<Transform>().position.x, 0, grid[i, j].GetComponent<Transform>().position.z);
                }
            }
        }
    }

    void deleteMap()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ProceduralTile");

        foreach (GameObject item in objects)
        {
            Destroy(item);
        }
    }

    void spawnPlayer()
    {
        GameObject[,] grid = MakeGridArray();
        int floorCount = 0;

        for (int i = 0; i < tileCount.x; i++)
        {
            for (int j = 0; j < tileCount.z; j++)
            {
                floorCount = 0;

                try
                {
                    if (grid[i, j + 1].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        floorCount++;
                }
                catch (System.Exception e) { }


                try
                {
                    if (grid[i, j - 1].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        floorCount++;
                }
                catch (System.Exception e) { }

                try
                {
                    if (grid[i + 1, j].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        floorCount++;
                }
                catch (System.Exception e) { }

                try
                {
                    if (grid[i - 1, j].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        floorCount++;
                }
                catch (System.Exception e) { }

                if (floorCount == 4)
                {
                    Instantiate(player, new Vector3(i,1.6f,j), Quaternion.identity);
                    return;
                }
            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (DEVMODE)
        {
            if (Input.GetKeyDown("[1]"))
            {
                deleteMap();
                RegenerateMap();
                Debug.Log("Regenerated map");
            }

            if (Input.GetKeyDown("[2]"))
            {
                clusterPass();
                Debug.Log("pass complete");
            }

            if (Input.GetKeyDown("[3]"))
            {
                MakeBoundingWalls();
                Debug.Log("walls generated");
            }

            if (Input.GetKeyDown("[5]"))
            {
                spawnPlayer();
            }
        }
	}
}
