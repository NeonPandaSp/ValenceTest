using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ExploreMode_GameController : MonoBehaviour {
	
	public int mapSize = 50;
	public int[,] tiles;
	public Node[,] graph;
	
	public TileType[] tileTypes;

	public List<Unit> folk = new List<Unit>();
	public List<Unit> elite = new List<Unit>();

	public Unit selectedUnit;
	public int selectedIndex;
	public SelectedIcon icon;

	public int currentElite = -1;
	public float bufferValue;
	public float currentTime;

	public bool newElite;
	public bool newAction;

	public GameObject[,] tileVisuals;
	public GameObject moveTile;
	public List<GameObject> moveTiles = new List<GameObject>();

	public GameObject pointObject;
	int[,] borderVectors;

	public int GameState;
	public Transform GUIFrame;
	public Fade playerTurnObject;
	public Fade enemyTurnObject;

	public CameraTargetController cameraObject;

	public Button moveButton, attackButton, pickUpButton, waitButton;

	// Use this for initialization
	void Start () {
		tiles = new int[mapSize,mapSize];
		tileVisuals = new GameObject[mapSize, mapSize];
		
		borderVectors = new int[mapSize, mapSize];

		GenerateNodeGraph ();

		for( int x = 0; x < mapSize; x++){
			for( int y = 0; y < mapSize; y++){
				int rand = (int) Random.Range( 0, 100 );
				if( rand < 100 ){
					tiles[x,y] = 1;
					//Instantiate ( tileTypes[0].visualPrefab,new Vector3( x+0.5f, 0, y+0.5f ), Quaternion.identity );
				} else{
					tiles[x,y] = 2;
					Instantiate ( tileTypes[2].visualPrefab,new Vector3( x+0.5f, 0, y+0.5f ), Quaternion.identity );
				}
			}
		}

		GameState = 0;

		selectedUnit = folk [0];
		selectedIndex = 0;

		icon.selectUnit = selectedUnit;
		//GenerateMovementRange ((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y);

		GenerateMap();

		cameraObject.MoveCameraTo (cameraObject.transform.position, folk [0].transform.position);
	}

	// Update is called once per frame
	void Update () {



		switch (GameState)
		{
		case 1:
			int movementRemaing = GetNumberOfPlayerUnits();
			foreach( Unit fU in folk ){
				if( fU.turnComplete == false ){
					break;
				} else {
					movementRemaing--;
				}
				
				if( movementRemaing == 0 ){
					enemyTurnObject.ReStart();
					GameState = 2; 
					foreach( Unit eU in elite ){
						currentElite = -1;
						eU.canMove = true;
						selectedUnit = elite[0];
					}
				}
			}

			if( selectedUnit.canMove ){
				moveButton.interactable = true;
			} else {
				moveButton.interactable = false;
			} 

			if( canAttack(selectedUnit)) {
				attackButton.interactable = true;
			} else {
				attackButton.interactable = false;
			} 

			break;
		case 2:
			int eMovementRemaing = GetNumberOfEnemyUnits();
			foreach( Unit eU in elite ){
				if( eU.isActiveAndEnabled ){
					if( eU.canMove == true ){
						break;
					} else {
						eMovementRemaing--;
					}
				}
			}
			if( eMovementRemaing == 0 ){

				GameState = 1;
				playerTurnObject.ReStart();
				
				selectedUnit = folk[0];
				selectedIndex = 0;

				MoveIcon();
				cameraObject.MoveCameraTo( cameraObject.transform.position, selectedUnit.transform.position );
				foreach( Unit fU in folk ){
					fU.turnComplete = false;
					fU.actionPoints = 2;
					fU.canMove = true;
					fU.movePressed = false;
					fU.attackPressed = false;
					fU.grabPressed = false;
					fU.waitPressed = false;
					fU.hasAttacked = false;
				}
				break;
			} else {
				EnemyTurn();
			}

			break;
		default:
			GameState = 1;
			foreach( Unit fU in folk ){
				fU.canMove = true;
			}
			break;
		}





		// LINE MOVEMENT RADIUS CODE DO NOT DELETE
//		List<Vector3> lineVertex = new List<Vector3> ();
//		Vector3 lastVector = new Vector3( -1, -1, -1 );
//		
//		for( int i = 0; i < mapSize; i++ ){
//			for( int j = 0; j < mapSize; j++ ){
//				if( borderVectors[i,j] <= 3 && borderVectors[i,j] >= 1 ){
//					// border vertex!
//					lineVertex.Add ( new Vector3( i, 0.1f, j ) );
//					//Debug.Log ( "borderVectors added To Line Vertex" );
//					//Instantiate ( pointObject, new Vector3( i, 0.5f, j ), Quaternion.identity );
//				}
//			}
//		}
//		//Instantiate ( pointObject, lineVertex[0], Quaternion.identity );
//		List<Vector3> orderedVertex = new List<Vector3>();
//		orderedVertex.Add ( lineVertex[0] );
//		int index = 0;
//		while (index < lineVertex.Count) {
//			if( lineVertex.Contains( new Vector3( orderedVertex[index].x, 0.1f, orderedVertex[index].z-1 ) ) && 
//			   !orderedVertex.Contains( new Vector3( orderedVertex[index].x,0.1f, orderedVertex[index].z-1 ) ) &&
//			   CostToEnterTile((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y, (int)orderedVertex[index].x, (int)orderedVertex[index].z-1) < selectedUnit.movement  ){
//				//Down
//				orderedVertex.Add ( new Vector3( orderedVertex[index].x, 0.1f, orderedVertex[index].z-1 ) );
//				
//				
//			} else if( lineVertex.Contains( new Vector3( orderedVertex[index].x + 1, 0.1f, orderedVertex[index].z ) ) && 
//			        !orderedVertex.Contains( new Vector3( orderedVertex[index].x + 1, 0.1f, orderedVertex[index].z ) ) &&
//			          CostToEnterTile((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y, (int)orderedVertex[index].x, (int)orderedVertex[index].z) < selectedUnit.movement ){
//				//Right
//				orderedVertex.Add ( new Vector3( orderedVertex[index].x + 1, 0.1f, orderedVertex[index].z ) );
//				
//			} else if( lineVertex.Contains( new Vector3( orderedVertex[index].x, 0.1f, orderedVertex[index].z+1) ) && 
//			          !orderedVertex.Contains( new Vector3( orderedVertex[index].x , 0.1f, orderedVertex[index].z+1 ) ) &&
//			         CostToEnterTile((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y, (int)orderedVertex[index].x-1, (int)orderedVertex[index].z) < selectedUnit.movement){
//				//Up
//				orderedVertex.Add ( new Vector3( orderedVertex[index].x, 0.1f, orderedVertex[index].z+1) );
//			} else if( lineVertex.Contains( new Vector3( orderedVertex[index].x -1, 0.1f, orderedVertex[index].z ) ) && 
//			         !orderedVertex.Contains( new Vector3( orderedVertex[index].x -1 ,0.1f, orderedVertex[index].z ) ) &&
//			          CostToEnterTile((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y, (int)orderedVertex[index].x-1, (int)orderedVertex[index].z-1) < selectedUnit.movement ){
//				//Left
//				orderedVertex.Add ( new Vector3( orderedVertex[index].x -1, 0.1f, orderedVertex[index].z ) );
//				
//			}  else {
//				orderedVertex.Add( orderedVertex[0] );
//
//			}
//			index++;
//
//		}
//		for (int i = 0; i < 12; i++) {
//			//Instantiate ( pointObject, orderedVertex[i], Quaternion.identity );
//		}
//		foreach( Vector3 v in orderedVertex ){
//			if( lastVector.x != -1 || lastVector.y != -1 || lastVector.z != -1 ){
//				//Debug.DrawLine(lastVector, v, Color.blue);
//			}
//			lastVector = v;
//		}

	}

	public class Node{
		public List<Node> neighbours;
		public Vector2 nodePos;
		public int x, y;

		public Node(){
			neighbours = new List<Node>();
		}
		
		public float DistanceTo(Node n) {
			if(n == null) {
				Debug.LogError("WTF?");
			}
			
			return Vector2.Distance(
				new Vector2(x, y),
				new Vector2(n.x, n.y)
				);
		}
	}
	public void DestroyMovementRange(){
		foreach( GameObject n in moveTiles){
			Destroy (n);
		}
		moveTiles.Clear ();
	}
	public void GenerateMovementRange(int x, int y){
		int[,] vectors = new int[mapSize,mapSize];

		foreach( GameObject n in moveTiles){
			Destroy (n);
		}
		moveTiles.Clear ();
		for (int i = -selectedUnit.movement; i <= selectedUnit.movement; i++) {
			for (int j=-selectedUnit.movement; j <= selectedUnit.movement; j++) {

				if ((int)selectedUnit.currentPosition.x + i >= 0 && (int)selectedUnit.currentPosition.x + i < mapSize && 
					(int)selectedUnit.currentPosition.y + j >= 0 && (int)selectedUnit.currentPosition.y + j < mapSize) {

					if (selectedUnit.withinMoveRange ( new Vector2( (int)selectedUnit.currentPosition.x + i, (int)selectedUnit.currentPosition.y + j) ) && 
					    GeneratePathTo ((int)selectedUnit.currentPosition.x + i,(int)selectedUnit.currentPosition.y + j,0) ) {
						//int tPx = (int)selectedUnit.currentPosition.x + i;
						//int tPy = (int)selectedUnit.currentPosition.y + j;

						//vectors[tPx, tPy] += 1;
						//vectors[tPx+1, tPy] += 1;
						//vectors[tPx+1, tPy+1] += 1;
						//vectors[tPx, tPy+1] += 1;

						GameObject tempObject = (GameObject) Instantiate( moveTile , new Vector3((int)selectedUnit.currentPosition.x + i, 0, (int)selectedUnit.currentPosition.y + j), Quaternion.identity ); 
						tempObject.transform.SetParent( this.transform );
						moveTiles.Add( tempObject );
						//Debug.Log ("Loaded"+i+" "+j);
						//GameObject ob = (GameObject)Instantiate (moveTile, new Vector3 ((int)selectedUnit.currentPosition.x + i, 0,(int)selectedUnit.currentPosition.y + j), Quaternion.identity);
						//ob.transform.SetParent (selectedUnit.transform);
					} 
				}
			}
		}
		borderVectors = vectors;


	}

	public void GenerateMap(){
		for( int x = 0; x < mapSize; x++){
			for( int y = 0; y < mapSize; y++){
				/**
				TileType tt = tileTypes[ tiles[x,y] ];
				GameObject tileVisual = (GameObject) Instantiate ( tt.visualPrefab, new Vector3( x+0.5f, 0 , y+0.5f ), Quaternion.identity );
				tileVisual.transform.SetParent(this.gameObject.transform);
				tileVisuals[x,y] = tileVisual;
				**/
			}
		}
	}

	void GenerateNodeGraph(){
		graph = new Node[mapSize, mapSize];
		for (int x=0; x < mapSize; x++) {
			for (int y=0; y < mapSize; y++) {
				graph [x, y] = new Node ();
			}
		}
		for (int x=0; x < mapSize; x++) {
			for( int y=0; y < mapSize; y++){
				graph[x,y].nodePos = new Vector2(x,y);
				graph[x,y].x = x;
				graph[x,y].y = y;
				if( x > 0 )
					graph[x,y].neighbours.Add ( graph[x-1, y ] );
				if( x < mapSize - 1 )
					graph[x,y].neighbours.Add (graph[x+1, y ] );
				if( y > 0 )
					graph[x,y].neighbours.Add (graph[x, y-1 ] );
				if( y < mapSize - 1 )
					graph[x,y].neighbours.Add (graph[x, y+1 ] );
			}
		}

	}
	public void MoveIcon(){
		icon.selectUnit = selectedUnit;
		//selectedUnit.canMove = true;
	}

	public void GeneratePathTo(int x, int y) {
		// Clear out our unit's old path.
		selectedUnit.currentPath = null;
		if( UnitCanEnterTile(x,y) == false ) {

			// We probably clicked on a mountain or something, so just quit out.
			return;
		}
		
		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
		
		// Setup the "Q" -- the list of nodes we haven't checked yet.
		List<Node> unvisited = new List<Node>();
		
		Node source = graph[
		                    (int) selectedUnit.currentPosition.x, 
		                    (int) selectedUnit.currentPosition.y
		                    ];
		
		Node target = graph[
		                    x, 
		                    y
		                    ];
		
		dist[source] = 0;
		prev[source] = null;
		
		// Initialize everything to have INFINITY distance, since
		// we don't know any better right now. Also, it's possible
		// that some nodes CAN'T be reached from the source,
		// which would make INFINITY a reasonable value
		foreach(Node v in graph) {
			if(v != source) {
				dist[v] = Mathf.Infinity;
				//dist[v] = selectedUnit.movement;
				prev[v] = null;
			}
			
			unvisited.Add(v);
		}
		
		while(unvisited.Count > 0) {
			// "u" is going to be the unvisited node with the smallest distance.
			Node u = null;
			
			foreach(Node possibleU in unvisited) {
				if(u == null || dist[possibleU] < dist[u]) {
					u = possibleU;
				}
			}
			
			if(u == target) {
				break;	// Exit the while loop!
			}
			
			unvisited.Remove(u);
			
			foreach(Node v in u.neighbours) {
				//float alt = dist[u] + u.DistanceTo(v);
				float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if( alt < dist[v] ) {
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}
		
		// If we get there, the either we found the shortest route
		// to our target, or there is no route at ALL to our target.
		
		if(prev[target] == null) {
			// No route between our target and the source
			return;
		}
		
		List<Node> currentPath = new List<Node>();
		
		Node curr = target;
		
		// Step through the "prev" chain and add it to our path
		while(curr != null) {
			currentPath.Add(curr);
			curr = prev[curr];
		}
		
		// Right now, currentPath describes a route from out target to our source
		// So we need to invert it!
		
		currentPath.Reverse();
		selectedUnit.currentPath = currentPath;
		/**
		if (currentPath.Count - 1 <= selectedUnit.movement) {
			selectedUnit.currentPath = currentPath;
		}
		**/
	}

	public bool GeneratePathTo(int x, int y, int w ) {
		// Clear out our unit's old path.
		selectedUnit.currentPath = null;
		if( UnitCanEnterTile(x,y) == false ) {
			// We probably clicked on a mountain or something, so just quit out.
			return false;
		}
		
		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
		
		// Setup the "Q" -- the list of nodes we haven't checked yet.
		List<Node> unvisited = new List<Node>();
		
		Node source = graph[
		                    (int) selectedUnit.currentPosition.x, 
		                    (int) selectedUnit.currentPosition.y
		                    ];
		
		Node target = graph[
		                    x, 
		                    y
		                    ];
		
		dist[source] = 0;
		prev[source] = null;
		
		// Initialize everything to have INFINITY distance, since
		// we don't know any better right now. Also, it's possible
		// that some nodes CAN'T be reached from the source,
		// which would make INFINITY a reasonable value
		foreach(Node v in graph) {
			if(v != source) {
				dist[v] = Mathf.Infinity;
				//dist[v] = selectedUnit.movement;
				prev[v] = null;
			}
			
			unvisited.Add(v);
		}
		
		while(unvisited.Count > 0) {
			// "u" is going to be the unvisited node with the smallest distance.
			Node u = null;
			
			foreach(Node possibleU in unvisited) {
				if(u == null || dist[possibleU] < dist[u]) {
					u = possibleU;
				}
			}
			
			if(u == target) {
				break;	// Exit the while loop!
			}
			
			unvisited.Remove(u);
			
			foreach(Node v in u.neighbours) {
				//float alt = dist[u] + u.DistanceTo(v);
				float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if( alt < dist[v] ) {
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}
		
		// If we get there, the either we found the shortest route
		// to our target, or there is no route at ALL to our target.
		
		if (prev [target] == null) {
			// No route between our target and the source
			return false;
		}
		
		List<Node> currentPath = new List<Node>();
		
		Node curr = target;
		
		// Step through the "prev" chain and add it to our path
		while(curr != null) {
			currentPath.Add(curr);
			curr = prev[curr];
		}
		// Right now, currentPath describes a route from out target to our source
		// So we need to invert it!
		
		currentPath.Reverse();
		if (currentPath.Count - 1 <= selectedUnit.movement) {
			return true;
		} else {
			return false;
		}
		
	}

	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY) {
		
		TileType tt = tileTypes[ tiles[targetX,targetY] ];
		if(UnitCanEnterTile(targetX, targetY) == false)
			return Mathf.Infinity;
		
		float cost = tt.movementCost;
		
		if( sourceX!=targetX && sourceY!=targetY) {
			// We are moving diagonally!  Fudge the cost for tie-breaking
			// Purely a cosmetic thing!
			cost += 0.001f;
		}
		//Debug.Log (cost);
		return cost;
		
	}

	public bool UnitCanEnterTile(int x, int y) {
		
		// We could test the unit's walk/hover/fly type against various
		// terrain flags here to see if they are allowed to enter the tile.
		return tileTypes[ tiles[x,y] ].isWalkable;
	}
	

	public void EnemyTurn(){
		bool nextElite = false;

		if (currentElite == -1) {
			nextElite = true;
		}

		updateBuffer ();

		if (checkBuffer () && newElite ) {
			elite[currentElite].EliteObserve();
			elite[currentElite].EliteDetermineState();
			elite[currentElite].Move (elite[currentElite].EliteCalcOptMoveTile());
			elite[currentElite].canMove = false;
			newElite = false;
			newAction = true;
			startBuffer(1);
		} 

		if (checkBuffer () && newAction) {
			//elite[currentElite].action ();
			newAction = false;
			newElite = false;
			nextElite = true;
		}

		
		
		if (nextElite) {
			currentElite++;
			int whileCount = 0;
			
			if( currentElite >= GetNumberOfEnemyUnits() ){
				currentElite = 0;
			}

			while( !elite[currentElite].isActiveAndEnabled ){
				currentElite++;
				if( currentElite >= GetNumberOfEnemyUnits() ){
					currentElite = 0;
				}
			}
			selectedUnit = elite[ currentElite ];
			newElite = true;
			nextElite = false;
			
			cameraObject.MoveCameraTo(cameraObject.transform.position, elite[currentElite].transform.position);
			startBuffer(2);
		}
	}

	void startBuffer( int bufferLength ){
		bufferValue = bufferLength;
		currentTime = 0;
	}

	void updateBuffer(){
		currentTime += Time.deltaTime;
	}

	bool checkBuffer(){
		if (currentTime >= bufferValue) {
			return true;
			bufferValue = 0;
			currentTime = 0;
		}
		return false;
	}

	public int GetNumberOfPlayerUnits(){
		int count = 0;
		foreach (Unit fU in folk) {
			if( fU.isActiveAndEnabled ){
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfEnemyUnits(){
		int count = 0;
		foreach (Unit eU in elite) {
			if( eU.isActiveAndEnabled ){
				count++;
			}
		}
		return count;
	}

	public bool canAttack(Unit fU){
		foreach (Unit eU in elite) {
			if( eU.health > 0 ){
				if( Vector3.Distance ( eU.transform.position, fU.transform.position ) <= 5 ){
					if( fU.actionPoints >= 1 && !fU.hasAttacked ){
						return true;
					}
				}
			}
		}
		return false;
	}

	public void enableAttackBox(Unit fU){
		foreach (Unit eU in elite) {
			if( Vector3.Distance ( eU.transform.position, fU.transform.position ) <= 5 ){
				if( eU.health > 0 ){
					Transform[] eUChildren = eU.GetComponentsInChildren<Transform>(true);
					foreach( Transform ob in eUChildren ){
						if ( ob.tag == "enemyTouchBox" ){
							ob.gameObject.SetActive(true);
						}
					}
				}
			}
		}
	}
	public void disableAttackBox(){
		foreach (Unit eU in elite) {
			if( Vector3.Distance ( eU.transform.position, selectedUnit.transform.position ) <= 5 ){
				Transform[] eUChildren = eU.GetComponentsInChildren<Transform>(true);
				foreach( Transform ob in eUChildren ){
					if ( ob.tag == "enemyTouchBox" ){
						ob.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	public void selectedNextUnit(){
		DestroyMovementRange ();
		int i = selectedIndex;
		if( GameState == 1 ){
			i += 1;
			if( i > GetNumberOfPlayerUnits()-1 ){
				i = 0;
			} 
			int dCount = 0;
			while( !folk[i].isActiveAndEnabled && folk[i].actionPoints > 0 ){
				i++;
				if( i > GetNumberOfPlayerUnits()-1 ){
					i = 0;
				} 
				dCount++;
				if( dCount == GetNumberOfPlayerUnits() ){
					//GameOver
					Debug.Log ("GAMEOVER");
					//break;
				}
			}
			selectedUnit = folk[i];
			if( selectedUnit.canMove ){
				selectedUnit.movePressed = false;
			}
			selectedIndex = i;
			MoveIcon();
			cameraObject.MoveCameraTo( cameraObject.transform.position, selectedUnit.transform.position );
			
		}
	}

}
