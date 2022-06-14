using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiateLevel3_5 : MonoBehaviour {

	public GameObject actionPrefab;
	public GameObject acquirePrefab;
	public GameObject returnPrefab;

	// Use this for initialization
	void Start () {
 
        

        GameObject box;

        foreach (Thread t in GameObject.Find("Threads").GetComponent<ExecuteThreadsLevel3_5>().threads)
        {
            foreach(BlockInfo bi in t.blockInfo)
            {
                switch(bi.actionEnum)
                {
                    case ActionEnum.CheckIn:
                        newBox(actionPrefab, "checkin", t.tabDropArea, "CheckInBox");
                        break;
                    case ActionEnum.CheckOut:
                        newBox(actionPrefab, "checkout", t.tabDropArea, "CheckOutBox");
                        break;
                    case ActionEnum.Cut:
                        newBox(actionPrefab, "Cut", t.tabDropArea, "CutBox");
                        break;
                    case ActionEnum.Dry:
                        newBox(actionPrefab, "Dry", t.tabDropArea, "DryBox");
                        break;
                    case ActionEnum.Get:
                        box = newBox(acquirePrefab, "get", t.tabDropArea, "ResourceBox");

                        switch(bi.itemsEnum)
                        {
                            case ItemsEnum.Null:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "[null]";
                                box.gameObject.transform.Find("Label").GetComponent<Text>().color = Color.red;
                                break;
                            case ItemsEnum.brush:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "brush";
                                break;
                            case ItemsEnum.clippers:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "clippers";
                                break;
                            case ItemsEnum.conditioner:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "cond.";
                                break;
                            case ItemsEnum.dryer:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "dryer";
                                break;
                            case ItemsEnum.scissors:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "scissors";
                                break;
                            case ItemsEnum.shampoo:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "shampoo";
                                break;
                            case ItemsEnum.station:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "station";
                                break;
                            case ItemsEnum.towel:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "towel";
                                break;
                        }

                        break;
                    case ActionEnum.Groom:
                        newBox(actionPrefab, "Groom", t.tabDropArea, "GroomBox");
                        break;
                    case ActionEnum.Return:
                        box = newBox(returnPrefab, "ret", t.tabDropArea, "ReturnBox");

                        switch (bi.itemsEnum)
                        {
                            case ItemsEnum.Null:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "[null]";
                                box.gameObject.transform.Find("Label").GetComponent<Text>().color = Color.red;
                                break;
                            case ItemsEnum.brush:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "brush";
                                break;
                            case ItemsEnum.clippers:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "clippers";
                                break;
                            case ItemsEnum.conditioner:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "cond.";
                                break;
                            case ItemsEnum.dryer:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "dryer";
                                break;
                            case ItemsEnum.scissors:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "scissors";
                                break;
                            case ItemsEnum.shampoo:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "shampoo";
                                break;
                            case ItemsEnum.station:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "station";
                                break;
                            case ItemsEnum.towel:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "towel";
                                break;
                        }
                        break;
                    case ActionEnum.Wash:
                        newBox(actionPrefab, "Wash", t.tabDropArea, "WashBox");
                        break;
                }
           
            }
        }

        
        

    }
		
	GameObject newBox(GameObject boxPrefab, string actionName, GameObject threadParent, string boxName) {
		
		GameObject newActionBox = (GameObject)Instantiate (boxPrefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
        newActionBox.GetComponent<Draggable>().isFrom = Draggable.THREAD;
        newActionBox.name = boxName;
		newActionBox.transform.SetParent (threadParent.transform);
		newActionBox.transform.localScale = Vector3.one;
		newActionBox.transform.GetChild (0).GetComponentInChildren<Text> ().text = actionName;
		//newActionBox.transform.Find("Halo").gameObject.SetActive (false);

		return newActionBox;
	}
}
