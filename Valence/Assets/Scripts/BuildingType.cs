using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuildingType  {
	public string typeName;

	public int cost;

	public float pRate;
	public float pTime;
	public int area;
	// Use this for initialization

	public BuildingType( string n, int a ){
		typeName = n;
		area = a;

		cost = getBuildingCost ();
		pRate = getProductionRate (area);
		pTime = getProductionTime ();
	}
	public int getBuildingCost(){
		if (typeName == "shelter") {
			return 50;
		} else if (typeName == "food") {
			return 25;
		} else if (typeName == "power") {
			return 25;
		} else if (typeName == "water") {
			return 25;
		} else {
			Debug.Log ( "Building Cost Error");
			return 0;
		}
	}

	public float getProductionRate(int a){
		if (typeName == "shelter") {
			return 0.0f * a;
		} else if (typeName == "food") {
			return 1.0f * a;
		} else if (typeName == "power") {
			return 1.0f * a;
		} else if (typeName == "water") {
			return 1.0f * a;
		} else {
			Debug.Log ( "Production Type Error");
			return 0.0f;
		}
	}

	public float getProductionTime(){
		if (typeName == "shelter") {
			return 5.0f;
		} else if (typeName == "food") {
			return 5.0f;
		} else if (typeName == "power") {
			return 5.0f;
		} else if (typeName == "water") {
			return 5.0f;
		} else {
			Debug.Log ( "Production Type Error");
			return 0.0f;
		}
	}
}
