using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiateLevel3 : MonoBehaviour {

	public GameObject actionPrefab;
	public GameObject acquirePrefab;
	public GameObject returnPrefab;

	public GameObject t1; // drop area thread
	public GameObject t2; // drop area thread

	// Use this for initialization
	void Start () {

		GameObject box;

		// ---------- IN THREAD 1 ----------

		newBox (actionPrefab, "checkin", t1);
		box = newBox (acquirePrefab, "get", t1); // acquire (brush);
		box.gameObject.transform.Find ("Dropdown").GetComponent<dropDownManager> ().selected = "brush";

		box = newBox (acquirePrefab, "get", t1); // acquire (brush);
		box.gameObject.transform.Find ("Dropdown").GetComponent<dropDownManager> ().selected = "scissors";

		newBox (actionPrefab, "cut", t1);

		box = newBox (returnPrefab, "ret", t1);
		box.gameObject.transform.Find ("Dropdown").GetComponent<dropDownManager> ().selected = "brush";

		box = newBox (returnPrefab, "ret", t1);
		box.gameObject.transform.Find ("Dropdown").GetComponent<dropDownManager> ().selected = "scissors";

		// acquire (station);
		// acquire (towel);
		// acquire (shampoo);
		// acquire (conditioner);
		// wash
		// return (station);
		// return (towel);
		// return (shampoo);
		// return (conditioner);

		newBox (actionPrefab, "checkout", t1);

		// ---------- IN THREAD 2 ----------

		newBox (actionPrefab, "checkin", t2);

		box = newBox (acquirePrefab, "get", t2); // acquire (brush);
		box.gameObject.transform.Find ("Dropdown").GetComponent<dropDownManager> ().selected = "scissors";

		box = newBox (acquirePrefab, "get", t2); // acquire (brush);
		box.gameObject.transform.Find ("Dropdown").GetComponent<dropDownManager> ().selected = "brush";

		newBox (actionPrefab, "cut", t2);

		box = newBox (returnPrefab, "ret", t2);
		box.gameObject.transform.Find ("Dropdown").GetComponent<dropDownManager> ().selected = "scissors";

		box = newBox (returnPrefab, "ret", t2);
		box.gameObject.transform.Find ("Dropdown").GetComponent<dropDownManager> ().selected = "brush";

		newBox (actionPrefab, "checkout", t2);
	
	}
		
	GameObject newBox(GameObject boxPrefab, string actionName, GameObject threadParent) {
		
		GameObject newActionBox = (GameObject)Instantiate (boxPrefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

		newActionBox.transform.SetParent (threadParent.transform);
		//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
		//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
		//newActionBox.AddComponent<Draggable>();
		newActionBox.transform.localScale = Vector3.one;
		newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = actionName;
		//newActionBox.GetComponent<Image> ().color = Color.magenta;
		newActionBox.transform.Find ("Halo").gameObject.SetActive (false);

		return newActionBox;
	}
}
