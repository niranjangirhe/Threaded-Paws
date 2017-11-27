using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public Transform LoadingBar;
	public Transform TextIndicator;
	public Transform TextLoading;
	public float currentAmount;
	public float speed;

	// Use this for initialization
	void Start () {
		// currentAmount = 0;
		LoadingBar.GetComponent<Image>().fillAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
//		if (currentAmount < 100) {
//			currentAmount += speed * Time.deltaTime;
//			TextIndicator.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
//			TextLoading.gameObject.SetActive (true);
//		} else {
//			TextLoading.gameObject.SetActive (false);
//			TextIndicator.GetComponent<Text> ().text = "DONE!";
//		}
//
//		LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
	}
}
