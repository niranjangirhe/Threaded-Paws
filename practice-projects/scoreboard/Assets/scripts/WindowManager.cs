using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour {

	//important that its not a prefab (whats dragged into the inspector)
	public GameObject scoreBoard;

	// Use this for initialization
	void Start () {
		//doesnt work on anything thats inactive/hidden/not visible
		//scoreBoard = GameObject.Find ("Panel");
	}
	
	// Update is called once per frame
	void Update () {
		//when the tab key is pressed,
		if (Input.GetKeyDown (KeyCode.Tab)) {
			//set active to the opposite of whatever it currently is
			scoreBoard.SetActive(!scoreBoard.activeSelf);
		}
	}
}
