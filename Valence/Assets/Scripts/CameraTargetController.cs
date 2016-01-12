using UnityEngine;
using System.Collections;

public class CameraTargetController : MonoBehaviour {
	public GameObject ground;

	public Vector3 lastPosition, lastRotation;

	public ExploreMode_GameController _GameController;

	public float lastMousePositionX, lastMousePositionY;

	public bool moving;
	Vector3 endPos, startPos;
	float t;
	// Use this for initialization
	void Start () {
		lastPosition = transform.position;
		lastMousePositionX = Input.mousePosition.x;
		lastRotation = new Vector3 ( transform.rotation.x, transform.rotation.y, transform.rotation.z);
	}
	
	// Update is called once per frame
	void Update () {
		float rotateDelta = 180;
		Vector2 centerScreen = new Vector2( Screen.width/2, Screen.height/2 );

		Ray ray = Camera.main.ScreenPointToRay ( centerScreen );
		RaycastHit hitInfo;
		
		if ( ground.GetComponent<Collider>().Raycast (ray, out hitInfo, Mathf.Infinity)) {
			float x = hitInfo.point.x;//Mathf.FloorToInt( hitInfo.point.x );
			float z = hitInfo.point.z;//Mathf.FloorToInt( hitInfo.point.z );
			Vector3 lookAtTarget;
			lookAtTarget = new Vector3( x, 0, z );
			if( x >= 0 && x <= ground.GetComponent<TileMap>().worldSizeX ){
				if( z >= 0 && z <= ground.GetComponent<TileMap>().worldSizeZ ){
					Vector3 camPosition = Camera.main.transform.position;
					transform.position = lookAtTarget;
					Camera.main.transform.position = camPosition;
				}
			}

			//Camera.main.transform.position = new Vector3 (lookAtTarget.x, Camera.main.transform.position.y, Camera.main.transform.position.z );
			//Camera.main.transform.LookAt (lookAtTarget);

			
		}
		if (Input.GetMouseButton (0) && Input.GetKey (KeyCode.LeftAlt)) {

			if( Input.mousePosition.x > lastMousePositionX + 2 ){
				transform.Rotate(transform.up * rotateDelta * Time.deltaTime);
			} else if( Input.mousePosition.x < lastMousePositionX - 2 ) {
				transform.Rotate(-1*transform.up * rotateDelta * Time.deltaTime);
			}
			/**
			if( Input.mousePosition.y > lastMousePositionY + 2 ){
				transform.Rotate(transform.right * rotateDelta * Time.deltaTime);
				transform.Rotate(transform.forward * rotateDelta * Time.deltaTime);
			} else if( Input.mousePosition.y < lastMousePositionY - 2 ) {
				transform.Rotate(-1*transform.right * rotateDelta * Time.deltaTime);
				transform.Rotate(-1*transform.forward * rotateDelta * Time.deltaTime);
			} **/
			Vector3 myRotation = new Vector3 ( transform.rotation.x, transform.rotation.y, transform.rotation.z );
			//Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
			//Camera.main.transform.RotateAround (this.transform.position, Vector3.right, 10 );
			//Camera.main.transform.LookAt (this.transform.position);
			//Camera.main.transform.position += transform.position - lastPosition;
			//Camera.main.transform.eulerAngles += myRotation - lastRotation;
		}
		lastMousePositionX = Input.mousePosition.x;
		lastMousePositionY = Input.mousePosition.y;
		lastPosition = transform.position;
		lastRotation = new Vector3 ( transform.rotation.x, transform.rotation.y, transform.rotation.z);


		if (moving) {
			this.transform.position = Vector3.Lerp (startPos,endPos, t); 
			if (t < 3 ){ // while t below the end limit...
				// increment it at the desired rate every update:
				t += Time.deltaTime;
			} else if ( t > 3 ){
				moving = false;
			} 
		}
	}

	public void MoveCameraTo(Vector3 sPos, Vector3 ePos){
		moving = true;
		startPos = sPos;
		endPos = ePos;
		t = 0;
	}
}
