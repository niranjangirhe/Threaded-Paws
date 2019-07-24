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
		GameLogData.chronologicalLogs.Add ("MainMenuBtn: " + LogManager.instance.UniEndTime ());
		SceneManager.LoadScene ("MainMenu");
	}

	public void tryAgain () {
		GameLogData.chronologicalLogs.Add ("TryAgain: " + LogManager.instance.UniEndTime ());

		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		CreateNewBlock.canCreate = true;

		// instructionsPanel.SetActive (false);
	}

	public void getLevel2 () {
		GameLogData.chronologicalLogs.Add ("StartLevel02: " + LogManager.instance.UniEndTime ());
		SceneManager.LoadScene ("Level2");
	}

	public void getLevel3 () {
		GameLogData.chronologicalLogs.Add ("StartLevel03: " + LogManager.instance.UniEndTime ());
		SceneManager.LoadScene ("Level3");
	}

	public void getLevel4 () {
		GameLogData.chronologicalLogs.Add ("StartLevel04: " + LogManager.instance.UniEndTime ());
		SceneManager.LoadScene ("Level4");
	}
}