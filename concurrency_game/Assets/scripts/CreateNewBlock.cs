using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateNewBlock : MonoBehaviour {

	public GameObject prefab;
	public GameObject canvas;

	public void NewActionBlock() {

		Debug.Log ("Action block was clicked");

		Vector3 position = GameObject.Find ("MainActionBox").transform.position;

		//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

		//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
		GameObject newActionBox = (GameObject) Instantiate(prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
		newActionBox.transform.SetParent(canvas.GetComponent<Canvas>().transform);
		newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
		newActionBox.AddComponent<Draggable>();
	}

	public void NewWhileLoopBlock() {
		Debug.Log ("While loop block was clicked");

		Vector3 position = GameObject.Find ("MainActionBox").transform.position;

		//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

		//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
		GameObject newActionBox = (GameObject) Instantiate(prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
		newActionBox.transform.SetParent(canvas.GetComponent<Canvas>().transform);
		newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
		newActionBox.AddComponent<Draggable>();
	}

	public void NewIfStatementBlock() {
		Debug.Log ("If statement block was clicked");

		Vector3 position = GameObject.Find ("MainActionBox").transform.position;

		//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

		//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
		GameObject newActionBox = (GameObject) Instantiate(prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
		newActionBox.transform.SetParent(canvas.GetComponent<Canvas>().transform);
		newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
		newActionBox.AddComponent<Draggable>();
	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}
}
