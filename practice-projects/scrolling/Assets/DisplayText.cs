using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class DisplayText : MonoBehaviour {
	public RectTransform myPanel;
	public GameObject myTextPrefab;
	List<string> chatEvents;
	private float nextMessage;
	private int myNumber = 0;
	private GameObject newText;
	// Use this for initialization
	void Start () {
		chatEvents = new List<string>();
		chatEvents.Add("this");
		chatEvents.Add("is");
		chatEvents.Add("a");
		chatEvents.Add("test");
		chatEvents.Add("for");
		chatEvents.Add("showing");
		chatEvents.Add("strings");
		chatEvents.Add("and");
		chatEvents.Add("displaying");
		chatEvents.Add("in");
		chatEvents.Add("a");
		chatEvents.Add("scrollable");
		chatEvents.Add("panel");
		chatEvents.Add("in");
		chatEvents.Add("the");
		chatEvents.Add("new");
		chatEvents.Add("GUI");
		chatEvents.Add("really we can put anything in this");
		chatEvents.Add("anything at all");
		chatEvents.Add("Knock, knock");
		chatEvents.Add("who's there");
		chatEvents.Add("Doctor");
		chatEvents.Add("Doctor who?");
		chatEvents.Add("I refuse to participate in a joke older than I am");
		chatEvents.Add("yeah right older than you are!");
		chatEvents.Add("It is older than me honest");
		chatEvents.Add("as if, remember I know how old you are");
		chatEvents.Add("Look there's clear evidence that they started in 1930's");
		chatEvents.Add("Maybe but Dr Who didn't did it");
		chatEvents.Add("Well OK but Dr Who's older than me");
		chatEvents.Add("Yes but that joke didn't start with the first episode");
		chatEvents.Add("Right I've had enough of this");
		chatEvents.Add("Stop being so melodramatic");
		nextMessage = Time.time + 1f;
	}
	// Update is called once per frame
	void Update () {
		if(Time.time > nextMessage && myNumber < chatEvents.Count)
		{
			GameObject newText = (GameObject)Instantiate(myTextPrefab);
			newText.transform.SetParent(myPanel);
			newText.GetComponent<Text>().text = chatEvents[myNumber];
			myNumber ++;
			nextMessage = Time.time + 1f;
		}
	}
}