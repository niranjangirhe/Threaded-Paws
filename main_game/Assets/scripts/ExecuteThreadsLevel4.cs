using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ExecuteThreadsLevel4 : MonoBehaviour {

	ToolboxManager manager;
	GameObject disablePanel;
	ProgressBar bar;

	public GameObject runButton;
	public GameObject stopButton;

	private Timer timer;
	private int numActions;
	private string toPrint;
	//get simulation space for printing
	public Text simulationTextArea;

	Transform[] blocks;
	Transform[] blocks_t1;
	Transform[] blocks_t2;

	bool stop;
	bool err;
	bool paused;
	bool lost;

	// bool t1_has_dog;
	// bool t2_has_dog;

	bool t1_checkedin;
	bool t1_checkedout;
	bool t1_has_brush;
	bool t1_has_clippers;
	bool t1_has_conditioner;
	bool t1_has_dryer;
	bool t1_has_scissors;
	bool t1_has_shampoo;
	bool t1_has_station;
	bool t1_has_towel;

	bool t2_checkedin;
	bool t2_checkedout;
	bool t2_has_brush;
	bool t2_has_clippers;
	bool t2_has_conditioner;
	bool t2_has_dryer;
	bool t2_has_scissors;
	bool t2_has_shampoo;
	bool t2_has_station;
	bool t2_has_towel;

	bool t1_needs_cut;
	bool t1_needs_dry;
	bool t1_needs_wash;
	bool t1_needs_groom;
	bool t1_did_cut;
	bool t1_did_dry;
	bool t1_did_wash;
	bool t1_did_groom;

	bool t2_needs_cut;
	bool t2_needs_dry;
	bool t2_needs_wash;
	bool t2_needs_groom;
	bool t2_did_cut;
	bool t2_did_dry;
	bool t2_did_wash;
	bool t2_did_groom;

	string returnErrMsg = "\n> ERROR: You are trying to return a resource you don't have.";
	string acquireErrMsg = "\n> ERROR: You are trying to acquire a resource you already have.";

	void Start() {

		// t1_has_dog = false;
		// t2_has_dog = false;

		stop = false;
		err = false;
		paused = false;
		lost = false;

		t1_checkedin = false;
		t1_checkedout = false;
		t2_checkedin = false;
		t2_checkedout = false;

		t1_has_brush = false;
		t1_has_clippers = false;
		t1_has_conditioner = false;
		t1_has_dryer = false;
		t1_has_scissors = false;
		t1_has_shampoo = false;
		t1_has_station = false;
		t1_has_towel = false;

		t2_has_brush = false;
		t2_has_clippers = false;
		t2_has_conditioner = false;
		t2_has_dryer = false;
		t2_has_scissors = false;
		t2_has_shampoo = false;
		t2_has_station = false;
		t2_has_towel = false;

		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
		timer = GameObject.FindObjectOfType<Timer> ();
		disablePanel = GameObject.Find ("DisablePanel");
		bar = GameObject.Find ("RadialProgressBar").GetComponent<ProgressBar>();

		try { 
			disablePanel.SetActive (false);
		} catch {
			Debug.Log ("Disable Panel can't be found.");
		}
	}

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


	// USED FOR LEVEL **4** ONLY
	public void Execute_MultiThreads_Level4() {

		t1_did_cut = false;
		t1_did_dry = false;
		t1_did_wash = false;
		t1_did_groom = false;

		t2_did_cut = false;
		t2_did_dry = false;
		t2_did_wash = false;
		t2_did_groom = false;

//		t1_needs_cut = true;
//		t1_needs_dry = false;
//		t1_needs_wash = false;
//		t1_needs_groom = false;
//
//		t2_needs_cut = true;
//		t2_needs_dry = false;
//		t2_needs_wash = false;
//		t2_needs_groom = true;

		simulationTextArea.text = "";

		try {
			GameObject.Find("InformationPanel").SetActive(false);
		} catch {}

		try {

			GameObject.Find("AgendaPanel").SetActive(false);
		} catch { }

		stop = false;
		err = false;
		paused = false;
		lost = false;

		t1_checkedin = false;
		t1_checkedout = false;
		t2_checkedin = false;
		t2_checkedout = false;

		t1_has_brush = false;
		t1_has_clippers = false;
		t1_has_conditioner = false;
		t1_has_dryer = false;
		t1_has_scissors = false;
		t1_has_shampoo = false;
		t1_has_station = false;
		t1_has_towel = false;

		t2_has_brush = false;
		t2_has_clippers = false;
		t2_has_conditioner = false;
		t2_has_dryer = false;
		t2_has_scissors = false;
		t2_has_shampoo = false;
		t2_has_station = false;
		t2_has_towel = false;

		try {
			// disable all other functionalities
			disablePanel.SetActive (true);
		} catch {
			Debug.Log ("Cannot enable DisablePanel");
		}

		// switch to stop button
		runButton.transform.SetAsFirstSibling ();

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
						return;

					} else {

						blocks_names_t1.Add ("[thread 1] acquire ( " + resource + " );\n");
						i++;
					}

				} else if(blocks_t1 [i].transform.GetComponentInChildren<Text> ().text == "ret") {

					string resource = blocks_t1 [i].transform.Find ("Dropdown").Find ("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {
						terminateSimulation ();
						manager.showError ("Please select a resource to return in thread 1.");
						return;
					} else {

						blocks_names_t1.Add ("[thread 1] return ( " + resource + " );\n");
						i++;
					}

				} else {

					//blocks_names_t1 [i] = "[thread 1] " + blocks_t1 [i].transform.GetComponentInChildren<Text> ().text + ";";
					blocks_names_t1.Add ("[thread 1] " + blocks_t1 [i].transform.GetComponentInChildren<Text> ().text + ";\n");

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

					} else if (condition == "list != empty") {

//						if (whileChildrenCount > 1) {
							
							for (int m = 0; m < whileChildrenCount; m++) {

								Transform currChild = whileChildren[m];
								
								if (currChild.GetComponent<Draggable>().typeOfItem == Draggable.Type.IFSTAT) {

								// Debug.Log("IF statement first child: " + currChild.GetChild(1).name);

									string action = currChild.GetChild(1).GetComponentInChildren<Text>().text;
									blocks_names_t1.Add ("[thread 1] " + action + "; " +
										"[ while ( " + condition + " ), if needed ]\n");
									
									// Debug.Log("Adding to the list: " + "[thread 1] " + action + "; " +
										// "[ while ( " + condition + " ), if needed ]\n");

								} else {

									blocks_names_t1.Add ("[thread 1] " + whileChildren [m].GetComponentInChildren<Text> ().text + "; " +
										"[ while ( " + condition + " ) ]\n");

									// Debug.Log("Adding to the list: " + "[thread 1] " + whileChildren [m].GetComponentInChildren<Text> ().text + "; " +
										// "[ while ( " + condition + " ) ]\n");
								}
						
						} 

//						else {
//							blocks_names_t1.Add ("[thread 1] " + whileChildren [0].GetComponentInChildren<Text> ().text + "; " +
//									"[ while ( " + condition + " ) ]\n");
//						}

					} else {
						Debug.Log ("Unidentified condition");
					}


					//line = "[thread 1] while ( " + condition + " ) {\n" + actionText + "}";

				} catch (Exception e) {
					manager.showError ("Exception caught.");
					line = ">>> Exception caught.";
				}

				//blocks_names_t1 [i] = line;

				i++;
			}
		}

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

					// Debug.Log ("... attempting resource: " + resource);


					if (resource == "[null]") {

						Debug.Log ("Please select a resource to acquire in thread 2.");

						terminateSimulation ();
						manager.showError ("Please select a resource to acquire in thread 2.");
						return;

					} else {

						blocks_names_t2.Add ("[thread 2] acquire ( " + resource + " );\n");
						i++;
					}

				} else if(blocks_t2 [i].transform.GetComponentInChildren<Text> ().text == "ret") {

					string resource = blocks_t2 [i].transform.Find ("Dropdown").Find("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {

						terminateSimulation ();
						manager.showError ("Please select a resource to return in thread 2.");
						return;

					} else {

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

				// Debug.Log ("Thread 2 whileChildrenCount: " + whileChildrenCount);
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

									blocks_names_t2.Add ("[thread 2] " + whileChildren [m].GetComponentInChildren<Text> ().text + "; " +
										"[ while ( " + condition + " ), iter = " + (l + 1) + " ]\n");
								}
							}

						} else {
							//Debug.Log ("There is 1 child.");
							for (int k = 0; k < 2; k++)
								blocks_names_t2.Add ("[thread 2] " + whileChildren [0].GetComponentInChildren<Text> ().text + "; " +
									"[ while ( " + condition + " ), iter = " + (k + 1) + " ]\n");
						}

					} else if (condition == "list != empty") {
						
						for (int m = 0; m < whileChildrenCount; m++) {

							Transform currChild = whileChildren[m];

							if (currChild.GetComponent<Draggable>().typeOfItem == Draggable.Type.IFSTAT) {

								string action = currChild.GetChild(1).GetComponentInChildren<Text>().text;
								blocks_names_t2.Add ("[thread 1] " + action + "; " +
									"[ while ( " + condition + " ), if needed ]\n");

								// Debug.Log("Adding to the list: " + "[thread 1] " + action + "; " +
								// 	"[ while ( " + condition + " ), if needed ]\n");

							} else {

								blocks_names_t2.Add ("[thread 1] " + whileChildren [m].GetComponentInChildren<Text> ().text + "; " +
									"[ while ( " + condition + " ) ]\n");

								// Debug.Log("Adding to the list: " + "[thread 1] " + whileChildren [m].GetComponentInChildren<Text> ().text + "; " +
								// 	"[ while ( " + condition + " ) ]\n");
							}

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
			
		if (blocks_t1.Length > 1) {
			manager.showError ("You only need one element per thread. It wraps everything else.");
			simulationTextArea.text = "";
			terminateSimulation ();
			return;
		}

		if (blocks_t2.Length > 1) {
			manager.showError ("You only need one element per thread. It wraps everything else.");
			simulationTextArea.text = "";
			terminateSimulation ();
			return;
		}

		try {
			if ((blocks_names_t1 [0].Substring (11, 7) != "checkin" /*&& blocks_names_t1 [0].Substring (11, 17) != "pickup"*/)
				|| (blocks_names_t2 [0].Substring (11, 7) != "checkin" /*&& blocks_names_t2 [0].Substring (11, 17) != "pickup"*/)) {

				manager.showError ("Remember to always check-in your costumer first!");
				terminateSimulation ();
				return;
			}
		} catch {
			manager.showError ("Remember to always check-in your costumer first!");
			terminateSimulation ();
			return;
		}

		try {

			// Debug.Log(blocks_names_t1.Count);

			if ((blocks_names_t1 [blocks_names_t1.Count - 1].Substring (11, 8) != "checkout")
				|| (blocks_names_t2 [blocks_names_t2.Count - 1].Substring (11, 8) != "checkout")) {

				manager.showError ("Remember to always check-out your costumer when you're done!");
				terminateSimulation ();
				return;
			}
		} catch{
			manager.showError ("Remember to always check-out your costumer when you're done!");
			terminateSimulation ();
			return;
		}

		if (!err)
			StartCoroutine (printThreads (blocks_names_t1, blocks_names_t2, 1));

	}

	public void pauseSimulation() {

		err = true;
		lost = true;
		stop = true;
		// paused = true;

		try {
			disablePanel.SetActive (false);
		} catch {
			Debug.Log ("Cannot disable DisablePanel.");
		}

		runButton.transform.SetAsLastSibling ();
		bar.LoadingBar.GetComponent<Image> ().fillAmount = 0;
	}

	public void terminateSimulation() {

		err = true;
		lost = true;
		stop = true;
		// paused = true;

		try {
			disablePanel.SetActive (false);
		} catch {
			Debug.Log ("Cannot disable DisablePanel.");
		}
		simulationTextArea.text = "";

		runButton.transform.SetAsLastSibling ();
		bar.LoadingBar.GetComponent<Image> ().fillAmount = 0;
	}

	IEnumerator printThreads(List<string> b1, List<string> b2, int speed) {

		bar.currentAmount = 0;

		int step_counter = 1;
		int t1_curr_index = 0;
		int t2_curr_index = 0;

		bool t1_canPrint = true;
		bool t2_canPrint = true;

		int limit = 0;
		int j = 0;

		if (b1.Count > b2.Count)
			limit = b1.Count;
		else
			limit = b2.Count;

		// for (int j = 0; j < limit; j++) {
		// while (j < limit) {

		// while (j < 100) {

		while ((t1_curr_index < b1.Count) || (t2_curr_index < b2.Count)) {

			// Debug.Log ("b1.Count = " + b1.Count + ", t1_curr_index = " + t1_curr_index);

			if (bar.currentAmount < 100) {

				// Debug.Log ("bar.currentAmount < 100. Bar updated.");

				bar.currentAmount += speed;
				bar.LoadingBar.GetComponent<Image>().fillAmount = bar.currentAmount / 100;

			} else {

				manager.gameLost();
				stop = true;
				paused = true;
				lost = true;

				stopButton.transform.GetComponent<Button> ().interactable = false;
				// bar.LoadingBar.GetComponent<Image> ().fillAmount = 0;

				// break;
				// yield break;
				yield return 0;
			}

			if (stop) {

				if (!paused) {

					try {
						disablePanel.SetActive (false);
					} catch {
						Debug.Log ("Cannot disable DisablePanel");
					}
					//simulationTextArea.text = "";

					runButton.transform.SetAsLastSibling ();
					// bar.LoadingBar.GetComponent<Image> ().fillAmount = 0;

				}

				// Debug.Log ("Bar set to 0 in if(stop)");

				bar.LoadingBar.GetComponent<Image> ().fillAmount = 0;

				break;
				yield break;
				yield return 0;

			} else {

				simulationTextArea.text += "\nSTEP " + (j+1) + ": \n";

				// ------------------------------  THREAD 1 ------------------------------

				try {
					
					// {"[null]", "brush" ,"clippers" , "cond.", "dryer", "scissors", "shampoo", "station", "towel"};

					if (b1[t1_curr_index].Substring(11, 3) == "acq") {

						// Debug.Log("ACQUIRING " + b1[t1_curr_index].Substring(21, 5));
							
						// acquiring resource
						switch(b1[t1_curr_index].Substring(21, 5)) {

						case "brush":

							if (!t1_has_brush && t2_has_brush) { // need to wait for resource

								simDisplay("[thread 1] Waiting for a brush...\n");
								t1_canPrint = false;

							} else {
								int output = acquire (ref t1_has_brush);
								t1_canPrint = true;
								// lost = false;

								if (output < 0) {
									resError(b1[t1_curr_index]);
									resError(acquireErrMsg);
								}
							}

							break;

						case "clipp":
							
							if (!t1_has_clippers && t2_has_clippers) { // need to wait for resource

								simDisplay("[thread 1] Waiting for nail clippers...\n");
								t1_canPrint = false;

							} else {
								int output = acquire (ref t1_has_clippers);
								t1_canPrint = true;
								// lost = false;

								if (output < 0) {
									resError(b1[t1_curr_index]);
									resError(acquireErrMsg);
								}
							}

							break;

						case "cond.":

							if (!t1_has_conditioner && t2_has_conditioner) { // need to wait for resource

								simDisplay("[thread 1] Waiting for condtioner...\n");
								t1_canPrint = false;

							} else {
								
								int output = acquire (ref t1_has_conditioner);
								t1_canPrint = true;
								// lost = false;

								if (output < 0) {
									resError(b1[t1_curr_index]);
									resError(acquireErrMsg);
								}
							}

							break;

						case "dryer":

							if (!t1_has_dryer && t2_has_dryer) { // need to wait for resource

								simDisplay("[thread 1] Waiting for dryer...\n");
								t1_canPrint = false;

							} else {
								int output = acquire (ref t1_has_dryer);
								t1_canPrint = true;
								// lost = false;

								if (output < 0) {
									resError(b1[t1_curr_index]);
									resError(acquireErrMsg);
								}
							}

							break;

						case "sciss":
							
							if (!t1_has_scissors && t2_has_scissors) { // need to wait for resource

								simDisplay("[thread 1] Waiting for scissors...\n");
								t1_canPrint = false;

							} else {
								int output = acquire (ref t1_has_scissors);
								t1_canPrint = true;
								// lost = false;

								if (output < 0) {
									resError(b1[t1_curr_index]);
									resError(acquireErrMsg);
								}
							}

							break;

						case "shamp":

							if (!t1_has_shampoo && t2_has_shampoo) { // need to wait for resource

								simDisplay("[thread 1] Waiting for shampoo...\n");
								t1_canPrint = false;

							} else {
								int output = acquire (ref t1_has_shampoo);
								t1_canPrint = true;
								// lost = false;

								if (output < 0) {
									resError(b1[t1_curr_index]);
									resError(acquireErrMsg);
								}
							}

							break;

						case "stati":

							if (!t1_has_station && t2_has_station) { // need to wait for resource

								simDisplay("[thread 1] Waiting for a station...\n");
								t1_canPrint = false;

							} else {
								int output = acquire (ref t1_has_station);
								t1_canPrint = true;
								// lost = false;

								if (output < 0) {
									resError(b1[t1_curr_index]);
									resError(acquireErrMsg);
								}
							}

							break;

						case "towel":

							if (!t1_has_towel && t2_has_towel) { // need to wait for resource

								simDisplay("[thread 1] Waiting for a towel...\n");
								t1_canPrint = false;

							} else {
								int output = acquire (ref t1_has_towel);
								t1_canPrint = true;
								// lost = false;

								if (output < 0) {
									resError(b1[t1_curr_index]);
									resError(acquireErrMsg);
								}
							}

							break;
						}

					} else if (b1[t1_curr_index].Substring(11, 3) == "ret") {

						// Debug.Log("RETURNING " + b1[t1_curr_index].Substring(20, 5));

						// returning resource
						switch(b1[t1_curr_index].Substring(20, 5)) {

						case "brush":
							
							int output1 = return_res (ref t1_has_brush);

							if (output1 < 0) {

								resError(b1[t1_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "clipp":

							int output2 = return_res (ref t1_has_clippers);

							if (output2 < 0) {

								resError(b1[t1_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "cond.":

							int output3 = return_res (ref t1_has_conditioner);

							if (output3 < 0) {

								resError(b1[t1_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "dryer":
							int output4 = return_res (ref t1_has_dryer);

							if (output4 < 0) {

								resError(b1[t1_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "sciss":
							int output5 = return_res (ref t1_has_scissors);

							if (output5 < 0) {

								resError(b1[t1_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "shamp":

							int output6 = return_res (ref t1_has_shampoo);

							if (output6 < 0) {

								resError(b1[t1_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "stati":
							int output7 = return_res (ref t1_has_station);

							if (output7 < 0) {

								resError(b1[t1_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "towel":
							int output8 = return_res (ref t1_has_towel);

							if (output8 < 0) {

								resError(b1[t1_curr_index]);
								resError(returnErrMsg);
							}

							break;
						}

					} else if (b1[t1_curr_index].Substring(11, 3) == "cut") {

						if (!t1_has_brush || !t1_has_scissors) {
							simulationTextArea.text += "<color=red>" + b1 [t1_curr_index] + "</color>";
							resError("\n> ERROR: You can't cut without a brush and some scissors.\n\n");
						}

						t1_did_cut = true;
							
					} else if (b1[t1_curr_index].Substring(11, 3) == "dry") {

						if (!t1_has_station || !t1_has_dryer || !t1_has_towel) {
							simulationTextArea.text += "<color=red>" + b1 [t1_curr_index] + "</color>";
							resError("\n> ERROR: You can't dry without a station, a dryer and a towel.\n\n");
						}

						t1_did_dry = true;

					} else if (b1[t1_curr_index].Substring(11, 4) == "wash") {

						if (!t1_has_station || !t1_has_shampoo || !t1_has_towel || !t1_has_conditioner) {
							simulationTextArea.text += "<color=red>" + b1 [t1_curr_index] + "</color>";
							resError("\n> ERROR: You can't wash without a station, shampoo, conditioner, and a towel.\n\n");
						}

						t1_did_wash = true;

					} else if (b1[t1_curr_index].Substring(11, 5) == "groom") {

						if (!t1_has_brush || !t1_has_clippers) {
							simulationTextArea.text += "<color=red>" + b1 [t1_curr_index] + "</color>";
							resError("\n> ERROR: You can't groom without a brush and some nail clippers.\n\n");
						}

						t1_did_groom = true;

					} else if (b1[t1_curr_index].Substring(11, 7) == "checkin") {

						if (t2_checkedin) {
							simulationTextArea.text += "<color=red>" + b1 [t1_curr_index] + "</color>";
							resError("\n> ERROR: You are already checked in. You have to check out before attempting to check in a different customer.\n\n");
						} else {
							t1_checkedin = true;
							t1_checkedout = false;
						}
					
					} else if (b1[t1_curr_index].Substring(11, 8) == "checkout") {

						if ((!t1_did_cut) || (!t1_did_dry) || (!t1_did_wash) || (!t1_did_groom)) {

							// Debug.Log("worker 1 is missing actions. add them.");

							simulationTextArea.text += "<color=red>" + b1 [t1_curr_index] + "</color>";
							resError("\n> ERROR: Seems like thread 1 hasn't covered all posibilities yet; it should be done before check-out is reached. Please try again.\n\n");

						}

						else if (t1_has_brush || t1_has_clippers || t1_has_conditioner || t1_has_dryer || t1_has_scissors || t1_has_shampoo || t1_has_station || t1_has_towel) {

							// Debug.Log("worker 1: still have some resources.");

							simulationTextArea.text += "<color=red>" + b1 [t1_curr_index] + "</color>";
							resError("\n> ERROR: You need to return all the resources you acquired before checking out.\n\n");
						}

						else if (t1_checkedout) {
							
							// Debug.Log("check in before checking out.");

							simulationTextArea.text += "<color=red>" + b1 [t1_curr_index] + "</color>";
							resError("\n> ERROR: You have to check in before attempting to check out a customer.\n\n");

						} else {
							
							t1_checkedin = false;
							t1_checkedout = true;
						}

						// Debug.Log("1. lost var status: " + lost);
					}

				} catch { }

				try {

					if (t1_canPrint) {

						if (!err)
							simulationTextArea.text += "" + b1 [t1_curr_index];
						t1_curr_index++;
					}

				} catch { }


				// ------------------------------  THREAD 2 ------------------------------

									
				try {

					// {"[null]", "brush" ,"clippers" , "cond.", "dryer", "scissors", "shampoo", "station", "towel"};

					if (b2[t2_curr_index].Substring(11, 3) == "acq") {

						// Debug.Log("ACQUIRING " + b2[t2_curr_index].Substring(21, 5));

						// acquiring resource
						switch(b2[t2_curr_index].Substring(21, 5)) {

						case "brush":

							if (!t2_has_brush && t1_has_brush) { // need to wait for resource

								simDisplay("[thread 2] Waiting for a brush...\n");
								t2_canPrint = false;

							} else {
								
								int output1 = acquire (ref t2_has_brush);
								t2_canPrint = true;
								// lost = false;

								if (output1 < 0) {
									resError(b2[t2_curr_index]);
									resError("\n> ERROR: You are trying to acquire a resource you already have.");
								}
							}

							break;

						case "clipp":

							if (!t2_has_clippers && t1_has_clippers) { // need to wait for resource

								simDisplay("[thread 2] Waiting for nail clippers...\n");
								t2_canPrint = false;

							} else {

								int output2 = acquire (ref t2_has_clippers);
								t2_canPrint = true;
								// lost = false;

								if (output2 < 0) {
									resError(b2[t2_curr_index]);
									resError("\n> ERROR: You are trying to acquire a resource you already have.");
								}
							}

							break;

						case "cond.":

							if (!t2_has_conditioner && t1_has_conditioner) { // need to wait for resource

								simDisplay("[thread 2] Waiting for conditioner...\n");
								t2_canPrint = false;

							} else {
								int output3 = acquire (ref t2_has_conditioner);
								t2_canPrint = true;
								// lost = false;

								if (output3 < 0) {
									resError(b2[t2_curr_index]);
									resError("\n> ERROR: You are trying to acquire a resource you already have.");
								}
							}

							break;

						case "dryer":

							if (!t2_has_dryer && t1_has_dryer) { // need to wait for resource
								
								simDisplay("[thread 2] Waiting for dryer...\n");
								t2_canPrint = false;

							} else {
								
								int output4 = acquire (ref t2_has_dryer);
								t2_canPrint = true;
								// lost = false;

								if (output4 < 0) {
									resError(b2[t2_curr_index]);
									resError("\n> ERROR: You are trying to acquire a resource you already have.");
								}
							}

							break;

						case "sciss":

							if (!t2_has_scissors && t1_has_scissors) { // need to wait for resource

								simDisplay("[thread 2] Waiting for scissors...\n");
								t2_canPrint = false;

							} else {
								
								int output5 = acquire (ref t2_has_scissors);
								t2_canPrint = true;
								// lost = false;

								if (output5 < 0) {
									resError(b2[t2_curr_index]);
									resError("\n> ERROR: You are trying to acquire a resource you already have.");
								}
							}

							break;

						case "shamp":

							if (!t2_has_shampoo && t1_has_shampoo) { // need to wait for resource

								simDisplay("[thread 2] Waiting for shampoo...\n");
								t2_canPrint = false;

							} else {

								int output6 = acquire (ref t2_has_shampoo);
								t2_canPrint = true;
								// lost = false;

								if (output6 < 0) {
									resError(b2[t2_curr_index]);
									resError("\n> ERROR: You are trying to acquire a resource you already have.");
								}
							}

							break;

						case "stati":

							if (!t2_has_station && t1_has_station) { // need to wait for resource

								simDisplay("[thread 2] Waiting for a station...\n");
								t2_canPrint = false;

							} else {

								int output7 = acquire (ref t2_has_station);
								t2_canPrint = true;
								// lost = false;

								if (output7 < 0) {
									resError(b2[t2_curr_index]);
									resError("\n> ERROR: You are trying to acquire a resource you already have.");
								}
							}

							break;

						case "towel":

							if (!t2_has_towel && t1_has_towel) { // need to wait for resource

								simDisplay("[thread 2] Waiting for a towel...\n");
								t2_canPrint = false;

							} else {
								int output8 = acquire (ref t2_has_towel);
								t2_canPrint = true;
								// lost = false;

								if (output8 < 0) {
									resError(b2[t2_curr_index]);
									resError("\n> ERROR: You are trying to acquire a resource you already have.");
								}
							}

							break;
						}

					} else if (b2[t2_curr_index].Substring(11, 3) == "ret") {

						// Debug.Log("RETURNING " + b2[t2_curr_index].Substring(20, 5));

						// returning resource
						switch(b2[t2_curr_index].Substring(20, 5)) {

						case "brush":
							int output1 = return_res (ref t2_has_brush);
							if (output1 < 0) {
								resError(b2[t2_curr_index]);
								resError(returnErrMsg);
							}
							break;

						case "clipp":
							int output2 = return_res (ref t2_has_clippers);
							if (output2 < 0) {
								resError(b2[t2_curr_index]);
								resError(returnErrMsg);
							}
							break;

						case "cond.":
							int output3 = return_res (ref t2_has_conditioner);

							if (output3 < 0) {
								resError(b2[t2_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "dryer":
							int output4 = return_res (ref t2_has_dryer);

							if (output4 < 0) {
								resError(b2[t2_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "sciss":
							int output5 = return_res (ref t2_has_scissors);

							if (output5 < 0) {
								resError(b2[t2_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "shamp":
							int output6 = return_res (ref t2_has_shampoo);

							if (output6< 0) {
								resError(b2[t2_curr_index]);
								resError(returnErrMsg);
							}

							break;

						case "stati":
							int output7 = return_res (ref t2_has_station);
							if (output7 < 0) {
								resError(b2[t2_curr_index]);
								resError(returnErrMsg);
							}
							break;

						case "towel":
							int output8 = return_res (ref t2_has_towel);
							if (output8 < 0) {
								resError(b2[t2_curr_index]);
								resError(returnErrMsg);
							}
							break;
						}

					} else if (b2[t2_curr_index].Substring(11, 3) == "cut") {
						
						if (!t2_has_brush || !t2_has_scissors) {

							simulationTextArea.text += "<color=red>" + b2 [t2_curr_index] + "</color>";
							resError("\n> ERROR: You can't cut without a brush and some scissors.");
						}

						t2_did_cut = true;

					} else if (b2[t2_curr_index].Substring(11, 3) == "dry") {

						if (!t2_has_station || !t2_has_dryer || !t2_has_towel) {
							simulationTextArea.text += "<color=red>" + b2 [t2_curr_index] + "</color>";
							resError("\n> ERROR: You can't dry without a station, a dryer and a towel.\n\n");
						}

						t2_did_dry = true;

					} else if (b2[t2_curr_index].Substring(11, 4) == "wash") {

						if (!t2_has_station || !t2_has_shampoo || !t2_has_towel || !t2_has_conditioner) {
							simulationTextArea.text += "<color=red>" + b2 [t2_curr_index] + "</color>";
							resError("\n> ERROR: You can't wash without a station, shampoo, conditioner, and a towel.\n\n");
						}

						t2_did_wash = true;
					
					} else if (b2[t2_curr_index].Substring(11, 5) == "groom") {

						if (!t2_has_brush || !t2_has_clippers) {
							simulationTextArea.text += "<color=red>" + b2 [t2_curr_index] + "</color>";
							resError("\n> ERROR: You can't groom without a brush and some nail clippers.\n\n");
						}

						t2_did_groom = true;

					} else if (b2[t2_curr_index].Substring(11, 7) == "checkin") {

						if (t2_checkedin) {
							simulationTextArea.text += "<color=red>" + b2 [t2_curr_index] + "</color>";
							resError("\n> ERROR: You are already checked in. You have to check out before attempting to check in a different customer.\n\n");
						} else {
							t2_checkedin = true;
							t2_checkedout = false;
						}
					
					} else if (b2[t2_curr_index].Substring(11, 8) == "checkout") {

						if ((t2_needs_cut && !t2_did_cut) || (t2_needs_dry && !t2_did_dry) || (t2_needs_wash && !t2_did_wash) || (t2_needs_groom && !t2_did_groom)) {

							// Debug.Log("worker 2 is missing actions. add them.");

							simulationTextArea.text += "<color=red>" + b2 [t2_curr_index] + "</color>";
							resError("\n> ERROR: Seems like thread 1 hasn't covered all posibilities yet; it should be done before check-out is reached. Please try again.\n\n");

						} 

						else if (t2_has_brush || t2_has_clippers || t2_has_conditioner || t2_has_dryer || t2_has_scissors || t2_has_shampoo || t2_has_station || t2_has_towel) {

							// Debug.Log("worker 2 hasnt returned all resources.");

							simulationTextArea.text += "<color=red>" + b2 [t2_curr_index] + "</color>";
							resError("\n> ERROR: You need to return all the resources you acquired before checking out.\n\n");

						} 

						else if (t2_checkedout) {

							// Debug.Log("worker 2 needs to check in before checking out.");

							simulationTextArea.text += "<color=red>" + b2 [t2_curr_index] + "</color>";
							resError("\n> ERROR: You have to check in before attempting to check out a customer.\n\n");

						} else {
							
							t2_checkedin = false;
							t2_checkedout = true;
							// lost = false;
						}

						// Debug.Log("2. lost var status: " + lost);

					}
						
				} catch { }

				try {

					if (t2_canPrint) {
						if (!err)
							simulationTextArea.text += "" + b2 [t2_curr_index];
						t2_curr_index++;
					}

				} catch { }

				j++; // increment step

				yield return new WaitForSeconds (1);
			}
		}

		// Debug.Log ("lost?: " + lost);

		if (!lost) {
			manager.gameWon ();
		}

		Debug.Log ("Finished in " + j + " steps.");
	}

	int acquire(ref bool resource) {

		if (resource) {

			err = true;
			lost = true;
			stop = true;
			paused = true;

			// resError("\n> ERROR: You are trying to acquire a resource you already have.");

			return -1;

		} else {
			resource = true;
			return 0;
		}

	}

	int return_res(ref bool resource) {

		if (!resource) {

			err = true;
			lost = true;
			stop = true;
			paused = true;

			// resError("\n> ERROR: You are trying to return a resource you don't have.");

			return -1;

		} else {
			resource = false;

			return 0;
		}

	}

	void resError(String msg) {

		err = true;
		lost = true;
		stop = true;
		paused = true;

		// simulationTextArea.text += "\n<color=red>" + msg + "</color>";
		simDisplay (msg);

		// terminateSimulation ();

		try {
			disablePanel.SetActive (false);
		} catch {
			Debug.Log ("Cannot disable DisablePanel.");
		}

		runButton.transform.SetAsLastSibling ();
		bar.LoadingBar.GetComponent<Image> ().fillAmount = 0;

	}

	void simDisplay(String msg) {

		lost = true;
		simulationTextArea.text += "<color=red>" + msg + "</color>";

	}
}
