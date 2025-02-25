﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOut : MonoBehaviour {
	public Image fadeObject; 
	public Text text;
	public float speed;
	
	public float delay;
	float currentTime;
	public bool begin;
	public bool andDestroy;
	// Use this for initialization
	void Start () {
		currentTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (begin) {
			currentTime += Time.deltaTime;
		}
		//Debug.Log (Time.deltaTime);
		if (currentTime > delay) {
			//Debug.Log ( "fading" );
			if( fadeObject.color.a > ( 0.0f ) ){
				fadeObject.color = new Color (fadeObject.color.r, fadeObject.color.g, fadeObject.color.b, fadeObject.color.a - Time.deltaTime );
				text.color = new Color (text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime );
			} else if(andDestroy) {
				Destroy (this.gameObject);
			}
			
			//Debug.Log ( fadeObject.color.a );
			//Debug.Log ( text.color.a );
		}
		
		if (currentTime > delay * 2 ) {
			
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
