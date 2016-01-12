using UnityEngine;
using System.Collections;

public class SelectedIcon : MonoBehaviour {
	public Unit selectUnit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector3(  selectUnit.transform.position.x + 0.5f, 0,  selectUnit.transform.position.z + 0.5f );

		transform.Rotate(0,50 * Time.deltaTime,0);
	}
}
