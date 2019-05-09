using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Pathfinder : MonoBehaviour
{
    public ProceduralGeneration mapManager; //the class that contains the map data
    public int NPCIndex;
    void A_Star(Vector3 start, Vector3 goal)
    {
        #region Declarations
        GameObject[,] map = mapManager.MakeGridArray(); //a 2d array that represents the map
        Vector3 lastCheckedNode;
        BasicMeshProperties[] openSet = new BasicMeshProperties[8]; //nodes that are currently being evaluated

        ArrayList closedSet;
        //BasicMeshProperties[] closedSet; //nodes that have already been checked 
        BasicMeshProperties[] cameFrom;
        #endregion

        openSet[0] = map[(int)start.x, (int)start.z].GetComponent<BasicMeshProperties>(); //setting the openSet to start as it is still currently known
        openSet[0].setGActualPathCost(NPCIndex, 0); //setting the start node to 0
        openSet[0].setHHeuristicCostEstimate(NPCIndex, heuristicCostEstimate(start, goal)); //for first node only heuristic value can be given

        while (openSet != null || openSet.Length > 0) //if array is not empty
        {
            int score = 10000;
            int currentLowestCostOpenSetIndex = 0;

            for (int i = 0; i < openSet.Length; i++) //find lowest cost
            {
                if (openSet[i].getFTotalPathCost(NPCIndex) < score)
                {
                    currentLowestCostOpenSetIndex = i;
                }
            }

            if (openSet[currentLowestCostOpenSetIndex].getPosition() == goal)
            {
                Debug.Log("Path found");
                return;
            }
            else
            {
                
                removeFromArray(openSet, openSet[currentLowestCostOpenSetIndex]);
                
            }
        }
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

    /*function A_Star(start, goal)
    // The set of nodes already evaluated
    closedSet := {}

    // The set of currently discovered nodes that are not evaluated yet.
    // Initially, only the start node is known.
    openSet := {start
}

// For each node, which node it can most efficiently be reached from.
// If a node can be reached from many nodes, cameFrom will eventually contain the
// most efficient previous step.
cameFrom := an empty map

// For each node, the cost of getting from the start node to that node.
gScore := map with default value of Infinity

// The cost of going from start to start is zero.
gScore[start] := 0

    // For each node, the total cost of getting from the start node to the goal
    // by passing by that node. That value is partly known, partly heuristic.
    fScore := map with default value of Infinity

    // For the first node, that value is completely heuristic.
    fScore[start] := heuristic_cost_estimate(start, goal)

    while openSet is not empty
        current := the node in openSet having the lowest fScore[] value
        if current = goal
            return reconstruct_path(cameFrom, current)

        openSet.Remove(current)
        closedSet.Add(current)

        for each neighbor of current
            if neighbor in closedSet
                continue		// Ignore the neighbor which is already evaluated.

            // The distance from start to a neighbor
            tentative_gScore := gScore[current] + dist_between(current, neighbor)

            if neighbor not in openSet	// Discover a new node
                openSet.Add(neighbor)
            else if tentative_gScore >= gScore[neighbor]
                continue

            // This path is the best until now. Record it!
            cameFrom[neighbor] := current
            gScore[neighbor] := tentative_gScore
            fScore[neighbor] := gScore[neighbor] + heuristic_cost_estimate(neighbor, goal)*/
}












    /*public Vector3 start;
    public Vector3 end;
    public bool allowDiagonals = true;

    //this.map = map;

    public ProceduralGeneration mapManager;
    public GameObject[,] map;
    private Vector3 lastCheckedNode;

    private BasicMeshProperties[] openSet = new BasicMeshProperties[8];
    // openSet starts with beginning node only
    //this.openSet.push(start);
    private BasicMeshProperties[] closedSet;
    public int NPCIndex = 0;


    // Use this for initialization
    void Start ()
    {
        map = mapManager.MakeGridArray();
        lastCheckedNode = start;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    bool AStarPathFinder(Vector3 start, Vector3 end, bool allowDiagonals)
    {
        while (true)
        {
            step();
        }
    }

    //Run one finding step.
    //returns 0 if search ongoing
    //returns 1 if goal reached
    //returns -1 if there is no solution found
    int step()
    {
        if (openSet.Length > 0)
        {

            // Best next option
            var winner = 0;
            for (int i = 1; i < openSet.Length; i++)
            {
                if (openSet[i].getTotalPathCost(NPCIndex) < openSet[winner].getTotalPathCost(NPCIndex))
                {
                    winner = i;
                }
                //if we have a tie according to the standard heuristic
                if (openSet[i].getTotalPathCost(NPCIndex) == openSet[winner].getTotalPathCost(NPCIndex))
                {
                    //Prefer to explore options with longer known paths (closer to goal)
                    if (openSet[i].getActualPathCost(NPCIndex) > openSet[winner].getActualPathCost(NPCIndex))
                    {
                        winner = i;
                    }

                    //if we're using Manhattan distances then also break ties
                    //of the known distance measure by using the visual heuristic.
                    //This ensures that the search concentrates on routes that look
                    //more direct. This makes no difference to the actual path distance
                    //but improves the look for things like games or more closely
                    //approximates the real shortest path if using grid sampled data for
                    //planning natural paths.
                    if (!this.allowDiagonals)
                    {
                        if (this.openSet[i].getActualPathCost(NPCIndex) == this.openSet[winner].getActualPathCost(NPCIndex) && this.openSet[i].getVisualHeuristic(NPCIndex) < this.openSet[winner].getVisualHeuristic(NPCIndex))
                        {
                            winner = i;
                        }
                    }
                }
            }

            GameObject current = this.openSet[winner];
            lastCheckedNode = current.transform.position;

            // Did I finish?
            if (current.transform.position == end)
            {
                Debug.Log("pathfinding solution found");
                return 1;
            }

            // Best option moves from openSet to closedSet
            this.removeFromArray(openSet, current);
            this.closedSet.push(current);

            // Check all the neighbors
            var neighbors = current.getNeighbors();

            for (int i = 0; i < neighbors.length; i++)
            {
                var neighbor = neighbors[i];

                // Valid next spot?
                if (!this.closedSet.includes(neighbor))
                {
                    // Is this a better path than before?
                    var tempG = current.getActualPathCost(NPCIndex) + this.heuristic(neighbor, current);

                    // Is this a better path than before?
                    if (!this.openSet.includes(neighbor))
                    {
                        this.openSet.push(neighbor);
                    }
                    else if (tempG >= neighbor.getActualPathCost(NPCIndex))
                    {
                        // No, it's not a better path
                        continue;
                    }

                    neighbor.g = tempG;
                    neighbor.h = this.heuristic(neighbor, end);
                    if (!allowDiagonals)
                    {
                        neighbor.vh = this.visualDist(neighbor, end);
                    }
                    neighbor.f = neighbor.g + neighbor.h;
                    neighbor.previous = current;
                }

            }
            Debug.Log("No path found");
            return 0;
        }
        else
        {
            Debug.Log("No path found");
            return -1;
        }
    }

    //This function returns a measure of aesthetic preference for
    //use when ordering the openSet. It is used to prioritise
    //between equal standard heuristic scores. It can therefore
    //be anything you like without affecting the ability to find
    //a minimum cost path.
    float visualDistance(Vector3 a, Vector3 b)
    {
        return dist(a.x, a.z, b.x, b.z);
    }

    // An educated guess of how far it is between two points
    float heuristic(a, b)
    {
        float d;
        if (allowDiagonals)
        {
            d = dist(a.i, a.j, b.i, b.j);
        }
        else
        {
            d = Math.Abs(a.i - b.i) + Math.Abs(a.j - b.j);
        }
        return d;
    }

    //delete a specific element from an array
    void removeFromArray(BasicMeshProperties[] openSet, GameObject current)
    {
        // Could use indexOf here instead to be more efficient
        for (int i = openSet.Length - 1; i >= 0; i--)
        {
            if (openSet[i] == current)
            {
                openSet = openSet.Where((source, index) => index != i).ToArray();
                //openSet.splice(i, 1);
            }
        }
    }*/