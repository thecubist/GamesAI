using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderGrid : MonoBehaviour
{
    public GameObject[,] mapArray { get; set; }

    private List<Point> openSet;
    private List<Point> closedSet;

    public  Point current; 
    public Point[,] points;

    // Use this for initialization
    void Start ()
    {
		
	}

    public void loadOpenSet()
    {
        openSet.Clear(); //clearing the open set

        if (current.moveLocations.front)
            openSet.Add(current.neighbours[0]);

        if (current.moveLocations.back)
            openSet.Add(current.neighbours[1]);

        if (current.moveLocations.left)
            openSet.Add(current.neighbours[2]);

        if (current.moveLocations.right)
            openSet.Add(current.neighbours[3]);
    }

    public List<Point> getOpenSet()
    {
        return openSet;
    }

    public void createPointsArray()
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
                    try //front neighbour
                    {
                        if (mapArray[x, z + 1].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        {
                            points[x, z].neighbours[0] = points[x, z + 1];
                            points[x, z].moveLocations.front = true;
                        }
                    }
                    catch (System.Exception e) { }

                    try //rear neighbour
                    {
                        if (mapArray[x, z - 1].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        {
                            points[x, z].neighbours[1] = points[x, z - 1];
                            points[x, z].moveLocations.back = true;
                        }
                    }
                    catch (System.Exception e) { }


                    try //left neighbour
                    {
                        if (mapArray[x - 1, z].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        {
                            points[x, z].neighbours[2] = points[x - 1, z];
                            points[x, z].moveLocations.left = true;
                        }
                    }
                    catch (System.Exception e) { }

                    try //right neighbour
                    {
                        if (mapArray[x + 1, z].GetComponent<BasicMeshProperties>().objectType.Equals("floor"))
                        {
                            points[x, z].neighbours[3] = points[x + 1, z];
                            points[x, z].moveLocations.right = true;
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

    public void A_Star(Vector3 start, Vector3 goal)
    {
        createPointsArray();

    }
}

//OPEN //the set of nodes to be evaluated
//CLOSED //the set of nodes already evaluated
//add the start node to OPEN


//loop
//        current = node in OPEN with the lowest f_cost
//        remove current from OPEN
//        add current to CLOSED

//        if current is the target node //path has been found
//                return

//        foreach neighbour of the current node
//                if neighbour is not traversable or neighbour is in CLOSED
//                        skip to the next neighbour

//                if new path to neighbour is shorter OR neighbour is not in OPEN
//                        set f_cost of neighbour
//                        set parent of neighbour to current
//                        if neighbour is not in OPEN
//                                add neighbour to OPEN
//https://github.com/SebLague/Pathfinding/blob/master/Episode%2001%20-%20pseudocode/Pseudocode
//https://www.youtube.com/watch?v=nhiFx28e7JY&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=2
