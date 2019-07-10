using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LogManager : MonoBehaviour {

	public static LogManager instance;
	[HideInInspector]
	public double timeStart, timeEnd, totalTime, uniTimeStart;
	[HideInInspector]

	public int infoCount, agendaCount, failCount;

	public bool isQuitLogNeed;
	//	private LogData loggingData = new LogData();
	void Awake () {

		if (instance != null) {
			Destroy (instance);
		} else {
			instance = this;

			DontDestroyOnLoad (this);

		}
		//	 StartCoroutine (SendLogJson ());
	}

	void Start () {
		uniTimeStart = Time.realtimeSinceStartup;
	}

	public IEnumerator SendLogJson () {

		LogData logData = new LogData {
			UserID = LogData.userID,
				SessionID = LogData.sessionID,
				ChronologicalLogs = LogData.chronologicalLogs,
				LevelNo = LogData.levelNo,
				IsLevelCleared = LogData.isLevelCleared,
				FailedReason = LogData.failedReason,
				InputList_Worker1 = LogData.inputList_t1,
				InputList_Worker2 = LogData.inputList_t2,
				//	LevelSteps = LogData.levelSteps,
				LevelClearedTime = System.Math.Round (LogData.levelClearedTime, 2),
				LevelClearAmount = LogData.levelClearAmount,
				FailedAttempt = LogData.failedAttempts,
				InfoButtonCount = LogData.infoButtonCount,
				AgendaButtonCount = LogManager.instance.agendaCount,
		};

		WWWForm form = new WWWForm ();
		string json = JsonUtility.ToJson (logData, true);
		//	File.AppendAllText (Application.dataPath + "save1.txt", json);
		form.AddField ("log", json);
		//	WWW www = new WWW ("http://localhost/formunity.php", form);

		WWW www = new WWW ("/paws/formunity.php", form);

		yield return www;
		if (www.error != null) {
			//	successful = false;
			Debug.Log (www.text);
		} else {
			Debug.Log (www.text);
			//	successful = true;
		}
		Debug.Log (json);
		LogData.inputList_t1.Clear ();
		LogData.inputList_t2.Clear ();
		LogData.chronologicalLogs.Clear();

	}

	//Not Using this module now
	public IEnumerator sendLogToFile () {
		//	bool successful = true;

		WWWForm form = new WWWForm ();
		form.AddField ("sessionID", LogData.sessionID.ToString ());
		form.AddField ("userID", LogData.userID.ToString ());
		form.AddField ("levelNo", LogData.levelNo.ToString ());
		form.AddField ("isLevelCleared", LogData.isLevelCleared.ToString ());
		//	form.AddField ("isLevelSteps", LogData.levelSteps.ToString ());
		form.AddField ("levelClearedTime", LogData.levelClearedTime.ToString ());
		form.AddField ("levelClearAmount", LogData.levelClearAmount.ToString ());
		form.AddField ("failedAttempts", LogData.failedAttempts.ToString ());
		form.AddField ("infoButtonCount", LogData.infoButtonCount.ToString ());
		form.AddField ("agendaButtonCount", LogData.agendaButtonCount.ToString ());

		WWW www = new WWW ("http://localhost/formunity.php", form);

		yield return www;
		if (www.error != null) {
			//	successful = false;
			Debug.Log (www.text);
		} else {
			Debug.Log (www.text);
			//	successful = true;
		}
	}
	public void StartTimer () {
		agendaCount = infoCount = 0;
		//timeStart = ((float)DateTime.Now.Hour + ((float)DateTime.Now.Minute * 0.01f));
		timeStart = Time.realtimeSinceStartup;
		//	print ("S");
	}

	/// <summary>
	/// Level Time calculation
	/// </summary>
	/// <returns></returns>
	public double EndTimer () {

		timeEnd = Time.realtimeSinceStartup; // ((float)DateTime.Now.Hour + ((float)DateTime.Now.Minute * 0.01f)) +  ((float)DateTime.Now.Minute * 0.01f);
		totalTime = timeEnd - timeStart;
		return System.Math.Round (totalTime, 2);
	}

	/// <summary>
	/// Universal Time of the game
	/// </summary>
	/// <returns></returns>
	public double UniEndTime () {
		return System.Math.Round (Time.realtimeSinceStartup - uniTimeStart, 2);
	}

	void OnApplicationQuit () {
		if (!isQuitLogNeed)
			return;
		LogData.isLevelCleared = false;
		LogData.failedReason = "Game Quit";
		//	LogData.isLevelSteps = j;
		LogData.levelClearedTime = LogManager.instance.EndTimer ();
		//	LogData.levelClearAmount = bar.LoadingBar.GetComponent<Image> ().fillAmount;
		LogData.failedAttempts = LogManager.instance.failCount;
		LogData.infoButtonCount = LogManager.instance.infoCount;
		LogData.agendaButtonCount = LogManager.instance.agendaCount;
		StartCoroutine (SendLogJson ());
	}

	// 	public LogData data;
	// 	public string file = "logging.txt";
	// 	string url = "http://localhost:80/formunity.php";
	// 	private string filePath;
	// 	void Start () {
	// 		filePath = Path.Combine (Application.dataPath, "save.txt");

	// 	}

	// 	IEnumerator Save () {
	// 		WWWForm form = new WWWForm ();
	// 		string jsonString = JsonUtility.ToJson (data, true);
	// 		File.WriteAllText (filePath, jsonString);
	// 		form.AddField ("x", jsonString);

	// 		WWW www = new WWW (url, form);
	// 		yield return null;
	// 	}
	// 	public void EnterLog () {
	// 		LogData data = new LogData {
	// 			id = 1,
	// 				status = "Passed",
	// 				time = 10.1f
	// 		};
	// 		string json = JsonUtility.ToJson (data);
	// 		WriteToFile (file, json);
	// 		print (data);
	// 	}

	// 	private void WriteToFile (string filename, string json) {
	// 		string path = Application.persistentDataPath + "/" + filename;
	// 		FileStream fileStream = new FileStream (path, FileMode.Create);

	// 		using (StreamWriter writer = new StreamWriter (fileStream)) {
	// 			writer.Write (json);
	// 		}
	// 	}

	// }

}