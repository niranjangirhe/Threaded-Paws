using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableForLevelTab : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler
{

	public Transform parentToReturnTo = null;
	GameObject placeholder = null;
	// keep track of this in order to bounce back in case of illegal area
	public Transform placeholderParent = null;
	// GameObject threadArea;
	GameObject canvas;
	GameObject toolbox;

	private AudioSource audioSource;
	private AudioClip select, drop;


	public static byte TOOLBOX = 0;
	public static byte THREAD = 1;

	public byte isFrom = TOOLBOX;

	//to fix scrolling of toolbox. ---I have used raycast trick, but might be easy solution

	public enum Type { IFNEEDED, WHILELOOP, IFSTAT, ACTION, ALL, INVENTORY };
	public Type typeOfItem = Type.ALL; //default

	public void OnBeginDrag(PointerEventData eventData)
	{
		audioSource.clip = select;
		audioSource.Play();
		


		placeholder = new GameObject();
		placeholder.transform.SetParent(this.transform.parent); //places it at the end of the list by default
																//want the placeholder to have the same dimensions as the draggable object removed
		LayoutElement le = placeholder.AddComponent<LayoutElement>();
		placeholder.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 35);

		le.flexibleWidth = 0; //not flexible
		le.flexibleHeight = 0;



		//want the placeholder to also be in the same spot as the object we just removed
		placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

		//save old parent
		//parentToReturnTo = this.transform.parent;
		parentToReturnTo = toolbox.transform;

		//make sure it defaults to old parent
		placeholderParent = parentToReturnTo;

		//instead of the toolbox, wanna set parent to canvas
		this.transform.SetParent(canvas.transform);

		

		//highlight threadArea
		//threadArea.transform.GetComponent<Image> ().color = Color.green;

		
	}

	public void OnDrag(PointerEventData eventData)
	{
		//physically move the card
		this.transform.position = eventData.position;

		//to visualize that the selection is active
		this.GetComponent<CanvasGroup>().alpha = 0.5f;
		this.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);


		// do not shift items in the toolbox
		if ((parentToReturnTo.transform.name != toolbox.transform.name) && (placeholder.transform.parent.name != toolbox.transform.name) && (placeholderParent.transform.name != toolbox.transform.name))
		{

			//Debug.Log ("SHIFTING!! \n\ntoolbox.transform.name: " + toolbox.transform.name + "\nparentToReturnTo: " + parentToReturnTo + "\nplaceholder.transform.parent.name:  " + placeholder.transform.parent.name);

			if (placeholder.transform.parent != placeholderParent)
				placeholder.transform.SetParent(placeholderParent);

			int newSiblingIndex = placeholderParent.childCount; //initialized to what it currently is

			//adjust where the placeholder is, depending on what the object is hover over (using y coordinates)
			//iterate through all the children in the original parent, and check if the box is up or down to the box under it

			try
			{

				for (int i = 0; i < placeholderParent.childCount; i++)
				{

					newSiblingIndex = i;

					if (this.transform.position.y > parentToReturnTo.GetChild(i).position.y)
					{

						//if the placeholder is actually already below the sibling index
						if (placeholder.transform.GetSiblingIndex() > newSiblingIndex)
							newSiblingIndex--; //ignore the placeholder in the list

						placeholder.transform.SetSiblingIndex(i); //make the placeholder be in this position instead
						break; //since we'll obviously be in the same position in respect to the rest of the boxes afterwards
					}

				}

			}
			catch
			{
				//Debug.Log ("An exception occured: " + e.GetBaseException());
				//Debug.Log (e.Message);
			}
		}
	}


	public void OnEndDrag(PointerEventData eventData)
	{
		


		audioSource.clip = drop;
		audioSource.Play();
		Debug.Log("OnEndDrag called");

		//to end the visualization of selection
		this.GetComponent<CanvasGroup>().alpha = 1;
		this.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

		//set parent back to where we came from (at the end of the list)
		this.transform.SetParent(parentToReturnTo);

		//bounce back the object to where the placeholder is
		this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

		GetComponent<CanvasGroup>().blocksRaycasts = true;

		//iterate through corresponding zones and remove highlights, if any


		if (this.transform.parent.name == "DropAreaTools")
		{
			
			//self-destroy
			Destroy(this.gameObject);

		}
		else if (this.transform.parent.gameObject == canvas)
		{

			this.transform.SetParent(toolbox.transform);

		}
		else
		{

			
			//new parent is inside an if statement or a look
			if (this.transform.parent.name == "DropArea")
			{
				//this.transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (this.GetComponent<RectTransform> ().sizeDelta.x, this.GetComponent<RectTransform> ().sizeDelta.y + 45);

				//this.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2(75, 25);
			}

		}

		

		Destroy(placeholder);

		
	}

	void Start()
	{
		// threadArea = GameObject.Find("DropAreaThread");
		audioSource = GameObject.Find("_SCRIPTS_").GetComponent<AudioSource>();
		select = Resources.Load<AudioClip>("audio/select");
		drop = Resources.Load<AudioClip>("audio/drop");

		

		canvas = GameObject.Find("Canvas");
		toolbox = GameObject.Find("LevelParent");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		/*if (gameObject.name == transform.parent.name)
		{
			Destroy(gameObject);
		}*/
	}


}