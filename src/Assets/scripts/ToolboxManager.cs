using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToolboxManager : MonoBehaviour {



    public Text txtErrorMsg;
    public GameObject LostPanel;
    public GameObject WonPanel;
    public GameObject InstructionsPanel;
    public bool isLevel4;

    // Use this for initialization
    void Start () {

		try {
			//Debug.Log ("Start in manager called");
			txtErrorMsg.enabled = false;
			LostPanel.SetActive (false);

			//updateValues ();

			CreateNewBlock.canCreate = true;
		} catch { }
	}

	// Update is called once per frame
	void Update () {

	}

	public void getMainMenu () {
		LogManager.instance.logger.sendChronologicalLogs("MainMenuBtn", "", LogManager.instance.UniEndTime().ToString());

		SceneManager.LoadScene ("MainMenu");
	}

	public void showError (string msg) {
		StartCoroutine (ErrorMsg (msg));

	}

	IEnumerator ErrorMsg (string msg) {

		txtErrorMsg.enabled = true;
		txtErrorMsg.text = msg;
		yield return new WaitForSeconds (2.0f);
		txtErrorMsg.enabled = false;
	}

	public void gameLost () {
		try {
			LostPanel.SetActive (true);
		} catch {
			Debug.Log ("Could not find LostPanel");
		}
	}

	public void gameWon () {

		// Debug.Log("In gameWon() function");

		try {

			WonPanel.SetActive (true);

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