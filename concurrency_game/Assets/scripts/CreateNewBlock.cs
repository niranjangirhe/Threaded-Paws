using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateNewBlock : MonoBehaviour {

	public GameObject prefab;
	public GameObject canvas;

	ToolboxManager manager;

	public void NewActionBlock() {

		if (manager.actionsLeft > 0) {
			
			Debug.Log ("Action block was clicked");

			//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

			//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
			GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
			newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
			newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
			//newActionBox.AddComponent<Draggable>();

			manager.actionsLeft -= 1;
			manager.updateValues ();

		} else {
			//TODO: display message to user
			manager.showError();
		}
	}

	public void NewWhileLoopBlock() {
		Debug.Log ("While loop block was clicked");

		if (manager.loopsLeft > 0) {

			//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

			//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
			GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
			newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform);
			newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
			//newActionBox.GetComponent<Draggable>().typeOfItem = Draggable.Type.ACTION;

			manager.loopsLeft -= 1;
			manager.updateValues ();

		} else {
			manager.showError ();
		}
	}

	public void NewIfStatementBlock() {
		Debug.Log ("If statement block was clicked");

		if (manager.ifsLeft > 0) {
		
			//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

			//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
			GameObject newActionBox = (GameObject) Instantiate(prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
			newActionBox.transform.SetParent(canvas.GetComponent<Canvas>().transform);
			newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
			//newActionBox.AddComponent<Draggable>();

			manager.ifsLeft -= 1;
			manager.updateValues ();

		} else {
			manager.showError ();
		}
	}

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
