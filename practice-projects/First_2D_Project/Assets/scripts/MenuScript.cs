using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
	public void StartGame () {
		//"Stage1" is the name oft he first scene we created
		SceneManager.LoadScene("Stage1");
	}
}
