using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileType : MonoBehaviour {

	public string name;
	public GameObject visualPrefab;

	public bool isWalkable;
	public float movementCost;

	void Start(){

	}
	//public bool isWalkable = true;
	void Update(){
		//Destroy (this.gameObject, 3.0f);
		/**
		if (Input.GetKeyDown (KeyCode.Return)) {
			//Destroy (this.gameObject);
		}
		**/
	}

}
