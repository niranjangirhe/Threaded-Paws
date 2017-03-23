using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationIcon : MonoBehaviour {

	GameObject informationWindow;

	// Use this for initialization
	void Start () {
		informationWindow = GameObject.Find ("InformationPanel");


		try {
			informationWindow.SetActive (false);

		} catch {
			Debug.Log ("Information Panel can't be found.");

		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void toggleInformationWindow() {

		// Debug.Log ("Information button clicked");

		try {
			if (informationWindow.activeSelf) {


					informationWindow.SetActive (false);

			} else {


					informationWindow.SetActive (true);
			}

		} catch {
			Debug.Log ("Information Panel can't be found.");
		}
	}
}