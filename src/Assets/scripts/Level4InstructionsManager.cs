using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class Level4InstructionsManager : MonoBehaviour {

	public GameObject level4_part1;
	public GameObject level4_part2;
	public GameObject level4_part3;
	public GameObject level4_part4;

	public GameObject level4_disableFunctionality;

	public void level4_getPanel1() {

		level4_part1.SetActive (true);
		level4_part2.SetActive (false);
	}

	public void level4_getPanel2() {

		level4_part2.SetActive (true);
		level4_part1.SetActive (false);
		level4_part3.SetActive (false);
	}

	public void level4_getPanel3() {

		level4_part3.SetActive (true);
		level4_part2.SetActive (false);
		level4_part4.SetActive (false);
	}

	public void level4_getPanel4() {

		level4_part4.SetActive (true);
		level4_part3.SetActive (false);
	}

	public void level4_activateAllFunctionality() {
		
		level4_disableFunctionality.SetActive (false);
	}

	public void skipInstructions() {
		LogData.chronologicalLogs.Add ("StartLevel04: " + LogManager.instance.UniEndTime ());

		level4_part1.SetActive (false);
		level4_part2.SetActive (false);
		level4_part3.SetActive (false);
		level4_part4.SetActive (false);

		level4_activateAllFunctionality ();
	}
		
	void Start() {

		level4_part1.SetActive (true);
		level4_part2.SetActive (false);

		level4_disableFunctionality.SetActive (true);

			LogManager.instance.isQuitLogNeed=true;
		LogData.sessionID = AnalyticsSessionInfo.sessionId;
		LogData.levelNo = 4;
		LogData.userID = AnalyticsSessionInfo.userId;
		LogManager.instance.StartTimer ();
	}
}
