using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfluenceMapGameManager : MonoBehaviour {
    public Camera[] cameras;
    private int currentCameraIndex;

	// Moveable Camera & Units stuff--------------
	Ray ray;
	Vector3 hit;
	private float camSpeed = 200.0f;
	private float zoomSpeed = 100.0f;
	private float maxZoom = 250f;
	private float minZoom = 50f;

	public GameObject whiteCubePrefab;
	public GameObject blueCubePrefab;
	public GameObject yellowCubePrefab;
	public GameObject blackCubePrefab;

	private int selectedUnit;
    private string selectedTeam;
	//----------------------------------

    //-----------------------------------------------------------------------
    // Start and Update
    //-----------------------------------------------------------------------
    void Start()
    {
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
        selectedTeam = "Red";
		//----------------------------
    }


    void Update()
    {
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
		if (currentCameraIndex == 0) {
			if (Input.GetKey (KeyCode.W)) {
				cameras [0].transform.position += Vector3.forward * camSpeed * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.A)) {
				cameras [0].transform.position += Vector3.left * camSpeed * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.S)) {
				cameras [0].transform.position += Vector3.back * camSpeed * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.D)) {
				cameras [0].transform.position += Vector3.right * camSpeed * Time.deltaTime;
			}
			float scroll = cameras[0].GetComponent<Camera>().orthographicSize;
			scroll -= Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
			scroll = Mathf.Clamp (scroll, minZoom, maxZoom);
			cameras [0].GetComponent<Camera>().orthographicSize = scroll;

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

            if (Input.GetKeyDown(KeyCode.R))
            {
                selectedTeam = "Red";
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                selectedTeam = "Green";
            }

            if (Input.GetMouseButtonDown(0)) {
				ray = cameras[currentCameraIndex].ScreenPointToRay(Input.mousePosition);
				RaycastHit rayHit = new RaycastHit ();

				if (Physics.Raycast (ray, out rayHit, Mathf.Infinity)) {
                    //Debug.Log ("Ray hit at:" + rayHit.point);
                    GameObject instantiatedObject = null;

                    switch (selectedUnit){
					case 1:
                            instantiatedObject = Instantiate(whiteCubePrefab, rayHit.point, Quaternion.identity);
						    break;
					case 2:
                            instantiatedObject = Instantiate(blueCubePrefab, rayHit.point, Quaternion.identity);
						    break; 
					case 3:
                            instantiatedObject = Instantiate(yellowCubePrefab, rayHit.point, Quaternion.identity);
						    break;
					case 4:
                            instantiatedObject = Instantiate(blackCubePrefab, rayHit.point, Quaternion.identity);
						    break;
					}

                    instantiatedObject.GetComponent<UnitStats>().team = selectedTeam;
				}
				//Debug.DrawLine (ray.origin, rayHit.point, Color.white);
			}
		}

		// ------------------------------------
    }
}