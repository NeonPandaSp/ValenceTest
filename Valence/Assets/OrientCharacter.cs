using UnityEngine;
using System.Collections;

public class OrientCharacter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 lookDirection = transform.parent.position + transform.parent.GetComponent<Unit> ().facing;
		lookDirection.y = 0.0f;
		lookDirection.x += 0.5f;
		lookDirection.z += 0.5f;
		transform.LookAt (lookDirection, Vector3.up );
	}
}
