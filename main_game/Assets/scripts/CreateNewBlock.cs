using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateNewBlock : MonoBehaviour {

	public GameObject prefab;
	public GameObject canvas;

	ToolboxManager manager;

	public static bool canCreate;

	public void NewActionBlock() {

		// if tab 1 is the active panel
		if (GameObject.Find ("Tab1").transform.GetSiblingIndex () > GameObject.Find ("Tab2").transform.GetSiblingIndex ()) {

			if (this.transform.name == "WashBox") {
				//Debug.Log ("This is a wash box");

				if (manager.washLeft_thread1 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;
						newActionBox.transform.Find("Halo").gameObject.SetActive(true);

						manager.washLeft_thread1 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "CheckInBox") {

				if (manager.checkinLeft_thread1 > 0) {

					if (canCreate) {
						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.checkinLeft_thread1 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You dont have any more of those left!");
				}

			} else if (this.transform.name == "CutBox") {

				if (manager.cutLeft_thread1 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.cutLeft_thread1 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "DryBox") {


				if (manager.dryLeft_thread1 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.dryLeft_thread1 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "ResourceBox") {

				if (manager.resourcesLeft_thread1 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.resourcesLeft_thread1 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}
			
			} else if (this.transform.name == "ReturnBox") {

				// Debug.Log ("Generating a return box");

				if (manager.returnLeft_thread1 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.returnLeft_thread1 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "CheckOutBox") {

				if (manager.checkoutLeft_thread1 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.checkoutLeft_thread1 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "GroomBox") {

				if (manager.groomLeft_thread1 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.groomLeft_thread1 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "PickUpBox") {

				if (manager.pickupLeft_thread1 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.pickupLeft_thread1 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			}

		// tab 2 is the active panel
		} else { 
			if (this.transform.name == "WashBox") {
				//Debug.Log ("This is a wash box");

				if (manager.washLeft_thread2 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.cyan;

						manager.washLeft_thread2 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "CheckInBox") {

				if (manager.checkinLeft_thread2 > 0) {

					if (canCreate) {
						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.cyan;

						manager.checkinLeft_thread2 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You dont have any more of those left!");
				}

			} else if (this.transform.name == "CutBox") {

				if (manager.cutLeft_thread2 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.cyan;

						manager.cutLeft_thread2 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "DryBox") {


				if (manager.dryLeft_thread2 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.cyan;

						manager.dryLeft_thread2 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "ResourceBox") {

				if (manager.resourcesLeft_thread2 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.cyan;

						manager.resourcesLeft_thread2 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "ReturnBox") {

				if (manager.returnLeft_thread2 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.cyan;

						manager.returnLeft_thread2 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "CheckOutBox") {

				if (manager.checkoutLeft_thread2 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.cyan;

						manager.checkoutLeft_thread2 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "GroomBox") {

				if (manager.groomLeft_thread2 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.groomLeft_thread2 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			} else if (this.transform.name == "PickUpBox") {

				if (manager.pickupLeft_thread2 > 0) {

					if (canCreate) {

						GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

						newActionBox.transform.SetParent (this.transform);
						//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
						//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
						//newActionBox.AddComponent<Draggable>();
						newActionBox.transform.localScale = Vector3.one;
						newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = this.GetComponentInChildren<Text> ().text;
						//newActionBox.GetComponent<Image> ().color = Color.magenta;

						manager.pickupLeft_thread2 -= 1;
						manager.updateValues ();

						canCreate = false;
					} else {
						manager.showError ("Use or discard your current object first");
					}

				} else {
					//display error message to user
					manager.showError ("You don\'t have any more of those left!");
				}

			}
		}
	}

	public void NewWhileLoopBlock() {
		//Debug.Log ("While loop block was clicked");
		//Debug.Log (canCreate);

		// if tab 1 is the active panel
		if (GameObject.Find ("Tab1").transform.GetSiblingIndex () > GameObject.Find ("Tab2").transform.GetSiblingIndex ()) {
			if (manager.whileLeft_thread1 > 0) {

				if (canCreate) {

					//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

					//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
					GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

					newActionBox.transform.SetParent (this.transform);
					//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform);
					//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
					//newActionBox.GetComponent<Draggable>().typeOfItem = Draggable.Type.ACTION;
					newActionBox.transform.localScale = Vector3.one;

					//newActionBox.GetComponent<Image> ().color = Color.magenta;

					manager.whileLeft_thread1 -= 1;
					manager.updateValues ();

					canCreate = false;
				} else {
					manager.showError ("Use or discard your current object first");
				}

			} else {
				manager.showError ("You don\'t have any more of those left!");
			}
		// tab 2 is the active panel
		} else {
			if (manager.whileLeft_thread2 > 0) {

				if (canCreate) {

					//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

					//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
					GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

					newActionBox.transform.SetParent (this.transform);
					//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform);
					//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
					//newActionBox.GetComponent<Draggable>().typeOfItem = Draggable.Type.ACTION;
					newActionBox.transform.localScale = Vector3.one;

					//newActionBox.GetComponent<Image> ().color = Color.cyan;

					manager.whileLeft_thread2 -= 1;
					manager.updateValues ();

					canCreate = false;
				} else {
					manager.showError ("Use or discard your current object first");
				}

			} else {
				manager.showError ("You don\'t have any more of those left!");
			}
		}


	}

	public void NewIfStatementBlock() {
		//Debug.Log ("If statement block was clicked");
		//Debug.Log ("Before if is clicked: " + canCreate);

		// if tab 1 is the active panel
		if (GameObject.Find ("Tab1").transform.GetSiblingIndex () > GameObject.Find ("Tab2").transform.GetSiblingIndex ()) {
			if (manager.ifLeft_thread1 > 0) {

				if (canCreate) {


					//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

					//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
					GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
					//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform);
					newActionBox.transform.SetParent (this.transform);
					newActionBox.transform.localScale = Vector3.one;

					//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
					//newActionBox.AddComponent<Draggable>();

					manager.ifLeft_thread1 -= 1;
					manager.updateValues ();

					//newActionBox.GetComponent<Image> ().color = Color.magenta;

					canCreate = false;
				} else {
					manager.showError ("Use or discard your current object first");
				}

			} else {
				manager.showError ("You don\'t have any more of those left!");
			}
		// tab 2 is the active panel
		} else {
			if (manager.ifLeft_thread2 > 0) {

				if (canCreate) {


					//Transform newActionBox = (Instantiate(prefab) as GameObject).transform;

					//prefab = (GameObject)Instantiate (Resources.Load ("ActionBox")); //doesnt work
					GameObject newActionBox = (GameObject)Instantiate (prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
					//newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform);
					newActionBox.transform.SetParent (this.transform);
					newActionBox.transform.localScale = Vector3.one;

					//newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (105, 85); //width, height
					//newActionBox.AddComponent<Draggable>();

					manager.ifLeft_thread2 -= 1;
					manager.updateValues ();

					//newActionBox.GetComponent<Image> ().color = Color.cyan;

					canCreate = false;
				} else {
					manager.showError ("Use or discard your current object first");
				}

			} else {
				manager.showError ("You don\'t have any more of those left!");
			}
		}
	}
		
	// Use this for initialization
	void Start () {

		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
		canCreate = true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
