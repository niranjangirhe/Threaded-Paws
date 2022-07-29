using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class MainMenuManager : MonoBehaviour {

	public GameObject volumeOnIcon;
	public GameObject volumeOffIcon;
	[SerializeField] private bool DebugRoundOn;
	public static bool doLoging = false;

	// Use this for initialization
	void Start () {
		volumeOffIcon.SetActive (false);
	}

	public void playMusic () {
		LogManager.instance.logger.sendChronologicalMenuLogs("PlayMusic", LogManager.instance.UniEndTime().ToString());

		volumeOnIcon.SetActive (false);
		volumeOffIcon.SetActive (true);
	}

	public void pauseMusic () {
		LogManager.instance.logger.sendChronologicalMenuLogs("PauseMusic", LogManager.instance.UniEndTime().ToString());
		volumeOnIcon.SetActive (true);
		volumeOffIcon.SetActive (false);
	}

	public void startGame () {

		LogManager.instance.logger.sendChronologicalMenuLogs("StartGame", LogManager.instance.UniEndTime().ToString());
		if(DebugRoundOn)
			SceneManager.LoadScene("debug");
		else
			SceneManager.LoadScene("Tutorial");
		// SceneManager.LoadScene ("LevelTemplate");
	}

	public void quitGame () {
		LogManager.instance.logger.sendChronologicalMenuLogs("QuitGame", LogManager.instance.UniEndTime().ToString());
		StartCoroutine (LogManager.instance.PublishLogData ());
		Application.Quit ();
	}

	public void continueLevel() {
		//GameLogData.chronologicalLogs.Add ("InstructionBtn: " + LogManager.instance.UniEndTime ());
		SceneManager.LoadScene ("Level Map");
	}

	public void getCredits () {
		LogManager.instance.logger.sendChronologicalMenuLogs("CreditBtn", LogManager.instance.UniEndTime().ToString());
		SceneManager.LoadScene ("Credits");
	}
}