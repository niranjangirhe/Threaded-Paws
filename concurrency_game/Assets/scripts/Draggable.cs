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
	GameObject threadArea;

	public enum Type {IFNEEDED, LOOPIF , ACTION, ALL, INVENTORY};
	public Type typeOfItem = Type.ALL; //default

	public void OnBeginDrag(PointerEventData eventData) {
		//Debug.Log ("OnBeginDrag");

		//create new placeholder object and paret it to the draggable object's parent
		placeholder = new GameObject ();
		placeholder.transform.SetParent (this.transform.parent); //places it at the end of the list by default
		//want the placeholder to have the same dimensions as the draggable object removed
		LayoutElement le = placeholder.AddComponent<LayoutElement>();
		le.preferredWidth = this.GetComponent<LayoutElement> ().preferredWidth;
		le.preferredHeight = this.GetComponent<LayoutElement> ().preferredHeight;
		le.flexibleWidth = 0; //not flexible
		le.flexibleHeight = 0;

		//want the placeholder to also be in the same spot as the object we just removed
		placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
	
		//save old parent
		parentToReturnTo = this.transform.parent;

		//make sure it defaults to old parent
		placeholderParent = parentToReturnTo;

		//instead of the toolbox, wanna set parent to canvas
		this.transform.SetParent(this.transform.parent.parent.parent.parent);

		GetComponent<CanvasGroup> ().blocksRaycasts = false;

		//find all "legal" drop zones in the game for this item and highlight them
		//DropZone[] zones = GameObject.FindObjectsOfType<DropZone>();
		//TODO: iterate through corresponding zones and highlight
		/*
		for (int i = 0; i < zones.Length; i++) {

			if (zones [i].typeOfArea == Type.ALL || zones [i].typeOfArea == this.typeOfItem) {
				Debug.Log (zones [i].name); //"legal" areas
				 
				Debug.Log (threadArea.transform.childCount);

				threadArea.GetComponent<Button> ();
			}

			if ( || zones [i].typeOfArea == Type.ALL) {
				//highlight
				zones[i].GetComponent<Button>().Select();
			}
		*/

		//TODO: highlight threadArea
		threadArea.transform.GetComponent<Image> ().color = Color.cyan;
		//TODO: highlight corresponding children
		Transform[] threadChildren = new Transform[threadArea.transform.childCount];

		//Debug.Log ("childCount: " + threadChildren.Length);

		for (int i = 0; i < threadChildren.Length; i++) {
			//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;
			threadChildren [i] = threadArea.transform.GetChild (i);
			Debug.Log (threadChildren [i].name);
			//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);
			if (threadChildren [i].gameObject.GetComponentInChildren<DropZone>()) {
				string zoneName = threadChildren [i].gameObject.GetComponentInChildren<DropZone> ().name;
				Debug.Log("Theres a dropzone!: " + zoneName);
				threadChildren [i].Find(zoneName).GetComponent<Image>().color = Color.cyan;
			}
		}
	}

	public void OnDrag(PointerEventData eventData) {
		//Debug.Log ("OnDrag");


		//physically move the card
		this.transform.position = eventData.position;

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

		} catch (Exception e) {
			//Debug.Log ("An exception occured: " + e.GetBaseException());
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		//Debug.Log ("OnEndDrag");

		//set parent back to where we came from (at the end of the list)
		this.transform.SetParent(parentToReturnTo);

		//bounce back the object to where the placeholder is
		this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

		GetComponent<CanvasGroup> ().blocksRaycasts = true;

		//TODO: iterate through corresponding zones and remove highlights, if any
		threadArea.transform.GetComponent<Image>().color = Color.white;
		Transform[] threadChildren = new Transform[threadArea.transform.childCount];

		//Debug.Log ("childCount: " + threadChildren.Length);

		for (int i = 0; i < threadChildren.Length; i++) {
			//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;
			threadChildren [i] = threadArea.transform.GetChild (i);
			Debug.Log (threadChildren [i].name);
			//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);
			if (threadChildren [i].gameObject.GetComponentInChildren<DropZone>()) {
				string zoneName = threadChildren [i].gameObject.GetComponentInChildren<DropZone> ().name;
				Debug.Log("Theres a dropzone!: " + zoneName);
				threadChildren [i].Find(zoneName).GetComponent<Image>().color = Color.white;
			}
		}

		//TODO: apply "logic" here (e.g. add +2 health)

		Destroy (placeholder);
	}

	void Start() {
		threadArea = GameObject.Find("DropAreaThread");

	}
}