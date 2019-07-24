using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LogManager : MonoBehaviour {

	public static LogManager instance;
	[HideInInspector]
	public double timeStart, timeEnd, totalTime, uniTimeStart;
	[HideInInspector]

	public int infoCount, agendaCount, failCount;
	public static int chronoInputCount;
	public string jsonFilePath;

	public bool isQuitLogNeed;
	List<GameLogData> sessionData = new List<GameLogData> ();
	//	private LogData loggingData = new LogData();
	void Awake () {

		if (instance != null) {
			Destroy (instance);
		} else {
			instance = this;

			DontDestroyOnLoad (this);

		}

	//	StartCoroutine (PublishLogData ());
	}

	void Start () {
		uniTimeStart = Time.realtimeSinceStartup;

		// WWW www = new WWW ("/paws/LogFile.json");
		// //UnityWebRequest myWr = UnityWebRequest.Get("http://localhostpaws/LogFile.json");
		// using (StreamReader stream = new StreamReader ("Paws01/LogFile.json")) {
		// 	string json = stream.ReadToEnd ();
		// 	LogData[] logi = JsonHelper.getJsonArray<LogData> (json);
		// 	//logi = JsonUtility.FromJson<List<LogData>>(json);
		// 	foreach (LogData a in logi) {
		// 		Debug.Log (a.ChronologicalLogs.Count);
		// 	}
		// }
	}

	public class JsonHelper {
		public static List<T> getJsonArray<T> (string json) {
			string newJson = "{ \"Array\": " + json + "'}'";
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (newJson);
			return wrapper.Logs;
		}
		public static string arrayToJson<T> (List<T> array) {
			Wrapper<T> wrapper = new Wrapper<T> ();
			wrapper.Logs = array;
			return JsonUtility.ToJson (wrapper, true);
		}

		[System.Serializable]
		private class Wrapper<T> {
			//			public T[] array;
			public List<T> Logs;
		}
	}
	public void SendLogJson () {

		// LogData logData = new LogData {
		// 	UserID = LogData.userID,
		// 		SessionID = LogData.sessionID,
		// 		ChronologicalLogs = LogData.chronologicalLogs,
		// 		LevelNo = LogData.levelNo,
		// 		IsLevelCleared = LogData.isLevelCleared,
		// 		FailedReason = LogData.failedReason,
		// 		InputList_Worker1 = LogData.inputList_t1,
		// 		InputList_Worker2 = LogData.inputList_t2,
		// 		//	LevelSteps = LogData.levelSteps,
		// 		LevelClearedTime = System.Math.Round (LogData.levelClearedTime, 2),
		// 		LevelClearAmount = LogData.levelClearAmount,
		// 		FailedAttempt = LogData.failedAttempts,
		// 		InfoButtonCount = LogData.infoButtonCount,
		// 		AgendaButtonCount = LogManager.instance.agendaCount,
		// };

		// List<TimeSessionData> sessionD = new List<TimeSessionData> ();
		// TimeSessionData x = new TimeSessionData (TimeSessionData.userID, TimeSessionData.sessionID);
		// sessionD.Add (x);
		// TimeSessionData logs = new TimeSessionData (
		// 		 TimeSessionData.chronologicalLogs,
		// 		 LogData.levelNo,
		// 		 LogData.isLevelCleared,
		// 		 LogData.failedReason,
		// 		 LogData.inputList_t1,
		// 		 LogData.inputList_t2,
		// 		System.Math.Round (LogData.levelClearedTime, 2),
		// 		 LogData.levelClearAmount,
		// 		LogData.failedAttempts,
		// 		LogData.infoButtonCount,
		// 		 LogManager.instance.agendaCount
		// );
		// sessionD.Add(logs);
		// 		sessionD.Add(logs);

		//		 TimeSessionData[] sessionData= new TimeSessionData[2];

		//  sessionData[0] = new TimeSessionData ("AAAAa",TimeSessionData.sessionID);
		//  sessionData[1] = new TimeSessionData ( TimeSessionData.chronologicalLogs,
		// 		 LogData.levelNo,
		// 		 LogData.isLevelCleared,
		// 		 LogData.failedReason,
		// 		 LogData.inputList_t1,
		// 		 LogData.inputList_t2,
		// 		System.Math.Round (LogData.levelClearedTime, 2),
		// 		 LogData.levelClearAmount,
		// 		LogData.failedAttempts,
		// 		LogData.infoButtonCount,
		// 		 LogManager.instance.agendaCount);
	}
	public void CreateLogData () {
		// InputWorkerData c = new InputWorkerData { action = InputWorkerData.Action, typeOf = InputWorkerData.TypeOf };
		// InputWorkerData cc = new InputWorkerData { action = "B", typeOf = "X" };
		// GameLogData.inputList_t1.Add (c);

		// GameLogData.inputList_t2.Add (cc);
		GameLogData logs = new GameLogData {
			UserId = GameLogData.userID,
			SessionID = GameLogData.sessionID,
			ChronologicalLogs = GameLogData.chronologicalLogs,
				LevelNo = GameLogData.levelNo,
				IsLevelCleared = GameLogData.isLevelCleared,
				FailedReason = GameLogData.failedReason,
				InputList_Worker1 = GameLogData.inputList_t1,
				InputList_Worker2 = GameLogData.inputList_t2,
				LevelClearedTime = System.Math.Round (GameLogData.levelClearedTime, 2),
				LevelClearAmount = GameLogData.levelClearAmount,
				FailedAttempt = GameLogData.failedAttempts,
				InfoButtonCount = GameLogData.infoButtonCount,
				AgendaButtonCount = LogManager.instance.agendaCount
		};

		sessionData.Add (logs);
	//	print ("Add");

	}

	public IEnumerator PublishLogData () {
		WWWForm form = new WWWForm ();
		//string json = "{ \"Array\": " + JsonUtility.ToJson (sessionData, true) + "," + JsonUtility.ToJson (logData, true) + "}";
		string json = JsonHelper.arrayToJson<GameLogData> (sessionData);
		print (sessionData.Count);
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
		//	LogData.inputList_t1.Clear ();
		//	LogData.inputList_t2.Clear ();
		//	LogData.chronologicalLogs.Clear ();
	}

	//Not Using this module now
	public IEnumerator sendLogToFile () {
		//	bool successful = true;

		WWWForm form = new WWWForm ();
		// form.AddField ("sessionID", LogData.sessionID.ToString ());
		// form.AddField ("userID", "a");
		// form.AddField ("levelNo", LogData.levelNo.ToString ());
		// form.AddField ("isLevelCleared", LogData.isLevelCleared.ToString ());
		// //	form.AddField ("isLevelSteps", LogData.levelSteps.ToString ());
		// form.AddField ("levelClearedTime", LogData.levelClearedTime.ToString ());
		// form.AddField ("levelClearAmount", LogData.levelClearAmount.ToString ());
		// form.AddField ("failedAttempts", LogData.failedAttempts.ToString ());
		// form.AddField ("infoButtonCount", LogData.infoButtonCount.ToString ());
		// form.AddField ("agendaButtonCount", LogData.agendaButtonCount.ToString ());

		WWW www = new WWW ("/paws/formunity.php", form);

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
		// GameLogData.isLevelCleared = false;
		// GameLogData.failedReason = "Game Quit";
		// //	LogData.isLevelSteps = j;
		// GameLogData.levelClearedTime = LogManager.instance.EndTimer ();
		// //	LogData.levelClearAmount = bar.LoadingBar.GetComponent<Image> ().fillAmount;
		// GameLogData.failedAttempts = LogManager.instance.failCount;
		// GameLogData.infoButtonCount = LogManager.instance.infoCount;
		// GameLogData.agendaButtonCount = LogManager.instance.agendaCount;
		// CreateLogData ();
		// StartCoroutine (PublishLogData ());
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