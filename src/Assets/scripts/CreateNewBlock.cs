using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Collections;

public class CreateNewBlock : MonoBehaviour {

	public GameObject prefab;
	public GameObject canvas;
    private GameObject child;

    private Text txtErrorMsg;

    ToolBoxValues manager;

	public static bool canCreate;

    public void NewActionBlock()
    {

        //active tab

        int activeTab = Int32.Parse(Regex.Match(GameObject.Find("TabParent").transform.GetChild(GameObject.Find("TabParent").transform.childCount - 1).gameObject.name, @"\d+").Value);
        manager = GameObject.Find("Threads").GetComponent<ExecuteThreadsLevel3_5>().threads[activeTab].toolBoxValues;
        int cardCount = (int)manager.GetType().GetField(this.transform.name).GetValue(manager);
        if (cardCount > 0)
        {

            if (canCreate)
            {
                DeleteOtherClone();
                GameObject newActionBox = (GameObject)Instantiate(prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
                child = newActionBox;
                newActionBox.name = this.transform.name;
                newActionBox.transform.SetParent(this.transform);
                newActionBox.transform.localScale = Vector3.one;
                newActionBox.transform.GetChild(0).GetComponentInChildren<Text>().text = this.GetComponentInChildren<Text>().text;
                
                
            }
            else
            {
                showError("Use or discard your current object first");
            }

        }
        else
        {
            //display error message to user
            showError("You don\'t have any more of those left!");
        }

    }

    private void DeleteOtherClone()
    {
        int childCount = gameObject.transform.childCount;
        for(int i=childCount-1; i>=0;i--)
        {
            if(gameObject.transform.GetChild(i).name.Equals(gameObject.name))
            {
                Destroy(gameObject.transform.GetChild(i));
            }
            
        }
    }



    public void newActionBlockMaker(ref int cardCount)
    {
        
    }


    public void showError(string msg)
    {
        StartCoroutine(ErrorMsg(msg));

    }

    IEnumerator ErrorMsg(string msg)
    {

        txtErrorMsg.enabled = true;
        txtErrorMsg.text = msg;
        yield return new WaitForSeconds(2.0f);
        txtErrorMsg.enabled = false;
    }

    // Use this for initialization
    void Start () {
		canCreate = true;
        txtErrorMsg = GameObject.Find("ErrorMsg").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
