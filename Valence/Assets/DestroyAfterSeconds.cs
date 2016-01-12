using UnityEngine;
using System.Collections;

public class DestroyAfterSeconds : MonoBehaviour {
	public int seconds;
	float currentTime;
	// Use this for initialization
	void Start () {
		currentTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		if (currentTime > seconds) {
			Destroy( this.gameObject);
		}
	}
}
