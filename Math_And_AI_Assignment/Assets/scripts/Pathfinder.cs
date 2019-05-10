using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Pathfinder : MonoBehaviour
{
    public ProceduralGeneration mapManager; //the class that contains the map data
    //public int NPCIndex;

    void A_Star(Vector3 start, Vector3 goal)
    {
        #region Declarations
        GameObject[,] map = mapManager.MakeGridArray(); //a 2d array that represents the map

        PathfinderGrid grid = new PathfinderGrid();
        #endregion

        grid.mapArray = map; //assigning the map reference within the grid
        grid.createPointsArray();

        grid.current = grid.points[(int)start.x, (int)start.z];
        grid.loadOpenSet();

        Point bestNodeFound = new Point();
        bestNodeFound.Xpos = start.x;
        bestNodeFound.Zpos = start.z;
        bool routeFound = false;

        while (!routeFound)
        {
            foreach (Point node in grid.getOpenSet())
            {
                if (dotProduct(node.getPosition(), goal) < dotProduct(bestNodeFound.getPosition(), goal))
                {
                    bestNodeFound = node;
                }
            }
        }
    }

    float dotProduct(Vector3 vec1, Vector3 vec2)
    {
        return (float)Math.Sqrt((vec1 - vec2).magnitude);
    }

    int heuristicCostEstimate(Vector3 start, Vector3 goal)
    {
        return 0;
    }

    //delete a specific element from an array
    void removeFromArray(BasicMeshProperties[] openSet, BasicMeshProperties current)
    {
        // Could use indexOf here instead to be more efficient
        for (int i = openSet.Length - 1; i >= 0; i--)
        {
            if (openSet[i] == current)
            {
                openSet = openSet.Where((source, index) => index != i).ToArray();
            }
        }
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