using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A queue sorting the priority of items; lowest value is first
/// </summary>
public class PriorityQueue : MonoBehaviour
{
    private List<PriorityItem> pQueue = new List<PriorityItem>(); //P's and Q's (This also throws a warning, but it's a dumb warning, so we'll pretend it doesn't exist)

    void Start() //Use this for initialization
    {
        
    }

    void Update() //Update is called once per frame
    {

    }

    /// <summary>
    /// Add items into the queue
    /// </summary>
    /// <param name="pItem">The item to be added to the queue</param>
    public void Add(PriorityItem pItem)
    {
        if (pQueue.Count == 0) //If the queue is empty
        {
            pQueue.Add(pItem);
            return;
        }

        //Determine where to start searching for an insertion point
        int i = 0;
        if (pItem.EstimatedTotalDistance > pQueue[pQueue.Count / 2].EstimatedTotalDistance)
            i = pQueue.Count / 2;

        //Find the insertion point for the item
        for (; i < pQueue.Count; i++) //WHAT AND HOW
            if (pItem.EstimatedTotalDistance <= pQueue[i].EstimatedTotalDistance)
            {
                pQueue.Insert(i, pItem);
                break;
            }
    }

    /// <summary>
    /// Remove the first item from the queue
    /// </summary>
    /// <returns>The first item in the queue</returns>
    public PriorityItem Remove()
    {
        if (pQueue.Count == 0) //If the queue is empty
            return null;

        //Save the return value and then remove it before returning it
        PriorityItem returnValue = pQueue[0];
        pQueue.RemoveAt(0);
        return returnValue;
    }

    /// <summary>
    /// Remove a specific object from the queue
    /// </summary>
    /// <param name="pItem">The item to be removed from the queue</param>
    public void Remove(PriorityItem pItem)
    {
        pQueue.Remove(pItem);
    }

    /// <summary>
    /// Check if an item is in the queue
    /// </summary>
    /// <param name="pItem">The item to be checked for in the queue</param>
    /// <returns>True/False</returns>
    public bool Contains(PriorityItem pItem)
    {
        return pQueue.Contains(pItem);
    }

    /// <summary>
    /// If the queue is empty or not
    /// </summary>
    /// <returns>True/False</returns>
    public bool Empty()
    {
        if (pQueue.Count == 0) //If the queue is empty
            return true;
        else //If the queue is not empty
            return false;
    }

    /// <summary>
    /// Get a value at the location requested
    /// </summary>
    /// <param name="location">The location of the requested value</param>
    /// <returns>The value at the location requested</returns>
    public PriorityItem Get(int location)
    {
        return pQueue[location];
    }

    /// <summary>
    /// Get the index of a value
    /// </summary>
    /// <param name="pItem">The item to be checked for in the queue</param>
    /// <returns>The location of the item in the queue</returns>
    public int IndexOf(PriorityItem pItem)
    {
        return pQueue.IndexOf(pItem);
    }
}