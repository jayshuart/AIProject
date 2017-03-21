using UnityEngine;
using System.Collections;

public class AStarSeeker : Vehicle
{

    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------
    public GameObject seekerTarget;

    //Seeker's steering force (will be added to acceleration)
    private Vector3 force;

    //weights
    public float seekWeight;
    public float avoidWeight;
    public float alignWeight;
    public float cohesionWeight;
    public float seperationWeight;


    //START AND UPDATE -------------------------------------------------------------------------
    override public void Start()
    {
        // Call Inherited Start and then do our own
        base.Start();

        //initialize
        force = Vector3.zero;

        //add itself to the list of flockers
        //gm.flockers.Add(gameObject);  
    }

    public void Awake()
    {

    }

    override public void Update()
    {
        //call parents update
        base.Update();
    }

    //METHODS ----------------------------------------------------------------------------------

    /// <summary>
    /// calcualtes sterring forces needed based on where the seek and flee atrgets are, as well as obstacles avoiance, and general flocking weights.
    /// Applies resulting force to our flocker
    /// </summary>
    protected override void CalcSteeringForces()
    {
        //reset value to (0, 0, 0)
        force = Vector3.zero;

        //got a seeking force
        force += Seek(seekerTarget.transform.position) * seekWeight;

        //get seperation force
        force += Separation() * seperationWeight;

        //get alignment force
        force += Alignment() * alignWeight;

        //get cohesion force
        force += Cohesion() * cohesionWeight;

        //loop through obstacles, as to avoid them
        foreach(GameObject obst in gm.obstacles)
        {
            //add force to avoid each obstacle
            force += AvoidObstacle(obst, safeDistance) * avoidWeight;
        }

        //limited the seeker's steering force
        force = Vector3.ClampMagnitude(force, maxForce);

        //applied the steering force to this Vehicle's acceleration (ApplyForce)
        ApplyForce(force);
    }
}
