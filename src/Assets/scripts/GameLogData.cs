using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InputWorkerData {
	public string action;
	public string typeOf;
	public static string Action;
	public static string TypeOf;
}

public static class option{

public static bool logmode=true;


}


[System.Serializable]
public class GameLogData {

	public static string DB_URL = "http://127.0.0.1:3000/paws/";

	public static string userID { get; set; }
	public static long sessionID { get; set; }



	public string UserId;
	public long SessionID;

	public static List<string> chronologicalLogs = new List<string> ();
	public static int levelNo { get; set; }
	public static bool isLevelCleared { get; set; }
	public static string failedReason { get; set; }
	public static List<InputWorkerData> inputList_t1 = new List<InputWorkerData> ();
	public static List<InputWorkerData> inputList_t2 = new List<InputWorkerData> ();
	public static double levelClearedTime { get; set; }
	public static float levelClearAmount { get; set; }
	public static int failedAttempts { get; set; }
	public static int infoButtonCount { get; set; }
	public static int agendaButtonCount { get; set; }

	public List<string> ChronologicalLogs = new List<string> ();
	public int LevelNo;
	public bool IsLevelCleared;
	public string FailedReason;
	public List<InputWorkerData> InputList_Worker1 = new List<InputWorkerData> ();
	public List<InputWorkerData> InputList_Worker2 = new List<InputWorkerData> ();

	//public int LevelSteps;
	public double LevelClearedTime;
	public float LevelClearAmount;
	public int FailedAttempt;
	public int InfoButtonCount;
	public int AgendaButtonCount;
	public GameLogData()
	{
		
	}

	public GameLogData (string userid, long sessionid) {

		userID = userid;
		sessionID = sessionid;

		UserLogData userProfile = new UserLogData();
		userProfile.userID = userID;
		userProfile.sessionID = sessionID.ToString();
		userProfile.startTime = System.DateTime.Now.ToString();

		string json = JsonUtility.ToJson(userProfile);
		//POST TO DB
		sendDatatoDBPOST(GameLogData.DB_URL, json);
	}

	public void sendChronologicalLogs(string action, string dropdown, string time){
		ChronologicalLogData chronoData = new ChronologicalLogData();
		chronoData.action = action;
		chronoData.dropdown = dropdown;
		chronoData.time = time;
		chronoData.timestamp = System.DateTime.Now.ToString();

		//PUT TO DB

		string json = JsonUtility.ToJson(chronoData);
		json = "{\"chronologicalLogs\":" + json + "}";
		sendDatatoDB(GameLogData.DB_URL + "currentlevel/" + GameLogData.sessionID + "/" + GameLogData.userID + "/chronologicalLogs", json);

	}

	public void sendInputWorkerOne(string action, string typeofW, string time){
		InputLogData inputOne = new InputLogData();
		inputOne.action = action;
		inputOne.typeofW = typeofW;
		inputOne.time = time;
		inputOne.timestamp = System.DateTime.Now.ToString();

		//PUT TO DB
		string json = JsonUtility.ToJson(inputOne);
		json = "{\"inputList_t1\":" + json + "}";
		sendDatatoDB(GameLogData.DB_URL + "currentlevel/" + GameLogData.sessionID + "/" + GameLogData.userID + "/inputList_t1", json);
	}

	public void sendInputWorkerTwo(string action, string typeofW, string time){
		InputLogData inputTwo = new InputLogData();
		inputTwo.action = action;
		inputTwo.typeofW = typeofW;
		inputTwo.time = time;
		inputTwo.timestamp = System.DateTime.Now.ToString();

		//PUT TO DB
		string json = JsonUtility.ToJson(inputTwo);
		json = "{\"inputList_t2\":" + json + "}";
		sendDatatoDB(GameLogData.DB_URL + "currentlevel/" + GameLogData.sessionID + "/" + GameLogData.userID + "/inputList_t2", json);
		
	}

