using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour {
	public Image fadeObject; 
	public Text text;
	public float speed;

	public float delay;
	float currentTime;
	// Use this for initialization
	void Start () {
		currentTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		//Debug.Log (Time.deltaTime);
		if (currentTime > delay) {
			//Debug.Log ( "fading" );
			fadeObject.color = new Color (fadeObject.color.r, fadeObject.color.g, fadeObject.color.b, fadeObject.color.a - Time.deltaTime );
			text.color = new Color (text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime * speed * ( speed / 0.1f ));
			//Debug.Log ( fadeObject.color.a );
			//Debug.Log ( text.color.a );
		}
	}

	public void ReStart(){
		currentTime = 0;
		//Debug.Log ("Pre:" + fadeObject.color.r);
		fadeObject.color = new Color (fadeObject.color.r, fadeObject.color.g, fadeObject.color.b, 0.4f);
		text.color = new Color (text.color.r, text.color.g, text.color.b, 1.0f);
		//Debug.Log ("Post: " + fadeObject.color.a);
	}
}
