using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public Draggable.Type typeOfArea;
	public bool isInventory;

	ToolboxManager manager;
	GameObject toolbox;

	//usually only triggered for the mouse pointer only
	public void OnPointerEnter(PointerEventData eventData) {
		//Debug.Log ("OnPointerEnter to " + this.gameObject.name);


		if (eventData.pointerDrag == null)
			return; //nothing is being dragged

		try {
			if ((this.transform.parent.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Type.WHILELOOP) && (eventData.pointerDrag.GetComponent<Draggable>().typeOfItem == Draggable.Type.ACTION)) {

				//Debug.Log (eventData.pointerDrag + " entered a while loop");

				int num_children = this.transform.childCount;
				float curr_width = this.transform.GetComponent<RectTransform> ().sizeDelta.x;
				float curr_height = this.transform.GetComponent<RectTransform> ().sizeDelta.y;
				float new_height = curr_height + 25;

				//Debug.Log("num_children: " + num_children);

				float parent_curr_width = this.transform.parent.GetComponent<RectTransform> ().sizeDelta.x;
				// float parent_curr_height = this.transform.parent.GetComponent<RectTransform> ().sizeDelta.y;
				float parent_new_height = new_height + 25;

				this.transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (parent_curr_width, parent_new_height);
				this.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (curr_width, new_height);

				//Debug.Log("parent_curr_width: " + parent_curr_width);
				//Debug.Log("parent_curr_height: " + parent_curr_height);
				//Debug.Log("parent_new_height: " + parent_new_height);
			} else if ((this.transform.parent.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Type.WHILELOOP) && (eventData.pointerDrag.GetComponent<Draggable>().typeOfItem == Draggable.Type.IFSTAT)) {
			
				int num_children = this.transform.childCount;
				float curr_width = this.transform.GetComponent<RectTransform> ().sizeDelta.x;
				float curr_height = this.transform.GetComponent<RectTransform> ().sizeDelta.y;
				float new_height = curr_height + 50;

				float parent_curr_width = this.transform.parent.GetComponent<RectTransform> ().sizeDelta.x;
				// float parent_curr_height = this.transform.parent.GetComponent<RectTransform> ().sizeDelta.y;
				float parent_new_height = new_height + 25;

				this.transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (parent_curr_width, parent_new_height);
				this.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (curr_width, new_height);
			
			}

		}catch{}
			
		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		if (d != null) {

			if (typeOfArea == d.typeOfItem || typeOfArea == Draggable.Type.ALL) {
				d.placeholderParent = this.transform;
				d.parentToReturnTo = this.transform;

				//Debug.Log ("[OnPointerEnter] Parent to return to: " + d.parentToReturnTo.transform.name);
			}
		}

	}

	//usually only triggered for the mouse pointer only
	public void OnPointerExit(PointerEventData eventData) {
		//Debug.Log ("OnPointerExit from " + gameObject.name);

		if (eventData.pointerDrag == null)
			return; //nothing is being dragged

		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		//if placeholderParent is equal to us - otherwise, someones already taken the placeholder parent
		if (d != null && d.placeholderParent == this.transform) {
			if (typeOfArea == d.typeOfItem || typeOfArea == Draggable.Type.ALL || typeOfArea == Draggable.Type.INVENTORY) {
				d.placeholderParent = d.parentToReturnTo;
				//Debug.Log ("[OnPointerExit] Placeholder parent: " + d.parentToReturnTo.transform.name);
			}
		}

		try {
			if ((this.transform.parent.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Type.WHILELOOP) && (eventData.pointerDrag.GetComponent<Draggable>().typeOfItem == Draggable.Type.ACTION)) {

				//Debug.Log (eventData.pointerDrag + " exited a while loop");

				int num_children = this.transform.childCount;
				float curr_width = this.transform.GetComponent<RectTransform> ().sizeDelta.x;
				float curr_height = this.transform.GetComponent<RectTransform> ().sizeDelta.y;
				float new_height = curr_height - 25;

				//Debug.Log("num_children: " + num_children);

				float parent_curr_width = this.transform.parent.GetComponent<RectTransform> ().sizeDelta.x;
				// float parent_curr_height = this.transform.parent.GetComponent<RectTransform> ().sizeDelta.y;
				float parent_new_height = new_height + 25;

				this.transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (parent_curr_width, parent_new_height);
				this.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (curr_width, new_height);

				//Debug.Log("parent_curr_width: " + parent_curr_width);
				//Debug.Log("parent_curr_height: " + parent_curr_height);
				//Debug.Log("parent_new_height: " + parent_new_height);
			} else if ((this.transform.parent.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Type.WHILELOOP) && (eventData.pointerDrag.GetComponent<Draggable>().typeOfItem == Draggable.Type.IFSTAT)) {

				//Debug.Log (eventData.pointerDrag + " exited a while loop");

				int num_children = this.transform.childCount;
				float curr_width = this.transform.GetComponent<RectTransform> ().sizeDelta.x;
				float curr_height = this.transform.GetComponent<RectTransform> ().sizeDelta.y;
				float new_height = curr_height - 50;
			
				float parent_curr_width = this.transform.parent.GetComponent<RectTransform> ().sizeDelta.x;
				// float parent_curr_height = this.transform.parent.GetComponent<RectTransform> ().sizeDelta.y;
				float parent_new_height = new_height + 25;

				this.transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (parent_curr_width, parent_new_height);
				this.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (curr_width, new_height);
			}

		}catch{}
	}

	public void OnDrop(PointerEventData eventData) {
		//Debug.Log (eventData.pointerDrag.name + " was dropped on " + gameObject.name);

		//to know the name of what was dropped on it: eventData.pointerDrag.name
		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if (d != null) {
			if (typeOfArea == d.typeOfItem || typeOfArea == Draggable.Type.ALL || typeOfArea == Draggable.Type.INVENTORY) {
				d.parentToReturnTo = this.transform;
				//Debug.Log ("[OnDrop] Parent to return to: " + d.parentToReturnTo.transform.name);
			
			}

			/*
			if (d.parentToReturnTo.transform.name == "DropArea" && (d.parentToReturnTo.parent.GetComponent<Draggable>().typeOfItem == Draggable.Type.WHILELOOP)) {
				//resize

				float action_height = 25;

				float new_height = d.parentToReturnTo.transform.GetComponent<RectTransform> ().sizeDelta.y + action_height;
				float curr_width = d.parentToReturnTo.transform.GetComponent<RectTransform> ().sizeDelta.x;
				d.parentToReturnTo.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (curr_width, new_height);
				//Debug.Log("Curr height: " + curr_height);

				//Debug.Log("Parent Name: " + d.parentToReturnTo.parent.name);
				//float parent_new_height = d.parentToReturnTo.parent.GetComponent<RectTransform> ().sizeDelta.y;

				float parent_new_height = d.parentToReturnTo.parent.GetComponent<RectTransform> ().sizeDelta.y + action_height;
				float parent_curr_width = d.parentToReturnTo.parent.GetComponent<RectTransform> ().sizeDelta.x;
				d.parentToReturnTo.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (parent_curr_width, parent_new_height);
			}
			*/

			if (d.parentToReturnTo.transform.name == "DropArea" && (d.parentToReturnTo.parent.GetComponent<Draggable> ().typeOfItem == Draggable.Type.IFSTAT)) {

				//Debug.Log ("IFSTAT's childCound: " + d.parentToReturnTo.childCount);

				// its full, dont add. add back to toolbox
				if (d.parentToReturnTo.childCount > 1) {
					//Debug.Log ("IFSTAT is full. Can't add box.");
					manager.showError ("This if statement is full.");
					d.parentToReturnTo = toolbox.transform;
				}
			}
		}
	}

	void Start() {
		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
		toolbox = GameObject.Find ("DropAreaTools");
	}
}
