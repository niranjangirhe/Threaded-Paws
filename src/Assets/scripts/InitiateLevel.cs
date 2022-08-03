using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InitiateLevel : MonoBehaviour {

	public GameObject actionPrefab;
    public GameObject dataPrefab;
    public GameObject acquirePrefab;
	public GameObject returnPrefab;
    public GameObject checkinoutPrefab;
    // Use this for initialization
    void Start () {

        bool isDataRace = GameObject.Find("_SCRIPTS_").GetComponent<ExecuteThreadsLevel>().IsDataRace();
        GameObject.Find("Canvas").transform.Find("InstructionsPanel").gameObject.SetActive(true);
        GameObject.Find("CashParent").SetActive(isDataRace);
        GameObject bin = GameObject.Find("Bin");
        bin.transform.SetAsFirstSibling();
        bin.GetComponent<CanvasGroup>().alpha = 0;

        GameObject box;

        foreach (Thread t in GameObject.Find("_SCRIPTS_").GetComponent<ExecuteThreadsLevel>().threads)
        {
            foreach(BlockInfo bi in t.blockInfo)
            {
                switch(bi.actionEnum)
                {
                    case ActionEnum.CheckIn:
                        newBox(checkinoutPrefab, "checkin", t.tabDropArea, "CheckInBox");
                        break;
                    case ActionEnum.CheckOut:
                        newBox(checkinoutPrefab, "checkout", t.tabDropArea, "CheckOutBox");
                        break;
                    case ActionEnum.Cut:
                        newBox(actionPrefab, "cut", t.tabDropArea, "CutBox");
                        break;
                    case ActionEnum.Dry:
                        newBox(actionPrefab, "dry", t.tabDropArea, "DryBox");
                        break;
                    case ActionEnum.Get:
                        box = newBox(acquirePrefab, "get", t.tabDropArea, "ResourceBox");

                        switch(bi.itemsEnum)
                        {
                            case ItemsEnum.Null:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "???";
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
                            case ItemsEnum.spray:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "spray";
                                break;
                            case ItemsEnum.towel:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "towel";
                                break;
                            case ItemsEnum.sponge:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "sponge";
                                break;
                            case ItemsEnum.cash_reg:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "cash reg.";
                                break;
                        }

                        break;
                    case ActionEnum.Brush:
                        newBox(actionPrefab, "brush", t.tabDropArea, "BrushBox");
                        break;
                    case ActionEnum.Return:
                        box = newBox(returnPrefab, "ret", t.tabDropArea, "ReturnBox");

                        switch (bi.itemsEnum)
                        {
                            case ItemsEnum.Null:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "???";
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
                            case ItemsEnum.spray:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "spray";
                                break;
                            case ItemsEnum.towel:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "towel";
                                break;
                            case ItemsEnum.sponge:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "sponge";
                                break;
                            case ItemsEnum.cash_reg:
                                box.gameObject.transform.Find("Dropdown").GetComponent<dropDownManager>().selected = "cash reg.";
                                break;
                        }
                        break;
                    case ActionEnum.Wash:
                        newBox(actionPrefab, "wash", t.tabDropArea, "WashBox");
                        break;
                    case ActionEnum.Read:
                        if(isDataRace)
                            newBox(dataPrefab, "read", t.tabDropArea, "ReadBox");
                        break;
                    case ActionEnum.Calculate:
                        if (isDataRace)
                            newBox(dataPrefab, "calculate", t.tabDropArea, "CalculateBox");
                        break;
                    case ActionEnum.Write:
                        if (isDataRace)
                            newBox(dataPrefab, "write", t.tabDropArea, "WriteBox");
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
        if(gameObject.GetComponent<ExecuteThreadsLevel>().isTutorial)
        {
            Destroy(newActionBox.GetComponent<Draggable>());
            Color32 blockColor = newActionBox.GetComponent<Image>().color;
            blockColor.a = 100;
            newActionBox.GetComponent<Image>().color = blockColor;
        }

		return newActionBox;
	}
}
