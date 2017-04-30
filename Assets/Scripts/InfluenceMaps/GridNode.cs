using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour {

    #region Fields
    private enum Teams
    {
        Red,
        Green,
        None
    };
    private Teams team;
    private SpriteRenderer nodeIcon; 
    private Dictionary<Teams, Color32> teamColors; //dictionary of teamColors colors

    public byte alpha; //for coloring gridIcons

    //influence of each team on this particular node
    private int greenInfluence;
    private int redInfluence;
    private int activeInfluence; //whatever team controls this node will have its influence here

    private bool source; //is this node a source of influence? - for linear loss of influence
    #endregion

    #region Properties
    public string Team
    {
        get { return team.ToString(); } //convert team enum into a string
        set
        {
            //check for validity of value
            if(value == "Red" || value == "red" || value == "RED") //incase yall wanna do somethin other than "Red"
            {
                //set team to red
                team = Teams.Red;
            }
            else if (value == "Green" || value == "green" || value == "GREEN") //incase yall wanna do somethin other than "Red"
            {
                //set team to green
                team = Teams.Green;
            }
            else
            {
                //anything else should be a none
                team = Teams.None;
            }
        }
    }

    public int RedInfluence
    {
        get { return redInfluence; }
        set
        {
            redInfluence = value;
        }
    }

    public int GreenInfluence
    {
        get { return greenInfluence; }
        set
        {
            greenInfluence = value;
        }
    }

    public int ActiveInfluence
    {
        get { return activeInfluence; }
        set
        {
            activeInfluence = value;
        }
    }

    public bool Source
    {
        get { return source; }
        set
        {
            source = value;
        }
    }
    #endregion

    #region Start and Update
    // Use this for initialization
    void Start()
    {
        //build dict
        teamColors = new Dictionary<Teams, Color32>();
        teamColors.Add(Teams.Red, new Color32(255, 0, 0, alpha));
        teamColors.Add(Teams.Green, new Color32(0, 255, 0, alpha));
        teamColors.Add(Teams.None, new Color32(101, 101, 101, alpha));

        //set team initally
        team = Teams.None;

        //get spriterenderer and set inital color
        nodeIcon = gameObject.GetComponent<SpriteRenderer>();
        nodeIcon.color = teamColors[team];

        //set inital influence for both teams
        greenInfluence = 0;
        redInfluence = 0;
        source = false;
    }
	
	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Update map on keypress
        {
            //update this nodes team
            CompareInfluence(redInfluence, greenInfluence);
        }
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
        //compare strengths and set tthis nodes team
        if(redStrength > greenStrength) //red better than green
        {
            //update team and iconColor
            team = Teams.Red;
            nodeIcon.color = teamColors[team];
            activeInfluence = redInfluence;
        }
        else if(greenStrength > redStrength) //both strengths are the same
        {
            //update team and iconColor
            team = Teams.Green;
            nodeIcon.color = teamColors[team];
            activeInfluence = greenInfluence;
        }
        else //green is the only option left
        {
            //update team and iconColor
            team = Teams.None;
            nodeIcon.color = teamColors[team];
            activeInfluence = 0;
        }
    }
    #endregion
}
