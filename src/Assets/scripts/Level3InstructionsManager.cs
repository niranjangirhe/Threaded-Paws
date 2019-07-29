using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Level3InstructionsManager : MonoBehaviour {

	public GameObject level3_part1;
	public GameObject level3_part2;

	public GameObject level3_disableFunctionality;

	public void level3_getPanel1 () {

		level3_part1.SetActive (true);
		level3_part2.SetActive (false);
	}

	public void level2_getPanel2 () {

		level3_part2.SetActive (true);
		level3_part1.SetActive (false);
	}

	public void level2_activateAllFunctionality () {

		level3_disableFunctionality.SetActive (false);
	}

	public void skipInstructions () {
		LogManager.instance.logger.sendChronologicalLogs("StartLevel03", "", LogManager.instance.UniEndTime().ToString());

		level3_part1.SetActive (false);
		level3_part2.SetActive (false);

		level2_activateAllFunctionality ();
	}

	void Start () {

		level3_part1.SetActive (true);
		level3_part2.SetActive (false);

		level3_disableFunctionality.SetActive (true);
		LogManager.instance.isQuitLogNeed = true;
		//GameLogData.sessionID = AnalyticsSessionInfo.sessionId;
		GameLogData.levelNo = 3;
		//GameLogData.userID = AnalyticsSessionInfo.userId;
		LogManager.instance.StartTimer ();
		LogManager.instance.logger.startLoggingData(GameLogData.levelNo.ToString(), "", "" , "" , "", "", "", "", System.DateTime.Now.ToString(), "");
	}
}