using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour {

    #region Fields
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject terrain;

    [SerializeField] private GameObject[,] grid; //array of nodes
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    #endregion

    #region Start and Update
    // Use this for initialization
    void Start () {
        //instantiate grid
        grid = new GameObject[rows, columns];

        //fill grid with nodes
        BuildGrid();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    #endregion

    #region Helper Methods
    /// <summary>
    /// gets the node that covers a specified area in world space
    /// </summary>
    /// <param name="postion">Position in world sapce</param>
    /// <returns></returns>
    public GameObject GetNode(Vector3 position)
    {
        //find spread of nodes
        float xDiff = terrain.GetComponent<Terrain>().terrainData.size.x / rows;
        float zDiff = terrain.GetComponent<Terrain>().terrainData.size.z / columns;

        //remove inital offset from position
        position.x -= terrain.transform.position.x + (xDiff / 2);
        position.z -= terrain.transform.position.z + (zDiff / 2);

        //give back the node that covers that area
        return grid[0, 0];
    }

    /// <summary>
    /// Builds grid of influence map nodes
    /// </summary>
    private void BuildGrid()
    {
        //find spread of nodes
        float xDiff = terrain.GetComponent<Terrain>().terrainData.size.x / rows;
        float zDiff = terrain.GetComponent<Terrain>().terrainData.size.z / columns;

        //node scale
        Vector3 scale = new Vector3(xDiff * 7, zDiff * 7, 1); //its rotated so use z in the y scale spot

        //node position
        Vector3 pos = new Vector3(0, 150, 0); //get terrains pos
        pos.x = terrain.transform.position.x + (xDiff / 2); //offset x to start in corner
        pos.z = terrain.transform.position.z + (zDiff / 2); //offset z

        //fill grid with nodes
        for (int x = 0; x < rows; x++)
        {
            for(int z = 0; z < columns; z++)
            {
                //create node and move it to the right spot
                GameObject node = Instantiate(nodePrefab);
                node.transform.position = pos; //move to right spot
                node.transform.localScale = scale; 

                //add node to the matrix
                grid[x, z] = node;

                //update pos z for next node
                pos.z += zDiff;
            }

            //update pos x for next node
            pos.x += xDiff;

            //reset pos z for next row
            //pos.z = -terrain.GetComponent<Terrain>().terrainData.size.z / 2;
            pos.z = terrain.transform.position.z + (zDiff / 2);
        }
    }
    #endregion
}
