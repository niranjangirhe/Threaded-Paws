using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	private RectTransform inventoryRect;

	//save width and height of inventory
	private float inventoryWidth, inventoryHeight;

	//set number of rows and slots from the inspector window
	public int slots;
	public int rows;

	//add padding/gap between slots
	public float slotPaddingLeft, slotPaddingTop;

	public float slotSize;

	//get slot prefab.
	public GameObject slotPrefab;

	//collection to contain all the objects.
	private List<GameObject> allSlots;

	//keep track of how many empty fields there are
	private int emptySlots;

	// Use this for initialization
	void Start () {
		CreateLayout ();
		emptySlots = slots;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//to be called from Start()
	private void CreateLayout() {

		//actually instantiate list
		allSlots = new List<GameObject> ();
	
		//calculate width of the inventory using the num. of slots & rows
		inventoryWidth = (slots/rows) * (slotSize+slotPaddingLeft) + slotPaddingLeft;
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

		//make a reference to the rect transform in the game object
		inventoryRect = GetComponent<RectTransform>();

		//resize the inventory horizontally using the width that was just calculated
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

		int columns = slots / rows;

		for (int y = 0; y < rows; y++) { //rows
			for (int x = 0; x < columns; x++) { //columns

				//instantiate new slot from prefab and get its recttransform to move around etc
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform> ();

				newSlot.name = "Slot";

				//make the canvas the parent
				//this object's parent's parent
				newSlot.transform.SetParent(this.transform.parent);

				//slot position
				slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop *
					(y + 1) - (slotSize * y));

				//set size to variable picked
				slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotSize);

				//add slot to the list
				allSlots.Add(newSlot);
			}		
		}
	}
}