using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePanel : MonoBehaviour {

	ToolboxManager manager;

	public void DisablePanelClicked() {

		manager.showError ("Click \"stop\" to terminate the simulation");

	}

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}