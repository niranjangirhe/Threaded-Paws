using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//could also be done using an event trigger (add component in inspector window)
public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	// 1 - set a type of zone for this drop area. to make a dragging action "legal", the type of zone needs to match the type of card being dragged into it
	public Draggable.Slot typeOfItem = Draggable.Slot.INVENTORY; //any can be selected for the object with this component from the inspector window

	//from interface
	//called when the MOUSE enters an area (without the card when its dragging)
	public void OnPointerEnter(PointerEventData eventData){
		//Debug.Log ("OnPointerEnter");

		// 5.1 -
		if (eventData.pointerDrag == null)
			return; //if nothing is being dragged, we don't have any work to do

		// 5.1 -
		//get the component on the object being dragged
		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		//override the parent and set it to the one that the object (card) is being dragged on
		if (d != null) {
			// 1 - if the type matches OR if the drop area is the inventory (since this area should accept all)
			if (typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.INVENTORY) {
				d.placeholderParent = this.transform;
			}
		}
	}

	//from interface
	//called when the MOUSE exits an area (without the card when its dragging)
	public void OnPointerExit(PointerEventData eventData) {
		//Debug.Log ("OnPointerExit");

		// 5.1 -
		if (eventData.pointerDrag == null)
			return; //if nothing is being dragged, we don't have any work to do

		// 5.1 -
		//get the component on the object being dragged
		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		//override the parent and set it to the one that the object (card) is being dragged on
		if (d != null && d.placeholderParent == this.transform) { //if no one else has already stolen the placeholderParent
			// 1 - if the type matches OR if the drop area is the inventory (since this area should accept all)
			// when we exit legal zone:
			if (typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.INVENTORY) {
				d.placeholderParent = d.parentToReturnTo; //set it back to wherever we started from
			}
		}
	}

	//from interface
	public void OnDrop(PointerEventData eventData) {
		//pointer dragged is the object being dragged
		Debug.Log (eventData.pointerDrag.name + " was dragged on " + gameObject.name);

		//get the component on the object being dragged
		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();
		//override the parent and set it to the one that the object (card) is being dragged on
		//only change parenttoreturnto if we try to drop somewhere legal
		if (d != null) {
			// 1 - if the type matches OR if the drop area is the inventory (since this area should accept all)
			if (typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.INVENTORY) {
				d.parentToReturnTo = this.transform;
			}
		}
	}
}
