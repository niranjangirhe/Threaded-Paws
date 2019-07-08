using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogData {

	public static string userID { get; set; }
	public static long sessionID { get; set; }
	public static List<string> chronologicalLogs = new List<string>();
	public static int levelNo { get; set; }
	public static bool isLevelCleared { get; set; }
	public static string failedReason { get; set; }
	public static List<string> inputList_t1 = new List<string> ();
	public static List<string> inputList_t2 = new List<string> ();

	// public static int levelSteps { get; set; }
	public static double levelClearedTime { get; set; }
	public static float levelClearAmount { get; set; }
	public static int failedAttempts { get; set; }
	public static int infoButtonCount { get; set; }
	public static int agendaButtonCount { get; set; }

	public string UserID;
	public long SessionID;
	
	public List<string> ChronologicalLogs = new List<string>();
	public int LevelNo;
	public bool IsLevelCleared;
	public string FailedReason;
	public List<string> InputList_Worker1 = new List<string> ();
	public List<string> InputList_Worker2 = new List<string> ();


	//public int LevelSteps;
	public double LevelClearedTime;
	public float LevelClearAmount;
	public int FailedAttempt;
	public int InfoButtonCount;
	public int AgendaButtonCount;

}