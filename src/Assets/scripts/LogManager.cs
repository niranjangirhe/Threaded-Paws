using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LogManager : MonoBehaviour {

	public static LogManager instance;
	public GameLogData logger;
	[HideInInspector]
	public double timeStart, timeEnd, totalTime, uniTimeStart;
	[HideInInspector]

	public int infoCount, agendaCount, failCount;
	public static int chronoInputCount;
	public string jsonFilePath;

	public bool isQuitLogNeed;
	List<GameLogData> sessionData = new List<GameLogData> ();
	//	private LogData loggingData = new LogData();
	public string url {get; set;}
    public string jsonData {get; set;}
    void Awake(){
        if(!instance){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            DestroyImmediate(gameObject);
        }
    }

	void Start () {
		uniTimeStart = Time.realtimeSinceStartup;
		if(PlayerPrefs.HasKey("sessionID") && PlayerPrefs.HasKey("userID")){
			logger = new GameLogData(PlayerPrefs.GetString("userID"), Convert.ToInt64( PlayerPrefs.GetString("sessionID")));
		}else{
			logger = new GameLogData(AnalyticsSessionInfo.userId,AnalyticsSessionInfo.sessionId);
			PlayerPrefs.SetString("userID", GameLogData.userID);
			PlayerPrefs.SetString("sessionID", GameLogData.sessionID.ToString());
		}
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

		logger.endLoggingData(GameLogData.levelNo.ToString(), GameLogData.isLevelCleared.ToString(), GameLogData.failedReason, GameLogData.levelClearedTime.ToString(),
								GameLogData.levelClearAmount.ToString(), GameLogData.failedAttempts.ToString(), GameLogData.infoButtonCount.ToString(), GameLogData.agendaButtonCount.ToString(),
								"", System.DateTime.Now.ToString());
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

    //TODO Change Access Control Allow Origin , *  to an actual adress
    //Sends Data to DB thorugh POST
    IEnumerator PostToDB(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        Debug.Log("POST URL: " + url);
        Debug.Log("POST Data: " + bodyJsonString );
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "*");
        request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
        request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Requested-With,content-type");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();
		//Debug.Log("Status Code for POST: " + request.responseCode);
    }

   //TODO Change Access Control Allow Origin , *  to an actual adress
   //Sends Data to DB thorugh PUT
    IEnumerator Put(string url, string bodyJsonString)
    {
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        Debug.Log("PUT URL: " + url);
        Debug.Log("Put Data: " + bodyJsonString );
        UnityWebRequest request = UnityWebRequest.Put(url, myData);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "*");
        request.SetRequestHeader("cache-control", "no-cache");
        request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
        request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Requested-With,content-type");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();
        //Debug.Log("Status Code for PUT: " + request.responseCode);
    }

	public void PostToDataBase(){
        StartCoroutine(PostToDB(url, jsonData));
    }

    public void PutToDataBase(){
        StartCoroutine(Put(url, jsonData));
    }

}