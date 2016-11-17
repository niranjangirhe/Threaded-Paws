using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public Draggable.Slot typeOfItem = Draggable.Slot.ALL;

	//usually only triggered for the mouse pointer only
	public void OnPointerEnter(PointerEventData eventData) {
		//Debug.Log ("OnPointerEnter to " + gameObject.name);

		if (eventData.pointerDrag == null)
			return; //nothing is being dragged

		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		if (d != null) {
			if (typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.ALL) {
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
			if (typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.ALL) {
				d.placeholderParent = d.parentToReturnTo;
			}
		}
	}

	public void OnDrop(PointerEventData eventData) {
		//Debug.Log (eventData.pointerDrag.name + " was dropped on " + gameObject.name);

		//to know the name of what was dropped on it: eventData.pointerDrag.name
		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if (d != null) {
			if (typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.ALL) {
				d.parentToReturnTo = this.transform;
			}
		}
	}
}
