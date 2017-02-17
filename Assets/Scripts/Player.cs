using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//require rigidbody so we can move
[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour {

    //fields
    public float speed;
    public float turnSpeed;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        //get rigidbody
        rb = this.gameObject.GetComponent<Rigidbody>();

        //hide cursor
        Cursor.visible = false;
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMove(); //move player via wasd input
        PlayerLook(); //change direction  by mouse input
    }

    //player input
    /// <summary>
    /// takes wasd input and moves player
    /// </summary>
    private void PlayerMove()
    {
        //movement vector
        Vector3 move = new Vector3(0, 0, 0);

        //forward
        if(Input.GetKey("w"))
        {
            move += this.gameObject.transform.forward * speed;
        }

        //backward
        if (Input.GetKey("s")) 
        {
            move += this.gameObject.transform.forward * -speed;
        }

        //strafe right
        if (Input.GetKey("d"))
        {
            move += this.gameObject.transform.right * speed;
        }

        //strafe left
        if (Input.GetKey("a"))
        {
            move += this.gameObject.transform.right * -speed;
        }

        //add move force to the rigidbody
        rb.AddForce(move);

        //set height to that of the world - countr balance for not having gravity on
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position) + 1.2f, gameObject.transform.position.z);
    }

    /// <summary>
    /// takes mouse input and rotates player and camera(because cam is a child of player)
    /// </summary>
    private void PlayerLook()
    {
        //get mouse iput along the x axis (left right movement)
        float mouseInput = Input.GetAxis("Mouse X");

        //create look vector based off mouse axis
        Vector3 turn = new Vector3(0, mouseInput * turnSpeed, 0);

        //rotate player
        transform.Rotate(turn);
    }
}
