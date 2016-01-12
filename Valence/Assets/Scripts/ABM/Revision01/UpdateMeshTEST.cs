using UnityEngine;
using System.Collections;
using Pathfinding;

public class UpdateMeshTEST : MonoBehaviour {



	void OnGUI(){
		if (GUI.Button (new Rect (30, 30, 200, 30), "Update World")) {
			//GameObject obstical = GameObject.FindWithTag("bound");
						
			foreach (GameObject obstcale in GameObject.FindGameObjectsWithTag("prop")) {
				AstarPath.active.UpdateGraphs(obstcale.gameObject.GetComponent<Collider>().bounds);
			}


		}
	}

}
