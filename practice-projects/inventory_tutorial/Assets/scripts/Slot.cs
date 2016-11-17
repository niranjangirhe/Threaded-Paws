using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

	//add a stack of items
	private Stack<Item> items;

	public Text stackTxt;

	//for when the slot is empty
	public Sprite slotEmpty;
	public Sprite slotHighlight;

	public bool IsEmpty {
		get { return items.Count == 0; }
	}

	// Use this for initialization
	void Start () {
		items = new Stack<Item> ();

		//reference to the transform sitting on the slot
		RectTransform slotRect = GetComponent<RectTransform>();
		//reference to transform sitting on the "stack" text object
		RectTransform txtRect = GetComponent<RectTransform> ();

		//bigger text, larger text! text should always be 60% the size of the slotrect
		int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
		stackTxt.resizeTextMaxSize = txtScaleFactor;
		stackTxt.resizeTextMinSize = txtScaleFactor;
		txtRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
		txtRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddItem(Item item) {

		//add item to the stack
		items.Push (item);

		//update text
		stackTxt.text = items.Count.ToString();

		//change sprite from empty to whatever item was added
		ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
	}

	//change the state of the button
	private void ChangeSprite(Sprite neutral, Sprite highlight) {
		//need to change the neutral and highlight sprites to show there is something there

		GetComponent<Image> ().sprite = neutral;

		SpriteState st = new SpriteState ();
		st.highlightedSprite = highlight;
		st.pressedSprite = neutral;

		GetComponent<Button> ().spriteState = st;
	}
}
