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

    //public GameObject cube; //FOR TESTING PURPOSES

    private List<GameObject> units;
    #endregion

    #region Start and Update
    // Use this for initialization
    void Start()
    {
        //instantiate grid
        grid = new GameObject[rows, columns];

        //fill grid with nodes
        BuildGrid();

        units = new List<GameObject>();
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Update map on keypress
        {
            //Reset tile influences
            foreach (GameObject node in grid)
            {
                node.GetComponent<GridNode>().GreenInfluence = 0;
                node.GetComponent<GridNode>().RedInfluence = 0;
                node.GetComponent<GridNode>().ActiveInfluence = 0;
            }

            //Find and add all units
            GameObject[] unitsArray = GameObject.FindGameObjectsWithTag("Unit");

            //If more units have been added
            if (unitsArray.Length != units.Count)
            {
                units.Clear(); //Clear the array of units

                foreach (GameObject unit in unitsArray)
                {
                    units.Add(unit);
                }
            }

            foreach (GameObject unit in units)
            {
                //Save positions
                int x = GetNode(unit.transform.position)[0];
                int y = GetNode(unit.transform.position)[1];

                GridNode tile = grid[x, y].GetComponent<GridNode>(); //Save the tile

                tile.Source = true; //Make this node a source node

                if (unit.GetComponent<UnitStats>().team == "Red")
                    tile.RedInfluence += unit.GetComponent<UnitStats>().strength;
                else
                    tile.GreenInfluence += unit.GetComponent<UnitStats>().strength;

                tile.CompareInfluence(tile.RedInfluence, tile.GreenInfluence); //Compare the influence

                InfluenceNeighbors(x, y); //Calculate the grid's influence
            }
        }
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
        if (!grid[x, y].GetComponent<GridNode>().Source)
        {
            return; //leave the method bc its not a source node
        }

        int influence = grid[x, y].GetComponent<GridNode>().ActiveInfluence; //The influence of the grid point

        //Loop through the grid around the point of influence, don't go out of bounds
        for (int i = x - influence; (i < x + influence + 1) && i >= 0 && i < 20; i++)
        {
            for (int j = y - influence; (j < y + influence + 1) && j >= 0 && j < 20; j++)
            {
                int distance = influence - (int)Vector2.Distance(new Vector2(i, j), new Vector2(x, y)); //The distance from the center point to the current point

                if (!(x == i && y == j) && distance > 0) //Not the center point
                {
                    //Save the current node
                    GridNode gNode = grid[i, j].GetComponent<GridNode>();

                    if (grid[x, y].GetComponent<GridNode>().Team == "None") //Both teams on tile
                    {
                        gNode.RedInfluence += distance;
                        gNode.GreenInfluence += distance;
                    }
                    else if (grid[x, y].GetComponent<GridNode>().Team == "Red") //Red team tile
                        gNode.RedInfluence += distance;
                    else if (grid[x, y].GetComponent<GridNode>().Team == "Green") //Green team tile
                        gNode.GreenInfluence += distance;
                }
            }
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
