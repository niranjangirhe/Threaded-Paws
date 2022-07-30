using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventRecorded : MonoBehaviour
{
    List<Thread> threads = new List<Thread>();
    ExecuteThreadsLevel exe;
    [SerializeField] private int TutLevel;
    [SerializeField] private Animator animator;
    public List<BlockInfo> validSeq;
    private List<bool> flags;

    // Start is called before the first frame update
    void Start()
    {
        exe = gameObject.GetComponent<ExecuteThreadsLevel>();
        threads = exe.threads;
        switch(TutLevel)
        {
            case 1: animator.Play("Tut1S1"); break;
            case 2: animator.Play("Tut2S1"); break;
        }
        
        StartCoroutine(checkoneBlock(0));
    }

    // Update is called once per frame

    IEnumerator checkoneBlock(int i)
    {
        if(i==1)
        {
            Debug.Log("Inserted one Block");
            if(TutLevel==1)
            {
                animator.Play("Tut1S2");
            }
        }
        if (i == 2)
        {
            Debug.Log("Inserted one Block");
            if (TutLevel == 2)
            {
                animator.Play("Tut1S2");
            }
        }
        int count = 0;
        flags = new List<bool>();
        foreach (Thread t in threads)
        {
            foreach (Transform child in exe.GetActionBlocks(t.tabDropArea))
            {
                
                switch (validSeq[count].actionEnum)
                {
                    case ActionEnum.CheckIn:
                        flags.Add(child.name == "CheckInBox");
                        break;
                    case ActionEnum.CheckOut:
                        flags.Add(child.name == "CheckOutBox");
                        break;
                    case ActionEnum.Cut:
                        flags.Add(child.name == "CutBox");
                        break;
                    case ActionEnum.Dry:
                        flags.Add(child.name == "DryBox");  
                        break;
                    case ActionEnum.Get:
                        flags.Add(child.name == "ResourceBox");
                        switch (validSeq[count].itemsEnum)
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

                        switch (validSeq[count].itemsEnum)
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
                        if (isDataRace)
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
                count++;
            }
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(checkoneBlock(count));
    }

}
