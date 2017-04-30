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

	// Moveable Camera & Units stuff--------------
	Ray ray;
	Vector3 hit;
	private float camSpeed = 200.0f;
	private float zoomSpeed = 10.0f;
	private float maxZoom = 42.779f;
	private float minZoom = 1.0f;

	public GameObject whiteCubePrefab;
	public GameObject blueCubePrefab;
	public GameObject yellowCubePrefab;
	public GameObject blackCubePrefab;

	private int selectedUnit;
	//----------------------------------

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
			Debug.Log("Camera with name: " + cameras[0].GetComponent<Camera>().name + " (" + currentCameraIndex + "), is now enabled");
        }

		//Units stuff-----------------
		//default unit to white at first
		selectedUnit = 1;
		//----------------------------
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
				Debug.Log("Camera with name: " + cameras[currentCameraIndex].GetComponent<Camera>().name + " (" + currentCameraIndex + "), is now enabled");
            }
            else
            {
                //turn off curent cam, goto first cam in array
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                currentCameraIndex = 0;
                cameras[currentCameraIndex].gameObject.SetActive(true);
				Debug.Log("Camera with name: " + cameras[currentCameraIndex].GetComponent<Camera>().name + " (" + currentCameraIndex + "), is now enabled");
            }
        }

		// Moveable Camera stuff--------------
		if (currentCameraIndex == 5) {
			if (Input.GetKey (KeyCode.W)) {
				cameras [5].transform.position += Vector3.forward * camSpeed * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.A)) {
				cameras [5].transform.position += Vector3.left * camSpeed * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.S)) {
				cameras [5].transform.position += Vector3.back * camSpeed * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.D)) {
				cameras [5].transform.position += Vector3.right * camSpeed * Time.deltaTime;
			}
			float scroll = cameras[5].GetComponent<Camera>().fieldOfView;
			scroll += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
			scroll = Mathf.Clamp (scroll, minZoom, maxZoom);
			cameras [5].GetComponent<Camera>().fieldOfView = scroll;

			// Change placeable unit color
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				selectedUnit = 1;
				Debug.Log ("White Unit selected");
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				selectedUnit = 2;
				Debug.Log ("Blue Unit selected");
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				selectedUnit = 3;
				Debug.Log ("Yellow Unit selected");
			}
			if (Input.GetKeyDown(KeyCode.Alpha4)) {
				selectedUnit = 4;
				Debug.Log ("Black Unit selected");
			}


			if (Input.GetMouseButtonDown(0)) {
				ray = cameras[currentCameraIndex].ScreenPointToRay(Input.mousePosition);
				RaycastHit rayHit = new RaycastHit ();

				if (Physics.Raycast (ray, out rayHit, Mathf.Infinity)) {
					//Debug.Log ("Ray hit at:" + rayHit.point);
					switch(selectedUnit){
					case 1:
						Instantiate (whiteCubePrefab, rayHit.point, Quaternion.identity);
						break;
					case 2:
						Instantiate (blueCubePrefab, rayHit.point, Quaternion.identity);
						break; 
					case 3:
						Instantiate (yellowCubePrefab, rayHit.point, Quaternion.identity);
						break;
					case 4:
						Instantiate (blackCubePrefab, rayHit.point, Quaternion.identity);
						break;
					}

				}
				//Debug.DrawLine (ray.origin, rayHit.point, Color.white);
			}
		}

		// ------------------------------------
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
