using UnityEngine;
using System.Collections;

public class InputController_Explore : MonoBehaviour {
	TileMap _tileMap;
	generateZone _generateZone;
	public ExploreMode_GameController _GameController;
	public FolkUnit _myFolkUnit;
	
	Vector2 currentTile;
	
	Vector2 rootMousePos;
	
	GameObject myHoverObject;
	
	public Material redMat, blueMat, yellowMat, greenMat;
	
	string currentColor;
	
	bool generate;
	
	void Start(){
		_tileMap = GetComponent<TileMap> ();
		myHoverObject = (GameObject) Instantiate (Resources.Load("Tile"), new Vector3 (0, 0, 0), Quaternion.identity);

		currentColor = "blue";
	}
	
	
	public void selectedMaterial( string color ){
		if (color == "blue") {
			myHoverObject.GetComponentInChildren<MeshRenderer> ().material = blueMat;
			currentColor = "blue";
		} else if (color == "yellow") {
			myHoverObject.GetComponentInChildren<MeshRenderer> ().material = yellowMat;
			currentColor = "yellow";
		} else if (color == "green") {
			myHoverObject.GetComponentInChildren<MeshRenderer> ().material = greenMat;
			currentColor = "green";
		}
	}
	

	

	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		
		if ( GetComponent<Collider>().Raycast (ray, out hitInfo, Mathf.Infinity)) {

			int x = Mathf.FloorToInt( hitInfo.point.x  / _tileMap.tileSize );
			int z = Mathf.FloorToInt( hitInfo.point.z / _tileMap.tileSize );
			currentTile.x = x;
			currentTile.y = z;
			
			if( currentTile.x <= _tileMap.worldSizeX && currentTile.y <= _tileMap.worldSizeZ && currentTile.x >= 0 && currentTile.y >= 0 ){
				myHoverObject.transform.position = new Vector3( currentTile.x, 0, currentTile.y );
			}
			
		} else {
			
		}

		if( Input.GetMouseButton(0) ){

			if( _GameController.selectedUnit.canMove ){
				if( _GameController.selectedUnit.withinMoveRange( currentTile ) ){
					_GameController.selectedUnit.Move(currentTile);
				} else {
					// not within range
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			int i = _GameController.selectedIndex;

			i += 1;
			if( i <= 3 ){

				Debug.Log ( i );
				_GameController.selectedUnit = _GameController.folk[i];
				_GameController.GenerateMovementRange((int)_GameController.selectedUnit.currentPosition.x, (int)_GameController.selectedUnit.currentPosition.y);
				_GameController.MoveIcon();
				_GameController.selectedIndex = i;
			} else {
				i = 0;
				_GameController.selectedUnit = _GameController.folk[i];
				_GameController.GenerateMovementRange((int)_GameController.selectedUnit.currentPosition.x,(int) _GameController.selectedUnit.currentPosition.y);
				_GameController.MoveIcon();
				_GameController.selectedIndex = i;
			}
		}
	}


}
