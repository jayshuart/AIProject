using UnityEngine;
using System.Collections;

public class Seeker : Vehicle
{

    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------
    public GameObject seekerTarget;
    public GameObject fleeTarget;

    //Seeker's steering force (will be added to acceleration)
    private Vector3 force;

    //weights
    public float seekWeight;
    public float avoidWeight;
    public float alignWeight;
    public float cohesionWeight;
    public float seperationWeight;
    public float fleeWeight;



    //-----------------------------------------------------------------------
    // Start - No Update
    //-----------------------------------------------------------------------
    // Call Inherited Start and then do our own
    override public void Start()
    {
        //call parent's start
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

    //-----------------------------------------------------------------------
    // Class Methods
    //-----------------------------------------------------------------------
    protected override void CalcSteeringForces()
    {
        //reset value to (0, 0, 0)
        force = Vector3.zero;

        //got a seeking force
        force += Seek(seekerTarget.transform.position) * seekWeight;

        //get flee force
        force += Flee(fleeTarget) * fleeWeight;

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
