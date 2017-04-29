using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour {

    #region Fields
    private SpriteRenderer nodeIcon; 
    private Dictionary<string, Color32> team; //dictionary of teams colors
    #endregion

    #region Start and Update
    // Use this for initialization
    void Start () {
        //build dict
        team = new Dictionary<string, Color32>();
        team.Add("Red", new Color32(255, 0, 0, 155));
        team.Add("Green", new Color32(0, 255, 0, 155));
        team.Add("None", new Color32(101, 101, 101, 155));

        //get spriterenderer and set inital color
        nodeIcon = this.gameObject.GetComponent<SpriteRenderer>();
        nodeIcon.color = team["None"];

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    #endregion

    #region Helper Methods
    /// <summary>
    /// Defines the color of the node based on the two strengths input
    /// </summary>
    /// <param name="redStrength">Influence on this ndoe from red team</param>
    /// <param name="greenStrength">Influence on this node form green team</param>
    public void CompareInfluence(int redStrength, int greenStrength)
    {

    }
    #endregion
}
