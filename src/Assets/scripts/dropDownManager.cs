using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dropDownManager : MonoBehaviour {

	public Dropdown dropDown;
	public Text showSelected;

	public string selected;
	public bool isGetDropdown;

	public List<string> options = new List<string> () { "[null]", "brush", "clippers", "cond.", "dryer", "scissors", "shampoo", "towel", "sponge", "spray","cash reg." };

	public void indexChanged (int index) { // takes selected option index

		// Debug.Log ("indexChanged() called");
		//	print(options[index]);
		if (isGetDropdown)
			LogManager.instance.logger.sendChronologicalLogs ("get", options[index], LogManager.instance.UniEndTime ().ToString ());
		else {
			LogManager.instance.logger.sendChronologicalLogs ("ret", options[index], LogManager.instance.UniEndTime ().ToString ());
		}

		showSelected.text = options[index];

		if (index == 0)
			showSelected.color = Color.red;
		else
			showSelected.color = Color.black;
	}

	// Get data from a resource
	void Start () {
		PopulateList ();
	}

	void PopulateList () {

		dropDown.AddOptions (options);

		if (selected != "") {
			showSelected.text = selected;
			showSelected.color = Color.black;
		}
	}
}