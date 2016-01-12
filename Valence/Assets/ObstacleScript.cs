using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {
	public ExploreMode_GameController _GameController;
	public int myTileType;
	// Use this for initialization
	void Start () {
		_GameController = FindObjectOfType<ExploreMode_GameController>();
		_GameController.tiles [(int)transform.position.x,(int)transform.position.z] = myTileType;
	}
}
