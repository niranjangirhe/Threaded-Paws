using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ExecuteThreadsLevel3 : MonoBehaviour {

	// --- IMAGE SIMULATION ---

	public GameObject scrollRect;

	public GameObject simulationImagePrefab;
	public GameObject simulationErrorPrefab;
	public GameObject layoutPanel1;
	public GameObject layoutPanel2;
	public Text stepsIndicator;

	public Sprite dogSprite1;
	public Sprite dogSprite2;
	public Sprite workerSprite1;
	public Sprite workerSprite2;
	public Sprite displayErrorSprite;
	public Sprite[] itemsSprites;
	public Sprite[] actionsSprites;

	// GameObject contentContainer;

	// ------------------------

	ToolboxManager manager;
	GameObject disablePanel;
	ProgressBar bar;
	ScrollRect simulationScrollRect;

	public GameObject runButton;
	public GameObject stopButton;

	// private Timer timer;
	private int numActions;
	private string toPrint;

	Transform[] blocks_t1;
	Transform[] blocks_t2;

	bool stop;
	bool err;
	bool paused;
	bool lost;

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

	string returnErrMsg = "> ERROR: You are trying to return a resource you don't have.";
	string acquireErrMsg = "> ERROR: You are trying to acquire a resource you already have.";

	void Start() {
		
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
		// timer = GameObject.FindObjectOfType<Timer> ();
		disablePanel = GameObject.Find ("DisablePanel");
		bar = GameObject.Find ("RadialProgressBar").GetComponent<ProgressBar>();
		try {
			simulationScrollRect = scrollRect.transform.GetComponent<ScrollRect>();
			// contentContainer = layoutPanel1.transform.parent.gameObject;
		} catch { }

		try { 
			disablePanel.SetActive (false);
		} catch {
			Debug.Log ("Disable Panel can't be found.");
		}
	}

	private Transform[] GetActionBlocks_MultiThreads(String tabNum) {

		//get children in drop area for thread

		string path = "";

		if (tabNum == "1")
			path = "Tab1/ScrollRect/Holder/DropAreaThread1";
		else
			path = "Tab2/ScrollRect/Holder/DropAreaThread2";

		Debug.Log ("children (T" + tabNum + "): " + GameObject.Find (path).transform.childCount);
		int childCount = GameObject.Find (path).transform.childCount;

		Transform[] threadChildren = new Transform[childCount];

		for (int i = 0; i < childCount; i++) {

			threadChildren [i] = GameObject.Find (path).transform.GetChild(i);
		}

		return threadChildren;
	}

	public void ExecuteThreads() {

		scrollToTop ();

		clearAllClones ();
		clearVerticalLayouts ();

		t1_did_cut = false;
		t1_did_dry = false;
		t1_did_wash = false;
		t1_did_groom = false;

		t2_did_cut = false;
		t2_did_dry = false;
		t2_did_wash = false;
		t2_did_groom = false;

		// ----- SET UP FOR LOLA AND ROCKY, CUSTOMERS FOR LEVEL 3 -----

		t1_needs_cut = true;
		t1_needs_dry = false;
		t1_needs_wash = true;
		t1_needs_groom = false;

		t2_needs_cut = true;
		t2_needs_dry = true;
		t2_needs_wash = false;
		t2_needs_groom = false;

		// ------ START EXECUTE THREADS -------

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

		t1_has_brush = false;
		t1_has_clippers = false;
		t1_has_conditioner = false;
		t1_has_dryer = false;
		t1_has_scissors = false;
		t1_has_shampoo = false;
		t1_has_station = false;
		t1_has_towel = false;

		t2_checkedin = false;
		t2_checkedout = false;

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


		// ------------------------ READING THREAD 1 ------------------------

		// int thread1_whilesChildren = 0;

		// retrieving the objects (blocks) current in thread 1
		blocks_t1 = GetActionBlocks_MultiThreads ("1");

		// this structure will store the text lines to display
		List<string> blocks_names_t1 = new List<string> ();
		List<GameObject> simulationImagesToDisplay_T1 = new List<GameObject> ();

		int i = 0;

		foreach (Transform child in blocks_t1) {

			if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.ACTION) {

				//Debug.Log ("TYPE ACTION");

				// action block is a GET action
				if (blocks_t1 [i].transform.GetComponentInChildren<Text> ().text == "get") {

					string resource = blocks_t1 [i].transform.Find ("Dropdown").Find ("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {
						terminateSimulation ();
						manager.showError ("Please select a resource to acquire in thread 1.");
						return;

					} else {

						blocks_names_t1.Add ("[thread 1] acquire ( " + resource + " );");
						i++;

						// create new object from prefab
						GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = workerSprite1;
						newItem.transform.Find ("AcqRet").GetComponent<Image> ().sprite = actionsSprites [0];

						Sprite item;

						if (resource == "brush")
							item = itemsSprites [0];
						else if (resource == "clippers")
							item = itemsSprites [1];
						else if (resource == "cond.")
							item = itemsSprites [2];
						else if (resource == "dryer")
							item = itemsSprites [3];
						else if (resource == "scissors")
							item = itemsSprites [4];
						else if (resource == "shampoo")
							item = itemsSprites [5];
						else if (resource == "station")
							item = itemsSprites [6];
						else if (resource == "towel")
							item = itemsSprites [7];
						else
							item = displayErrorSprite;

						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = item;
						newItem.transform.Find ("ActionText").GetComponent<Text>().text = "get(" + resource + ");";
						simulationImagesToDisplay_T1.Add (newItem);
					}

				// action block is a RETURN action
				} else if(blocks_t1 [i].transform.GetComponentInChildren<Text> ().text == "ret") {

					string resource = blocks_t1 [i].transform.Find ("Dropdown").Find ("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {
						terminateSimulation ();
						manager.showError ("Please select a resource to return in thread 1.");
						return;
					} else {

						blocks_names_t1.Add ("[thread 1] return ( " + resource + " );");
						i++;

						// create new object from prefab
						GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = workerSprite1;
						newItem.transform.Find ("AcqRet").GetComponent<Image> ().sprite = actionsSprites [1];

						Sprite item;

						if (resource == "brush")
							item = itemsSprites [0];
						else if (resource == "clippers")
							item = itemsSprites [1];
						else if (resource == "cond.")
							item = itemsSprites [2];
						else if (resource == "dryer")
							item = itemsSprites [3];
						else if (resource == "scissors")
							item = itemsSprites [4];
						else if (resource == "shampoo")
							item = itemsSprites [5];
						else if (resource == "station")
							item = itemsSprites [6];
						else if (resource == "towel")
							item = itemsSprites [7];
						else
							item = displayErrorSprite;

						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = item;
						newItem.transform.Find ("ActionText").GetComponent<Text>().text = "return(" + resource + ");";
						simulationImagesToDisplay_T1.Add (newItem);

					}

				} else {

					String action = blocks_t1 [i].transform.GetComponentInChildren<Text> ().text;
					blocks_names_t1.Add ("[thread 1] " + action + ";");

					i++;

					GameObject newItem = Instantiate (simulationImagePrefab) as GameObject;


					if (action == "checkin") {

						// Debug.Log ("CHECKING IN");

						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = workerSprite1;
						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = dogSprite1;
						newItem.transform.Find ("AcqRet").GetComponent<Image> ().sprite = actionsSprites [0];

					} else if (action == "checkout") {

						// Debug.Log ("CHECKING OUT");

						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = workerSprite1;
						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = dogSprite1;
						newItem.transform.Find ("AcqRet").GetComponent<Image> ().sprite = actionsSprites [1];

					} else {

						// create new object from prefab (single action)
						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = dogSprite1;

						Sprite item;

						if (action == "cut")
							item = actionsSprites [2];
						else if (action == "dry")
							item = actionsSprites [3];
						else if (action == "wash")
							item = actionsSprites [4];
						else if (action == "groom")
							item = actionsSprites [5];
						else
							item = displayErrorSprite;

						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = item;
					}
					newItem.transform.Find ("ActionText").GetComponent<Text> ().text = action + ";";
					simulationImagesToDisplay_T1.Add (newItem);

				}

			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.IFSTAT) {

				//Debug.Log ("TYPE IFSTAT");

			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.WHILELOOP) {

				//Debug.Log ("TYPE WHILELOOP");
			}
		}

		// ------------------------ READING THREAD 2 ------------------------

		// int thread2_whilesChildren = 0;

		// retrieving the objects (blocks) current in thread 1
		blocks_t2 = GetActionBlocks_MultiThreads ("2");

		// this structure will store the text lines to display
		List<string> blocks_names_t2 = new List<string> ();
		List<GameObject> simulationImagesToDisplay_T2 = new List<GameObject> ();

		i = 0;

		foreach (Transform child in blocks_t2) {

			if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.ACTION) {

				//Debug.Log ("TYPE ACTION");

				// action block is a GET action
				if (blocks_t2 [i].transform.GetComponentInChildren<Text> ().text == "get") {

					string resource = blocks_t2 [i].transform.Find ("Dropdown").Find ("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {

						terminateSimulation ();
						manager.showError ("Please select a resource to acquire in thread 2.");
						return;

					} else {

						blocks_names_t2.Add ("[thread 2] acquire ( " + resource + " );");
						i++;

						// create new object from prefab
						GameObject newItem = Instantiate (simulationImagePrefab) as GameObject;
						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = workerSprite2;
						newItem.transform.Find ("AcqRet").GetComponent<Image> ().sprite = actionsSprites [0];

						Sprite item;

						if (resource == "brush")
							item = itemsSprites [0];
						else if (resource == "clippers")
							item = itemsSprites [1];
						else if (resource == "cond.")
							item = itemsSprites [2];
						else if (resource == "dryer")
							item = itemsSprites [3];
						else if (resource == "scissors")
							item = itemsSprites [4];
						else if (resource == "shampoo")
							item = itemsSprites [5];
						else if (resource == "station")
							item = itemsSprites [6];
						else if (resource == "towel")
							item = itemsSprites [7];
						else
							item = displayErrorSprite;

						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = item;
						newItem.transform.Find ("ActionText").GetComponent<Text> ().text = "get(" + resource + ");";
						simulationImagesToDisplay_T2.Add (newItem);

					}
				
					// action block is a RETURN action
				} else if (blocks_t2 [i].transform.GetComponentInChildren<Text> ().text == "ret") {

					string resource = blocks_t2 [i].transform.Find ("Dropdown").Find ("Label").GetComponent<Text> ().text;

					if (resource == "[null]") {

						terminateSimulation ();
						manager.showError ("Please select a resource to return in thread 2.");
						return;

					} else {

						blocks_names_t2.Add ("[thread 2] return ( " + resource + " );");
						i++;

						// create new object from prefab
						GameObject newItem = Instantiate (simulationImagePrefab) as GameObject;
						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = workerSprite2;
						newItem.transform.Find ("AcqRet").GetComponent<Image> ().sprite = actionsSprites [1];

						Sprite item;

						if (resource == "brush")
							item = itemsSprites [0];
						else if (resource == "clippers")
							item = itemsSprites [1];
						else if (resource == "cond.")
							item = itemsSprites [2];
						else if (resource == "dryer")
							item = itemsSprites [3];
						else if (resource == "scissors")
							item = itemsSprites [4];
						else if (resource == "shampoo")
							item = itemsSprites [5];
						else if (resource == "station")
							item = itemsSprites [6];
						else if (resource == "towel")
							item = itemsSprites [7];
						else
							item = displayErrorSprite;

						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = item;
						newItem.transform.Find ("ActionText").GetComponent<Text> ().text = "return(" + resource + ");";
						simulationImagesToDisplay_T2.Add (newItem);
					}

				} else {

					String action = blocks_t2 [i].transform.GetComponentInChildren<Text> ().text;

					blocks_names_t2.Add ("[thread 2] " + action + ";");
					i++;

					GameObject newItem = Instantiate (simulationImagePrefab) as GameObject;


					if (action == "checkin") {

						//Debug.Log ("CHECKING IN");

						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = workerSprite2;
						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = dogSprite2;
						newItem.transform.Find ("AcqRet").GetComponent<Image> ().sprite = actionsSprites [0];

					} else if (action == "checkout") {

						// Debug.Log ("CHECKING OUT");

						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = workerSprite2;
						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = dogSprite2;
						newItem.transform.Find ("AcqRet").GetComponent<Image> ().sprite = actionsSprites [1];

					} else {

						// create new object from prefab (single action)
						newItem.transform.Find ("Icon").GetComponent<Image> ().sprite = dogSprite2;

						Sprite item;

						if (action == "cut")
							item = actionsSprites [2];
						else if (action == "dry")
							item = actionsSprites [3];
						else if (action == "wash")
							item = actionsSprites [4];
						else if (action == "groom")
							item = actionsSprites [5];
						else
							item = displayErrorSprite;

						newItem.transform.Find ("ItemAction").GetComponent<Image> ().sprite = item;
					}
					newItem.transform.Find ("ActionText").GetComponent<Text> ().text = action + ";";
					simulationImagesToDisplay_T2.Add (newItem);

				}

			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.IFSTAT) {

				//Debug.Log ("TYPE IFSTAT");

			} else if (child.GetComponent<Draggable> ().typeOfItem == Draggable.Type.WHILELOOP) {

				//Debug.Log ("TYPE WHILELOOP");

			}
		}

		if (blocks_t1.Length < 1) {
			
			manager.showError ("There are no actions to run in thread 1.");
			terminateSimulation ();
			return;
		}

		if (blocks_t2.Length < 1) {
			
			manager.showError ("There are no actions to run in thread 2.");
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

		if (!err) {
			StartCoroutine (printThreads (blocks_names_t1, blocks_names_t2, simulationImagesToDisplay_T1, simulationImagesToDisplay_T2, 5));
		}
	}

	IEnumerator printThreads(List<string> b1, List<string> b2, List<GameObject> s1, List<GameObject> s2, int speed) {

		// waitOneSecond ();

		// scrollToTop ();

		bar.currentAmount = 0;

		// int step_counter = 1;
		int t1_curr_index = 0;
		int t2_curr_index = 0;

		bool t1_canPrint = true;
		bool t2_canPrint = true;

		int j = 0;
		
		while ((t1_curr_index < b1.Count) || (t2_curr_index < b2.Count)) {

			if (bar.currentAmount < 100) {

				bar.currentAmount += speed;
				bar.LoadingBar.GetComponent<Image>().fillAmount = bar.currentAmount / 100;

			} else {

				manager.gameLost();
				stop = true;
				paused = true;
				lost = true;

				stopButton.transform.GetComponent<Button> ().interactable = false;

				yield return 0;
			}

			if (stop) {

				if (!paused) {

					try {
						
						disablePanel.SetActive (false);
					
					} catch {
					
						Debug.Log ("Cannot disable DisablePanel");
					}

					runButton.transform.SetAsLastSibling ();

				}
					
				bar.LoadingBar.GetComponent<Image> ().fillAmount = 0;

				break;
				// yield break;
				// yield return 0;

			} else {

				stepsIndicator.text = "" + (j + 1);

				// ------------------------------  THREAD 1 ------------------------------

				try {

					// {"[null]", "brush" ,"clippers" , "cond.", "dryer", "scissors", "shampoo", "station", "towel"};

					if (b1[t1_curr_index].Substring(11, 3) == "acq") {

						// acquiring resource
						switch(b1[t1_curr_index].Substring(21, 5)) {

						case "brush":

							if (!t1_has_brush && t2_has_brush) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[0];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for brush...</color>";
								newItem.transform.SetParent(layoutPanel1.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t1_canPrint = false;

							} else {
								
								int output = acquire (ref t1_has_brush);
								t1_canPrint = true;

								if (output < 0) {
									resError(acquireErrMsg, 1); // ERROR: You are trying to acquire a resource you already have.";
								}
							}

							break;

						case "clipp":

							if (!t1_has_clippers && t2_has_clippers) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[1];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for nail clippers...</color>";
								newItem.transform.SetParent(layoutPanel1.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t1_canPrint = false;

							} else {
								
								int output = acquire (ref t1_has_clippers);
								t1_canPrint = true;

								if (output < 0) {
									resError(acquireErrMsg, 1);
								}
							}

							break;

						case "cond.":

							if (!t1_has_conditioner && t2_has_conditioner) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[2];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for conditioner...</color>";
								newItem.transform.SetParent(layoutPanel1.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t1_canPrint = false;

							} else {

								int output = acquire (ref t1_has_conditioner);
								t1_canPrint = true;

								if (output < 0) {
									resError(acquireErrMsg, 1);
								}
							}

							break;

						case "dryer":

							if (!t1_has_dryer && t2_has_dryer) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[3];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for dryer...</color>";
								newItem.transform.SetParent(layoutPanel1.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t1_canPrint = false;

							} else {
								
								int output = acquire (ref t1_has_dryer);
								t1_canPrint = true;

								if (output < 0) {
									resError(acquireErrMsg, 1);
								}
							}

							break;

						case "sciss":

							if (!t1_has_scissors && t2_has_scissors) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[4];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for scissors...</color>";
								newItem.transform.SetParent(layoutPanel1.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t1_canPrint = false;

							} else {

								int output = acquire (ref t1_has_scissors);
								t1_canPrint = true;

								if (output < 0) {
									resError(acquireErrMsg, 1);
								}
							}

							break;

						case "shamp":

							if (!t1_has_shampoo && t2_has_shampoo) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[5];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for shampoo...</color>";
								newItem.transform.SetParent(layoutPanel1.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t1_canPrint = false;

							} else {
								int output = acquire (ref t1_has_shampoo);
								t1_canPrint = true;

								if (output < 0) {
									resError(acquireErrMsg, 1);
								}
							}

							break;

						case "stati":

							if (!t1_has_station && t2_has_station) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[6];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for station...</color>";
								newItem.transform.SetParent(layoutPanel1.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t1_canPrint = false;

							} else {
								int output = acquire (ref t1_has_station);
								t1_canPrint = true;

								if (output < 0) {
									resError(acquireErrMsg, 1);
								}
							}

							break;

						case "towel":

							if (!t1_has_towel && t2_has_towel) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[7];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for towel...</color>";
								newItem.transform.SetParent(layoutPanel1.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t1_canPrint = false;

							} else {
								
								int output = acquire (ref t1_has_towel);
								t1_canPrint = true;

								if (output < 0) {
									resError(acquireErrMsg, 1);
								}
							}

							break;
						}

					} else if (b1[t1_curr_index].Substring(11, 3) == "ret") {
						
						// returning resource
						switch(b1[t1_curr_index].Substring(20, 5)) {

						case "brush":

							int output1 = return_res (ref t1_has_brush);

							if (output1 < 0) {
								resError(returnErrMsg, 1);
							}

							break;

						case "clipp":

							int output2 = return_res (ref t1_has_clippers);

							if (output2 < 0) {
								resError(returnErrMsg, 1);
							}

							break;

						case "cond.":

							int output3 = return_res (ref t1_has_conditioner);

							if (output3 < 0) {
								resError(returnErrMsg, 1);
							}

							break;

						case "dryer":
							
							int output4 = return_res (ref t1_has_dryer);

							if (output4 < 0) {
								resError(returnErrMsg, 1);
							}

							break;

						case "sciss":
							
							int output5 = return_res (ref t1_has_scissors);

							if (output5 < 0) {
								resError(returnErrMsg, 1);
							}

							break;

						case "shamp":

							int output6 = return_res (ref t1_has_shampoo);

							if (output6 < 0) {
								resError(returnErrMsg, 1);
							}

							break;

						case "stati":
							
							int output7 = return_res (ref t1_has_station);

							if (output7 < 0) {
								resError(returnErrMsg, 1);
							}

							break;

						case "towel":
							
							int output8 = return_res (ref t1_has_towel);

							if (output8 < 0) {
								resError(returnErrMsg, 1);
							}

							break;
						}

					} else if (b1[t1_curr_index].Substring(11, 3) == "cut") {

						if (!t1_has_brush || !t1_has_scissors) {

							String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
							s1[t1_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You can't cut without a brush and some scissors.", 1);
							scrollToBottom();
						
						} else {

							// perform cut
							t1_did_cut = true;
						}
					} else if (b1[t1_curr_index].Substring(11, 3) == "dry") {

						if (!t1_has_station || !t1_has_dryer || !t1_has_towel) {

							String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
							s1[t1_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You can't dry without a station, a dryer and a towel.", 1);
							scrollToBottom();
						
						} else {

							// perform dry
							t1_did_dry = true;
						}

					} else if (b1[t1_curr_index].Substring(11, 4) == "wash") {

						if (!t1_has_station || !t1_has_shampoo || !t1_has_towel || !t1_has_conditioner) {

							String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
							s1[t1_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You can't wash without a station, shampoo, conditioner, and a towel.", 1);
							scrollToBottom();
						
						} else {

							// perform wash
							t1_did_wash = true;
						}

					} else if (b1[t1_curr_index].Substring(11, 5) == "groom") {

						if (!t1_has_brush || !t1_has_clippers) {

							String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
							s1[t1_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You can't groom without a brush and some nail clippers.", 1);
							scrollToBottom();
						
						} else {

							// perform groom
							t1_did_groom = true;
						}

					} else if (b1[t1_curr_index].Substring(11, 7) == "checkin") {

						if (t1_checkedin) {

							String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
							s1[t1_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You are already checked in. You have to check out before attempting to check in a different customer.", 1);
							scrollToBottom();
						
						} else {

							// perform check-in
							t1_checkedin = true;
							t1_checkedout = false;
						}

					} else if (b1[t1_curr_index].Substring(11, 8) == "checkout") {

						if ((t1_needs_cut && !t1_did_cut) || (t1_needs_dry && !t1_did_dry) || (t1_needs_wash && !t1_did_wash) || (t1_needs_groom && !t1_did_groom)) {

							String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
							s1[t1_curr_index].transform.localScale = Vector3.one;
							scrollToBottom();

							resError("> ERROR: Seems like worker 1 didn't fulfill all of the customer's requests. Please try again.", 1);
							scrollToBottom();

						} else if (t1_has_brush || t1_has_clippers || t1_has_conditioner || t1_has_dryer || t1_has_scissors || t1_has_shampoo || t1_has_station || t1_has_towel) {

							String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
							s1[t1_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You need to return all the resources you acquired before checking out.", 1);
							scrollToBottom();

						} else if (t1_checkedout) {

							String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
							s1[t1_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You have to check in before attempting to check out a customer.", 1);
							scrollToBottom();

						} else {

							// perform check-out
							t1_checkedin = false;
							t1_checkedout = true;
						}
					}

				} catch { }

				try {

					if (t1_canPrint) {

						if (!err) {
							s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
							s1[t1_curr_index].transform.localScale = Vector3.one;
						}
						t1_curr_index++;
					}
					scrollToBottom();

				} catch { }

				scrollToBottom();

				// ------------------------------  THREAD 2 ------------------------------


				try {

					// {"[null]", "brush" ,"clippers" , "cond.", "dryer", "scissors", "shampoo", "station", "towel"};

					if (b2[t2_curr_index].Substring(11, 3) == "acq") {

						// acquiring resource
						switch(b2[t2_curr_index].Substring(21, 5)) {

						case "brush":

							if (!t2_has_brush && t1_has_brush) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[0];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for brush...</color>";
								newItem.transform.SetParent(layoutPanel2.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t2_canPrint = false;

							} else {

								int output1 = acquire (ref t2_has_brush);
								t2_canPrint = true;

								if (output1 < 0) {
									resError(acquireErrMsg, 2);
								}
							}

							break;

						case "clipp":

							if (!t2_has_clippers && t1_has_clippers) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[1];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for nail clippers...</color>";
								newItem.transform.SetParent(layoutPanel2.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t2_canPrint = false;

							} else {

								int output2 = acquire (ref t2_has_clippers);
								t2_canPrint = true;

								if (output2 < 0) {
									resError(acquireErrMsg, 2);
								}
							}

							break;

						case "cond.":

							if (!t2_has_conditioner && t1_has_conditioner) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[2];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for conditioner...</color>";
								newItem.transform.SetParent(layoutPanel2.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t2_canPrint = false;

							} else {
								
								int output3 = acquire (ref t2_has_conditioner);
								t2_canPrint = true;

								if (output3 < 0) {
									resError(acquireErrMsg, 2);
								}
							}

							break;

						case "dryer":

							if (!t2_has_dryer && t1_has_dryer) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[3];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for dryer...</color>";
								newItem.transform.SetParent(layoutPanel2.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t2_canPrint = false;

							} else {

								int output4 = acquire (ref t2_has_dryer);
								t2_canPrint = true;

								if (output4 < 0) {
									resError(acquireErrMsg, 2);
								}
							}

							break;

						case "sciss":

							if (!t2_has_scissors && t1_has_scissors) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[4];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for scissors...</color>";
								newItem.transform.SetParent(layoutPanel2.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t2_canPrint = false;

							} else {

								int output5 = acquire (ref t2_has_scissors);
								t2_canPrint = true;

								if (output5 < 0) {
									resError(acquireErrMsg, 2);
								}
							}

							break;

						case "shamp":

							if (!t2_has_shampoo && t1_has_shampoo) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[5];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for shampoo...</color>";
								newItem.transform.SetParent(layoutPanel2.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t2_canPrint = false;

							} else {

								int output6 = acquire (ref t2_has_shampoo);
								t2_canPrint = true;

								if (output6 < 0) {
									resError(acquireErrMsg, 2);
								}
							}

							break;

						case "stati":

							if (!t2_has_station && t1_has_station) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[6];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for station...</color>";
								newItem.transform.SetParent(layoutPanel2.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t2_canPrint = false;

							} else {

								int output7 = acquire (ref t2_has_station);
								t2_canPrint = true;

								if (output7 < 0) {
									resError(acquireErrMsg, 2);
								}
							}

							break;

						case "towel":

							if (!t2_has_towel && t1_has_towel) { // need to wait for resource

								GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
								newItem.transform.Find("Icon").GetComponent<Image>().sprite = actionsSprites[6];
								newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = itemsSprites[7];
								newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for towel...</color>";
								newItem.transform.SetParent(layoutPanel2.transform);
								newItem.transform.localScale = Vector3.one;
								scrollToBottom();

								t2_canPrint = false;

							} else {
								int output8 = acquire (ref t2_has_towel);
								t2_canPrint = true;

								if (output8 < 0) {
									resError(acquireErrMsg, 2);
								}
							}

							break;
						}

					} else if (b2[t2_curr_index].Substring(11, 3) == "ret") {
						
						// returning resource
						switch(b2[t2_curr_index].Substring(20, 5)) {

						case "brush":
							
							int output1 = return_res (ref t2_has_brush);

							if (output1 < 0) {
								resError(returnErrMsg, 2);
							}

							break;

						case "clipp":
							
							int output2 = return_res (ref t2_has_clippers);

							if (output2 < 0) {
								resError(returnErrMsg, 2);
							}

							break;

						case "cond.":
							
							int output3 = return_res (ref t2_has_conditioner);

							if (output3 < 0) {
								resError(returnErrMsg, 2);
							}

							break;

						case "dryer":
							
							int output4 = return_res (ref t2_has_dryer);

							if (output4 < 0) {
								resError(returnErrMsg, 2);
							}

							break;

						case "sciss":
							
							int output5 = return_res (ref t2_has_scissors);

							if (output5 < 0) {
								resError(returnErrMsg, 2);
							}

							break;

						case "shamp":
							
							int output6 = return_res (ref t2_has_shampoo);

							if (output6< 0) {
								resError(returnErrMsg, 2);
							}

							break;

						case "stati":
							
							int output7 = return_res (ref t2_has_station);

							if (output7 < 0) {
								resError(returnErrMsg, 2);
							}

							break;

						case "towel":

							int output8 = return_res (ref t2_has_towel);

							if (output8 < 0) {
								resError(returnErrMsg, 2);
							}

							break;
						}

					} else if (b2[t2_curr_index].Substring(11, 3) == "cut") {

						if (!t2_has_brush || !t2_has_scissors) {

							String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
							s2[t2_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You can't cut without a brush and some scissors.", 2);
							scrollToBottom();
						
						} else {

							// perform cut
							t2_did_cut = true;
						}

					} else if (b2[t2_curr_index].Substring(11, 3) == "dry") {

						if (!t2_has_station || !t2_has_dryer || !t2_has_towel) {

							String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
							s2[t2_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You can't dry without a station, a dryer and a towel.", 2);
							scrollToBottom();
						
						} else {

							// perform dry
							t2_did_dry = true;
						}

					} else if (b2[t2_curr_index].Substring(11, 4) == "wash") {

						if (!t2_has_station || !t2_has_shampoo || !t2_has_towel || !t2_has_conditioner) {

							String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
							s2[t2_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You can't wash without a station, shampoo, conditioner, and a towel.", 2);
							scrollToBottom();
						
						} else {

							// perform wash
							t2_did_wash = true;
						}

					} else if (b2[t2_curr_index].Substring(11, 5) == "groom") {

						if (!t2_has_brush || !t2_has_clippers) {

							String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
							s2[t2_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You can't groom without a brush and some nail clippers.", 2);
							scrollToBottom();

						} else {

							// perform groom
							t2_did_groom = true;
						}

					} else if (b2[t2_curr_index].Substring(11, 7) == "checkin") {

						if (t2_checkedin) {

							String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
							s2[t2_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You are already checked in. You have to check out before attempting to check in a different customer.", 2);
							scrollToBottom();
						
						} else {

							// perform check-in
							t2_checkedin = true;
							t2_checkedout = false;
						}

					} else if (b2[t2_curr_index].Substring(11, 8) == "checkout") {

						if ((t2_needs_cut && !t2_did_cut) || (t2_needs_dry && !t2_did_dry) || (t2_needs_wash && !t2_did_wash) || (t2_needs_groom && !t2_did_groom)) {

							String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
							s2[t2_curr_index].transform.localScale = Vector3.one;
							scrollToBottom();

							resError("> ERROR: Seems like worker 2 didn't fulfill all of the customer's requests. Please try again.", 2);
							scrollToBottom();

						} else if (t2_has_brush || t2_has_clippers || t2_has_conditioner || t2_has_dryer || t2_has_scissors || t2_has_shampoo || t2_has_station || t2_has_towel) {

							String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
							s2[t2_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You need to return all the resources you acquired before checking out.", 2);
							scrollToBottom();

						} else if (t2_checkedout) {

							String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
							s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
							s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
							s2[t2_curr_index].transform.localScale = Vector3.one;

							resError("> ERROR: You have to check in before attempting to check out a customer.", 2);
							scrollToBottom();

						} else {

							// perform check-out
							t2_checkedin = false;
							t2_checkedout = true;
						}
					}

				} catch { }

				try {

					if (t2_canPrint) {
						if (!err) {
							s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
							s2[t2_curr_index].transform.localScale = Vector3.one;
						}

						t2_curr_index++;
					}

				} catch { 
					scrollToBottom ();
				}

				j++; // increment step
				yield return new WaitForSeconds (1);
				scrollToBottom ();
			}
		}
			
		if (!lost) {
			manager.gameWon ();
			Debug.Log ("Finished in " + j + " steps.");
		}
			
		Canvas.ForceUpdateCanvases();
		scrollToBottom ();
	}

	public void terminateSimulation() {

		stepsIndicator.text = "0";

		err = true;
		lost = true;
		stop = true;
		paused = true;

		try {
			disablePanel.SetActive (false);
		} catch {
			Debug.Log ("Cannot disable DisablePanel.");
		}

		runButton.transform.SetAsLastSibling ();
		bar.LoadingBar.GetComponent<Image> ().fillAmount = 0;

		// StartCoroutine (waitOneSecond());
		// scrollToBottom ();
	}

	void resError(String msg, int thread_num) {

		// display error
		Transform newItemParent;

		if (thread_num == 1)
			newItemParent = layoutPanel1.transform;
		else
			newItemParent = layoutPanel2.transform;

		GameObject newItem = Instantiate(simulationErrorPrefab) as GameObject;
		newItem.transform.Find ("ActionText").GetComponent<Text>().text = "<color=red>" + msg + "</color>";
		// newItem.transform.parent = newItemParent;
		newItem.transform.SetParent(newItemParent);
		newItem.transform.localScale = Vector3.one;
		// scrollToBottom ();

		// terminate simulation
		terminateSimulation ();
	}

	int acquire(ref bool resource) {

		if (resource) {

			err = true;
			lost = true;
			stop = true;
			paused = true;

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

			return -1;

		} else {
			resource = false;

			return 0;
		}
	}

	void clearVerticalLayouts() {
		stepsIndicator.text = "0";

		//layoutPanel1
		foreach (Transform child in layoutPanel1.transform) {
			GameObject.Destroy (child.gameObject);
		}

		//layoutPanel2
		foreach (Transform child in layoutPanel2.transform) {
			GameObject.Destroy (child.gameObject);
		}
	}

	void scrollToBottom() {
		
		// Debug.Log ("scrollToBottom()");
		Canvas.ForceUpdateCanvases ();
		waitOneFrame ();
		simulationScrollRect.verticalNormalizedPosition = 0f;
		Canvas.ForceUpdateCanvases ();
	}

	void scrollToTop() {

		// Debug.Log ("scrollToBottom()");
		Canvas.ForceUpdateCanvases ();
		waitOneFrame ();
		simulationScrollRect.verticalNormalizedPosition = 1f;
		Canvas.ForceUpdateCanvases ();
	}

	void clearAllClones() {

		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;

		foreach (GameObject obj in allObjects) {

			if (obj.transform.name == "SimulationImage(Clone)")
				GameObject.Destroy (obj);
		}
	}

	IEnumerator waitOneFrame() {
		yield return 0;
	}
}