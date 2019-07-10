using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class InstructionsManager : MonoBehaviour {

	public GameObject level2_part1;
	public GameObject level2_part2;
	public GameObject level2_disableFunctionality;

	public ProgressBar timer;

	public GameObject coverError;
	public GameObject coverTimer;
	public GameObject coverPlayStop;
	public GameObject coverToolBox;
	public GameObject coverThreads;
	public GameObject coverSimulation;

	public GameObject panel1; // introduction
	public GameObject panel2;
	public GameObject panel3; // threads
	public GameObject panel4; 	// threads - tabs
	public GameObject panel5;	// threads- icons
	public GameObject panel6; // toolbox
	public GameObject panel7;	// toolbox - quantities
	public GameObject panel8;	// toolbox - get and ret
	public GameObject panel9; // simulation
	public GameObject panel10;	// simulation - start button
	public GameObject panel11;	// simulation - timer
	public GameObject panel12; // error bar

	// Use this for initialization
	void Start () {

		try {

			panel1.SetActive (true);
			timer = GameObject.Find ("RadialProgressBar").GetComponent<ProgressBar>();

			timer.LoadingBar.GetComponent<Image> ().fillAmount = 0.15F;
		} catch { }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void getMainMenu() {
		
		SceneManager.LoadScene ("MainMenu");
	
	}

	public void getPanel1() { // introduction
		
		panel2.SetActive (false);
		panel1.SetActive (true);

		coverThreads.SetActive (true);
	}

	public void getPanel2() { // threads
	
		panel1.SetActive (false);
		panel2.SetActive (true);
		panel3.SetActive (false);

		coverThreads.SetActive (true);
	}

	public void getPanel3() { // threads - tabs

		panel2.SetActive (false);
		panel3.SetActive (true);
		panel4.SetActive (false);

		coverThreads.SetActive (false);
	}

	public void getPanel4() { // threads- icons
		panel3.SetActive (false);
		panel4.SetActive (true);
		panel5.SetActive (false);
	}

	public void getPanel5() { // threads- icons
		panel4.SetActive (false);
		panel5.SetActive (true);
		panel6.SetActive (false);

		coverToolBox.SetActive (true);
		coverThreads.SetActive (false);

	}

	public void getPanel6() { // toolbox
		panel5.SetActive (false);
		panel6.SetActive (true);
		panel7.SetActive (false);

		coverThreads.SetActive (true);
		coverToolBox.SetActive (false);
	}

	public void getPanel7() { // toolbox - quantities
		panel6.SetActive (false);
		panel7.SetActive (true);
		panel8.SetActive (false);
	}

	public void getPanel8() { // toolbox - get and ret
		panel7.SetActive (false);
		panel8.SetActive (true);
		panel9.SetActive (false);

		coverToolBox.SetActive (false);
		coverSimulation.SetActive (true);
	}

	public void getPanel9() { // simulation
		panel8.SetActive (false);
		panel9.SetActive (true);
		panel10.SetActive (false);

		coverToolBox.SetActive (true);
		coverSimulation.SetActive (false);

		coverPlayStop.SetActive (true);
	}

	public void getPanel10() { // simulation - start button
		panel9.SetActive (false);
		panel10.SetActive (true);
		panel11.SetActive (false);

		coverPlayStop.SetActive (false);

		coverTimer.SetActive (true);
	}

	public void getPanel11() { // simulation - timer
		panel10.SetActive (false);
		panel11.SetActive (true);
		panel12.SetActive (false);

		coverTimer.SetActive (false);
		coverSimulation.SetActive (false);

		coverError.SetActive (true);
	}

	public void getPanel12() { // error bar
		panel11.SetActive (false);
		panel12.SetActive (true);

		coverSimulation.SetActive (true);
		coverTimer.SetActive (true);
		coverPlayStop.SetActive (true);

		coverError.SetActive (false);
	}

	public void getAudioURL() {

		String URL = "https://www.youtube.com/watch?v=RVEGGGKMdn0";
		Application.OpenURL (URL);
	}
}
