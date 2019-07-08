using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level2InstructionsManager : MonoBehaviour {

	public GameObject level2_part1;
	public GameObject level2_part2;
	public GameObject level2_part3;

	public GameObject level2_disableFunctionality;

	public void level2_getPanel1 () {

		level2_part1.SetActive (true);
		level2_part2.SetActive (false);
	}

	public void level2_getPanel2 () {

		level2_part2.SetActive (true);
		level2_part1.SetActive (false);
		level2_part3.SetActive (false);
	}

	public void level2_getPanel3 () {

		level2_part3.SetActive (true);
		level2_part2.SetActive (false);
	}

	public void level2_activateAllFunctionality () {

		level2_disableFunctionality.SetActive (false);
	}

	public void skipInstructions () {
		LogData.chronologicalLogs.Add ("StartLevel02: " + LogManager.instance.UniEndTime ());

		level2_part1.SetActive (false);
		level2_part2.SetActive (false);
		level2_part3.SetActive (false);

		level2_activateAllFunctionality ();
	}

	void Start () {

		level2_part1.SetActive (true);
		level2_part2.SetActive (false);
		level2_part3.SetActive (false);

		level2_disableFunctionality.SetActive (true);

		LogManager.instance.isQuitLogNeed = true;
		LogData.sessionID = AnalyticsSessionInfo.sessionId;
		LogData.levelNo = 2;
		LogData.userID = AnalyticsSessionInfo.userId;
		LogManager.instance.StartTimer ();
	}
}