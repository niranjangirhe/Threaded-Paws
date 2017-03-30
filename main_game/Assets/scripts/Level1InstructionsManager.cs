using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1InstructionsManager : MonoBehaviour {

	public GameObject instructionsPanel;

	public void skipInstructions() {

		instructionsPanel.SetActive (false);
	}

	void Start() {

		instructionsPanel.SetActive (true);
	}
}
