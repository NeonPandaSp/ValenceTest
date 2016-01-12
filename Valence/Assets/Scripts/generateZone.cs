using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class generateZone : MonoBehaviour {
	public float zoneWidth;
	public float zoneHeight;
	public float zoneX, zoneZ; //Zone Location

	public GameObject boundObject;
	// Use this for initialization

	//Debug list of all building types the user can choose from -Zach
	public string[] buildingArray = new string[] {

		//NOTE: ensure that you have created a corresponding prefab which matches the building added here -Zach
		"farm", 
		"hospital"
	};

	//Temp variable to contain current building selected from building list -Zach
	public int buildingType;

	//Moved variable defintion outside of switch statement scope -Zach
	GameObject tempLandmark;

	void Start () {
		boundObject = (GameObject) Resources.Load ("BoundObject");
		//Generate (getPropNum());
	}

	// Update is called once per frame
	void Update () {
		print (buildingType);
	}

	public void setZoneSize( Vector2 minPos, Vector2 maxPos ){
		zoneWidth 	= maxPos.x - minPos.x;
		zoneHeight 	= maxPos.y - minPos.y;

		zoneX = minPos.x;
		zoneZ = minPos.y;
	}

	public int getZoneArea(){
		return (int) ( zoneWidth * zoneHeight );
	}

	public int getPropNum(){
		float zoneArea = zoneWidth * zoneHeight;
		if (zoneArea < 4) {
			return 0;
		} else {
			return ( (int) (zoneArea / 4)); 	
		}
		
	}	

	//Added a debug GUI button to switch between building types
	void OnGUI() {

		//Create a GUI toggle section where the user can select between a serise of building types, assigned variable is passed to Generate function -Zach
		buildingType = GUI.SelectionGrid(new Rect(Screen.width - 100, 50, 100, 50), buildingType, buildingArray, 1);
	}

	public void Generate(int numProps){
		GameObject zoneBounds 									= (GameObject)Instantiate (boundObject, new Vector3 (zoneX+zoneWidth/2, 3, zoneZ+zoneHeight/2), Quaternion.identity);
		zoneBounds.gameObject.GetComponent<BoxCollider> ().size = new Vector3 (zoneWidth, 6, zoneHeight);

		if ( getZoneArea() >= 25 && zoneWidth >= 5 && zoneWidth >= 5) {
			Vector3 landmarkPosition = new Vector3 (Random.Range (zoneX, zoneX + zoneWidth), 0.0f, Random.Range (zoneZ, zoneZ + (zoneHeight)));
            //GameObject tempLandmark = (GameObject)Instantiate (Resources.Load ("landmark_0"), landmarkPosition, Quaternion.identity);

            //Added switch statement to allow for debug toggling between building types (currently just farm and hospital) - Zach
			switch(buildingType){
				//Case number is associated to the value stored in building type, 0 = index 0 of buildingArray -Zach
				case 0:
					//Updated this code to now use a landmark which has a serise of attached waypoints which an agent can move between - Zach
					tempLandmark = (GameObject)Instantiate(Resources.Load("TestFarm"), landmarkPosition, Quaternion.identity);
					tempLandmark.transform.eulerAngles = new Vector3 (0, Random.Range (0, 360), 0);
					tempLandmark.transform.SetParent( zoneBounds.transform );
					
				break;

				case 1:
					
					tempLandmark  = (GameObject)Instantiate(Resources.Load("TestHospital"), landmarkPosition, Quaternion.identity);
					tempLandmark.transform.eulerAngles = new Vector3 (0, Random.Range (0, 360), 0);
					tempLandmark.transform.SetParent( zoneBounds.transform );

				break;

				default:
					print ("default reached in Generate function in generateZone.cs");
				break;
			}


		}

		if (numProps > 0) {
			for (int i = 0; i <= numProps; i++) {
				Vector3 propPosition 				= new Vector3 (Random.Range (zoneX ,zoneX + zoneWidth ), 0.5f ,  Random.Range (zoneZ,zoneZ + (zoneHeight)));
				int randVal 						= (int) Random.Range (0,7);
				GameObject tempObject 				= (GameObject ) Instantiate(Resources.Load("prop_"+randVal), propPosition, Quaternion.identity);
				tempObject.transform.eulerAngles 	= new Vector3 (Random.Range (0,45), Random.Range(0, 360), Random.Range (0,45));
				tempObject.GetComponent<PropScript>().setZoneCoords( zoneWidth, zoneHeight, zoneX, zoneZ );
				tempObject.transform.SetParent( zoneBounds.transform );
			}
		}
	}
}
