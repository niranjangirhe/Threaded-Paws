using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tabs : MonoBehaviour {

	GameObject curr_tab;
	ToolboxManager manager;
	GameObject toolbox;

	public void switchTabs() {

		//Debug.Log (this.transform.parent.name);

		curr_tab = this.transform.parent.gameObject;

		if (CreateNewBlock.canCreate) {
			curr_tab.transform.SetAsLastSibling ();

			if (curr_tab.transform.name == "Tab1") {

				manager.txt_checkinLeft_thread1.color = Color.magenta;
				manager.txt_cutLeft_thread1.color = Color.magenta;
				manager.txt_dryLeft_thread1.color = Color.magenta;
				manager.txt_washLeft_thread1.color = Color.magenta;
				manager.txt_whileLeft_thread1.color = Color.magenta;
				manager.txt_ifLeft_thread1.color = Color.magenta;

				manager.txt_checkinLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_cutLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_dryLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_washLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_whileLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_ifLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);

			} else if (curr_tab.transform.name == "Tab2") {

				manager.txt_checkinLeft_thread2.color = new Vector4 (0, 0.7F, 0.7F, 1);
				manager.txt_cutLeft_thread2.color = new Vector4 (0, 0.7F, 0.7F, 1);
				manager.txt_dryLeft_thread2.color = new Vector4 (0, 0.7F, 0.7F, 1);
				manager.txt_washLeft_thread2.color = new Vector4 (0, 0.7F, 0.7F, 1);
				manager.txt_whileLeft_thread2.color = new Vector4 (0, 0.7F, 0.7F, 1);
				manager.txt_ifLeft_thread2.color = new Vector4 (0, 0.7F, 0.7F, 1);

				manager.txt_checkinLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_cutLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_dryLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_washLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_whileLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_ifLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);

			}

		} else {
			manager.showError ("Use or discard your current object first");
		}
	}

	/*
	public void resetTabBlocks() {

		int tab_num = 0;
		bool is_tab1 = false;
		if (this.transform.parent.name == "Tab1") {
			tab_num = 1;
			is_tab1 = true;
		} else {
			tab_num = 2;
		}

		int child_count = this.transform.parent.FindChild ("ScrollRect").FindChild ("DropAreaThread" + tab_num).childCount;

		GameObject[] threadChildren = new GameObject[child_count];

		//Debug.Log ("thread childCount: " + childCount);

		for (int i = 0; i < child_count; i++) {
			//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;

			threadChildren [i] = this.transform.parent.FindChild ("ScrollRect").FindChild ("DropAreaThread" + tab_num).GetChild(i).gameObject;
			//threadChildren [i] = this.transform.Find ("DropAreaThread").GetChild (i).GetComponentInChildren<Text>().text;
			//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);

			//Debug.Log ("Child " + i + ": " + threadChildren [i].transform.name);
		}

		for (int i = 0; i < child_count; i++) {

			if (threadChildren [i].GetComponent<Draggable> ().typeOfItem == Draggable.Type.ACTION) {
				
				if (threadChildren [i].GetComponentInChildren<Text> ().text == "wash") {
					if (is_tab1) {
						manager.washLeft_thread1++;
					} else {
						manager.washLeft_thread2++;
					}
				} else if(threadChildren [i].GetComponentInChildren<Text> ().text == "cut") {
					if (is_tab1) {
						manager.cutLeft_thread1++;
					} else {
						manager.cutLeft_thread2++;
					}
				} else if(threadChildren [i].GetComponentInChildren<Text> ().text == "dry") {
					if (is_tab1) {
						manager.dryLeft_thread1++;
					} else {
						manager.dryLeft_thread2++;
					}
				} else if(threadChildren [i].GetComponentInChildren<Text> ().text == "checkin") {
					if (is_tab1) {
						manager.checkinLeft_thread1++;
					} else {
						manager.checkinLeft_thread2++;
					}
				}
					
			} else if(threadChildren [i].GetComponent<Draggable> ().typeOfItem == Draggable.Type.WHILELOOP) {

				// TODO: if while loop, check for all children
				if (threadChildren[i].transform.childCount > 0) {
					Debug.Log ("While loop is NOT empty");

					GameObject[] children = new GameObject[threadChildren[i].transform.childCount];

					//Debug.Log ("thread childCount: " + childCount);

					for (int j = 0; j < threadChildren[i].transform.childCount; j++) {
						//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;

						threadChildren [i] = this.transform.parent.GetChild(i).gameObject;
						//threadChildren [i] = this.transform.Find ("DropAreaThread").GetChild (i).GetComponentInChildren<Text>().text;
						//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);

						Debug.Log ("Child " + i + ": " + threadChildren [i].transform.name);
					}

					/*
					foreach(GameObject child in children) {
						if (child.GetComponentInChildren<Text> ().text == "wash") {
							if (is_tab1) {
								manager.washLeft_thread1++;
							} else {
								manager.washLeft_thread2++;
							}
						} else if(child.GetComponentInChildren<Text> ().text == "cut") {
							if (is_tab1) {
								manager.cutLeft_thread1++;
							} else {
								manager.cutLeft_thread2++;
							}
						} else if(child.GetComponentInChildren<Text> ().text == "dry") {
							if (is_tab1) {
								manager.dryLeft_thread1++;
							} else {
								manager.dryLeft_thread2++;
							}
						} else if(child.GetComponentInChildren<Text> ().text == "checkin") {
							if (is_tab1) {
								manager.checkinLeft_thread1++;
							} else {
								manager.checkinLeft_thread2++;
							}
						}
					}


				}

				if (is_tab1)
					manager.whileLeft_thread1++;
				else
					manager.whileLeft_thread2++;

			} else if(threadChildren [i].GetComponent<Draggable> ().typeOfItem == Draggable.Type.IFSTAT) {
				// TODO: if if statement, check for one child

			}

			Destroy (threadChildren [i]);
		}

		manager.updateValues ();
	
	}
	*/

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
		toolbox = GameObject.Find ("DropAreaTools");

		manager.txt_checkinLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_cutLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_dryLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_washLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_whileLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_ifLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}