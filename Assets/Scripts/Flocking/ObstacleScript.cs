using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour
{
    //Access to GameManager script
    protected GameManager gm;

    //radius
    public float radius;

    void Start()
    {
        //get our gm object
        gm = GameObject.Find("GameManagerGO").GetComponent<GameManager>();

        //add self tot he gm's list of obstacles
        gm.obstacles.Add(gameObject);

        //make radius that of its collider
        if(this.gameObject.GetComponent<SphereCollider>() != null)
        {
            radius = this.gameObject.GetComponent<SphereCollider>().radius;
        }
        else if (this.gameObject.GetComponent<CapsuleCollider>() != null)
        {
            radius = this.gameObject.GetComponent<CapsuleCollider>().radius;
        }
            //if its not a sphere or capsule set it manually in editor
        
    }

    public float Radius
    {
        get { return radius; }
    }
}