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
    public bool collisionCheckOverride = false;
	void Awake()
    {
        RegenerateMap();
        for (int i = 0; i < clusterPassIterations; i++)
        {
            clusterPass(false);
        }
        MakeBoundingWalls();
        spawnPlayer();

        if (collisionCheckOverride)
        {
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("ProceduralTile");

            foreach (var tile in tiles)
            {
                tile.GetComponent<BasicMeshProperties>().collisionCheckOverride = true;
            }
        }
    }

    void RegenerateMap()
    {
        Vector3 instancePos; //used for defining where the next mesh is generated
        float tempY;
        int randomNumberHolder = 0;

        if (tileCount.x < 1 || tileCount.y < 1 || tileCount.z < 1) //if there is a zero then set the tilecount to defaults
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
                    if (simpleInstance) //if this is used then no fancy code is used. it simply re-instances an object
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

                        Instantiate(mesh, new Vector3(i,j,k), Quaternion.identity);
                        
                        mesh.GetComponent<BasicMeshProperties>().changeType(randomNumberHolder);
                        //note in this state all materials are re instanced and cause performance issues
                    }
                }
            }
        }
    }

    /**
     * get an array of all tile objects and pack them into a 2D array
     * and return it back 
     */
    public GameObject[,] MakeGridArray()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("ProceduralTile");
        GameObject[,] grid = new GameObject[(int)tileCount.x, (int)tileCount.z];

        foreach (GameObject tile in tiles)
        {
            grid[(int)tile.GetComponent<Transform>().localPosition.x, (int)tile.GetComponent<Transform>().localPosition.z] = tile;
        }

        return grid;
    }

    /**
     * go through the tiles and if it is at the the edge
     * of the grid then make it a wall
     */
    void MakeBoundingWalls()
    {
        GameObject[,] grid = MakeGridArray();

        for (int i = 0; i < tileCount.x; i++)
        {
            for (int j = 0; j < tileCount.z; j++)
            {
                if (i == 0 || i == (tileCount.x - 1) || j == 0 || j == (tileCount.z - 1))
                {
                    grid[i, j].GetComponent<BasicMeshProperties>().setType("borderWall");
                    grid[i, j].GetComponent<Transform>().position = new Vector3(grid[i, j].GetComponent<Transform>().position.x, 1, grid[i, j].GetComponent<Transform>().position.z);
                }
            }
        }
    }
    void clusterPass(bool cleanupPass)
    {
        GameObject[,] grid = MakeGridArray();
        int wallCount = 0;

        for (int i = 0; i < tileCount.x; i++)
        {
            for (int j = 0; j < tileCount.z; j++)
            {
                wallCount = 0;

                #region Checking adjacent walls
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
                #endregion

                if (wallCount > 1 && !cleanupPass) //setting type to wall if there are 2 or more adjacent walls to expand out wall clusters
                {
                    grid[i, j].GetComponent<BasicMeshProperties>().setType("wall");
                    grid[i, j].GetComponent<Transform>().position = new Vector3(grid[i, j].GetComponent<Transform>().position.x, 1, grid[i, j].GetComponent<Transform>().position.z);
                }
                else if (wallCount == 0 && !cleanupPass)
                {
                    grid[i, j].GetComponent<BasicMeshProperties>().setType("floor");
                    grid[i, j].GetComponent<Transform>().position = new Vector3(grid[i, j].GetComponent<Transform>().position.x, 0, grid[i, j].GetComponent<Transform>().position.z);
                }
                //if cleanup pass is true then find small clusters of red and make them floors to clean up floorspace
                else if (cleanupPass && (grid[i, j].GetComponent<MeshRenderer>().material.color == Color.red && wallCount < 2) ) 
                {
                    Debug.Log("hit");
                    grid[i, j].GetComponent<BasicMeshProperties>().setType("floor");
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
                clusterPass(false);
                Debug.Log("pass complete");
            }

            if (Input.GetKeyDown("[3]"))
            {
                clusterPass(true);
            }

            if (Input.GetKeyDown("[4]"))
            {
                MakeBoundingWalls();
                Debug.Log("walls generated");
            }

            if (Input.GetKeyDown("[5]"))
            {
                spawnPlayer();
            }

            if (Input.GetKeyDown("[7]"))
            {
                GameObject[] tiles = GameObject.FindGameObjectsWithTag("ProceduralTile");

                if (collisionCheckOverride)
                {
                    foreach (var tile in tiles)
                    {
                        tile.GetComponent<BasicMeshProperties>().collisionCheckOverride = true;
                    }
                }
                else
                {
                    foreach (var tile in tiles)
                    {
                        tile.GetComponent<BasicMeshProperties>().collisionCheckOverride = false;
                    }
                }
            }
        }
	}
}
