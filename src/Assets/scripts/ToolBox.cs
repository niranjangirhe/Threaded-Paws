using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ToolBox : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public Draggable.Type typeOfArea = Draggable.Type.INVENTORY;

	//usually only triggered for the mouse pointer only
	public void OnPointerEnter(PointerEventData eventData) {
		//Debug.Log ("OnPointerEnter to " + gameObject.name);

		if (eventData.pointerDrag == null)
			return; //nothing is being dragged

		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		if (d != null) {
			if (typeOfArea == d.typeOfItem || typeOfArea == Draggable.Type.ALL || typeOfArea == Draggable.Type.INVENTORY) {
				d.placeholderParent = this.transform;
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
			}
		}
	}
}
