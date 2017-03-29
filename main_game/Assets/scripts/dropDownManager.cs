using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dropDownManager : MonoBehaviour {

	public Dropdown dropDown;
	public Text showSelected;

	public string selected;

	List<string> options = new List<string>() {"[null]", "brush" ,"clippers" , "cond.", "dryer", "scissors", "shampoo", "station", "towel"};

	public void indexChanged(int index) { // takes selected option index

		// Debug.Log ("indexChanged() called");

		showSelected.text = options [index];

		if (index == 0)
			showSelected.color = Color.red;
		else
			showSelected.color = Color.black;
	}

	// Get data from a resource
	void Start() {
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
