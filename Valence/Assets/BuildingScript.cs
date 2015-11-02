using UnityEngine;
using System.Collections;

public class BuildingScript : MonoBehaviour {

	public GameController _myGameController;

	public BuildingType bType;
	public string myType; 
	public int myArea; 

	public GameObject[] assignedAgents;
		
	public bool initProduction;

	// Use this for initialization
	void Start () {
		GameObject gameControllerObject =  GameObject.FindGameObjectWithTag("GameController");
		_myGameController = gameControllerObject.GetComponent<GameController> ();

		bType = new BuildingType (myType, myArea);
		assignedAgents = new GameObject[3];
		initProduction = false;
		beginProduction ();
	}
	
	// Update is called once per frame
	void Update () {
		if (initProduction) {
			//beginProduction ();
			//initProduction = false;
		}
	}

	void beginProduction(){
		InvokeRepeating("GenerateResource", bType.pTime, bType.pTime);
	}

	void GenerateResource(){
		if (bType.typeName == "shelter") {
			// nada 
		} else if (bType.typeName == "farm") {
			_myGameController.food += bType.pRate;
		} else if (bType.typeName == "power") {
			_myGameController.power += bType.pRate;
		} else if (bType.typeName == "water") {
			_myGameController.water += bType.pRate;
		} else {
			Debug.Log ( "Production Type Error @ Resource Generation");
		}
	}
}
