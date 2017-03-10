using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour
{
    //Access to GameManager script
    protected GameManager gm;

    //radius
    private float radius = 2.4f;

    void Start()
    {
        //get our gm object
        gm = GameObject.Find("GameManagerGO").GetComponent<GameManager>();

        //add self tot he gm's list of obstacles
        gm.obstacles.Add(gameObject);
    }

    public float Radius
    {
        get { return radius; }
    }
}