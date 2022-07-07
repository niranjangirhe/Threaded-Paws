using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class MainMenuManager : MonoBehaviour {

	public GameObject volumeOnIcon;
	public GameObject volumeOffIcon;

	// Use this for initialization
	void Start () {
		volumeOffIcon.SetActive (false);
	}

	public void playMusic () {
		//GameLogData.chronologicalLogs.Add ("PlayMusic: " + LogManager.instance.UniEndTime ());
		LogManager.instance.logger.sendChronologicalMenuLogs("PlayMusic", LogManager.instance.UniEndTime().ToString());

		volumeOnIcon.SetActive (false);
		volumeOffIcon.SetActive (true);
	}

	public void pauseMusic () {
		//GameLogData.chronologicalLogs.Add ("PauseMusic: " + LogManager.instance.UniEndTime ());
		LogManager.instance.logger.sendChronologicalMenuLogs("PauseMusic", LogManager.instance.UniEndTime().ToString());

		volumeOnIcon.SetActive (true);
		volumeOffIcon.SetActive (false);
	}

	public void startGame () {

		//GameLogData.chronologicalLogs.Add ("StartGame: " + LogManager.instance.UniEndTime ());
		LogManager.instance.logger.sendChronologicalMenuLogs("StartGame", LogManager.instance.UniEndTime().ToString());

		SceneManager.LoadScene ("debug");
		// SceneManager.LoadScene ("LevelTemplate");
	}

	public void quitGame () {
		//GameLogData.chronologicalLogs.Add ("QuitGame: " + LogManager.instance.UniEndTime ());
		LogManager.instance.logger.sendChronologicalMenuLogs("QuitGame", LogManager.instance.UniEndTime().ToString());

		StartCoroutine (LogManager.instance.PublishLogData ());
		Application.Quit ();
	}

	public void getInstructions () {
		//GameLogData.chronologicalLogs.Add ("InstructionBtn: " + LogManager.instance.UniEndTime ());
		LogManager.instance.logger.sendChronologicalMenuLogs("InstructionBtn", LogManager.instance.UniEndTime().ToString());

		SceneManager.LoadScene ("Instructions");
	}

	public void getCredits () {
		//GameLogData.chronologicalLogs.Add ("CreditBtn: " + LogManager.instance.UniEndTime ());
		Debug.Log("Testing");
		LogManager.instance.logger.sendChronologicalMenuLogs("CreditBtn", LogManager.instance.UniEndTime().ToString());

		SceneManager.LoadScene ("Credits");
	}
}