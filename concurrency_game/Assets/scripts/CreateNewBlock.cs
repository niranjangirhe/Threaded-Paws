using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateNewBlock : MonoBehaviour {

	public GameObject prefab;
	public GameObject canvas;

	ToolboxManager manager;

	GameObject toolbox;

	public static bool canCreate;

	public void NewActionBlock() {

		if (this.transform.name == "WashBox") {
			//Debug.Log ("This is a wash box");

			if (manager.washesLeft > 0) {

				if (canCreate) {
					
					GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

					newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
					//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
					//newActionBox.AddComponent<Draggable>();
					newActionBox.transform.localScale = Vector3.one;
					newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text>().text;
					newActionBox.GetComponent<Image> ().color = Color.green;

					manager.washesLeft -= 1;
					manager.updateValues ();

					canCreate = false;
				} else {
					manager.showError ("Use your current object first");
				}

			} else {
				//display error message to user
				manager.showError ("You don\'t have any more of those left!");
			}

		} else if (this.transform.name == "CheckInBox") {

			if (manager.checkInsLeft > 0) {

				if (canCreate) {
					GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

					newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
					//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
					//newActionBox.AddComponent<Draggable>();
					newActionBox.transform.localScale = Vector3.one;
					newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text>().text;
					newActionBox.GetComponent<Image> ().color = Color.green;

					manager.checkInsLeft -= 1;
					manager.updateValues ();

					canCreate = false;
				} else {
					manager.showError ("Use your current object first");
				}

			} else {
				//display error message to user
				manager.showError ("You dont have any more of those left!");
			}

		} else if (this.transform.name == "CutBox") {

			if (manager.cutsLeft > 0) {

				if (canCreate) {

					GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

					newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
					//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
					//newActionBox.AddComponent<Draggable>();
					newActionBox.transform.localScale = Vector3.one;
					newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text>().text;
					newActionBox.GetComponent<Image> ().color = Color.green;

					manager.cutsLeft -= 1;
					manager.updateValues ();

					canCreate = false;
				} else {
					manager.showError ("Use your current object first");
				}

			} else {
				//display error message to user
				manager.showError ("You don\'t have any more of those left!");
			}

		} else if (this.transform.name == "DryBox") {


			if (manager.driesLeft > 0) {

				if (canCreate) {
					
					GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

					newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
					//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
					//newActionBox.AddComponent<Draggable>();
					newActionBox.transform.localScale = Vector3.one;
					newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text>().text;
					newActionBox.GetComponent<Image> ().color = Color.green;

					manager.driesLeft -= 1;
					manager.updateValues ();

					canCreate = false;
				} else {
					manager.showError ("Use your current object first");
				}

			} else {
				//display error message to user
				manager.showError ("You don\'t have any more of those left!");
			}

		}
	}

	public void NewWhileLoopBlock() {
		Debug.Log ("While loop block was clicked");
		Debug.Log (canCreate);

		if (manager.loopsLeft > 0) {

			if (canCreate) {

				//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

				//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
				GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
				newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform);
				//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
				//newActionBox.GetComponent<Draggable>().typeOfItem = Draggable.Type.ACTION;
				newActionBox.transform.localScale = Vector3.one;

				newActionBox.GetComponent<Image> ().color = Color.green;

				manager.loopsLeft -= 1;
				manager.updateValues ();

				canCreate = false;
			} else {
				manager.showError ("Use your current object first");
			}

		} else {
			manager.showError ("You don\'t have any more of those left!");
		}
	}

	public void NewIfStatementBlock() {
		Debug.Log ("If statement block was clicked");
		Debug.Log ("Before if is clicked: " + canCreate);

		if (manager.ifsLeft > 0) {

			if (canCreate) {

		
				//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

				//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
				GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
				newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform);
				newActionBox.transform.localScale = Vector3.one;

				//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
				//newActionBox.AddComponent<Draggable>();

				manager.ifsLeft -= 1;
				manager.updateValues ();

				newActionBox.GetComponent<Image> ().color = Color.green;

				canCreate = false;
			} else {
				manager.showError ("Use your current object first");
			}

		} else {
			manager.showError ("You don\'t have any more of those left!");
		}

	}

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
		toolbox = GameObject.Find ("DropAreaTools");

		canCreate = true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
