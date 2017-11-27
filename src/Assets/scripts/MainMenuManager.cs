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
		
	public void playMusic() {
		volumeOnIcon.SetActive (false);
		volumeOffIcon.SetActive (true);
	}

	public void pauseMusic() {
		volumeOnIcon.SetActive (true);
		volumeOffIcon.SetActive (false);
	}

	public void startGame() {
	
		SceneManager.LoadScene ("Level1");
		// SceneManager.LoadScene ("LevelTemplate");
	}

	public void quitGame() {
		Application.Quit ();
	}

	public void getInstructions() {
		SceneManager.LoadScene ("Instructions");
	}

	public void getCredits() {
		SceneManager.LoadScene ("Credits");
	}
}