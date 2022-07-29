using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public Transform LoadingBar;
	public float currentAmount;

	// Use this for initialization
	void Start () {
		// currentAmount = 0;
		LoadingBar.GetComponent<Image>().fillAmount = 0;
	}
	
	// Update is called once per frame

}
