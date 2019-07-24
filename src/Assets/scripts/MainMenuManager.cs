using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public GameObject volumeOnIcon;
	public GameObject volumeOffIcon;

	// Use this for initialization
	void Start () {
		volumeOffIcon.SetActive (false);
	}

	public void playMusic () {
		GameLogData.chronologicalLogs.Add ("PlayMusic: " + LogManager.instance.UniEndTime ());

		volumeOnIcon.SetActive (false);
		volumeOffIcon.SetActive (true);
	}

	public void pauseMusic () {
		GameLogData.chronologicalLogs.Add ("PauseMusic: " + LogManager.instance.UniEndTime ());

		volumeOnIcon.SetActive (true);
		volumeOffIcon.SetActive (false);
	}

	public void startGame () {

		GameLogData.chronologicalLogs.Add ("StartGame: " + LogManager.instance.UniEndTime ());

		SceneManager.LoadScene ("Level1");
		// SceneManager.LoadScene ("LevelTemplate");
	}

	public void quitGame () {
		GameLogData.chronologicalLogs.Add ("QuitGame: " + LogManager.instance.UniEndTime ());

		StartCoroutine (LogManager.instance.PublishLogData ());
		Application.Quit ();
	}

	public void getInstructions () {
		GameLogData.chronologicalLogs.Add ("InstructionBtn: " + LogManager.instance.UniEndTime ());

		SceneManager.LoadScene ("Instructions");
	}

	public void getCredits () {
		GameLogData.chronologicalLogs.Add ("CreditBtn: " + LogManager.instance.UniEndTime ());

		SceneManager.LoadScene ("Credits");
	}
}