	public void startLoggingData(string levelNo, string isLevelCleared, string failedReason, string levelClearedTime, string levelClearAmount, string failedAttempts
									,string infoButtonCount, string agendaButtonCount, string startTime, string endTime){
		
		LevelLogData levelData = new LevelLogData();
		levelData.levelNo = levelNo;
		levelData.isLevelCleared = isLevelCleared;
		levelData.failedAttempts = failedAttempts;
		levelData.failedReason = failedReason;
		levelData.levelClearedTime = levelClearedTime;
		levelData.levelClearAmount = levelClearAmount;
		levelData.infoButtonCount = infoButtonCount;
		levelData.agendaButtonCount = agendaButtonCount;
		levelData.startTime = startTime;
		levelData.endTime = endTime;

		//PUT TO DB
		string json = JsonUtility.ToJson(levelData);
		json = "{\"levels\":" + json + "}";
		sendDatatoDB(GameLogData.DB_URL + GameLogData.sessionID + "/" + GameLogData.userID, json);
	}

	public void endLoggingData(string levelNo, string isLevelCleared, string failedReason, string levelClearedTime, string levelClearAmount, string failedAttempts
									,string infoButtonCount, string agendaButtonCount, string startTime, string endTime){
		
		LevelLogData levelData = new LevelLogData();
		levelData.levelNo = levelNo;
		levelData.isLevelCleared = isLevelCleared;
		levelData.failedAttempts = failedAttempts;
		levelData.failedReason = failedReason;
		levelData.levelClearedTime = levelClearedTime;
		levelData.levelClearAmount = levelClearAmount;
		levelData.infoButtonCount = infoButtonCount;
		levelData.agendaButtonCount = agendaButtonCount;
		levelData.startTime = startTime;
		levelData.endTime = endTime;

		//PUT TO DB
		string json = JsonUtility.ToJson(levelData);
		json = "{\"levels\":" + json + "}";
		sendDatatoDB(GameLogData.DB_URL + "currentlevel/" + GameLogData.sessionID + "/" + GameLogData.userID + "/levels", json);
	}

	public void sendChronologicalMenuLogs(string action, string time){
		ChronologicalMenuLogData cmData = new ChronologicalMenuLogData();
		cmData.action = action;
		cmData.time = time;
		cmData.timestamp = System.DateTime.Now.ToString();

		string json = JsonUtility.ToJson(cmData);
		json = "{\"chronologicalMenuLogs\":" + json + "}";
		sendDatatoDB(GameLogData.DB_URL + GameLogData.sessionID + "/" + GameLogData.userID, json);

		//SEND TO DB
	}

	/// <summary>
/// A method that sends data to DB through PUT
/// </summary>

    public void sendDatatoDB(string url, string jsonObj){
        LogManager.instance.url = url;
        LogManager.instance.jsonData = jsonObj;
        LogManager.instance.PutToDataBase();
    }

/// <summary>
/// A method that sends data to DB through POST
/// </summary>
    public void sendDatatoDBPOST(string url, string jsonObj){
        LogManager.instance.url = url;
        LogManager.instance.jsonData = jsonObj;
        LogManager.instance.PostToDataBase();
    }

}

[System.Serializable]
public class UserLogData{
	public string userID;
	public string sessionID;
	public string startTime;
}

[System.Serializable]
public class LevelLogData{
	public string levelNo;
	public string isLevelCleared;
	public string failedReason;
	public string levelClearedTime;
	public string levelClearAmount;
	public string failedAttempts;
	public string infoButtonCount;
	public string agendaButtonCount;
	public string startTime;
	public string endTime;
}

[System.Serializable]
public class InputLogData{
	public string action;
	public string typeofW;
	public string timestamp;
	public string time;
}

[System.Serializable]
public class ChronologicalLogData{
	public string action;
	public string dropdown;
	public string timestamp;
	public string time;
}

public class ChronologicalMenuLogData{
	public string action;
	public string timestamp;
	public string time;
}

