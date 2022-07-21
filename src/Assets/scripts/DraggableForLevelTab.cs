using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableForLevelTab : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{


	// GameObject threadArea;
	GameObject canvas;
	public static int level_int;
	[HideInInspector] public int my_level;
	private AudioSource audioSource;
	private AudioClip select, drop;
	private GameObject dropArea;
	private Vector3 dropAreaPosition;
	private GameObject btn;



	//to fix scrolling of toolbox. ---I have used raycast trick, but might be easy solution


	

	public void OnBeginDrag(PointerEventData eventData)
	{
		audioSource.clip = select;
		audioSource.Play();
		


		//instead of the toolbox, wanna set parent to canvas
		this.transform.SetParent(canvas.transform);

		if (dropArea.transform.childCount > 0)
		{
			Destroy(dropArea.transform.GetChild(0).gameObject);
			btn.GetComponent<Button>().interactable = false;
		}
		
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

		if(isInside(eventData.position))
        {
			dropArea.GetComponent<Image>().color = Color.green;
        }
		else
        {
			dropArea.GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 1);
		}
	}


	public void OnEndDrag(PointerEventData eventData)
	{

		audioSource.clip = drop;
		audioSource.Play();
		
		//to end the visualization of selection
		this.GetComponent<CanvasGroup>().alpha = 1;
		this.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

		if(isInside(eventData.position))
        {
			this.gameObject.transform.SetParent(dropArea.transform);
			this.gameObject.transform.position = dropAreaPosition;
			btn.GetComponent<Button>().interactable = true;
		}
		else
        {
			btn.GetComponent<Button>().interactable = false;
			Destroy(this.gameObject);
        }
	
	}

	void Start()
	{
		// threadArea = GameObject.Find("DropAreaThread");
		audioSource = GameObject.Find("_SCRIPTS_").GetComponent<AudioSource>();
		select = Resources.Load<AudioClip>("audio/select");
		drop = Resources.Load<AudioClip>("audio/drop");

		canvas = GameObject.Find("Canvas");
		dropArea = GameObject.Find("DropAreaThread");
		btn = GameObject.Find("Start");
	
		dropAreaPosition = dropArea.transform.position;

	}
	bool isInside(Vector3 v)
	{
		if (v.x > dropAreaPosition.x - 90 && v.x < dropAreaPosition.x + 90 && v.y < dropAreaPosition.y + 25 && v.y > dropAreaPosition.y - 25)
		{

			return true;

		}
		else
		{
			return false;
		}
	}
	
	
}