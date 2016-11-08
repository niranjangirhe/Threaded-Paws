using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//start or quit the game
public class GameOverScript : MonoBehaviour {

	public Button[] buttons;

	void Awake() {

		//get the buttons
		buttons = this.GetComponentsInChildren<Button>(true);
		Debug.Log ("buttons[] size (Awake()): " + buttons.Length);

		//disable them
		HideButtons();
	}

	public void HideButtons() {

		foreach (var b in buttons) {
			b.gameObject.SetActive (false);
		}

		Debug.Log ("buttons[] size (HideButtons()): " + buttons.Length);

	}

	public void ShowButtons() {

		Debug.Log ("ShowButtons() was called");
		Debug.Log ("buttons[] size (ShowButtons()): " + buttons.Length); //0 ?????

		foreach (var b in buttons) {
			b.gameObject.SetActive (true);
		}
	}

	public void ExitToMenu() {
		//reload the level
		SceneManager.LoadScene("Menu");
	}

	public void RestartGame() {
		//reload the level
		SceneManager.LoadScene("Stage1");
	}
}
