using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderGrid : MonoBehaviour
{
    public GameObject[,] mapArray { get; set; }

    private List<Point> openSet;
    private List<Point> closedSet;
    private Point current; 

    Point[,] points;
    // Use this for initialization
    void Start ()
    {
		
	}

    void createPointsArray()
    {
        if (mapArray != null) //ensuring that the array to be used has been assigned
        {
            points = new Point[mapArray.GetLength(0),mapArray.GetLength(1)];

            //assigning each of the points their positions
            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    points[i, j].Xpos = mapArray[i, j].transform.position.x;
                    points[i, j].Ypos = mapArray[i, j].transform.position.y;
                    points[i, j].Zpos = mapArray[i, j].transform.position.z;
                }
            }

            for (int x = 0; x < points.GetLength(0); x++)
            {
                for (int z = 0; z < points.GetLength(1); z++)
                {
                    #region Checking for neighbours
                    try //left neighbour
                    {
                        if (mapArray[x - 1, z].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        {
                            points[x, z].neighbours[0] = points[x - 1, z];
                        }
                    }
                    catch (System.Exception e) { }

                    try //right neighbour
                    {
                        if (mapArray[x + 1, z].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        {
                            points[x, z].neighbours[1] = points[x + 1, z];
                        }
                    }
                    catch (System.Exception e) { }

                    try //front neighbour
                    {
                        if (mapArray[x, z + 1].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        {
                            points[x, z].neighbours[2] = points[x, z+1];
                        }
                    }
                    catch (System.Exception e) { }

                    try //rear neighbour
                    {
                        if (mapArray[x, z - 1].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        {
                            points[x, z].neighbours[3] = points[x, z-1];
                        }
                    }
                    catch (System.Exception e) { }
                    #endregion
                }
            }
        }
        else
        {
            Debug.LogError("ERROR: mapArray was not initialized");
            return;
        }
    }
	// Update is called once per frame
	void Update ()
    {
		
	}
}
