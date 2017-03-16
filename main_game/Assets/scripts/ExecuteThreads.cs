using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ExecuteThreads : MonoBehaviour {

	ToolboxManager manager;
	GameObject disablePanel;
	ProgressBar bar;

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
	Transform[] blocks_t1;
	Transform[] blocks_t2;

	bool stop = false;

	bool t1_has_scissors;
	bool t1_has_soap;
	bool t1_has_dryer;
	bool t1_has_brush;
	bool t1_has_towel;
	bool t1_has_station;

	bool t2_has_scissors;
	bool t2_has_soap;
	bool t2_has_dryer;
	bool t2_has_brush;
	bool t2_has_towel;
	bool t2_has_station;

	void Start() {

		t1_has_scissors = false;
		t1_has_soap = false;
		t1_has_dryer = false;
		t1_has_brush = false;
		t1_has_towel = false;
		t1_has_station = false;

		t2_has_scissors = false;
		t2_has_soap = false;
		t2_has_dryer = false;
		t2_has_brush = false;
		t2_has_towel = false;
		t2_has_station = false;

		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
		timer = GameObject.FindObjectOfType<Timer> ();
		disablePanel = GameObject.Find ("DisablePanel");
		bar = GameObject.Find ("RadialProgressBar").GetComponent<ProgressBar>();

		try { 
			disablePanel.SetActive (false);
		} catch {
			Debug.Log ("Panel is disabled.");
		}
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

	private Transform[] GetActionBlocks_MultiThreads(String tabNum) {
		//get children in drop area for thread
		//threadChildren = new GameObject[this.transform.Find("DropAreaThread").childCount];
		Transform[] threadChildren = new Transform[this.transform.Find("Tab" + tabNum).FindChild("ScrollRect").FindChild("DropAreaThread" + tabNum).childCount];
		int childCount = threadChildren.Length;

		//Debug.Log ("thread childCount: " + childCount);

		for (int i = 0; i < childCount; i++) {
			//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;

			threadChildren [i] = this.transform.Find ("Tab" + tabNum).FindChild("ScrollRect").FindChild("DropAreaThread" + tabNum).GetChild(i);
			//threadChildren [i] = this.transform.Find ("DropAreaThread").GetChild (i).GetComponentInChildren<Text>().text;
			//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);

			//Debug.Log ("Child " + i + ": " + threadChildren [i].name);
		}
			
		return threadChildren;
	}

	public void Execute_MultiThreads() {

		simulationTextArea.text = "";
		stop = false;

		try {
			// disable all other functionalities
			disablePanel.SetActive (true);
		} catch {
			Debug.Log ("Cannot enable DisablePanel");
		}

		// switch to stop button
		GameObject.Find ("RunButton").transform.SetAsFirstSibling ();

		//simulationTextArea.text = "test";


		// ------------------------ READING TAB 1 ------------------------

		int thread1_whilesChildren = 0;

		blocks_t1 = GetActionBlocks_MultiThreads ("1");
		/*
		foreach (Transform action in blocks_t1)
			Debug.Log (action.GetComponentInChildren<Text>().text);
		*/

		//string[] blocks_names_t1 = new string[blocks_t1.Length];
		List<string> blocks_names_t1 = new List<string> ();

		int i = 0;
		bool isError = false; //unused, for now

		foreach (Transform child in blocks_t1) {
			
			if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.ACTION) {

				//Debug.Log ("TYPE ACTION");


				if (blocks_t1 [i].transform.GetComponentInChildren<Text> ().text == "get") {

					string resource = blocks_t1 [i].transform.Find ("Dropdown").Find ("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {
						terminateSimulation ();
						manager.showError ("Please select a resource to acquire in thread 1.");
					} else {

						switch (resource) {

						case "scissors":
							t1_has_scissors = true;
							break;
						
						case "soap":
							t1_has_soap = true;
							break;

						case "dryer":
							t1_has_dryer = true;
							break;
						
						case "brush":
							t1_has_brush = true;
							break;
						
						case "towel":
							t1_has_towel = true;
							break;
						
						case "station":
							t1_has_station = true;
							break;
						}


						blocks_names_t1.Add ("[thread 1] acquire ( " + resource + " );\n");
						i++;
					}

				} else if(blocks_t1 [i].transform.GetComponentInChildren<Text> ().text == "ret") {

					string resource = blocks_t1 [i].transform.Find ("Dropdown").Find ("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {
						terminateSimulation ();
						manager.showError ("Please select a resource to return in thread 1.");
					} else {

						switch (resource) {

						case "scissors":
							t1_has_scissors = false;
							break;

						case "soap":
							t1_has_soap = false;
							break;

						case "dryer":
							t1_has_dryer = false;
							break;

						case "brush":
							t1_has_brush = false;
							break;

						case "towel":
							t1_has_towel = false;
							break;

						case "station":
							t1_has_station = false;
							break;
						}

						blocks_names_t1.Add ("[thread 1] return ( " + resource + " );\n");
						i++;
					}

				} else {

					//blocks_names_t1 [i] = "[thread 1] " + blocks_t1 [i].transform.GetComponentInChildren<Text> ().text + ";";
					blocks_names_t1.Add ("[thread 1] " + blocks_t1 [i].transform.GetComponentInChildren<Text> ().text + ";\n");

					/*
					foreach(string name in blocks_names_t1)
						Debug.Log (name);
					*/

					i++;
				}

			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.IFSTAT) {

				//Debug.Log ("TYPE IFSTAT");

				string condition, actionText, line;
				try {

					condition = blocks_t1 [i].GetComponentInChildren<Text> ().text;
					actionText = blocks_t1 [i].FindChild ("DropArea").GetComponentInChildren<Text> ().text;

					//line = "[thread 1] if ( " + condition + " ) {\n    " + actionText + "\n}";
					line = "[thread 1] " + actionText + "; [ if ( " + condition + " ) ]\n";

				} catch (Exception e) {
					//manager.showError ("At least one if statement is empty.");
					//line = ">> ERROR: Empty if statement";
					simulationTextArea.text = "";
					manager.showError ("There is at least one empty if statement in thread 1.");
					terminateSimulation ();
					return;
				}

				//blocks_names_t1 [i] = line;
				blocks_names_t1.Add (line);

				//blocks_names [i] = blocks[i].transform.GetComponentInChildren<Text> ().text;
				i++;

			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.WHILELOOP) {

				string condition, line;
				string actionText = "";

				int whileChildrenCount = child.Find ("DropArea").childCount;
				thread1_whilesChildren += whileChildrenCount;
				//Debug.Log ("child " + child.name + ", child count: " + whileChildrenCount);

				//Debug.Log ("Thread 1 whileChildrenCount: " + whileChildrenCount);
				if (whileChildrenCount < 1) {
					//Debug.Log(">>> ERROR: There is at least one empty while loop");
					//simulationTextArea.text = "There is at least one empty while loop in thread 2.";
					simulationTextArea.text = "";
					manager.showError ("There is at least one empty while loop in thread 1.");
					terminateSimulation ();
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

				/*
				foreach (Transform whileChild in whileChildren) {
					actionText += "\t" + whileChild.GetComponentInChildren<Text>().text + ";\n";
				}
				*/

				try {

					condition = blocks_t1 [i].Find ("Condition").GetComponent<Text> ().text;
					if (condition == "< 2") {

						if (whileChildrenCount > 1) {

							//Debug.Log("There are " + whileChildrenCount + " children.");

							/*
							for (int k = 0; k < whileChildrenCount; k++) 
								Debug.Log(whileChildren[k].GetComponentInChildren<Text>().text);
							*/

							for (int l = 0; l < 2; l++) {
							
								for (int m = 0; m < whileChildrenCount; m++) {
								
									blocks_names_t1.Add ("[thread 1] " + whileChildren [m].GetComponentInChildren<Text> ().text + "; " +
									"[ while ( " + condition + " ), iter = " + (l + 1) + " ]\n");
								}
							}

						} else {
							//Debug.Log ("There is 1 child.");
							for (int k = 0; k < 2; k++) {
								blocks_names_t1.Add ("[thread 1] " + whileChildren [0].GetComponentInChildren<Text> ().text + "; " +
								"[ while ( " + condition + " ), iter = " + (k + 1) + " ]\n");
							}
						}

					} else {
						Debug.Log ("Unidentified condition");
					}
						
					/*
					int k = 0;
					condition = blocks_t1 [i].Find("Condition").GetComponent<Text> ().text;

					if (condition == "< 2") {

						if (whileChildrenCount == 1) {
							blocks_names_t1.Add("[thread 1] " + whileChildren[0].GetComponentInChildren<Text>().text + ";\n");
							blocks_names_t1.Add("[thread 1] " + whileChildren[0].GetComponentInChildren<Text>().text + ";\n");
						} else {
							for (int j = 0; j < 2; j++) {

								Debug.Log("iteration: " + j);
								blocks_names_t1.Add("[thread 1] " + whileChildren[k].GetComponentInChildren<Text>().text + ";\n");
								blocks_names_t1.Add("[thread 1] " + whileChildren[k+1].GetComponentInChildren<Text>().text + ";\n");

								Debug.Log("Current items in the list: ");
								foreach(string this_line in blocks_names_t1)
									Debug.Log(this_line);

								//k+=2;
							}
						}


					}*/
						
					//line = "[thread 1] while ( " + condition + " ) {\n" + actionText + "}";

				} catch (Exception e) {
					manager.showError ("Exception caught.");
					line = ">>> Exception caught.";
				}

				//blocks_names_t1 [i] = line;

				i++;
			}
		}

		//toPrint = "";

		/*
		try {
			if (blocks_t1.Length > 0) {
				foreach (string line in blocks_names_t1) {
					//Debug.Log (block);
					//toPrint += "\n (" + timer.GetCurrentTime() + ") " + name + "ing";
					toPrint += "\n" + line;
				}

				simulationTextArea.text = toPrint;
			} else {
				manager.showError ("There are no actions to run in thread 1.");
				simulationTextArea.text = "";
			}
		} catch (Exception e) {
			manager.showError ("There are no actions to run in thread 1.");
			simulationTextArea.text = "";
		}
		*/

		// ------------------------ READING TAB 2 ------------------------


		blocks_t2 = GetActionBlocks_MultiThreads ("2");

		int thread2_whilesChildren = 0;

		//string[] blocks_names_t2 = new string[blocks_t2.Length];
		List<string> blocks_names_t2 = new List<string> ();

		i = 0;

		foreach (Transform child in blocks_t2) {

			if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.ACTION) {

				if (blocks_t2 [i].transform.GetComponentInChildren<Text> ().text == "get") {

					string resource = blocks_t2 [i].transform.Find ("Dropdown").Find("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {
						terminateSimulation ();
						manager.showError ("Please select a resource to acquire in thread 2.");
					} else {

						switch (resource) {

						case "scissors":
							t2_has_scissors = true;
							break;

						case "soap":
							t2_has_soap = true;
							break;

						case "dryer":
							t2_has_dryer = true;
							break;

						case "brush":
							t2_has_brush = true;
							break;

						case "towel":
							t2_has_towel = true;
							break;

						case "station":
							t2_has_station = true;
							break;
						}

						blocks_names_t2.Add ("[thread 2] acquire ( " + resource + " );\n");
						i++;
					}
				} else if(blocks_t2 [i].transform.GetComponentInChildren<Text> ().text == "ret") {
					
					string resource = blocks_t2 [i].transform.Find ("Dropdown").Find("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {
						terminateSimulation ();
						manager.showError ("Please select a resource to return in thread 2.");

					} else {

						switch (resource) {

						case "scissors":
							t2_has_scissors = false;
							break;

						case "soap":
							t2_has_soap = false;
							break;

						case "dryer":
							t2_has_dryer = false;
							break;

						case "brush":
							t2_has_brush = false;
							break;

						case "towel":
							t2_has_towel = false;
							break;

						case "station":
							t2_has_station = false;
							break;
						}

						blocks_names_t2.Add ("[thread 2] return ( " + resource + " );\n");
						i++;
					}

				} else {

					blocks_names_t2.Add ("[thread 2] " + blocks_t2 [i].transform.GetComponentInChildren<Text> ().text + ";\n");
					i++;
				}
					
			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.IFSTAT) {

				//Debug.Log ("TYPE IFSTAT");

				string condition, actionText, line;
				try {

					condition = blocks_t2 [i].GetComponentInChildren<Text> ().text;
					actionText = blocks_t2 [i].FindChild ("DropArea").GetComponentInChildren<Text> ().text;

					//line = "[thread 1] if ( " + condition + " ) {\n    " + actionText + "\n}";
					line = "[thread 2] " + actionText + "; [ if ( " + condition + " ) ]\n";

				} catch (Exception e) {
					//manager.showError ("At least one if statement is empty.");
					//line = ">> ERROR: Empty if statement";
					simulationTextArea.text = "";
					manager.showError ("There is at least one empty if statement in thread 2.");
					terminateSimulation ();
					return;
				}

				//blocks_names_t2 [i] = line;
				blocks_names_t2.Add (line);

				//blocks_names [i] = blocks[i].transform.GetComponentInChildren<Text> ().text;
				i++;

			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.WHILELOOP) {

				string condition, line;
				string actionText = "";

				int whileChildrenCount = child.Find ("DropArea").childCount;
				thread2_whilesChildren += whileChildrenCount;
				//Debug.Log ("child " + child.name + ", child count: " + whileChildrenCount);

				Debug.Log ("Thread 2 whileChildrenCount: " + whileChildrenCount);
				if (whileChildrenCount < 1) {
					//Debug.Log(">>> ERROR: There is at least one empty while loop");
					//simulationTextArea.text = "There is at least one empty while loop in thread 2.";
					simulationTextArea.text = "";
					manager.showError ("There is at least one empty while loop in thread 2.");
					terminateSimulation ();
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

				try {

					condition = blocks_t2 [i].Find ("Condition").GetComponent<Text> ().text;
					if (condition == "< 2") {

						if (whileChildrenCount > 1) {

							//Debug.Log ("There are " + whileChildrenCount + " children.");

							for (int l = 0; l < 2; l++) {

								for (int m = 0; m < whileChildrenCount; m++) {

									blocks_names_t2.Add ("[thread 1] " + whileChildren [m].GetComponentInChildren<Text> ().text + "; " +
									"[ while ( " + condition + " ), iter = " + (l + 1) + " ]\n");
								}
							}

						} else {
							//Debug.Log ("There is 1 child.");
							for (int k = 0; k < 2; k++)
								blocks_names_t2.Add ("[thread 2] " + whileChildren [0].GetComponentInChildren<Text> ().text + "; " +
								"[ while ( " + condition + " ), iter = " + (k + 1) + " ]\n");
						}

					} else {
						Debug.Log ("Unidentified condition");
					}

					//line = "[thread 1] while ( " + condition + " ) {\n" + actionText + "}";

				} catch (Exception e) {
					manager.showError ("Exception caught.");
					line = ">>> Exception caught.";
				}

				//blocks_names_t2 [i] = line;

				i++;
			}
		}

		//toPrint = "";
		/*
		int all_blocks_names_length = 0;
		if (blocks_t1.Length > blocks_t2.Length)
			all_blocks_names_length = blocks_t1.Length * 2;
		else
			all_blocks_names_length = blocks_t2.Length * 2;

		string[] all_blocks_names = new string[all_blocks_names_length];
		*/

		/*
		Debug.Log ("BEFORE");
		foreach (string name in blocks_names_t1)
			Debug.Log(name);
		*/

		if (blocks_t1.Length < 1) {
			manager.showError ("There are no actions to run in thread 1.");
			simulationTextArea.text = "";
			terminateSimulation ();
			return;
		}

		if (blocks_t2.Length < 1) {
			manager.showError ("There are no actions to run in thread 2.");
			simulationTextArea.text = "";
			terminateSimulation ();
			return;
		}

		if (blocks_t1 [0].transform.GetComponentInChildren<Text> ().text != "checkin" || blocks_t2 [0].transform.GetComponentInChildren<Text> ().text != "checkin") {

			manager.showError ("Remember to always check-in your costumer first!");
			terminateSimulation();

		} else if (blocks_t1 [blocks_t1.Length - 1].transform.GetComponentInChildren<Text> ().text != "checkout" || blocks_t2 [blocks_t2.Length - 1].transform.GetComponentInChildren<Text> ().text != "checkout") {
			manager.showError ("Remember to always check-out your costumer when you're done!");
			terminateSimulation();
		}

		int all_blocks_names_length = 0;
		if (blocks_t1.Length > blocks_t2.Length)
			all_blocks_names_length = (blocks_t1.Length * 2) + thread1_whilesChildren + thread2_whilesChildren;
		else
			all_blocks_names_length = (blocks_t2.Length * 2) + thread1_whilesChildren + thread2_whilesChildren;

		//string[] all_blocks_names = new string[all_blocks_names_length];

		/*
		List<string> all_blocks_names = new List<string>();

		//Debug.Log ("all_blocks_names.Count: " + all_blocks_names.Count);
		//Debug.Log ("all_blocks_names_length: " + all_blocks_names_length);

		int curr_index = 0;
		//int curr_index_t1 = 0;
		//int curr_index_t2 = 0;

		if (blocks_t1.Length > 0) {
			
			if (blocks_t2.Length > 0) {

				for (int j = 0; j < all_blocks_names_length; j+=2) {

					//Debug.Log ("curr_index: " + curr_index);
					//Debug.Log ("j: " + j);

					try {
						//all_blocks_names[j] = blocks_names_t1[curr_index];
						all_blocks_names.Add(blocks_names_t1[curr_index]);
						//Debug.Log(blocks_names_t1[curr_index]);

					} catch {
						Debug.Log ("Exception caught for thread 1");
						//all_blocks_names[j] = "";
						all_blocks_names.Add("");
					}


					try {
						//all_blocks_names[j+1] = blocks_names_t2[curr_index];
						all_blocks_names.Add(blocks_names_t2[curr_index]);
						//Debug.Log(blocks_names_t2[curr_index]);
					} catch {
						Debug.Log ("Exception caught for thread 2");
						//all_blocks_names[j+1] = "";
						all_blocks_names.Add("");
					}

						
					curr_index++;
				}
				
			} else {
				manager.showError ("There are no actions to run in thread 2.");
				simulationTextArea.text = "";
				return;
			}
		} else {
			manager.showError ("There are no actions to run in thread 1.");
			simulationTextArea.text = "";
			return;
		}

		Debug.Log ("AFTER");
		//foreach(string line in all_blocks_names)
		foreach(string line in blocks_names_t1)
			Debug.Log (line);
		*/

		//StartCoroutine (printThreads (all_blocks_names, blocks_names_t1, blocks_names_t2));
		StartCoroutine (printThreads (blocks_names_t1, blocks_names_t2));

		/*
		int step_counter = 1;
		for (int j = 0; j < all_blocks_names.Length; j+=2) {
		
			toPrint += "\n\nSTEP " + step_counter + ": ";
			toPrint += "\n" + all_blocks_names [j];
			toPrint += "\n" + all_blocks_names [j+1];

			step_counter++;
		}
			
		simulationTextArea.text = toPrint;
		*/

		/*
		try {
			if (blocks_t2.Length > 0) {
				foreach (string line in blocks_names_t2) {
					//Debug.Log (block);
					//toPrint += "\n (" + timer.GetCurrentTime() + ") " + name + "ing";
					toPrint += "\n" + line;
				}

				simulationTextArea.text += toPrint;
				Debug.Log(""+toPrint);

			} else {
				//Debug.Log("[thread 2 - 1] There are no actions to run.");
				//manager.showError ("[3] There are no actions to run.");
				//simulationTextArea.text += "";
			}
		} catch (Exception e) {
			//Debug.Log("[thread 2 - 2] There are no actions to run.");
			manager.showError ("There are no actions to run in thread 2.");
			//simulationTextArea.text += "";
		}
		*/
	}

	public void terminateSimulation() {
	
		stop = true;

		try {
			disablePanel.SetActive (false);
		} catch {
			Debug.Log ("Cannot disable DisablePanel.");
		}
		simulationTextArea.text = "";

		GameObject.Find ("RunButton").transform.SetAsLastSibling ();
	}

	IEnumerator printThreads(List<string> b1, List<string> b2) {

		bar.currentAmount = 0;

		int step_counter = 1;
		int limit = 0;
		bool paused = false;

		if (b1.Count > b2.Count)
			limit = b1.Count;
		else
			limit = b2.Count;

		// Debug.Log ("limit = " + limit);

		//Debug.Log ("[IN COROUTINE]: (b1.Count = " + b1.Count);
		//foreach(string line in b1)
		//	Debug.Log (line);

		//		Debug.Log ("[IN COROUTINE]: (b2.Count = " + b2.Count);
		//		foreach(string line in b2)
		//			Debug.Log (line);


		for (int j = 0; j < limit; j++) {

			if (bar.currentAmount < 100) {
				bar.currentAmount += 50;
				bar.LoadingBar.GetComponent<Image>().fillAmount = bar.currentAmount / 100;

			} else {

				manager.simShowError ("Time is up! The day is over.");
				stop = true;
				paused = true;
				try {
					disablePanel.SetActive (true);
				} catch {
					Debug.Log ("Cannot disable DisablePanel");
				}
				GameObject.Find ("StopButton").transform.GetComponent<Button> ().interactable = false;
			}

			if (stop) {

				break;
				yield break;

				if (!paused) {

					try {
						disablePanel.SetActive (false);
					} catch {
						Debug.Log ("Cannot disable DisablePanel");
					}
					//simulationTextArea.text = "";

					GameObject.Find ("RunButton").transform.SetAsLastSibling ();
				}
					
				yield return 0;

			} else {

				simulationTextArea.text += "\nSTEP " + step_counter + ": \n";

				try {
					simulationTextArea.text += "" + b1 [j];
				} catch { }

				try {
					simulationTextArea.text += "" + b2 [j];
				} catch { }

				step_counter++;

				yield return new WaitForSeconds (1);
			}


		}



		//simulationTextArea.text = toPrint;

		//yield return new WaitForSeconds(5);



	}
}
