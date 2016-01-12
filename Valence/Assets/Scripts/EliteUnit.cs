using UnityEngine;
using System.Collections.Generic;

public class EliteUnit : MonoBehaviour {

	public ExploreMode_GameController _GameController;
	public Vector2 currentPosition, lastPosition;

	public int maxMovement;
	public int state; // states: 0 == default, 1 == wander, 2 == hostile
	public bool canMove;

	public int health = 1;

	public List<ExploreMode_GameController.Node> currentPath = null;
	// Use this for initialization
	void Start () {
		//maxMovement = 5;
		state = 1;
	}
	
	// Update is called once per frame
	void Update () {
		currentPosition = new Vector2 (transform.position.x, transform.position.z);
	}

	public void checkState(){

	}
	public void move(){
		switch (state)
		{
		case 1:
			float minDist = 99.9f;
			Vector2 minDistTarget = new Vector2( transform.position.x, transform.position.z);
			for( int i = (int) currentPosition.x-maxMovement; i <= (int) currentPosition.x+maxMovement; i++){
				for( int j = (int) currentPosition.y-maxMovement; j <= (int) currentPosition.y+maxMovement; j++){
					if( i < 50 && i > 0 && j < 50 && j > 0 ){
							foreach (Unit fU in _GameController.folk) {
								float dist = Vector3.Distance( new Vector3(i,0,j), fU.transform.position );
								if( dist < minDist && dist > 0 ){
									minDist = dist;
									minDistTarget = new Vector2(i,j);
								}
							}
					}
				}
			}
			if( minDist < 99 && canMove ){
				transform.position = new Vector3( minDistTarget.x, 0, minDistTarget.y );
			}
			canMove = false;
			break;
		case 2:
			Debug.Log ("NO ACTION");
			canMove = false;
			break;
		default:
			break;
		}
	}
	public void action(){
		Debug.Log ("No Action");
		Unit targetFolk = null;
		foreach (Unit fU in _GameController.folk) {
			float dist = Vector3.Distance( transform.position, fU.transform.position );
			if( dist > 0 && dist < 2 ){
				targetFolk = fU;
			}
		}
		if (targetFolk != null) {
			Attack(targetFolk);
		}
	}

	public void Attack( Unit target ){
		target.health--;
		if (target.health <= 0) {
			target.gameObject.SetActive(false);
			_GameController.folk.Remove( target );
		}
	}
}
