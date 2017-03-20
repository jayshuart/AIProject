using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbors : MonoBehaviour
{
    //private List<GameObject> neighboringNodes; //The neighbors to this node
    //private List<float> distanceToNeighbor; //The distance from the neighbors to this node
    //private bool debug;
    
    //#region Properties
    //public List<GameObject> NeighboringNodes
    //{
    //    get
    //    {
    //        return neighboringNodes;
    //    }
    //    set
    //    {
    //        neighboringNodes = value;
    //    }
    //}
    //public List<float> DistanceToNeighbor
    //{
    //    get
    //    {
    //        return distanceToNeighbor;
    //    }
    //    set
    //    {
    //        distanceToNeighbor = value;
    //    }
    //}
    //#endregion
    
    //void Start() //Use this for initialization
    //{
    //    neighboringNodes = new List<GameObject>();
    //    distanceToNeighbor = new List<float>();
    
    //    debug = false;
    //}
    
    //void Update() //Update is called once per frame
    //{
    //    ShowDebug();
    //}
    
    ///// <summary>
    ///// Debug for game view
    ///// </summary>
    //void ShowDebug()
    //{
    //    //Allow connections to be hidden or unhidden
    //    if (Input.GetKeyUp(KeyCode.H))
    //        if (debug == false)
    //            debug = true;
    //        else
    //            debug = false;
    
    //    if (debug == true)
    //        foreach (GameObject node in neighboringNodes)
    //            Debug.DrawLine(gameObject.transform.position, node.transform.position, Color.magenta); //Draw a line for debug purposes
    //}
}