using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour {

	public GameObject instructionsPanel;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void getMainMenu () {
		LogManager.instance.logger.sendChronologicalMenuLogs("MainMenuBtn", LogManager.instance.UniEndTime().ToString());
		SceneManager.LoadScene ("MainMenu");
	}

	public void tryAgain () {
		LogManager.instance.logger.sendChronologicalMenuLogs("TryAgain", LogManager.instance.UniEndTime().ToString());

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

		// instructionsPanel.SetActive (false);
	}

	public void getLevel2 () {
		LogManager.instance.logger.sendChronologicalMenuLogs("StartLevel02", LogManager.instance.UniEndTime().ToString());
		SceneManager.LoadScene ("Level2");
	}

	public void getLevel3 () {
		LogManager.instance.logger.sendChronologicalMenuLogs("StartLevel03", LogManager.instance.UniEndTime().ToString());
		SceneManager.LoadScene ("Level3");
	}

	public void getLevel4 () {
		LogManager.instance.logger.sendChronologicalMenuLogs("StartLevel04", LogManager.instance.UniEndTime().ToString());
		SceneManager.LoadScene ("Level4");
	}

	public void NextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void Review(GameObject wonPanel)
    {
		wonPanel.SetActive(false);
    }
}