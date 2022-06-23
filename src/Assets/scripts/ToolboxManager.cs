using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToolboxManager : MonoBehaviour {



    public Text txtErrorMsg;
    public GameObject lostPanel;
    public GameObject wonPanel;
	public GameObject errorMsgHolder;
	public bool isLevel4;

    // Use this for initialization
    void Start () {

		try {
			errorMsgHolder.SetActive(false);
			txtErrorMsg.enabled = false;
			lostPanel.SetActive (false);

			//updateValues ();

		} catch { }
	}



	public void getMainMenu () {
		LogManager.instance.logger.sendChronologicalLogs("MainMenuBtn", "", LogManager.instance.UniEndTime().ToString());

		SceneManager.LoadScene ("MainMenu");
	}

	public void showError (string msg) {

		errorMsgHolder.SetActive(true);
		txtErrorMsg.enabled = true;
		txtErrorMsg.text = msg;	
	}


	public void gameLost () {
		try {
			lostPanel.SetActive (true);
		} catch {
			Debug.Log ("Could not find LostPanel");
		}
	}

	public void gameWon () {

		// Debug.Log("In gameWon() function");

		try {

			wonPanel.SetActive (true);

		} catch {
			Debug.Log ("Could not find WonPanel");
		}
	}

	public void BackButton()
	{
		LogManager.instance.logger.sendChronologicalLogs("BackToMain", "", LogManager.instance.UniEndTime().ToString());
	SceneManager.LoadScene("MainMenu");
	}
}