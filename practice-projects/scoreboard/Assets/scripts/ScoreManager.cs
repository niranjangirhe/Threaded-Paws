using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour {

	//Need some sort of map that goes from a username to a score
	//This map needs to look like:
	//
	// LIST OF USERS --> A User --> LIST OF SCORES for that user
	//

	//Only re-load the whole list when the scoreManager has had some change done to it
	int changeCounter = 0;

	//using generics (e.g. List<Object>, Dictionary<Object>, etc)
	Dictionary<string, Dictionary<string, int>> playerScores; //refer to dictionary data by username (e.g. playerScores["luisa"]["kills"] = 9001)

	void Start () {
		//instantiate the structure: not in the start because we need to make sure that nothing is trying to access the object BEFORE Start() is called
		//playerScores = new Dictionary<string, Dictionary<string, int>> ();

		//some test data
		SetSCore("luisa", "Kills", 0);
		//SetSCore("luisa", "Deaths", 0);
		SetSCore("luisa", "Assists", 345);

		SetSCore("bob", "Kills", 1000);
		SetSCore("bob", "Deaths", 14345);

		SetSCore("AAAAAAAA", "Kills", 3);
		SetSCore("BBBBBBBB", "Kills", 2);
		SetSCore("CCCCCCCC", "Kills", 1);

		//Debug.Log (GetScore("luisa", "kills"));
	}

	//called every time
	void Init() {
		
		//if the dictionary is NOT empty, then do not initialize
		if (playerScores != null)
			return;
				
		playerScores = new Dictionary<string, Dictionary<string, int>> ();
	}

	//how everything else will interact with our program: handlers!
	public int GetScore(string username, string scoreType) {

		Init (); //make sure there is something in the dictionary when trying to access it

		//check the username is in the array
		if (playerScores.ContainsKey (username) == false) {
			//we have no score record at all for this username
			return 0; //hasnt gotten any scores yet, whatsoever... so score is 0!
		}
			
		//verify that the scoreType is registered
		if (playerScores [username].ContainsKey (scoreType) == false) {
			return 0;
		}
			
		return playerScores [username] [scoreType];
	}

	public void SetSCore(string username, string scoreType, int value) {
		
		Init (); //make sure there is something in the dictionary when trying to access it

		changeCounter++; //increment counter

		//check the username is in the array
		if (playerScores.ContainsKey (username) == false) {
			//we have no score record at all for this username
			playerScores[username] = new Dictionary<string, int>();//create one
		}

		playerScores [username] [scoreType] = value;
	}

	public void ChangeScore(string username, string scoreType, int amount) {
		
		Init (); //make sure there is something in the dictionary when trying to access it

		int currScore = GetScore (username, scoreType);
		SetSCore (username, scoreType, currScore + amount);
	}

	//sortingScoreType is the type we want to sort the list by
	public string[] GetPlayerNames(string sortingScoreType) {

		Init ();

		//convert Keys collection to a string[] using Linq
		string[] names = playerScores.Keys.ToArray();

		//return the sorted using the Linq library (based on the score of that name and the sorting score type)
		// n => : sort using the name as an entry
		// GetScore(n, sortingScoreType): sort based on the score of that name by the score type given
		return names.OrderByDescending (n => GetScore (n, sortingScoreType)).ToArray();
	}

	public void DEBUG_ADD_KILL_TO_LUISA() {
		ChangeScore ("luisa", "Kills", 1);
	}

	public int GetChangeCounter() {
		return changeCounter;
	}
}