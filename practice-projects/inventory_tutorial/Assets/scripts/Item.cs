using UnityEngine;
using System.Collections;

public enum ItemType {ACTION, LOOP}

public class Item : MonoBehaviour {

	public ItemType type;

	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;

	//how many times an item can stack on itself
	public int maxSize;

	public void Use() {
		
		switch (type) { 
		case ItemType.ACTION:
			Debug.Log("Used an ACTION item");
			break;
		case ItemType.LOOP:
			Debug.Log("Used a LOOP item");
			break;
		}
	}
}
