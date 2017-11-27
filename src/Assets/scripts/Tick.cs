using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//usually only triggered for the mouse pointer only
	public void OnPointerEnter(PointerEventData eventData) {
		// Debug.Log ("OnPointerEnter to " + this.gameObject.name);

		if (eventData.pointerDrag != null)
			return;

		this.GetComponent<Image> ().color = new Vector4(0.9F, 0.9F, 0.9F, 1F);
	}

	//usually only triggered for the mouse pointer only
	public void OnPointerExit(PointerEventData eventData) {
		// Debug.Log ("OnPointerExit from " + gameObject.name);

		this.GetComponent<Image> ().color = Color.white;
	}


}