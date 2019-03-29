using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public bool simpleInstance = true;
    public Vector3 tileCount = new Vector3(1, 1, 1);
    public Vector3 tilePosMult = new Vector3(0,0,0);
    public GameObject mesh;
    public GameObject[] tileMeshes = new GameObject[1]; 

	void Start()
    {
        Vector3 instancePos; //used for defining where the next mesh is generated
        float tempY;

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

                        instancePos = new Vector3(i * tilePosMult.x, j * tilePosMult.y, k * tilePosMult.z);
                        Instantiate(mesh, instancePos, Quaternion.identity);
                        Debug.Log(instancePos);
                    }
                    else
                    {

                    }
                }
            }
        }
	}


	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
