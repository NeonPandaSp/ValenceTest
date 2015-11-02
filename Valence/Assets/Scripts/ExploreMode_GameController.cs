using UnityEngine;
using System.Collections.Generic;

public class ExploreMode_GameController : MonoBehaviour {
	
	static int mapSize = 50;
	public int[,] tiles;
	public Node[,] graph;
	
	public TileType[] tileTypes;

	public FolkUnit[] folk = new FolkUnit[4];
	public GameObject[] elite = new GameObject[4];

	public FolkUnit selectedUnit;
	public int selectedIndex;
	public SelectedIcon icon;

	public GameObject[,] tileVisuals;
	public GameObject moveTile;
	public GameObject pointObject;
	int[,] borderVectors = new int[mapSize,mapSize];

	// Use this for initialization
	void Start () {
		tiles = new int[mapSize,mapSize];
		tileVisuals = new GameObject[mapSize, mapSize];

		GenerateNodeGraph ();

		for( int x = 0; x < mapSize; x++){
			for( int y = 0; y < mapSize; y++){
				int rand = (int) Random.Range( 0, 100 );
				if( rand < 100 ){
					tiles[x,y] = 1;
					Instantiate ( tileTypes[0].visualPrefab,new Vector3( x+0.5f, 0, y+0.5f ), Quaternion.identity );
				} else{
					tiles[x,y] = 2;
					Instantiate ( tileTypes[2].visualPrefab,new Vector3( x+0.5f, 0, y+0.5f ), Quaternion.identity );
				}
			}
		}
		for (int i = 0; i < 4; i++) {
			folk[i].transform.position =  new Vector3 (i * 2, 0, i*2);
			folk[i].currentPosition = new Vector2( i * 2, i*2);
		}

		//folk[0].transform.position =  new Vector3 (25, 0, 25);
		//folk[0].currentPosition =  new Vector3 (25, 0, 25);

		selectedUnit = folk [0];
		selectedIndex = 0;

		icon.selectUnit = selectedUnit;
		GenerateMovementRange ((int)selectedUnit.currentPosition.x,(int) selectedUnit.currentPosition.y);

		GenerateMap();
	}

