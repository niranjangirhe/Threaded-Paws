using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//query the scoremanager, find out what are the players, and spawn the list appropiately in the UI
public class PlayerScoreList : MonoBehaviour {

	public GameObject playerScoreEntryPrefab;

	ScoreManager scoreManager;

	int lastChangeCounter;

	void Start () {

		scoreManager = GameObject.FindObjectOfType<ScoreManager> ();

		lastChangeCounter = scoreManager.GetChangeCounter ();

		//scoreManager.ChangeScore ("luisa", "Deaths", 1); //runs when someone dies in the game
	}
	
	// Update is called once per frame
	//note: this function does not get called when the panel is not visible (on inspector)
	void Update () {

		//no need to update in EVERY frame - only if changes have been made (last change counter is not the same and current)
		//so, if the current change counter is the same as the last:
		if (lastChangeCounter == scoreManager.GetChangeCounter ()) {
			//no change since last update
			return;
		}

		//other wise, if there IS a change:
		//update the counter and do everything else

		lastChangeCounter = scoreManager.GetChangeCounter ();

		if (scoreManager == null) {
			Debug.Log ("You forgot to add the score manager component to a game object... !");
			return;
		}

		//remove any objects we already have before replacing them with new ones
		//while we have chilren:
		while (this.transform.childCount > 0) {
			//get the first child (index 0)
			Transform c = this.transform.GetChild(0);
			//remove it from the hierarchy (list of children). tell it that it no longer has a parent.
			c.SetParent(null);
			//and destroy it. removal from hierarchy is necessary too because destroy usually happens between frames, so an infinite loop would be caused.
			Destroy(c.gameObject);
		}

		//get the directory from the ScoreManager
		string[] names = scoreManager.GetPlayerNames("Kills");

		foreach (string name in names) {
			//no need for position or rotation, since its all  handled by the vertical layout manager
			GameObject go = (GameObject) Instantiate (playerScoreEntryPrefab);
			//need to tell it that its a child of me (PlayerScoreList)
			go.transform.SetParent(this.transform); //second argument is stay in the worldposition or not. false: location will be used as its "local position..."
			go.transform.Find("Username").GetComponent<Text>().text = name; //will find a child inside of this object with this name. we'll assume this exists...
			go.transform.Find("Kills").GetComponent<Text>().text = scoreManager.GetScore(name, "Kills").ToString();
			go.transform.Find("Deaths").GetComponent<Text>().text = scoreManager.GetScore(name, "Deaths").ToString();
			go.transform.Find("Assists").GetComponent<Text>().text = scoreManager.GetScore(name, "Assists").ToString();
		}
	}
}
