using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item in the priority queue
/// </summary>
public class PriorityItem : MonoBehaviour
{
    //This node
    private float distanceFromStart; //Distance from the start to this node
    private float estimatedTotalDistance; //An estimate of the total distance to the end

    //Neighbors
    private Dictionary<PriorityItem, float> neighboringNodes = new Dictionary<PriorityItem, float>(); //The neighbors to this node and how far away they are
    private bool debug;

    #region Properties
    public float DistanceFromStart
    {
        get
        {
            return distanceFromStart;
        }
        set
        {
            distanceFromStart = value;
        }
    }
    public float EstimatedTotalDistance
    {
        get
        {
            return estimatedTotalDistance;
        }
        set
        {
            estimatedTotalDistance = value;
        }
    }
    public Dictionary<PriorityItem, float> NeighboringNodes
    {
        get
        {
            return neighboringNodes;
        }
        set
        {
            neighboringNodes = value;
        }
    }
    #endregion

    void Start() //Use this for initialization
    {
        distanceFromStart = 0;
        estimatedTotalDistance = 0;

        debug = false;
    }

    void Update() //Update is called once per frame
    {
        ShowDebug();
    }

    /// <summary>
    /// Debug for game view
    /// </summary>
    void ShowDebug()
    {
        //Allow connections to be hidden or unhidden
        if (Input.GetKeyUp(KeyCode.H)) //PUT IT IN H
            if (debug == false)
                debug = true;
            else
                debug = false;

        if (debug == true)
            foreach (KeyValuePair<PriorityItem, float> node in neighboringNodes)
                Debug.DrawLine(gameObject.transform.position, node.Key.transform.position, Color.magenta); //Draw a line for debug purposes
    }
}