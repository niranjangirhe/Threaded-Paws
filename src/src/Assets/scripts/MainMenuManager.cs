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
		LogData.chronologicalLogs.Add ("PlayMusic: " + LogManager.instance.UniEndTime ());

		volumeOnIcon.SetActive (false);
		volumeOffIcon.SetActive (true);
	}

	public void pauseMusic () {
		LogData.chronologicalLogs.Add ("PauseMusic: " + LogManager.instance.UniEndTime ());

		volumeOnIcon.SetActive (true);
		volumeOffIcon.SetActive (false);
	}

	public void startGame () {

		LogData.chronologicalLogs.Add ("StartGame: " + LogManager.instance.UniEndTime ());

		SceneManager.LoadScene ("Level1");
		// SceneManager.LoadScene ("LevelTemplate");
	}

	public void quitGame () {
		LogData.chronologicalLogs.Add ("QuitGame: " + LogManager.instance.UniEndTime ());

		Application.Quit ();
	}

	public void getInstructions () {
		LogData.chronologicalLogs.Add ("InstructionBtn: " + LogManager.instance.UniEndTime ());

		SceneManager.LoadScene ("Instructions");
	}

	public void getCredits () {
		LogData.chronologicalLogs.Add ("CreditBtn: " + LogManager.instance.UniEndTime ());

		SceneManager.LoadScene ("Credits");
	}
}