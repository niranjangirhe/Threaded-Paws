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
		//Debug.Log ("OnPointerEnter to " + gameObject.name);

		if (eventData.pointerDrag == null)
			return; //nothing is being dragged

		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		if (d != null) {

			if (typeOfArea == d.typeOfItem || typeOfArea == Draggable.Type.ALL || typeOfArea == Draggable.Type.INVENTORY) {
				d.placeholderParent = this.transform;
				d.parentToReturnTo = this.transform;

				Debug.Log ("[OnPointerEnter] Parent to return to: " + d.parentToReturnTo.transform.name);
			}
		}
	}

	//usually only triggered for the mouse pointer only
	public void OnPointerExit(PointerEventData eventData) {
		//Debug.Log ("OnPointerExit to " + gameObject.name);

		if (eventData.pointerDrag == null)
			return; //nothing is being dragged

		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		//if placeholderParent is equal to us - otherwise, someones already taken the placeholder parent
		if (d != null && d.placeholderParent == this.transform) {
			if (typeOfArea == d.typeOfItem || typeOfArea == Draggable.Type.ALL || typeOfArea == Draggable.Type.INVENTORY) {
				d.placeholderParent = d.parentToReturnTo;
				Debug.Log ("[OnPointerExit] Placeholder parent: " + d.parentToReturnTo.transform.name);
			}
		}
	}

	public void OnDrop(PointerEventData eventData) {
		//Debug.Log (eventData.pointerDrag.name + " was dropped on " + gameObject.name);

		//to know the name of what was dropped on it: eventData.pointerDrag.name
		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if (d != null) {
			if (typeOfArea == d.typeOfItem || typeOfArea == Draggable.Type.ALL || typeOfArea == Draggable.Type.INVENTORY) {
				d.parentToReturnTo = this.transform;
				Debug.Log ("[OnDrop] Parent to return to: " + d.parentToReturnTo.transform.name);
			}
		}
	}

	void Start() {
		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
		toolbox = GameObject.Find ("DropAreaTools");
	}
}
