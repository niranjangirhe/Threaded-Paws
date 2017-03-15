using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dropDownManager : MonoBehaviour {

	public Dropdown dropDown;
	public Text showSelected;

	List<string> options = new List<string>() {"[null]", "soap", "scissors", "brush", "towel", "dryer", "station"};

	public void indexChanged(int index) { // takes selected option index

		showSelected.text = options[index];

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
	}
}
