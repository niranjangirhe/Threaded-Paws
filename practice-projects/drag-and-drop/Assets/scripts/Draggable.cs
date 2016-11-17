using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//this could also be done adding an Event Trigger component in the Inspector
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	/*Draggable IS a Monobehaviour, but we can also bring in some new functions
	 *could use void OnDrag(), but we'll use IBeginDragHandler instead
	 */

	// 1 - If the card is dragged to a non-draggable space, then put it back in the original parent (hand)
	// 2 - avoid the card keeping these methods (in DropZone) from being blocked, added a Canvas Group component to the card.
	// 3 - enable feature to drop a card on any position in a drop zone, not always at the end
	// 4 - want the placeholder to be where the card was taken from, not always at the end
	// 5 - enable the user to re-arrange the cards in however way the'd like
		// 5.1 - need to take into account: the placeholder is one of the children of this array. wanna ignore it
		// 5.2 - place the placeholder in the drop area that we're actually hovering over, not the parent necessarily: see OnPointerEnter in DropZone.cs script
	// 6 - want card to bounce back to the object it was grabbed from if it is dropped in an "illegal" area, rather than other objects it was hovered over (because of 5.2)
	// 7 - TODO: avoid small mouse hop when the object is not grabbed from the middle by taking note on what the position is, and the difference between this and the mouse position. keep this as an offset.

	// 1 - save our parent
	public Transform parentToReturnTo = null; //"original" parent

	// 6 - making new placeholder
	public Transform placeholderParent = null;

	// 3 -
	GameObject placeholder = null; //start off as null

	// 7 -
	//Vector2 dragOffset = new Vector2(0f, 0f);

	// 7 -
	//Vector2 mousePosition;
	//public Vector2 difference;
	//Vector2 currentPosition;
	//public float smooth;

	//could use something like this to use on a card itself to determine if the dragging option is "legal" or not
	public enum Slot { WEAPON, HEAD, CHEST, LEGS, FEET, INVENTORY }; //any can be selected for the object with this component from the inspector window
	public Slot typeOfItem = Slot.WEAPON; //weapon is the default. make sure it matches the DropZone "type of zone"

	//from interface
	//needs a pointer event data as parameter
	public void OnBeginDrag(PointerEventData eventData) {
		//Debug.Log ("OnBeginDrag()");

		// 7 -
		//take note on what the position is, and the difference between this and the mouse position. keep this as an offset.
		//dragOffset = eventData.position - (Vector2) this.transform.position;

		// 7 -
		//currentPosition = this.transform.position;
		//mousePosition = eventData.position;
		//difference = mousePosition - currentPosition;

		// 3 -
		//create a placeholder completely from scratch
		placeholder = new GameObject(); //create empty game object
		placeholder.transform.SetParent(this.transform.parent); //set it to our old parent. by default, will put it at the end of the list.
		LayoutElement le = placeholder.AddComponent<LayoutElement>(); //should have a rect transform. wanna set the height and width of it to match the card completely. add a layout element for this and grab a copy of it
		le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth; //get the preferred width and height on the layout that the card has
		le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		le.flexibleWidth = 0; //not flexible
		le.flexibleHeight = 0;

		// 4 -
		//SetSiblingIndex: this number is the order of the siblings within a parent
		//set it to be the same as the card object
		placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

		//  1 - 
		parentToReturnTo = this.transform.parent;

		// 6 -
		placeholderParent = parentToReturnTo; //make sure this defaults to the original parent

		//change the parent from the hand to the canvas itself (the card's parent parent
		this.transform.SetParent( this.transform.parent.parent ); //hardcoded

		// 2 -
		GetComponent<CanvasGroup> ().blocksRaycasts = false;

		//make all possible drag zones for this card "glow"
		//DropZone[] zones = GameObject.FindObjectOfType<DropZone>();
		//TODO: iterate through this and if it matches the type, then highlight it

		//make the card itself "glow"
	}

	//from interface
	//note: if you dont respond to OnDrag itself, it will not trigger OnBeginDrag or OnEndDrag
	public void OnDrag(PointerEventData eventData) {
		//Debug.Log ("OnDrag()");

		// 7 -
		//this.transform.position = eventData.position - dragOffset;

		// 7 -
		//position = GetComponent<RectTransform>().anchoredPosition;
		//this.transform.position = eventData.position - difference;

		//physically move the card
		//use the transform position to where the mouse is
		this.transform.position = eventData.position;

		// 5.2 / 6 - make sure that if our placeholder.transparent.parent is not the thing that IS supposed to be our placeholderParent, then set it
		if (placeholder.transform.parent != placeholderParent)
			placeholder.transform.SetParent (placeholderParent);

		// 5.1 -
		//create a new placeholder
		//int newSiblingIndex = placeholder.transform.GetSiblingIndex();
		int newSiblingIndex = /*cause of 6-parentToReturnTo*/placeholderParent.childCount; //instead of defaulting to the spot where we were before, default to the # of children. This
		//assumes that we should end up in the rightmost position of anything, and then check if its actually somewhere else

		// 5 -
		// loop through all the children of the parent (e.g. hand area) and check if the card that we're dragging around is
		//further to the left or to the right of the card its currently hovering over. using x coordinates of each gameobject.
		for (int i = 0; i < /*cause of 6-parentToReturnTo*/placeholderParent.childCount; i++) { //0th child is on the left
			if (this.transform.position.x < /*cause of 6-parentToReturnTo*/placeholderParent.GetChild (i).position.x) {

				//set the position of the placeholder of the card being dragged to i
				//placeholder.transform.SetSiblingIndex(i); //cant use this anymore because we need 5.1 instead

				// 5.1 -
				newSiblingIndex = i;
				//if the placeholder is actually already to the left of this new sibling index,
				if (placeholder.transform.GetSiblingIndex () < newSiblingIndex)
					newSiblingIndex--; //bring down the new sibling index: ignore the placeholder in the list

				// 6 (all above) - changed all to placeholderParent, since THIS is what we're supposed to be moving

				// 5.1 -
				//as soon as we find one that we are the left of, then stop - that's good enough!
				break;
			}
		}

		// 5.1 -
		//set the placeholder to the new sibling index
		placeholder.transform.SetSiblingIndex(newSiblingIndex);
	}

	//from interface
	//could also be used to apply some logic (e.g. +2 strength)
	public void OnEndDrag(PointerEventData eventData) {
		//Debug.Log ("OnEndDrag()");	

		// 1 - set parent back to where we came from (at the end of the list) - setting the parent back to the hand area
		this.transform.SetParent(parentToReturnTo);

		// 4 -
		// replace the placeholder created with the card using indexes
		this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

		// 2 -
		GetComponent<CanvasGroup> ().blocksRaycasts = true;

		//TODO: get right of any highlights set on OnBeginDrag()

		// 3 -
		//destroy the placeholder so that there are no gaps (where the invisible placeholders are)
		Destroy(placeholder);

		/* OPTION 2 TO RE-ASSIGN PARENT */
		//get current position of the mouse (note: eventData contains the position of the mouse)
		//will do a raycast in a place. except for anything that has "raycasts blocked" (canvas group) turned off,
		//will tell you every single object that is underneath there.
		//EventSystem.current.RaycastAll(eventData); //targets the thing you're dropping on top of (e.g. can affect cards themselves)
	}

	// 7 -
	/*
	void Start() {
		smooth = Time.deltaTime * 420;
	}
	*/
}