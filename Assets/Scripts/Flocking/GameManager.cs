using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------
    public List<GameObject> obstacles;
    public List<GameObject> flockers;
    public Vector3 centroid;
    public Vector3 averageDirection;

    public Camera[] cameras;
    private int currentCameraIndex;

    //-----------------------------------------------------------------------
    // Start and Update
    //-----------------------------------------------------------------------
    void Start()
    {
        //make our lists
        obstacles = new List<GameObject>();

        //intialize cam index
        currentCameraIndex = 0;

        //Turn all cameras off, except the first default one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        //If any cameras were added to the controller, enable the first one
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
            //write to the console which camera is enabled
            Debug.Log("Camera with name: " + cameras[0].GetComponent<Camera>().name + ", is now enabled");
        }
    }


    void Update()
    {
        //update centroid
        calcCentroid();
        calcFlockDirection();

        //set gm's body to that of the flocks average
        transform.position = calcCentroid();
        transform.forward = averageDirection;

        //change cams
        if (Input.GetKeyDown(KeyCode.C)) //can be any key you want
        {
            //up cam index
            currentCameraIndex++;

            //check if we hit the top of the array of cams
            if (currentCameraIndex < cameras.Length)
            {
                //turn off current cam, goto next
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
                Debug.Log("Camera with name: " + cameras[currentCameraIndex].GetComponent<Camera>().name + ", is now enabled");
            }
            else
            {
                //turn off curent cam, goto first cam in array
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                currentCameraIndex = 0;
                cameras[currentCameraIndex].gameObject.SetActive(true);
                Debug.Log("Camera with name: " + cameras[currentCameraIndex].GetComponent<Camera>().name + ", is now enabled");
            }
        }
    }

    //-----------------------------------------------------------------------
    // Flocking Methods
    //-----------------------------------------------------------------------
    Vector3 calcCentroid()
    {
        Vector3 position = new Vector3(0, 0, 0);
        foreach (GameObject flocker in flockers)
        {
            //add each flcokers position together
            position += flocker.transform.position;
        }

        //divide it out to find centroid
        position = position / flockers.Count;

        //save centroid
        centroid = position;

        //return it
        return centroid;
    }

    Vector3 calcFlockDirection()
    {
        Vector3 dir = new Vector3(0, 0, 0);
        foreach (GameObject flocker in flockers)
        {
            //add each flcokers forward vectors together
            dir += flocker.transform.forward;
        }

        //find avaerage flock dir
        dir = dir / flockers.Count;

        //save
        averageDirection = dir;

        //return
        return dir;
    }
}
