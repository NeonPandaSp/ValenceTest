using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GUIController : MonoBehaviour {
	public InputController _inputController;
	public GameObject GUIObject;

	public Text scrapText, foodText, powerText;
	public Button shelterButton, foodButton, powerButton;

	Vector2 lastMousePos;

	int scrapCount, foodCount, powerCount;
	// Use this for initialization
	void Start () {
		scrapCount = 0;
		foodCount = 0;
		powerCount = 0;
		scrapText.text = "Scrap: " + scrapCount;
		foodText.text = "Food: " + foodCount;
		powerText.text = "Power: " + powerCount;
		lastMousePos = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		scrapCount++;
		foodCount++;
		powerCount++;
		scrapText.text = "Scrap: " + scrapCount;
		foodText.text = "Food: " + foodCount;
		powerText.text = "Power: " + powerCount;


		int x, y;
		if (Input.mousePosition.x > lastMousePos.x) {
			x = -1;
		} else if (Input.mousePosition.x < lastMousePos.x) {
			x = 1;
		} else {
			x = 0;
		}

		if (Input.mousePosition.y > lastMousePos.y) {
			y = -1;
		} else if (Input.mousePosition.y < lastMousePos.y) {
			y = 1;
		} else {
			y = 0;
		}
		/*
		Vector2 deltaPos = new Vector2 (x,y);

		GUIObject.transform.position = new Vector2( GUIObject.transform.position.x + deltaPos.x, GUIObject.transform.position.y + deltaPos.y);
		Debug.Log (GUIObject.transform.position.x + " " + GUIObject.transform.position.y);

		if (GUIObject.transform.position.x > -110) {
			GUIObject.transform.position = new Vector2(-110,GUIObject.transform.position.y);
		} else if ( GUIObject.transform.position.x < -118 ){
			GUIObject.transform.position = new Vector2(-118,GUIObject.transform.position.y);;
		}

		if (GUIObject.transform.position.y > -6) {
			GUIObject.transform.position = new Vector2(GUIObject.transform.position.x, -6);;
		} else if ( GUIObject.transform.position.y < -16 ){
			GUIObject.transform.position = new Vector2(GUIObject.transform.position.x, -16);;
		}
		*/
		lastMousePos = Input.mousePosition;

	}

	public void shelterPressed(){
		_inputController.selectedMaterial ("blue");
		Debug.Log ("Green!");
	}
	public void foodPressed(){
		_inputController.selectedMaterial ("green");
	}
	public void powerPressed(){
		_inputController.selectedMaterial ("yellow");
	}
}
