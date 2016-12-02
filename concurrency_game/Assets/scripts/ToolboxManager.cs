using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolboxManager : MonoBehaviour {

	public int actionsLeft;
	public int loopsLeft;
	public int ifsLeft;

	public Text txtActionsLeft;
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
		txtActionsLeft.text = "x " + actionsLeft;
		txtLoopsLeft.text = "x " + loopsLeft;
		txtIfsLeft.text = "x " + ifsLeft;
	}

	public void showError() {

		StartCoroutine (ErrorMsg ());
		
	}

	IEnumerator ErrorMsg() {

		txtErrorMsg.enabled = true;
		txtErrorMsg.text = "You don't have any more of those left!";
		yield return new WaitForSeconds(2.0f);
		txtErrorMsg.enabled = false;
	}
}
