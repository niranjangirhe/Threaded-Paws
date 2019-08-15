using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[System.Serializable]
// public class LogData {

// 	public static string userID { get; set; }
// 	public static long sessionID { get; set; }
// 	public static List<string> chronologicalLogs = new List<string> ();
// 	public static int levelNo { get; set; }
// 	public static bool isLevelCleared { get; set; }
// 	public static string failedReason { get; set; }
// 	public static List<string> inputList_t1 = new List<string> ();
// 	public static List<string> inputList_t2 = new List<string> ();

// 	// public static int levelSteps { get; set; }
// 	public static double levelClearedTime { get; set; }
// 	public static float levelClearAmount { get; set; }
// 	public static int failedAttempts { get; set; }
// 	public static int infoButtonCount { get; set; }
// 	public static int agendaButtonCount { get; set; }

// 	public string UserID;
// 	public long SessionID;

// 	public List<string> ChronologicalLogs = new List<string> ();
// 	public int LevelNo;
// 	public bool IsLevelCleared;
// 	public string FailedReason;
// 	public List<string> InputList_Worker1 = new List<string> ();
// 	public List<string> InputList_Worker2 = new List<string> ();

// 	//public int LevelSteps;
// 	public double LevelClearedTime;
// 	public float LevelClearAmount;
// 	public int FailedAttempt;
// 	public int InfoButtonCount;
// 	public int AgendaButtonCount;

// }



//[System.Serializable]
//public class userinfo{
    //public string userID;
    //public long sessionID;
  //  public string startTime;
//}

//[System.Serializable]
//public class level{
    //public int levelNo;
   // public bool isLevelCleared;
    //public double failedReason;
    //public double levelClearedTime;
    //public float levelClearAmount;
    //public int failedAttempts;
    //public int infoButtonCount;
   // public int agendaButtonCount;
    //public string startTime;
  //  public string endTime;
//}


//[System.Serializable]
//public class InputWorkerData {
//	public string action;
//	public string typeOf;
//	public static string Action;
//	public static string TypeOf;
//	public string time-- //time user got each button
//}


//[System.Serializable]

//	public string time; //time user got each button
//}


//public GameLogData (string userid, long sessionid) {
//		userinfo uInfo = new userinfo();
//		uInfo.SessionID = sessionid.ToString();
//		uInfo.UserId = userid;
//		uInfo.startTime = System.DateTime.Now.ToString();

//		string json = JsonUtility.ToJson(uInfo);
		//SEND TO DB
//	}

	//public void chronologicalLogsDB(List<string> chrono){

//fn tght has a action/dropdown and time
//the chronological stuff -of what button is pressed and and time

//chronologicalLogs = chrono;
// public static List<string> chronologicalLogs = new List<string> ();

//public static List<InputWorkerData> inputList_t1 = new List<InputWorkerData> ();
//public static List<InputWorkerData> inputList_t2 = new List<InputWorkerData> ();


		

	//}

//
//[System.Serializable]
//public class GameLogData {


//	public static List<string> chronologicalLogs = new List<string> ();
//	public static int levelNo { get; set; }
//	public static bool isLevelCleared { get; set; }
//	public static string failedReason { get; set; }
//	public static List<InputWorkerData> inputList_t1 = new List<InputWorkerData> ();
//	public static List<InputWorkerData> inputList_t2 = new List<InputWorkerData> ();
//	public static double levelClearedTime { get; set; }
//	public static float levelClearAmount { get; set; }
//	public static int failedAttempts { get; set; }
//	public static int infoButtonCount { get; set; }
//	public static int agendaButtonCount { get; set; }

//	public List<string> ChronologicalLogs = new List<string> ();
//	public int LevelNo;
//	public bool IsLevelCleared;
//	public string FailedReason;
//	public List<InputWorkerData> InputList_Worker1 = new List<InputWorkerData> ();
//	public List<InputWorkerData> InputList_Worker2 = new List<InputWorkerData> ();

	//public int LevelSteps;
//	public double LevelClearedTime;
//	public float LevelClearAmount;
//	public int FailedAttempt;
//	public int InfoButtonCount;
//	public int AgendaButtonCount;
//	public GameLogData()
//	{
//		
//	}

//	public GameLogData (string userid, long sessionid) {
//		UserId = userid;
//		SessionID = sessionid;
//	}
//	public GameLogData (List<string> chrono, int levelno, bool islevelcleared,
//		string failedreason, List<InputWorkerData> inputlist1, List<InputWorkerData> inputlist2, double _levelClearedTime,
//		float _levelClearAmount, int _failedAttempts, int _infoButtonCount, int _agendaButtonCount) {

//		chronologicalLogs = chrono;
//		levelNo = levelno;
//		isLevelCleared = islevelcleared;
//		failedReason = failedreason;
//		inputList_t1 = inputlist1;
//		inputList_t2 = inputlist2;
//		levelClearedTime = _levelClearedTime;
//		levelClearAmount = _levelClearAmount;
//		failedAttempts = _failedAttempts;
//		infoButtonCount = _infoButtonCount;
//		agendaButtonCount = _agendaButtonCount;
//	}

//