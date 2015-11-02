using UnityEngine;
using System.Collections;

public class generateZone : MonoBehaviour {
	public float zoneWidth;
	public float zoneHeight;
	public float zoneX, zoneZ; //Zone Location

	public GameObject boundObject;
	// Use this for initialization
	void Start () {
		boundObject = (GameObject) Resources.Load ("BoundObject");
		//Generate (getPropNum());
	}

	// Update is called once per frame
	void Update () {
		
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
	
	public void Generate(int numProps){
		GameObject zoneBounds 									= (GameObject)Instantiate (boundObject, new Vector3 (zoneX+zoneWidth/2, 3, zoneZ+zoneHeight/2), Quaternion.identity);
		zoneBounds.gameObject.GetComponent<BoxCollider> ().size = new Vector3 (zoneWidth, 6, zoneHeight);

		if ( getZoneArea() >= 25 && zoneWidth >= 5 && zoneWidth >= 5) {
			Vector3 landmarkPosition = new Vector3 (Random.Range (zoneX, zoneX + zoneWidth), 0.0f, Random.Range (zoneZ, zoneZ + (zoneHeight)));
			GameObject tempLandmark = (GameObject)Instantiate (Resources.Load ("landmark_0"), landmarkPosition, Quaternion.identity);
			tempLandmark.transform.eulerAngles = new Vector3 (0, Random.Range (0, 360), 0);
			tempLandmark.transform.SetParent( zoneBounds.transform );
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
