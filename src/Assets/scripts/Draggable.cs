using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

	public Transform parentToReturnTo = null;
	GameObject placeholder = null;
	// keep track of this in order to bounce back in case of illegal area
	public Transform placeholderParent = null;
	// GameObject threadArea;
	GameObject canvas;
	GameObject toolbox;

	GameObject threadArea1;
	GameObject threadArea2;

	Text actionsCount;
	Text loopsCount;
	Text statsCount;

	ToolboxManager manager;

	public enum Type {IFNEEDED, WHILELOOP, IFSTAT , ACTION, ALL, INVENTORY};
	public Type typeOfItem = Type.ALL; //default

	public void OnBeginDrag(PointerEventData eventData) {
		//Debug.Log ("OnBeginDrag called: " + eventData.pointerDrag.name);

		this.transform.Find("Halo").gameObject.SetActive(false);

		try {

			// if the parent is a while loop, then make drop area smaller
			if (eventData.pointerDrag.transform.parent.parent.gameObject.GetComponent<Draggable>().typeOfItem == Type.WHILELOOP) {

				int num_children = eventData.pointerDrag.transform.parent.childCount;
				float curr_width = eventData.pointerDrag.transform.parent.GetComponent<RectTransform> ().sizeDelta.x;
				// float curr_height = eventData.pointerDrag.transform.parent.GetComponent<RectTransform> ().sizeDelta.y;
				float new_height = (num_children * 25);

				//Debug.Log("num_children: " + num_children);

				float parent_curr_width = eventData.pointerDrag.transform.parent.parent.GetComponent<RectTransform> ().sizeDelta.x;
				// float parent_curr_height = eventData.pointerDrag.transform.parent.parent.GetComponent<RectTransform> ().sizeDelta.y;
				float parent_new_height = new_height + 25;

				eventData.pointerDrag.transform.parent.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (parent_curr_width, parent_new_height);
				eventData.pointerDrag.transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (curr_width, new_height);

				//Debug.Log("parent_curr_width: " + parent_curr_width);
				//Debug.Log("parent_curr_height: " + parent_curr_height);
				//Debug.Log("parent_new_height: " + parent_new_height);
			}

		} catch {}

		//create new placeholder object and parent it to the draggable object's parent
		placeholder = new GameObject ();
		placeholder.transform.SetParent (this.transform.parent); //places it at the end of the list by default
		//want the placeholder to have the same dimensions as the draggable object removed
		LayoutElement le = placeholder.AddComponent<LayoutElement> ();
		placeholder.GetComponent<RectTransform> ().sizeDelta = new Vector2 (75, 35); //width, height
		//le.preferredWidth = this.GetComponent<LayoutElement> ().preferredWidth;
		//le.preferredHeight = this.GetComponent<LayoutElement> ().preferredHeight;
		//le.preferredHeight = 5;
		//le.minHeight = 5;
		le.flexibleWidth = 0; //not flexible
		le.flexibleHeight = 0;

		//Debug.Log (">>> HEIGHT: " + le.preferredHeight);

		//want the placeholder to also be in the same spot as the object we just removed
		placeholder.transform.SetSiblingIndex (this.transform.GetSiblingIndex ());

		//save old parent
		//parentToReturnTo = this.transform.parent;
		parentToReturnTo = toolbox.transform;

		//make sure it defaults to old parent
		placeholderParent = parentToReturnTo;

		//instead of the toolbox, wanna set parent to canvas
		this.transform.SetParent(canvas.transform);

		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	

		//highlight threadArea
		//threadArea.transform.GetComponent<Image> ().color = Color.green;
		threadArea1.transform.GetComponent<Image> ().color = Color.green;
		threadArea2.transform.GetComponent<Image> ().color = Color.green;

		//highlight corresponding children
		//Transform[] threadChildren = new Transform[threadArea.transform.childCount];
		Transform[] thread1Children = new Transform[threadArea1.transform.childCount];
		Transform[] thread2Children = new Transform[threadArea2.transform.childCount];

		//Debug.Log ("HIGHLIGHTING AREA");
		// Debug.Log ("childCount: " + thread1Children.Length);
		// Debug.Log("Dragging: " + this.transform.name + " , Block Type: " + this.typeOfItem);

		for (int i = 0; i < thread1Children.Length; i++) {
			//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;
			thread1Children [i] = threadArea1.transform.GetChild (i);
			//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);

			try {
							
				//Debug.Log (thread1Children [i].name + ", DropZone Type: " + thread1Children [i].GetComponentInChildren<DropZone>().typeOfArea);
				if ((thread1Children [i].gameObject.GetComponentInChildren<DropZone> ().typeOfArea == this.typeOfItem) ||
					(thread1Children [i].gameObject.GetComponentInChildren<DropZone> ().typeOfArea == Type.ALL)) {

					string zoneName = thread1Children [i].gameObject.GetComponentInChildren<DropZone> ().name;
					//Debug.Log("Theres a dropzone!: " + zoneName);
					thread1Children [i].Find (zoneName).GetComponent<Image> ().color = Color.green;
				}

			} catch {}

		}

		for (int i = 0; i < thread2Children.Length; i++) {
			//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;
			thread2Children [i] = threadArea2.transform.GetChild (i);
			//Debug.Log (threadChildren [i].name);
			//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);
			try {
				if ((thread2Children [i].gameObject.GetComponentInChildren<DropZone> ().typeOfArea == this.typeOfItem) ||
					(thread2Children [i].gameObject.GetComponentInChildren<DropZone> ().typeOfArea == Type.ALL)) {

					string zoneName = thread2Children [i].gameObject.GetComponentInChildren<DropZone> ().name;
					//Debug.Log("Theres a dropzone!: " + zoneName);
					thread2Children [i].Find (zoneName).GetComponent<Image> ().color = Color.green;
				}
			} catch {}
		}
	}

	public void OnDrag(PointerEventData eventData) {
		
		//Debug.Log ("OnDrag called");
		//physically move the card
		this.transform.position = eventData.position;

		// do not shift items in the toolbox
		if ((parentToReturnTo.transform.name != toolbox.transform.name) && (placeholder.transform.parent.name != toolbox.transform.name) && (placeholderParent.transform.name != toolbox.transform.name)) {

			//Debug.Log ("SHIFTING!! \n\ntoolbox.transform.name: " + toolbox.transform.name + "\nparentToReturnTo: " + parentToReturnTo + "\nplaceholder.transform.parent.name:  " + placeholder.transform.parent.name);

			if (placeholder.transform.parent != placeholderParent)
				placeholder.transform.SetParent (placeholderParent);

			int newSiblingIndex = placeholderParent.childCount;//initialized to what it currently is

			//adjust where the placeholder is, depending on what the object is hover over (using y coordinates)
			//iterate through all the children in the original parent, and check if the box is up or down to the box under it

			try {

				for (int i = 0; i < placeholderParent.childCount; i++) {

					newSiblingIndex = i;
			
					if (this.transform.position.y > parentToReturnTo.GetChild (i).position.y) {

						//if the placeholder is actually already below the sibling index
						if (placeholder.transform.GetSiblingIndex () > newSiblingIndex)
							newSiblingIndex--; //ignore the placeholder in the list

						placeholder.transform.SetSiblingIndex (i); //make the placeholder be in this position instead
						break; //since we'll obviously be in the same position in respect to the rest of the boxes afterwards
					}

				}

			} catch {
				//Debug.Log ("An exception occured: " + e.GetBaseException());
				//Debug.Log (e.Message);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData) {

		// Debug.Log ("OnEndDrag called");

		//set parent back to where we came from (at the end of the list)
		this.transform.SetParent(parentToReturnTo);

		//bounce back the object to where the placeholder is
		this.transform.SetSiblingIndex (placeholder.transform.GetSiblingIndex ());

		GetComponent<CanvasGroup> ().blocksRaycasts = true;


		//iterate through corresponding zones and remove highlights, if any
//		threadArea1.transform.GetComponent<Image>().color = Color.magenta;
//		threadArea2.transform.GetComponent<Image>().color = Color.cyan;


		Transform[] thread1Children = new Transform[threadArea1.transform.childCount];
		Transform[] thread2Children = new Transform[threadArea2.transform.childCount];

		for (int i = 0; i < thread1Children.Length; i++) {

			try {
				thread1Children [i] = threadArea1.transform.GetChild (i);
				if ((thread1Children [i].gameObject.GetComponentInChildren<DropZone> ().typeOfArea == this.typeOfItem) ||
					(thread1Children [i].gameObject.GetComponentInChildren<DropZone> ().typeOfArea == Type.ALL)) {
					
					string zoneName = thread1Children [i].gameObject.GetComponentInChildren<DropZone> ().name;
					//Debug.Log ("De-colouring: " + thread1Children [i].gameObject.GetComponentInChildren<DropZone> ().name);

					thread1Children [i].Find (zoneName).GetComponent<Image> ().color = Color.white;
				}

			} catch {}
		}

		if (manager.isLevel4) {

			threadArea1.transform.GetComponent<Image> ().color = Color.white;
			threadArea2.transform.GetComponent<Image>().color = Color.white;
			
		} else {

			threadArea1.transform.GetComponent<Image> ().color = new Vector4 (0.9F, 0.9F, 0.9F, 1);
			threadArea2.transform.GetComponent<Image>().color = new Vector4 (0.9F, 0.9F, 0.9F, 1);
		}

		for (int i = 0; i < thread2Children.Length; i++) {

			try {
				thread2Children [i] = threadArea2.transform.GetChild (i);
				if ((thread2Children [i].gameObject.GetComponentInChildren<DropZone> ().typeOfArea == this.typeOfItem) ||
					(thread2Children [i].gameObject.GetComponentInChildren<DropZone> ().typeOfArea == Type.ALL)) {

					string zoneName = thread2Children [i].gameObject.GetComponentInChildren<DropZone> ().name;
					//Debug.Log ("De-colouring: " + thread2Children [i].gameObject.GetComponentInChildren<DropZone> ().name);
					thread2Children [i].Find (zoneName).GetComponent<Image> ().color = Color.white;

				}

			} catch {}
		}

		if (this.transform.parent.name == "DropAreaTools") {
			//Debug.Log ("Dropped in the toolbox");

			// if tab 1 is the active panel
			if (GameObject.Find ("Tab1").transform.GetSiblingIndex () > GameObject.Find ("Tab2").transform.GetSiblingIndex ()) {
				

				if (this.typeOfItem == Type.ACTION) {

					if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "checkin") {
						manager.checkinLeft_thread1 += 1;

					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "wash") {
						manager.washLeft_thread1 += 1;

					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "cut") {
						manager.cutLeft_thread1 += 1;

					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "dry") {
						manager.dryLeft_thread1 += 1;
					
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "get") {
						manager.resourcesLeft_thread1 += 1;
					
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "ret") {
						manager.returnLeft_thread1 += 1;
					
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "checkout") {
						manager.checkoutLeft_thread1 += 1;
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "groom") {
						manager.groomLeft_thread1 += 1;
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "pickup") {
						manager.pickupLeft_thread1 += 1;
					}

					// Debug.Log ("An action was dropped in the toolbox");

				} else if (this.typeOfItem == Type.WHILELOOP) {

					manager.whileLeft_thread1 += 1;

					//Debug.Log (this.transform.Find("DropArea").childCount);

					if (this.transform.Find("DropArea").childCount > 0) {

						//Debug.Log ("While loop is NOT empty");

						GameObject[] children = new GameObject[this.transform.Find("DropArea").childCount];

						for (int j = 0; j < this.transform.Find("DropArea").childCount; j++) {

							children [j] = this.transform.Find("DropArea").GetChild (j).gameObject;

							//Debug.Log ("Child " + j + ": " + children [j].transform.name);

							if (children [j].transform.GetComponentInChildren<Text> ().text == "wash") {
								manager.washLeft_thread1++;
							} else if (children [j].transform.GetComponentInChildren<Text> ().text == "checkin") {
								manager.checkinLeft_thread1++;
							} else if (children [j].transform.GetComponentInChildren<Text> ().text == "cut") {
								manager.cutLeft_thread1++;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "dry") {
								manager.dryLeft_thread1++;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "get") {
								manager.resourcesLeft_thread1 += 1;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "ret") {
								manager.returnLeft_thread1 += 1;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "checkout") {
								manager.checkoutLeft_thread1 += 1;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "groom") {
								manager.groomLeft_thread1 += 1;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "pickup") {
								manager.pickupLeft_thread1 += 1;
							}
						}

					}

					Debug.Log ("A loop was dropped in the toolbox");

				} else if (this.typeOfItem == Type.IFSTAT) {

					manager.ifLeft_thread1 += 1;

					if (this.transform.Find ("DropArea").childCount > 0) {
				
						if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "wash") {
							manager.washLeft_thread1++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "checkin") {
							manager.checkinLeft_thread1++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "cut") {
							manager.cutLeft_thread1++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "dry") {
							manager.dryLeft_thread1++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "get") {
							manager.resourcesLeft_thread1 += 1;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "ret") {
							manager.returnLeft_thread1 += 1;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "checkout") {
							manager.checkoutLeft_thread1 += 1;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "groom") {
							manager.groomLeft_thread1 += 1;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "pickup") {
							manager.pickupLeft_thread1 += 1;
						}
					}

					Debug.Log ("An if statement was dropped in the toolbox");
				
				} else if (this.typeOfItem == Type.IFNEEDED) {
					Debug.Log ("An \"if needed\" tool was dropped in the toolbox");
				}
			} else {
				if (this.typeOfItem == Type.ACTION) {

					if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "checkin") {
						manager.checkinLeft_thread2 += 1;

					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "wash") {
						manager.washLeft_thread2 += 1;

					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "cut") {
						manager.cutLeft_thread2 += 1;

					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "dry") {
						manager.dryLeft_thread2 += 1;
					
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "get") {
						manager.resourcesLeft_thread2 += 1;
					
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "ret") {
						manager.returnLeft_thread2 += 1;
					
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "checkout") {
						manager.checkoutLeft_thread2 += 1;
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "groom") {
						manager.groomLeft_thread2 += 1;
					} else if (this.transform.GetChild (0).GetComponentInChildren<Text> ().text == "pickup") {
						manager.pickupLeft_thread2 += 1;
					}

					// Debug.Log ("An action was dropped in the toolbox");

				} else if (this.typeOfItem == Type.WHILELOOP) {
					
					manager.whileLeft_thread2 += 1;

					//Debug.Log (this.transform.Find("DropArea").childCount);

					if (this.transform.Find("DropArea").childCount > 0) {

						//Debug.Log ("While loop is NOT empty");

						GameObject[] children = new GameObject[this.transform.Find("DropArea").childCount];

						for (int j = 0; j < this.transform.Find("DropArea").childCount; j++) {

							children [j] = this.transform.Find("DropArea").GetChild (j).gameObject;

							//Debug.Log ("Child " + j + ": " + children [j].transform.name);

							if (children [j].transform.GetComponentInChildren<Text> ().text == "wash") {
								manager.washLeft_thread2++;
							} else if (children [j].transform.GetComponentInChildren<Text> ().text == "checkin") {
								manager.checkinLeft_thread2++;
							} else if (children [j].transform.GetComponentInChildren<Text> ().text == "cut") {
								manager.cutLeft_thread2++;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "dry") {
								manager.dryLeft_thread2++;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "get") {
								manager.resourcesLeft_thread2++;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "ret") {
								manager.returnLeft_thread2++;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "checkout") {
								manager.checkoutLeft_thread2++;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "groom") {
								manager.groomLeft_thread2++;
							} else if (children [j].transform.GetComponentInChildren<Text>().text == "pickup") {
								manager.pickupLeft_thread2++;
							}
						}

					}
						
					Debug.Log ("A loop was dropped in the toolbox");

				} else if (this.typeOfItem == Type.IFSTAT) {

					manager.ifLeft_thread2 += 1;


					if (this.transform.Find ("DropArea").childCount > 0) {
						if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "wash") {
							manager.washLeft_thread2++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "checkin") {
							manager.checkinLeft_thread2++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "cut") {
							manager.cutLeft_thread2++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "dry") {
							manager.dryLeft_thread2++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "get") {
							manager.resourcesLeft_thread2++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "ret") {
							manager.returnLeft_thread2++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "checkout") {
							manager.checkoutLeft_thread2++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "groom") {
							manager.groomLeft_thread2++;
						} else if (this.transform.Find ("DropArea").GetChild (0).transform.GetComponentInChildren<Text> ().text == "pickup") {
							manager.pickupLeft_thread2++;
						}
					}

					Debug.Log ("An if statement was dropped in the toolbox");
				
				} else if (this.typeOfItem == Type.IFNEEDED) {
					Debug.Log ("An \"if needed\" tool was dropped in the toolbox");
				}
			}

			manager.updateValues ();
			//self-destroy
			Destroy(this.gameObject);

		}

		else if (this.transform.parent.gameObject == canvas) {

			this.transform.SetParent (toolbox.transform);

		} else {


			//Debug.Log ("Dropped within another box... probably");
			//Debug.Log ("Parent: " + this.transform.parent.name);

			//new parent is inside an if statement or a look
			if (this.transform.parent.name == "DropArea") {
				//this.transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (this.GetComponent<RectTransform> ().sizeDelta.x, this.GetComponent<RectTransform> ().sizeDelta.y + 45);

				//this.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2(75, 25);
			}
		
		}

		if (this.typeOfItem == Type.IFSTAT) {
			this.GetComponent<Image> ().color = new Vector4 (0.3F, 0.8F, 0.83F, 1);

		} else if (this.typeOfItem == Type.ACTION && (this.GetComponentInChildren<Text> ().text == "get")) {
			this.GetComponent<Image> ().color = new Vector4 (0.94F, 0.28F, 0.94F, 1);

		} else if (this.typeOfItem == Type.ACTION && (this.GetComponentInChildren<Text> ().text == "ret")) {
			this.GetComponent<Image> ().color = new Vector4 (0.56F, 0.82F, 0.44F, 1);

		} else if (this.typeOfItem == Type.ACTION) {
			this.GetComponent<Image> ().color = new Vector4 (1, 0.76F, 0.24F, 1);

		} else if (this.typeOfItem == Type.WHILELOOP) {
			this.GetComponent<Image> ().color = new Vector4 (0.77F, 0.71F, 0.6F, 1);
		}

		// this.GetComponent<Image> ().color = Color.white;

		Destroy (placeholder);

		CreateNewBlock.canCreate = true;
	}

	void Start() {
		// threadArea = GameObject.Find("DropAreaThread");

		threadArea1 = GameObject.Find("DropAreaThread1");
		threadArea2 = GameObject.Find("DropAreaThread2");

		canvas = GameObject.Find("Canvas");
		toolbox = GameObject.Find ("DropAreaTools");

		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
	}
}