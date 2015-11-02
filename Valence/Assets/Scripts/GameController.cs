using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	static int mapSize = 50;
	public int[,] tiles = new int[mapSize,mapSize];
	public Vector3 rootMousePos;
	public bool rootSet = false;

	public float food, power, water;
	
	// Use this for initialization
	void Start () {
		for( int x = 0; x < mapSize; x++){
			for( int y = 0; y < mapSize; y++){
				
			}
		}
	}
	
	// Update is called once per frame
	void Update () {


	}

}
