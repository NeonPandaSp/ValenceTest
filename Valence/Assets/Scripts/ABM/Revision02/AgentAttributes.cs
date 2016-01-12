using UnityEngine;
using System.Collections;

public class AgentAttributes : MonoBehaviour {

	public AIFollow_07 aiFollow;

	public float agentAgility;

	// Use this for initialization
	void Start () {

		//Init all attributes with a random value
		agentAgility = AttributeInit ();

		//Apply the attribute values to corresponding agent ability
		SetAttributes ();

	}

	//When called Initialize return a random value between range 1 ~ 10
	float AttributeInit(){
		float tempValue = Random.Range (0.0F, 10.0F);
		tempValue = Mathf.Round (tempValue * 1.0f) / 1.0f;
		return(tempValue);
	}

	//	
	void SetAttributes(){

		float tempAgility = agentAgility / 10;
		float tempSpeed = aiFollow.speed * tempAgility;

		aiFollow.speed = aiFollow.speed + tempSpeed;

		/*float tempSpeed = (agentAgility - aiFollow.speed);
		tempSpeed = ((tempSpeed/aiFollow.speed)*100);
		aiFollow.speed = tempSpeed;*/
	}

}
