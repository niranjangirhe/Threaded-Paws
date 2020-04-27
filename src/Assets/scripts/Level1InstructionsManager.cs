using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1InstructionsManager : MonoBehaviour {

	public GameObject instructionsPanel;

	public void skipInstructions () {

		instructionsPanel.SetActive (false);
		LogManager.instance.logger.sendChronologicalLogs("StartLevel01", "", LogManager.instance.UniEndTime().ToString());

	}

	void Start () {

		instructionsPanel.SetActive (true);

	//	LogManager.instance.isQuitLogNeed = true;
		GameLogData.sessionID = AnalyticsSessionInfo.sessionId;
	//	print(AnalyticsSessionInfo.sessionId);
		GameLogData.levelNo = 1;
		GameLogData.userID = AnalyticsSessionInfo.userId;

		LogManager.instance.StartTimer ();
		LogManager.instance.logger.startLoggingData(GameLogData.levelNo.ToString(), "", "" , "" , "", "", "", "", System.DateTime.Now.ToString(), "");

	}
}