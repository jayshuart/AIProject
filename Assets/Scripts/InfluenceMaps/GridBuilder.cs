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

    public GameObject cube; //FOR TESTING PURPOSES
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

        //TEST CODE
        foreach (GameObject gridNode in grid) //clear all nodes
        {
            gridNode.GetComponent<GridNode>().RedInfluence = 0;
        }

        //get xy pos of cube in grid coords
        int x = GetNode(cube.transform.position)[0];
        int y = GetNode(cube.transform.position)[1];

        GameObject n = grid[x, y]; //get node ta the pos of the cube
        n.GetComponent<GridNode>().Source = true; //make it a source node
        n.GetComponent<GridNode>().RedInfluence = 3; //influence that nod

        //aplly influence to its neighbors
        //InfluenceNeighbors(x, y);
        //END TEST CODE

    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// gets the node that covers a specified area in world space
    /// </summary>
    /// <param name="postion">Position in world sapce</param>
    /// <returns>x/z values for grid array of node that covers that position</returns>
    public int[] GetNode(Vector3 position)
    {
        //find spread of nodes
        float xDiff = terrain.GetComponent<Terrain>().terrainData.size.x / rows;
        float zDiff = terrain.GetComponent<Terrain>().terrainData.size.z / columns;

        //remove inital offset from position
        position.x -= terrain.transform.position.x;
        position.z -= terrain.transform.position.z;

        //divide xDiff and zDiff out of position
        position.x = position.x / xDiff;
        position.z = position.z / zDiff;

        //convert position values to ints
        int x = (int)position.x;
        int z = (int)position.z;

        //give back the node that covers that area
        int[] values = { x, z };
        return values;
    }

    /// <summary>
    /// applies influence from a source node to its neighbors
    /// </summary>
    /// <param name="x">x value of grid[x,y]</param>
    /// <param name="y">y value of grid[x,y]</param>
    public void InfluenceNeighbors(int x, int y)
    {
        //check if node is a source node
        if(!grid[x, y].GetComponent<GridNode>().Source)
        {
            return; //leave the method bc its not a source node
        }

        //apply influence linearly to neighbors
        int localInfluence = grid[x, y].GetComponent<GridNode>().ActiveInfluence - 1;
        for (int a = x; a < (x + grid[x, y].GetComponent<GridNode>().ActiveInfluence - 1); a++)
        {
            for (int b = y; b < (y + grid[x, y].GetComponent<GridNode>().ActiveInfluence - 1); b++)
            {
                //get which team this node is on and apply that teams influence on neighbor
                if(grid[x, y].GetComponent<GridNode>().Team == "Red")
                {
                    grid[a, b].GetComponent<GridNode>().RedInfluence += localInfluence;
                }
                else if (grid[x, y].GetComponent<GridNode>().Team == "Green")
                {
                    grid[a, b].GetComponent<GridNode>().GreenInfluence += localInfluence;
                }

                //drop influence
                localInfluence--;

                //DEBUG LINE
                Debug.Log(a + ", " + b);
            }

            //reset influence
            localInfluence = grid[x, y].GetComponent<GridNode>().ActiveInfluence - 1;
        }
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
