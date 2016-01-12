using UnityEngine;
using System.Collections.Generic;

public class FolkUnit : MonoBehaviour {
	public Vector2 currentPosition, lastPosition;
	public ExploreMode_GameController _GameController;
	public GameObject _tile;

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

	//public LineRenderer myLine;


	// Use this for initialization
	void Start () {
		currentPosition = new Vector2( transform.position.x, transform.position.z );
		moving = false;
		movePressed = false;
		attackPressed = false;
		grabPressed = false;
		waitPressed = false;
		turnComplete = false;
		hasAttacked = false;
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
		float remainingMovement = moveSpeed;

		while(remainingMovement > 0) {
			if(currentPath==null)
				return;
			
			// Get cost from current tile to next tile
			remainingMovement -= _GameController.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y );
			
			// Move us to the next tile in the sequence
			currentPosition.x = currentPath[1].x;
			currentPosition.y = currentPath[1].y;
			
			transform.position = new Vector3(currentPosition.x, 0, currentPosition.y);

			// Remove the old "current" tile
			currentPath.RemoveAt(0);
			
			if(currentPath.Count == 1) {
				// We only have one tile left in the path, and that tile MUST be our ultimate
				// destination -- and we are standing on it!
				// So let's just clear our pathfinding info.
				currentPath = null;
				canMove = false;
			}
		}
		
	}


	public void Move( Vector2 targetPosition ){
		_GameController.GeneratePathTo ((int)targetPosition.x,(int) targetPosition.y);
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

		// int d = Mathf.FloorToInt( Vector2.Distance (p1, p2) );

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
}
