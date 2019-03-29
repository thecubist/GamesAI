using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public bool simpleInstance = true;
    public Vector3 tileCount = new Vector3(1, 1, 1);
    public Vector3 tilePosMult = new Vector3(0, 0, 0);
    public GameObject[] tileMeshes = new GameObject[1];
    public Vector2 perlinMultipliers = new Vector2(0.005f, 0.005f);
    private bool generateTileState = false;
    void Start()
    {
        generateTiles();
    }

    void generateTiles()
    {
        Vector3 instancePos; //used for defining where the next mesh is generated

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
        int meshID = 0;

        //generate the tiles
        for (int i = 0; i < tileCount.x; i++)
        {
            for (int j = 0; j < tileCount.y; j++)
            {
                for (int k = 0; k < tileCount.z; k++)
                {
                    if (simpleInstance)
                    {
                        instancePos = new Vector3(i * tilePosMult.x, j * tilePosMult.y, k * tilePosMult.z);
                        Instantiate(tileMeshes[0], instancePos, Quaternion.identity);
                    }
                    else
                    {
                        //meshID = Random.Range(0, tileMeshes.Length);


                        if (Mathf.PerlinNoise(Random.Range(0, i) * perlinMultipliers.x, Random.Range(0, j) * perlinMultipliers.y) > 0.5)
                            meshID = 1;
                        else
                            meshID = 0;

                        //Debug.Log("mesh is " + meshID);
                        //Debug.Log("perlin is " + Mathf.PerlinNoise(i * 0.005f, j * 0.005f));
                        instancePos = new Vector3(i * tilePosMult.x, j * tilePosMult.y, k * tilePosMult.z);
                        Instantiate(tileMeshes[meshID], instancePos, Quaternion.identity);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject[] allObjects = GameObject.FindGameObjectsWithTag("ProceduralGenerationTile");
            foreach (GameObject obj in allObjects)
            {
                Destroy(obj);
            }

            generateTiles();
        }
    }
}
