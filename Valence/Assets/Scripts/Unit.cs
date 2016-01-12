using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public Vector2 currentPosition, lastPosition;
	public ExploreMode_GameController _GameController;
	public GameObject _tile;

	public Vector3 facing;

	public bool canMove;
	
	public bool movePressed;
	public bool attackPressed;
	public bool grabPressed;
	public bool waitPressed;
	
	public bool turnComplete;
	
	public int actionPoints;
	
	public int health;
	
	public int movement;
	public int attackRange;
	
	public bool moving, hasAttacked;
	public Vector2 target;
	
	public float moveSpeed = 1;

	public List<ExploreMode_GameController.Node> currentPath = null;

	public bool isElite;
	public int EliteState;
	public List<Unit> FolkUnits;
	public List<Unit> FolkUnitsWithinView;
	public Vector2[] knownPosition;

	public Vector2 startPosition;
	public string EliteBehaviour;
	public List<Vector2> PatrolPoints;
	public int currentPatrolPoint;

	public float fieldOfViewAngle = 90;
	// Use this for initialization
	void Start () {
		currentPosition = new Vector2( transform.position.x, transform.position.z );
		startPosition = currentPosition;
		moving = false;
		movePressed = false;
		attackPressed = false;
		grabPressed = false;
		waitPressed = false;
		turnComplete = false;
		hasAttacked = false;

		currentPatrolPoint = 0;

		knownPosition = new Vector2[_GameController.GetNumberOfPlayerUnits()];
		for( int  i = 0; i < _GameController.GetNumberOfPlayerUnits(); i++ ){
			knownPosition [i] = new Vector2 (-1,-1);
		}
		//_GameController.tiles [(int)currentPosition.x,(int)currentPosition.y] = 2;
	}
	
	// Update is called once per frame
	void Update () {

		if (actionPoints <= 0) {
			turnComplete = true;
		}
		if(currentPath != null) {
			
			int currNode = 0;
			
			while( currNode < currentPath.Count-1 ) {
				//Debug.Log ( "generating line" );
				Vector3 start = new Vector3( currentPath[currNode].x +0.5f, 1, currentPath[currNode].y+0.5f );
				Vector3 end   = new Vector3( currentPath[currNode+1].x+0.5f,1, currentPath[currNode+1].y+0.5f );
				
				currNode++;
			}
			
		}
		MoveNextTile ();
	}
	public void MoveNextTile() {
		float remainingMovement = movement;
		
		while(remainingMovement > 0) {
			if(currentPath==null)
				return;
			
			// Get cost from current tile to next tile
			remainingMovement -= _GameController.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y );

			Vector2 tempFacing = new Vector2( currentPath[1].x, currentPath[1].y ) - currentPosition;
			tempFacing.Normalize();
			facing = new Vector3( tempFacing.x, 0, tempFacing.y );
			//_GameController.tiles [(int)currentPosition.x,(int) currentPosition.y] = 1;
			// Move us to the next tile in the sequence
			currentPosition.x = currentPath[1].x;
			currentPosition.y = currentPath[1].y;
			//_GameController.tiles [(int)currentPosition.x,(int) currentPosition.y] = 2;
			transform.position = new Vector3(currentPosition.x, 0, currentPosition.y);
			
			// Remove the old "current" tile
			currentPath.RemoveAt(0);

			if( isElite ){
				EliteObserve ();
			} else {
				foreach( Unit eU in _GameController.elite ){
					eU.EliteObserve();
				}
			}

			if(currentPath.Count == 1) {
				// We only have one tile left in the path, and that tile MUST be our ultimate
				// destination -- and we are standing on it!
				// So let's just clear our pathfinding info.
				currentPath = null;
				canMove = false;
			}
		} 
		currentPath = null;
		canMove = false;
	}
	
	
	public void Move( Vector2 targetPosition ){
		_GameController.GeneratePathTo ((int)targetPosition.x, (int)targetPosition.y);
	}

	public bool withinMoveRange( Vector2 targetPosition ){
		if (movement >= getDistance (currentPosition, targetPosition)) {
			return true;
		} else {
			return false;
		}
	}
	
	public int getDistance( Vector2 p1, Vector2 p2 ){
		int horD = (int) Mathf.FloorToInt( Vector2.Distance ( new Vector2( p2.x,0), new Vector2( p1.x,0) ) );
		int verD = (int) Mathf.FloorToInt( Vector2.Distance ( new Vector2( 0,p2.y), new Vector2( 0,p1.y) ) );
		int d = horD + verD;
		return d;
	}
	
	void MoveObject(Vector3 currentPos,  Vector3 endPos)
	{
		if (moving) {
			endPos = new Vector3 (endPos.x, 0, endPos.y);
			transform.position = Vector3.Lerp (currentPos, endPos, Time.deltaTime);
			if (transform.position == endPos) {
				moving = false;
			}
		}
	}

	public void EliteObserve(){
		int loopIndex = 0;
		foreach (Unit fU in FolkUnits) {

			Vector3 direction = fU.transform.position - transform.position;
			
			//Debug.Log ( direction.normalized.x + " " + direction.normalized.y + " " + direction.normalized.z );
			float angle = Vector3.Angle(direction, facing);
			
			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				RaycastHit hit;
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(transform.position + new Vector3(0,1.5f,0) , direction.normalized, out hit, 6.0f ) )
				{
					// ... and if the raycast hits the player...
					if(hit.collider.gameObject == fU.gameObject )
					{
						if( !FolkUnitsWithinView.Contains (fU) ){
							// ... the player is in sight.
							Debug.Log ("SPOTTED");
							FolkUnitsWithinView.Add (fU);
							
							// Set the last global sighting is the players current position.
							knownPosition[loopIndex] = fU.currentPosition;
						} else {
							// Set the last global sighting is the players current position.
							knownPosition[loopIndex] = fU.currentPosition;
						}
					} else {
						FolkUnitsWithinView.Remove (fU);
					}
				} else {
					if( FolkUnitsWithinView.Contains (fU) )
						FolkUnitsWithinView.Remove (fU);
				}
			} else {
				if( FolkUnitsWithinView.Contains (fU) )
					FolkUnitsWithinView.Remove (fU);
			}
			loopIndex++;
			/**
			if( Vector2.Distance ( currentPosition, fU.currentPosition ) < 6 ){
				if( !FolkUnitsWithinView.Contains( fU ) ){
					FolkUnitsWithinView.Add (fU);
					knownPosition.Add (fU.currentPosition);
				} else {
					knownPosition.Add (fU.currentPosition);
				}
			} else {
				if( FolkUnitsWithinView.Contains( fU ) ){
					FolkUnitsWithinView.Remove (fU);
				}
			}
			**/
		}
		for (int i = 0; i < knownPosition.Length; i++) {
			if (currentPosition == knownPosition [i]) {
				knownPosition [i] = new Vector2 (-1, -1);
			}
		}
	}

	public void EliteDetermineState(){
		int knownPositionCount = 0;
		Vector2 negVector = new Vector2 (-1, -1);

		for (int i = 0; i < knownPosition.Length; i++) {
			if( knownPosition[i] != negVector ){
				knownPositionCount++;
			}
		}

		if (knownPositionCount <= 0) {
			EliteState = 1; // Idle
		} else if (knownPositionCount > 0 && FolkUnitsWithinView.Count > 0) {
			EliteState = 3; // Attack
		} else if (knownPositionCount > 0) {
			EliteState = 2; // Search
		}

		switch (EliteState) 
		{
			case 1:
				// conduct default behaviour
				Debug.Log ( "Idle State");
				break;
			case 2:
				Debug.Log ( "Pursuit State");
				break;
			case 3:
				Debug.Log ( "Eliminate State");
				break;
			default:
				Debug.Log ( "No State Defined");
				break;

		}
	}

	public Vector2 EliteCalcOptMoveTile(){
		Vector2 targetPosition = currentPosition;
		int myMapSize = _GameController.mapSize;
		if (EliteState == 1) {
			float distToTargetPosition = 99.9f;
			if( currentPosition != startPosition && EliteBehaviour == "Guard" ){
				targetPosition = startPosition;
			} else if ( EliteBehaviour == "Patrol" ){
				if( currentPosition == PatrolPoints[currentPatrolPoint] ){
					currentPatrolPoint++;
					if( currentPatrolPoint >= PatrolPoints.Count ){
						currentPatrolPoint = 0;
					}
				}
				targetPosition = PatrolPoints[currentPatrolPoint];
			}
		}
		else if (EliteState == 2) {
			Vector2 negVector = new Vector2 (-1, -1);
			
			for (int i = 0; i < knownPosition.Length; i++) {
				if( knownPosition[i] != negVector ){
					targetPosition = knownPosition[i];
				}
			}

		} else if (EliteState == 3) {
			targetPosition = FolkUnitsWithinView[0].currentPosition;
		}
		return targetPosition;
	}

	void Attack(Unit targetUnit){
	}

	bool calcChanceToHit(float distance, float weaponChance){
		bool hit = false;
		return hit;
	}
}
