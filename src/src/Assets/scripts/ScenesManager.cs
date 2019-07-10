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

	public void getMainMenu () {
		LogData.chronologicalLogs.Add ("MainMenuBtn: " + LogManager.instance.UniEndTime ());
		SceneManager.LoadScene ("MainMenu");
	}

	public void tryAgain () {
		LogData.chronologicalLogs.Add ("TryAgain: " + LogManager.instance.UniEndTime ());

		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		CreateNewBlock.canCreate = true;

		// instructionsPanel.SetActive (false);
	}

	public void getLevel2 () {
		LogData.chronologicalLogs.Add ("StartLevel02: " + LogManager.instance.UniEndTime ());
		SceneManager.LoadScene ("Level2");
	}

	public void getLevel3 () {
		LogData.chronologicalLogs.Add ("StartLevel03: " + LogManager.instance.UniEndTime ());
		SceneManager.LoadScene ("Level3");
	}

	public void getLevel4 () {
		LogData.chronologicalLogs.Add ("StartLevel04: " + LogManager.instance.UniEndTime ());
		SceneManager.LoadScene ("Level4");
	}
}