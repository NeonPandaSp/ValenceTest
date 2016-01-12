using UnityEngine;
using System.Collections;

public class SelectAttackTarget : MonoBehaviour {
	public ExploreMode_GameController _GameController;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown(){
		_GameController.selectedUnit.actionPoints--;

		/****************************************************************************/
		/**

		ZACH PUT TRIGGER FOR MINIGAME HERE

		**/
		/****************************************************************************/

		/****************************************************************************/
		/**

		IF MINIGAME IS SUCCESSFUL CALL RemoveEnemy()

		**/
		/****************************************************************************/
		RemoveEnemy ();
	}

	void RemoveEnemy(){
		_GameController.elite.Remove (this.transform.parent.gameObject.GetComponent<Unit>());
		this.transform.GetComponentInParent<EliteUnit> ().health--;
		_GameController.selectedUnit.hasAttacked = true;
		//this.transform.parent.gameObject.GetComponent<EliteUnit>().enabled = false;
		this.transform.parent.gameObject.SetActive (false);
		_GameController.disableAttackBox();
	}
}
