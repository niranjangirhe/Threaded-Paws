using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ExecuteThreads : MonoBehaviour {

	//TODO: save original threadchildren positions (in case order is incorrect)

	public Transform runButton;
	private Timer timer;
	private int numActions;
	private string toPrint;
	//get simulation space for printing
	public Text simulationTextArea;
	//get instance of GenerateTasks
	public GenerateTasks genTasks;
	//Task t; //"playerTank"

	string[] blocks;

	void Start() {

		//blocks = GetActionBlocks ();
		timer = GameObject.FindObjectOfType<Timer> ();
		//toPrint = "";
	}

	public void Test() {

		if (!genTasks)
			Debug.Log ("No GenerateTasks object was found.");

		//update blocks
		blocks = GetActionBlocks();

		runButton.GetComponent<Button> ().interactable = false;

		//Debug.Log ("Running code in threads");

		//toPrint += "\nTesting!";
		//simulationTextArea.text = toPrint;

		Simulate ();

		runButton.GetComponent<Button> ().interactable = true;
	}

	private string[] GetActionBlocks() {

		//get children in drop area for thread
		//threadChildren = new GameObject[this.transform.Find("DropAreaThread").childCount];
		string[] threadChildren = new string[this.transform.Find("DropAreaThread").childCount];

		//Debug.Log ("childCount: " + threadChildren.Length);

		for (int i = 0; i < threadChildren.Length; i++) {
			//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;
			threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).name;
			//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);
		}

		if (threadChildren.Length > 0) {

			//if (threadChildren[0] == "CheckIn" && threadChildren[threadChildren.Length] == "Return")
			//	return threadChildren;

			//Debug.Log ("The first thing to do is check in and the last is return! Try again.");
			//return null;

			return threadChildren;

		} else {
			//Debug.Log ("There are no children in the thread drop area");
			return null;
		}
	}

	private void Simulate() {

		//Debug.Log ("actions.Length: " + actions.Length);

		//if there are blocks and there are tasks to be completed
		//try{
			if (genTasks.tasks.Count > 0) {

				ExecuteTasks();

				/*
				for (int i = 0; i < blocks.Length; i++) {
					//Debug.Log (timer.GetCurrentTime () + " -> " + actions [i]);
					toPrint += "\n"+timer.GetCurrentTime () + " -> " + blocks [i];
					simulationTextArea.text = toPrint;
				}
				*/

			} else {
				Debug.Log ("There are no tasks to be completed.");
			}
		//}catch(Exception e) {
		//	Debug.Log ("There are no blocks to be executed");
		//}
	}

	// ---------- sample actions --------------

	//TODO: execute each action (or not, depending on the task)
	public void ExecuteTasks() {
		
		toPrint += "\n";

		if (blocks.Length > 0) {

			//Debug.Log ("From ExecuteTask(): ");
			foreach (string block in blocks)
				Debug.Log (block);



			for (int i = 0; i < genTasks.tasks.Count; i++) {

				//Debug.Log ("From ExecuteTask(): Value of A1: " + genTasks.tasks [i].GetA1 ());
				//Debug.Log ("From ExecuteTask(): Value of A2: " + genTasks.tasks [i].GetA2 ());
				//Debug.Log ("From ExecuteTask(): Value of A3: " + genTasks.tasks [i].GetA3 ());


				for (int j = 0; j < blocks.Length; j++) {	
					
					if (blocks [j] == "CheckIn") {
						CheckIn (genTasks.tasks [i].GetName ());
						//toPrint += "\n (" + timer.GetCurrentTime() + ") " + genTasks.tasks [i].GetName() + " is being checked in";
					} else if (blocks [j] == "Wash" && genTasks.tasks [i].GetA1 ()) {
						ExecuteA1 (genTasks.tasks [i].GetName ());
						//toPrint += "\n (" + timer.GetCurrentTime() + ") " + genTasks.tasks [i].GetName() + " is being washed.";
					} else if (blocks [j] == "Cut" && genTasks.tasks[i].GetA2()) {
						ExecuteA2 (genTasks.tasks [i].GetName ());
						//toPrint +=  "\n (" + timer.GetCurrentTime() + ") " + genTasks.tasks [i].GetName() + "'s fur is being cut.";
					} else if (blocks [j] == "Dry" && genTasks.tasks[i].GetA3()){
						ExecuteA3 (genTasks.tasks [i].GetName ());
						//toPrint +=  "\n (" + timer.GetCurrentTime() + ") " + genTasks.tasks [i].GetName() + " is being dryed.";
					} else if (blocks [j] == "Return") {
						Return (genTasks.tasks [i].GetName ());
						//toPrint += "\n (" + timer.GetCurrentTime () + ") " + genTasks.tasks [i].GetName() + " was returned.";
					}
				}
				genTasks.tasks.RemoveAt (i);

			}
		} else {
			Debug.Log ("There are no blocks to execute!");
		}

		simulationTextArea.text = toPrint;
		//Debug.Log ("toPrint: " + toPrint);
	}

	private void CheckIn(string name) {
		toPrint += "\n (" + timer.GetCurrentTime() + ") " + name + " is being checked in";
	}

	private void ExecuteA1(string name) {
		toPrint += "\n (" + timer.GetCurrentTime() + ") " + name + " is being washed.";
	}

	private void ExecuteA2(string name) {
		toPrint +=  "\n (" + timer.GetCurrentTime() + ") " + name + "'s fur is being cut.";
	}

	private void ExecuteA3(string name) {
		toPrint +=  "\n (" + timer.GetCurrentTime() + ") " + name + " is being dryed.";
	}

	private void Return(string name) {
		toPrint +=  "\n (" + timer.GetCurrentTime() + ") " + name + " was returned.";
	}

}
