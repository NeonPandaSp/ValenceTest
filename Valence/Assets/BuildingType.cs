using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuildingType  {
	public string typeName;
	public float pRate;
	public float pTime;
	public int area;
	// Use this for initialization

	public BuildingType( string n, int a ){
		typeName = n;
		area = a;
		pRate = getProductionRate (area);
		pTime = getProductionTime ();
	}

	public float getProductionRate(int a){
		if (typeName == "shelter") {
			return 0.0f * a;
		} else if (typeName == "farm") {
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
			return 1;
		} else if (typeName == "farm") {
			return 1;
		} else if (typeName == "power") {
			return 1;
		} else if (typeName == "water") {
			return 1;
		} else {
			Debug.Log ( "Production Type Error");
			return 0.0f;
		}
	}
}
