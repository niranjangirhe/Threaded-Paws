using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreateNewBlock : MonoBehaviour {

	public GameObject prefab;
	public GameObject canvas;

	ToolboxManager manager;

	public static bool canCreate;

	public void NewActionBlock() {

		// if tab 1 is the active panel
	
		if (GameObject.Find ("Tab06").transform.GetSiblingIndex () > GameObject.Find ("Tab1").transform.GetSiblingIndex ()) {

			if (this.transform.name == "WashBox") {
				Debug.Log ("This is a wash box");

				newActionBlockMaker(ref manager.washLeft_thread1);
				
			} else if (this.transform.name == "CheckInBox") {
                Debug.Log("This is a CheckInBox");
                newActionBlockMaker(ref manager.checkinLeft_thread1);
                
            }
            else if (this.transform.name == "CutBox") {
                Debug.Log("This is a CutBox");

                newActionBlockMaker(ref manager.cutLeft_thread1);
				
            }
            else if (this.transform.name == "DryBox") {
                Debug.Log("This is a DryBox");
                newActionBlockMaker(ref manager.dryLeft_thread1);
				
            }
            else if (this.transform.name == "ResourceBox") {
                Debug.Log("This is a ResourceBox");
                newActionBlockMaker(ref manager.resourcesLeft_thread1);
				
            }
            else if (this.transform.name == "ReturnBox") {
                Debug.Log("This is a ReturnBox");
                // Debug.Log ("Generating a return box");
                newActionBlockMaker(ref manager.returnLeft_thread1);
				
            }
            else if (this.transform.name == "CheckOutBox") {
                Debug.Log("This is a CheckOutBox");
                newActionBlockMaker(ref manager.checkoutLeft_thread1);

				
            }
            else if (this.transform.name == "GroomBox") {
                Debug.Log("This is a GroomBox");
                newActionBlockMaker(ref manager.groomLeft_thread1);
				
            }
            else if (this.transform.name == "PickUpBox") {
                Debug.Log("This is a PickUpBox");
                newActionBlockMaker(ref manager.pickupLeft_thread1);

				
			}

		// tab 2 is the active panel
		} else { 
			if (this.transform.name == "WashBox") {
                //Debug.Log ("This is a wash box");
                newActionBlockMaker(ref manager.washLeft_thread2);
                

			} else if (this.transform.name == "CheckInBox") {
                newActionBlockMaker(ref manager.checkinLeft_thread2);
                

			} else if (this.transform.name == "CutBox") {
                newActionBlockMaker(ref manager.cutLeft_thread2);
                

			} else if (this.transform.name == "DryBox") {

                newActionBlockMaker(ref manager.dryLeft_thread2);
                

			} else if (this.transform.name == "ResourceBox") {
                newActionBlockMaker(ref manager.resourcesLeft_thread2);
                

			} else if (this.transform.name == "ReturnBox") {
                newActionBlockMaker(ref manager.returnLeft_thread2);
                

			} else if (this.transform.name == "CheckOutBox") {
                newActionBlockMaker(ref manager.checkoutLeft_thread2);
                

			} else if (this.transform.name == "GroomBox") {

                newActionBlockMaker(ref manager.groomLeft_thread2);
                

			} else if (this.transform.name == "PickUpBox") {
                newActionBlockMaker(ref manager.pickupLeft_thread2);
                

			}
		}
	}

	public void newActionBlockMaker(ref int cardCount)
    {
        if (cardCount > 0)
        {

            if (canCreate)
            {

                GameObject newActionBox = (GameObject)Instantiate(prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)

                newActionBox.transform.SetParent(this.transform);
                //newActionBox.transform.SetParent (canvas.GetComponent<Canvas> ().transform); //invisible otherwise
                //newActionBox.GetComponent<RectTransform> ().sizeDelta = new Vector2 (85, 40); //width, height
                //newActionBox.AddComponent<Draggable>();
                newActionBox.transform.localScale = Vector3.one;
                newActionBox.transform.GetChild(0).GetComponentInChildren<Text>().text = this.GetComponentInChildren<Text>().text;
                //newActionBox.GetComponent<Image> ().color = Color.magenta;

                cardCount -= 1;
                manager.updateValues();

                canCreate = false;
            }
            else
            {
                manager.showError("Use or discard your current object first");
            }

        }
        else
        {
            //display error message to user
            manager.showError("You don\'t have any more of those left!");
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
