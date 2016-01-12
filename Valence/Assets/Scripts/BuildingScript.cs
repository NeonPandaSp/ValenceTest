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


		assignedAgents = new GameObject[3];
		initProduction = false;
		//beginProduction ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void initBuildingType(){
		bType = new BuildingType (myType, myArea);
	}

	public void beginProduction(){
		Debug.Log (bType.pTime);
		InvokeRepeating("GenerateResource",bType.pTime,bType.pTime);
	}

	void GenerateResource(){
		Debug.Log ("GENERATING!");
		if (bType.typeName == "shelter") {
			// nada 
		} else if (bType.typeName == "food") {
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
