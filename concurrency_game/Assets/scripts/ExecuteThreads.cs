using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ExecuteThreads : MonoBehaviour {

	//TODO: save original threadchildren positions (in case order is incorrect)

	ToolboxManager manager;

	public Transform runButton;
	private Timer timer;
	private int numActions;
	private string toPrint;
	//get simulation space for printing
	public Text simulationTextArea;
	//get instance of GenerateTasks
	public GenerateTasks genTasks;
	//Task t; //"playerTank"

	Transform[] blocks;

	void Start() {

		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
		timer = GameObject.FindObjectOfType<Timer> ();
	}

	/*
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
	*/

	private Transform[] GetActionBlocks() {

		//get children in drop area for thread
		//threadChildren = new GameObject[this.transform.Find("DropAreaThread").childCount];
		Transform[] threadChildren = new Transform[this.transform.Find("DropAreaThread").childCount];
		int childCount = threadChildren.Length;

		Debug.Log ("thread childCount: " + childCount);

		for (int i = 0; i < childCount; i++) {
			//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;
		
			threadChildren [i] = this.transform.Find ("DropAreaThread").GetChild(i);
			//threadChildren [i] = this.transform.Find ("DropAreaThread").GetChild (i).GetComponentInChildren<Text>().text;
			//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);
		}

		return threadChildren;

		/*
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
		*/
	}

	//private void Simulate() {

		//Debug.Log ("actions.Length: " + actions.Length);

		//if there are blocks and there are tasks to be completed
		//try{
			//if (genTasks.tasks.Count > 0) {

				//ExecuteTasks();

				/*
				for (int i = 0; i < blocks.Length; i++) {
					//Debug.Log (timer.GetCurrentTime () + " -> " + actions [i]);
					toPrint += "\n"+timer.GetCurrentTime () + " -> " + blocks [i];
					simulationTextArea.text = toPrint;
				}
				*/

			//}

		//}catch(Exception e) {
		//	Debug.Log ("There are no blocks to be executed");
		//}
	//}

	// ---------- sample actions --------------

	//TODO: execute each action (or not, depending on the task)
	/*
	public void ExecuteTasks() {
		
		//toPrint += "\n";

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
					} else if (blocks [j] == "Cut" && genTasks.tasks [i].GetA2 ()) {
						ExecuteA2 (genTasks.tasks [i].GetName ());
						//toPrint +=  "\n (" + timer.GetCurrentTime() + ") " + genTasks.tasks [i].GetName() + "'s fur is being cut.";
					} else if (blocks [j] == "Dry" && genTasks.tasks [i].GetA3 ()) {
						ExecuteA3 (genTasks.tasks [i].GetName ());
						//toPrint +=  "\n (" + timer.GetCurrentTime() + ") " + genTasks.tasks [i].GetName() + " is being dryed.";
					} else if (blocks [j] == "Return") {
						Return (genTasks.tasks [i].GetName ());
						//toPrint += "\n (" + timer.GetCurrentTime () + ") " + genTasks.tasks [i].GetName() + " was returned.";
					}
				}
				genTasks.tasks.RemoveAt (i);

			}
		}

		simulationTextArea.text = toPrint;
		//Debug.Log ("toPrint: " + toPrint);
	}
	*/

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


	public void Execute() {

		blocks = GetActionBlocks ();
		string[] blocks_names = new string[blocks.Length];
		int i = 0;
		bool isError = false; //unused, for now

		foreach (Transform child in blocks) {
			if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.ACTION) {

				//Debug.Log ("TYPE ACTION");

				blocks_names [i] = blocks [i].transform.GetComponentInChildren<Text> ().text + ";";
				i++;
			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.IFSTAT) {

				//Debug.Log ("TYPE IFSTAT");

				string condition, actionText, line;
				try {
					
					condition = blocks [i].GetComponentInChildren<Text> ().text;
					actionText = blocks [i].FindChild ("DropArea").GetComponentInChildren<Text> ().text + ";";

					line = "\nif ( " + condition + " ) {\n    " + actionText + "\n}\n";

				} catch (Exception e) {
					//manager.showError ("At least one if statement is empty.");
					//line = ">> ERROR: Empty if statement";
					simulationTextArea.text = ">>> ERROR: There is at least one empty for statement";
					return;
				}
					
				blocks_names [i] = line;

				//blocks_names [i] = blocks[i].transform.GetComponentInChildren<Text> ().text;
				i++;

			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.WHILELOOP) {

				string condition, line;
				string actionText = "";

				int whileChildrenCount = child.Find ("DropArea").childCount;
				Debug.Log ("child " + child.name + ", child count: " + whileChildrenCount);

				if (whileChildrenCount < 1) {
					Debug.Log(">>> ERROR: There is at least one empty while loop");
					simulationTextArea.text = ">>> ERROR: There is at least one empty while loop";
					return;
				}

				Transform[] whileChildren = new Transform[whileChildrenCount];

				for (int k = 0; k < whileChildrenCount; k++) {
					//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;

					whileChildren [k] = child.Find ("DropArea").GetChild (k);
					//threadChildren [i] = this.transform.Find ("DropAreaThread").GetChild (i).GetComponentInChildren<Text>().text;
					//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);

					//Debug.Log ("actions: " + whileChildren [k]);
				}

				foreach (Transform whileChild in whileChildren) {
					actionText += "\t" + whileChild.GetComponentInChildren<Text>().text + ";\n";
				}
		
				try {
						condition = blocks [i].GetComponentInChildren<Text> ().text;
						
						//TODO: Need to get ALL children
						//actionText = blocks [i].FindChild ("DropArea").GetComponentInChildren<Text> ().text;

					line = "\nwhile ( " + condition + " ) {\n" + actionText + "}\n";

				} catch (Exception e) {
					manager.showError ("Exception caught.");
					line = ">>> Exception caught.";
				}

				blocks_names [i] = line;

				i++;
			}
		}
			
		toPrint = "";

		try {
			if (blocks.Length > 0) {
				foreach (string line in blocks_names) {
					//Debug.Log (block);
					//toPrint += "\n (" + timer.GetCurrentTime() + ") " + name + "ing";
					toPrint += "\n" + line;
				}

				simulationTextArea.text = toPrint;
			} else {
				manager.showError ("There are no actions to run.");
			}
		} catch (Exception e) {
			manager.showError ("There are no actions to run");
		}
	}
}
