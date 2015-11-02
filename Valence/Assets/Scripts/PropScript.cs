using UnityEngine;
using System.Collections;

public class PropScript : MonoBehaviour {
	public bool collision = false;
	bool set = false;
	float zoneWidth, zoneHeight, zoneX, zoneZ;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (collision) {
			Vector3 propPosition = new Vector3 ( Random.Range ( zoneX,zoneX + (zoneWidth) ) , 0.5f,  Random.Range (zoneZ ,zoneZ + (zoneHeight) ) );

			int randVal = (int) Random.Range (0,7);
			GameObject tempObject = (GameObject ) Instantiate(Resources.Load("prop_"+randVal), propPosition, Quaternion.identity);
			tempObject.transform.eulerAngles = new Vector3 (Random.Range(0, 25), Random.Range(0, 360), 0);
			tempObject.GetComponent<PropScript>().setZoneCoords( zoneWidth, zoneHeight, zoneX, zoneZ );
			tempObject.transform.SetParent( this.gameObject.transform.parent) ;
			Destroy( this.gameObject );
		}
	}

	public void setZoneCoords( float zW, float zH, float zX, float zZ ){
		zoneWidth = zW;
		zoneHeight = zH;
		zoneX = zX;
		zoneZ = zZ;
	}
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "prop" && set == false) {
			collision = true;
		} else {
			set = true;
		}
	}
}
