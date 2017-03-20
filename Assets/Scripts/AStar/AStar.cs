using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runs the A* algorithm
/// </summary>
public class AStar : MonoBehaviour
{
    private MeshRenderer[] pathNodeRenderers; //The path node renderers
    private Seeker seekerScript; //For seeking the next node on the path
    private List<PriorityItem> pathList;

    void Start() //Use this for initialization
    {
        PriorityItem startNode = null; //The starting point for the algorithm
        PriorityItem endNode = null; //The ending point for the algorithm
        List<GameObject> pathNodes = new List<GameObject>();
        pathNodes.AddRange(GameObject.FindGameObjectsWithTag("PathNode")); //Get all the path nodes
        PriorityItem[] priorityNodes = new PriorityItem[pathNodes.Count]; //Assign space for all of the priority nodes
        pathNodeRenderers = new MeshRenderer[pathNodes.Count]; //Assign a length to the pathNodeRenderers
        seekerScript = GetComponent<Seeker>();

        //Choose one of three possible end nodes
        int endNodeNumber = Random.Range(0, 3);
        string endNodeName = "PathNode39"; //Default value
        switch (endNodeNumber)
        {
            case 0:
                endNodeName = "PathNode7";
                break;
            case 1:
                endNodeName = "PathNode28";
                break;
            case 2:
                endNodeName = "PathNode39";
                break;
        }

        for (int i = 0; i < pathNodes.Count; i++)
        {
            //Get the renderers on the path nodes and turn them off by default
            pathNodeRenderers[i] = pathNodes[i].GetComponent<MeshRenderer>();
            pathNodeRenderers[i].enabled = false;

            //Get the priority node
            priorityNodes[i] = pathNodes[i].GetComponent<PriorityItem>();

            //Set the starting and ending node
            if (pathNodes[i].name == endNodeName)
                endNode = priorityNodes[i];
            else if (pathNodes[i].name == "PathNode0")
                startNode = priorityNodes[i];
        }

        //Determine what nodes are connected
        DetermineConnectedNodes(priorityNodes);

        //Run A*
        pathList = Pathfind(startNode, endNode);
    }

    void Update() //Update is called once per frame
    {
        ShowDebug();

        seekerScript.seekerTarget = pathList[0].gameObject; //Target the item at the top of the list

        //When reaching a node, decide whether to target a new node or if this is the last node
        if (pathList.Count == 1 && (seekerScript.seekerTarget.transform.position - gameObject.transform.position).magnitude < 10)
            return;
        else if ((seekerScript.seekerTarget.transform.position - gameObject.transform.position).magnitude < 10)
            pathList.RemoveAt(0);
    }

    /// <summary>
    /// Debug for game view
    /// </summary>
    void ShowDebug()
    {
        //Allow path nodes to be hidden or unhidden
        if (Input.GetKeyUp(KeyCode.H)) //PUT IT IN H
            if (pathNodeRenderers[0].enabled == false)
                for (int i = 0; i < pathNodeRenderers.Length; i++)
                    pathNodeRenderers[i].enabled = true;
            else
                for (int i = 0; i < pathNodeRenderers.Length; i++)
                    pathNodeRenderers[i].enabled = false;
    }

    /// <summary>
    /// Chooses what nodes are connected
    /// </summary>
    void DetermineConnectedNodes(PriorityItem[] priNodes)
    {
        //Loop through every node
        for (int i = 0; i < priNodes.Length; i++)
            for (int j = 0; j < priNodes.Length; j++)
            {
                float distance = (priNodes[i].transform.position - priNodes[j].transform.position).magnitude;
                if (i != j && distance <= 200)
                {
                    //Make sure the node isn't already in this list
                    if (!priNodes[i].NeighboringNodes.ContainsKey(priNodes[j]))
                        priNodes[i].NeighboringNodes.Add(priNodes[j], distance);

                    //Make sure the node isn't already in this list
                    if (!priNodes[j].NeighboringNodes.ContainsKey(priNodes[i]))
                        priNodes[j].NeighboringNodes.Add(priNodes[i], distance);
                }
            }
    }

    /// <summary>
    /// Run the A* algorithm
    /// </summary>
    /// <param name="start">The starting node for A*</param>
    /// <param name="end">The ending node for A*</param>
    /// <returns>A list of nodes to pathfind along</returns>
    List<PriorityItem> Pathfind(PriorityItem start, PriorityItem goal)
    {
        PriorityQueue open = new PriorityQueue(); //The open queue
        List<PriorityItem> closed = new List<PriorityItem>(); //The closed queue

        //End of path cost
        float endNodeHeuristic = 0;
        PriorityItem endNode = null;

        start.EstimatedTotalDistance = (goal.transform.position - start.transform.position).magnitude;
        open.Add(start); //Add the starting node to the queue
    
        //While the open list is not empty
        while (!open.Empty())
        {
            PriorityItem currentNode = open.Remove(); //Get the current node and remove it from the queue

            if (currentNode == goal) //If this is the final node
            {
                //Add the end node if it is not already in the closed list
                if (!closed.Contains(goal))
                    closed.Add(goal);

                return closed; //Return the closed list
            }

            //Add new nodes to the open list
            foreach (KeyValuePair<PriorityItem, float> node in currentNode.NeighboringNodes)
            {
                endNode = node.Key; //Set the next node to be visited
                float endNodeCost = currentNode.DistanceFromStart + node.Value; //Add the cost of the next node to the total

                if (closed.Contains(endNode)) //If the closed list contains the next node
                {
                    int index = closed.IndexOf(endNode); //Get the location of the node in the closed list

                    if (closed[index].DistanceFromStart <= endNodeCost) //Not a shorter route
                        continue;

                    closed.Remove(endNode); //Remove the end node from the closed list
                    endNodeHeuristic = endNodeCost - endNode.DistanceFromStart; //Recalculate the heuristic value
                }
                else if (open.Contains(endNode)) //If the item list contains the next node
                {
                    int index = open.IndexOf(endNode); //Get the location of the node in the open list

                    if (open.Get(index).DistanceFromStart <= endNodeCost) //When the node is found and it is not a shorter node
                        continue;

                    endNodeHeuristic = endNodeCost - endNode.DistanceFromStart; //Recalculate the heuristic value
                }
                else //If this is an unvisited node
                {
                    endNodeHeuristic = (goal.transform.position - endNode.transform.position).magnitude; //Get an estimate for the total cost from this node to the goal

                    endNode.EstimatedTotalDistance = endNodeCost + endNodeHeuristic; //Calculate the estimated total distance

                    if (!open.Contains(endNode)) //Add the node to the open list
                        open.Add(endNode);
                }
            }

            if (!closed.Contains(currentNode))
                closed.Add(currentNode); //Add the current node to the closed list
        }

        //Either we are at the goal or not
        if (endNode == goal)
            return null;
        else
        {
            //Add the end node if it is not already in the closed list
            if (!closed.Contains(goal))
                closed.Add(goal);

            return closed;
        }
    }
}