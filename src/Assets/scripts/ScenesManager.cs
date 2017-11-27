using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesManager : MonoBehaviour {

	public GameObject instructionsPanel;

	// Use this for initialization
	void Start () {
		CreateNewBlock.canCreate = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void getMainMenu() {
		SceneManager.LoadScene ("MainMenu");
	}

	public void tryAgain() {
	
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		CreateNewBlock.canCreate = true;

		// instructionsPanel.SetActive (false);
	}

	public void getLevel2() {
		SceneManager.LoadScene ("Level2");
	}

	public void getLevel3() {
		SceneManager.LoadScene ("Level3");
	}

	public void getLevel4() {
		SceneManager.LoadScene ("Level4");
	}
}