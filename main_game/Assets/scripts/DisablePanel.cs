using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisablePanel : MonoBehaviour {

	ToolboxManager manager;

	public void DisablePanelClicked() {

		if (GameObject.Find ("StopButton").GetComponent<Button> ().IsInteractable () == true) {
			manager.showError ("Click \"stop\" to terminate the simulation");
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