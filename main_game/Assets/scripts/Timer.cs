using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public int timePassed; //will manipulate everything.
	private int seconds;
	private int minutes;
	public Text textTimer;
	private bool stopped;

	public Transform startButton;
	public Transform stopButton;

	public void StartTimer() {

		Debug.Log ("Started timer");
		
		timePassed = 0;
		seconds = 0;
		minutes = 0;
		stopped = false;

		StartCoroutine ("Begin");

		startButton.GetComponent<Button>().interactable = false;
		stopButton.GetComponent<Button>().interactable = true;
	}

	public void StopTimer() {

		Debug.Log ("Stopped timer");
		stopped = true;

		textTimer.text = "00:00";

		startButton.GetComponent<Button>().interactable = true;
		stopButton.GetComponent<Button>().interactable = false;
	}

	private IEnumerator Begin() {

		while (true) {

			if (stopped) {
				textTimer.text = "00:00";
				break;
			}
				

			yield return new WaitForSeconds (1);
			timePassed++; //increased by 1 every second
			seconds = (timePassed % 60);
			minutes = (timePassed / 60) % 60;

			textTimer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
		}
	}
		
	public string GetCurrentTime() {
		return minutes.ToString("00") + ":" + seconds.ToString("00");
	}
}