	// Update is called once per frame
	void Update () {


		List<Vector3> lineVertex = new List<Vector3> ();
		Vector3 lastVector = new Vector3( -1, -1, -1 );
		
		for( int i = 0; i < mapSize; i++ ){
			for( int j = 0; j < mapSize; j++ ){
				if( borderVectors[i,j] <= 3 && borderVectors[i,j] >= 1 ){
					// border vertex!
					lineVertex.Add ( new Vector3( i, 0.1f, j ) );
					//Debug.Log ( "borderVectors added To Line Vertex" );
					//Instantiate ( pointObject, new Vector3( i, 0.5f, j ), Quaternion.identity );
				}
			}
		}
		//Instantiate ( pointObject, lineVertex[0], Quaternion.identity );
		List<Vector3> orderedVertex = new List<Vector3>();
		orderedVertex.Add ( lineVertex[0] );
		int index = 0;
		while (index < lineVertex.Count) {
			if( lineVertex.Contains( new Vector3( orderedVertex[index].x, 0.1f, orderedVertex[index].z-1 ) ) && 
			   !orderedVertex.Contains( new Vector3( orderedVertex[index].x,0.1f, orderedVertex[index].z-1 ) ) &&
			   CostToEnterTile((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y, (int)orderedVertex[index].x, (int)orderedVertex[index].z-1) < selectedUnit.movement  ){
				//Down
				orderedVertex.Add ( new Vector3( orderedVertex[index].x, 0.1f, orderedVertex[index].z-1 ) );
				
				
			} else if( lineVertex.Contains( new Vector3( orderedVertex[index].x + 1, 0.1f, orderedVertex[index].z ) ) && 
			        !orderedVertex.Contains( new Vector3( orderedVertex[index].x + 1, 0.1f, orderedVertex[index].z ) ) &&
			          CostToEnterTile((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y, (int)orderedVertex[index].x, (int)orderedVertex[index].z) < selectedUnit.movement ){
				//Right
				orderedVertex.Add ( new Vector3( orderedVertex[index].x + 1, 0.1f, orderedVertex[index].z ) );
				
			} else if( lineVertex.Contains( new Vector3( orderedVertex[index].x, 0.1f, orderedVertex[index].z+1) ) && 
			          !orderedVertex.Contains( new Vector3( orderedVertex[index].x , 0.1f, orderedVertex[index].z+1 ) ) &&
			         CostToEnterTile((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y, (int)orderedVertex[index].x-1, (int)orderedVertex[index].z) < selectedUnit.movement){
				//Up
				orderedVertex.Add ( new Vector3( orderedVertex[index].x, 0.1f, orderedVertex[index].z+1) );
			} else if( lineVertex.Contains( new Vector3( orderedVertex[index].x -1, 0.1f, orderedVertex[index].z ) ) && 
			         !orderedVertex.Contains( new Vector3( orderedVertex[index].x -1 ,0.1f, orderedVertex[index].z ) ) &&
			          CostToEnterTile((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y, (int)orderedVertex[index].x-1, (int)orderedVertex[index].z-1) < selectedUnit.movement ){
				//Left
				orderedVertex.Add ( new Vector3( orderedVertex[index].x -1, 0.1f, orderedVertex[index].z ) );
				
			}  else {
				orderedVertex.Add( orderedVertex[0] );

			}
			index++;

		}
		for (int i = 0; i < 12; i++) {
			//Instantiate ( pointObject, orderedVertex[i], Quaternion.identity );
		}
		foreach( Vector3 v in orderedVertex ){
			if( lastVector.x != -1 || lastVector.y != -1 || lastVector.z != -1 ){
				Debug.DrawLine(lastVector, v, Color.blue);
			}
			lastVector = v;
		}

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

	public void GenerateMovementRange(int x, int y){

		int[,] vectors = new int[mapSize,mapSize];

		for (int i = -selectedUnit.movement; i <= selectedUnit.movement; i++) {
			for (int j=-selectedUnit.movement; j <= selectedUnit.movement; j++) {

				if ((int)selectedUnit.currentPosition.x + i >= 0 && (int)selectedUnit.currentPosition.x + i < mapSize && 
					(int)selectedUnit.currentPosition.y + j >= 0 && (int)selectedUnit.currentPosition.y + j < mapSize) {

					if (selectedUnit.withinMoveRange ( new Vector2( (int)selectedUnit.currentPosition.x + i, (int)selectedUnit.currentPosition.y + j) ) && 
					    CostToEnterTile((int)selectedUnit.currentPosition.x, (int)selectedUnit.currentPosition.y, (int)selectedUnit.currentPosition.x + i, (int)selectedUnit.currentPosition.y + j) < selectedUnit.movement ) {
						int tPx = (int)selectedUnit.currentPosition.x + i;
						int tPy = (int)selectedUnit.currentPosition.y + j;

						vectors[tPx, tPy] += 1;
						vectors[tPx+1, tPy] += 1;
						vectors[tPx+1, tPy+1] += 1;
						vectors[tPx, tPy+1] += 1;
						//Debug.Log ("Loaded"+i+" "+j);
						//GameObject ob = (GameObject)Instantiate (moveTile, new Vector3 ((int)selectedUnit.currentPosition.x + i, 0,(int)selectedUnit.currentPosition.y + j), Quaternion.identity);
						//ob.transform.SetParent (selectedUnit.transform);
					}
				}
			}
		}
		borderVectors = vectors;
		Debug.Log ("borderVectors set");


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
					graph[x,y].neighbours.Add ( graph[x, y-1 ] );
				if( y < mapSize - 1 )
					graph[x,y].neighbours.Add (graph[x, y+1 ] );
			}
		}

	}
	public void MoveIcon(){
		icon.selectUnit = selectedUnit;
		selectedUnit.canMove = true;
	}

	public void GeneratePathTo(int x, int y) {
		// Clear out our unit's old path.
		selectedUnit.GetComponent<FolkUnit>().currentPath = null;
		
		if( UnitCanEnterTile(x,y) == false ) {
			// We probably clicked on a mountain or something, so just quit out.
			return;
		}
		
		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
		
		// Setup the "Q" -- the list of nodes we haven't checked yet.
		List<Node> unvisited = new List<Node>();
		
		Node source = graph[
		                    (int) selectedUnit.GetComponent<FolkUnit>().currentPosition.x, 
		                    (int) selectedUnit.GetComponent<FolkUnit>().currentPosition.y
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
		if (currentPath.Count - 1 <= selectedUnit.GetComponent<FolkUnit> ().movement) {
			selectedUnit.GetComponent<FolkUnit> ().currentPath = currentPath;
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
	


}
