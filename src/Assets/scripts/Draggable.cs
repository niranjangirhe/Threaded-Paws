using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler
{

	public Transform parentToReturnTo = null;
	GameObject placeholder = null;
	// keep track of this in order to bounce back in case of illegal area
	public Transform placeholderParent = null;
	// GameObject threadArea;
	GameObject canvas;
	GameObject toolbox;
	public static byte TOOLBOX = 0;
	public static byte THREAD = 1;

	public byte isFrom = TOOLBOX;


	ToolBoxValues manager;

	public enum Type { IFNEEDED, WHILELOOP, IFSTAT, ACTION, ALL, INVENTORY };
	public Type typeOfItem = Type.ALL; //default

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (isFrom == TOOLBOX)
		{
			int activeTab = Int32.Parse(Regex.Match(GameObject.Find("TabParent").transform.GetChild(GameObject.Find("TabParent").transform.childCount - 1).gameObject.name, @"\d+").Value);
			ExecuteThreadsLevel3_5 exeThread = GameObject.Find("Threads").GetComponent<ExecuteThreadsLevel3_5>();
			ToolBoxValues tbv = exeThread.threads[activeTab].toolBoxValues;
			int cardCount = (int)tbv.GetType().GetField(gameObject.name).GetValue(tbv);
			cardCount -= 1;
			tbv.GetType().GetField(gameObject.name).SetValue(tbv, cardCount);

			//update toolbox value (UI)
			exeThread.updateValues(activeTab);
		}
		try
		{

			if (eventData.pointerDrag.transform.parent.parent.gameObject.GetComponent<Draggable>().typeOfItem == Type.WHILELOOP)
			{

				int num_children = eventData.pointerDrag.transform.parent.childCount;
				float curr_width = eventData.pointerDrag.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
				float new_height = (num_children * 25);
				float parent_curr_width = eventData.pointerDrag.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.x;
				float parent_new_height = new_height + 25;

				eventData.pointerDrag.transform.parent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(parent_curr_width, parent_new_height);
				eventData.pointerDrag.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(curr_width, new_height);
			}

		}
		catch { }


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

		GetComponent<CanvasGroup>().blocksRaycasts = false;

		//highlight threadArea
		//threadArea.transform.GetComponent<Image> ().color = Color.green;

		foreach (Thread t in GameObject.Find("Threads").GetComponent<ExecuteThreadsLevel3_5>().threads)
		{
			t.tabDropArea.GetComponent<Image>().color = Color.green;
			Transform[] threadChildren = new Transform[t.tabDropArea.transform.childCount];
			for (int i = 0; i < threadChildren.Length; i++)
			{
				//threadChildren [i] = this.transform.Find("DropAreaThread").GetChild (i).gameObject;
				threadChildren[i] = t.tabDropArea.transform.GetChild(i);
				//Debug.Log ( timer.GetCurrentTime() + " -> " + threadChildren [i]);

				try
				{

					//Debug.Log (thread1Children [i].name + ", DropZone Type: " + thread1Children [i].GetComponentInChildren<DropZone>().typeOfArea);
					if ((threadChildren[i].gameObject.GetComponentInChildren<DropZone>().typeOfArea == this.typeOfItem) ||
						(threadChildren[i].gameObject.GetComponentInChildren<DropZone>().typeOfArea == Type.ALL))
					{

						string zoneName = threadChildren[i].gameObject.GetComponentInChildren<DropZone>().name;
						//Debug.Log("Theres a dropzone!: " + zoneName);
						threadChildren[i].Find(zoneName).GetComponent<Image>().color = Color.green;
					}

				}
				catch { }

			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		//physically move the card
		this.transform.position = eventData.position;

		//to visualize that the selection is active
		this.GetComponent<CanvasGroup>().alpha = 0.5f;
		this.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);

		if (ToolBox.onToolBox && isFrom == THREAD)
		{
			foreach (Thread t in GameObject.Find("Threads").GetComponent<ExecuteThreadsLevel3_5>().threads)
			{
				t.tabDropArea.GetComponent<Image>().color = Color.red;
			}
		}
		else if (ToolBox.onToolBox && isFrom == TOOLBOX)
		{
			foreach (Thread t in GameObject.Find("Threads").GetComponent<ExecuteThreadsLevel3_5>().threads)
			{
				t.tabDropArea.GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 1);
			}
		}
		else if(!ToolBox.onToolBox)
        {
			foreach (Thread t in GameObject.Find("Threads").GetComponent<ExecuteThreadsLevel3_5>().threads)
			{
				t.tabDropArea.GetComponent<Image>().color = Color.green;
			}
		}





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


		foreach (Thread t in GameObject.Find("Threads").GetComponent<ExecuteThreadsLevel3_5>().threads)
		{
			t.tabDropArea.GetComponent<Image>().color = Color.green;
			Transform[] threadChildren = new Transform[t.tabDropArea.transform.childCount];
			for (int i = 0; i < threadChildren.Length; i++)
			{
				try
				{
					threadChildren[i] = t.tabDropArea.transform.GetChild(i);
					if ((threadChildren[i].gameObject.GetComponentInChildren<DropZone>().typeOfArea == this.typeOfItem) ||
						(threadChildren[i].gameObject.GetComponentInChildren<DropZone>().typeOfArea == Type.ALL))
					{
						string zoneName = threadChildren[i].gameObject.GetComponentInChildren<DropZone>().name;
						//Debug.Log ("De-colouring: " + thread1Children [i].gameObject.GetComponentInChildren<DropZone> ().name);

						threadChildren[i].Find(zoneName).GetComponent<Image>().color = Color.white;
					}

				}
				catch { }
			}
			t.tabDropArea.transform.GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 1);
		}






		if (this.transform.parent.name == "DropAreaTools")
		{
			Debug.Log("Dropped in the toolbox");

			int activeTab = Int32.Parse(Regex.Match(GameObject.Find("TabParent").transform.GetChild(GameObject.Find("TabParent").transform.childCount - 1).gameObject.name, @"\d+").Value);
			ExecuteThreadsLevel3_5 exeThread = GameObject.Find("Threads").GetComponent<ExecuteThreadsLevel3_5>();
			ToolBoxValues tbv = exeThread.threads[activeTab].toolBoxValues;

			LogManager.instance.logger.sendChronologicalLogs("DropW1-" + this.transform.GetChild(0).GetComponentInChildren<Text>().text + "_" + LogManager.chronoInputCount, "", LogManager.instance.UniEndTime().ToString());
			int val = (int)tbv.GetType().GetField(this.transform.name).GetValue(tbv);
			tbv.GetType().GetField(this.transform.name).SetValue(tbv, val + 1);

			//update toolbox value (UI)
			exeThread.updateValues(activeTab);

			//self-destroy
			Destroy(this.gameObject);

		}
		else if (this.transform.parent.gameObject == canvas)
		{

			this.transform.SetParent(toolbox.transform);

		}
		else
		{

			LogManager.chronoInputCount++;
			LogManager.instance.logger.sendChronologicalLogs("Put_" + LogManager.chronoInputCount + this.GetComponentInChildren<Text>().text, "", LogManager.instance.UniEndTime().ToString());
			//new parent is inside an if statement or a look
			if (this.transform.parent.name == "DropArea")
			{
				//this.transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (this.GetComponent<RectTransform> ().sizeDelta.x, this.GetComponent<RectTransform> ().sizeDelta.y + 45);

				//this.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2(75, 25);
			}

		}

		if (this.typeOfItem == Type.IFSTAT)
		{
			this.GetComponent<Image>().color = new Vector4(0.3F, 0.8F, 0.83F, 1);

		}
		else if (this.typeOfItem == Type.ACTION && (this.GetComponentInChildren<Text>().text == "get"))
		{
			//			LogData.chronologicalLogs.Add ("Put-get: " + LogManager.instance.UniEndTime ());
			this.GetComponent<Image>().color = new Vector4(0.94F, 0.28F, 0.94F, 1);

		}
		else if (this.typeOfItem == Type.ACTION && (this.GetComponentInChildren<Text>().text == "ret"))
		{
			//			LogData.chronologicalLogs.Add ("Put-ret: " + LogManager.instance.UniEndTime ());
			this.GetComponent<Image>().color = new Vector4(0.56F, 0.82F, 0.44F, 1);

		}
		else if (this.typeOfItem == Type.ACTION)
		{
			//	print (this.GetComponentInChildren<Text> ().text);
			//			LogData.chronologicalLogs.Add ("Put-"+this.GetComponentInChildren<Text> ().text+": " + LogManager.instance.UniEndTime ());
			this.GetComponent<Image>().color = new Vector4(1, 0.76F, 0.24F, 1);

		}
		else if (this.typeOfItem == Type.WHILELOOP)
		{
			this.GetComponent<Image>().color = new Vector4(0.77F, 0.71F, 0.6F, 1);
		}

		// this.GetComponent<Image> ().color = Color.white;

		Destroy(placeholder);

		//CreateNewBlock.canCreate = true;
	}

	void Start()
	{
		// threadArea = GameObject.Find("DropAreaThread");

		canvas = GameObject.Find("Canvas");
		toolbox = GameObject.Find("DropAreaTools");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (gameObject.name == transform.parent.name)
		{
			Debug.Log("Destroyed");
			Destroy(gameObject);
			CreateNewBlock.canCreate = true;
		}
	}


}