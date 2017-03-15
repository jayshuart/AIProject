
using UnityEngine;
using System.Collections;

//use the Generic system here to make use of a Flocker list later on
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]

abstract public class Vehicle : MonoBehaviour
{
    //FEILDS -------------------------------------------------------------------------------------------------------
    //movement
    protected Vector3 acceleration;
    protected Vector3 velocity;
    protected Vector3 desired;

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    //public for changing in Inspector
    //define movement behaviors
    public float maxSpeed = 6.0f;
    public float maxForce = 12.0f;
    public float mass = 3.0f;
    public float radius = 1.0f;
    public float safeDistance = 100.0f;

    //access to Character Controller component
    CharacterController charControl;

    //set abstract for steering forces in a flocker has dif behaviors
    abstract protected void CalcSteeringForces();

    //Access to GameManager script
    protected GameManager gm;


    //START AND UPDATE -----------------------------------------------------------------------------------------------
    virtual public void Start()
    {
        //set variables     
        acceleration = Vector3.zero;
        velocity = transform.forward;

        //set char controller and gamemanager for later use
        charControl = GetComponent<CharacterController>();
        gm = GameObject.Find("GameManagerGO").GetComponent<GameManager>();
    }


    // Update is called once per frame
    virtual public void Update()
    {
        //calculate all necessary steering forces
        CalcSteeringForces();

        //add accel to vel
        velocity += acceleration * Time.deltaTime;

        //limit vel to max speed
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //set height to that of the world
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, (Terrain.activeTerrain.SampleHeight(transform.position) + 1.1f), gameObject.transform.position.z);
        

        //orient vehcle
        transform.forward = velocity.normalized;

        //move the character based on velocity
        charControl.Move(velocity * Time.deltaTime);

        //reset acceleration to 0
        acceleration = Vector3.zero;
    }

//METHODS --------------------------------------------------------------------------------------------------
    protected void ApplyForce(Vector3 steeringForce)
    {
        //f = m/a and a = F/m
        acceleration += steeringForce / mass;
    }

    protected Vector3 Seek(Vector3 targetPos)
    {
        desired = targetPos - transform.position; //get desired
        desired = desired.normalized * maxSpeed; //make force at size of speed
        desired -= velocity; //get dif between disired and current velocity
        return desired;
    }

    protected Vector3 AvoidObstacle(GameObject ob, float safe)
    {

        //reset desired velocity
        desired = Vector3.zero;

        //get radius from obstacle's script
        float obRad;
        if(ob == null)
        {
            //no object to avoid no radius
            obRad = 0;
        }
        else
        {
            obRad = ob.GetComponent<ObstacleScript>().Radius;
        }

        //get vector from vehicle to obstacle
        Vector3 vecToCenter = ob.transform.position - transform.position;

        //if object is out of my safe zone, ignore it
        if (vecToCenter.magnitude > (safe + obRad))
        {
            return Vector3.zero;
        }
        //if object is behind me, ignore it
        if (Vector3.Dot(vecToCenter, transform.forward) < 0)
        {
            return Vector3.zero;
        }
        //if object is not in my forward path, ignore it
        if (Mathf.Abs(Vector3.Dot(vecToCenter, transform.right)) > obRad + radius)
        {
            return Vector3.zero;
        }

        //if we get this far, we will collide with an obstacle!
        //object on left, steer right
        if (Vector3.Dot(vecToCenter, transform.right) < 0)
        {
            desired = transform.right * maxSpeed;
            //debug line to see if the dude is avoiding to the right
            Debug.DrawLine(transform.position, ob.transform.position, Color.red);
        }
        else
        {
            desired = transform.right * -maxSpeed;
            //debug line to see if the dude is avoiding to the left
            Debug.DrawLine(transform.position, ob.transform.position, Color.green);
        }
        return desired;
    }

    //separation
    protected Vector3 Separation()
    {
        //reset desired velocity
        desired = Vector3.zero;

        //run through all other flcokers
        GameObject nearest = null;
        float nearDist = 999999;
        foreach (GameObject flocker in GameObject.Find("GameManagerGO").GetComponent<GameManager>().flockers)
        {
            //get disatnce betweent he two
            Vector3 dist = flocker.transform.position - transform.position;

            //check safe distance
            if (dist.magnitude > safeDistance)
            {
                //if its outside the safe zone, ignore it
                return Vector3.zero;
            }
            else
            {
                //check if its the nearest
                if (dist.magnitude < nearDist)
                {
                    //save new nearest
                    nearest = flocker;
                    nearDist = dist.magnitude;
                }
            }
        }

        //flee from this flocker
        return (Seek(nearest.transform.position) * -1); //negative seek is flee
    }

    //alignment
    protected Vector3 Alignment()
    {
        //find sum of flockers forward vectors
        Vector3 dir = new Vector3(0, 0, 0);
        foreach (GameObject flocker in gm.flockers)
        {
            //add each flcokers forward vectors together
            dir += flocker.transform.forward;
        }

        //compute desired
        dir.Normalize();
        dir *= maxSpeed;

        //find steer force
        Vector3 steering = dir - velocity;

        //return
        return steering;
    }

    //cohesion
    protected Vector3 Cohesion()
    {
        //prep steering vector
        Vector3 steer;

        //seek centroid - save to steer
        steer = Seek(gm.centroid);

        //return
        return steer;
    }
}
