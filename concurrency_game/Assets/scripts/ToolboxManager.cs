using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolboxManager : MonoBehaviour {

	public int checkInsLeft;
	public int cutsLeft;
	public int driesLeft;
	public int washesLeft;

	public int loopsLeft;
	public int ifsLeft;

	public Text txtCheckInsLeft;
	public Text txtCutsLeft;
	public Text txtDriesLeft;
	public Text txtWashesLeft;

	public Text txtLoopsLeft;
	public Text txtIfsLeft;

	public Text txtErrorMsg;

	// Use this for initialization
	void Start () {

		Debug.Log ("Start in manager called");
		txtErrorMsg.enabled = false;

		updateValues ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateValues() {
		txtCheckInsLeft.text = "x " + checkInsLeft;
		txtCutsLeft.text = "x " + cutsLeft;
		txtWashesLeft.text = "x " + washesLeft;
		txtDriesLeft.text = "x " + driesLeft;
		txtLoopsLeft.text = "x " + loopsLeft;
		txtIfsLeft.text = "x " + ifsLeft;
	}

	public void showError(string msg) {

		StartCoroutine (ErrorMsg (msg));
		
	}

	IEnumerator ErrorMsg(string msg) {

		txtErrorMsg.enabled = true;
		txtErrorMsg.text = msg;
		yield return new WaitForSeconds(2.0f);
		txtErrorMsg.enabled = false;
	}
}
