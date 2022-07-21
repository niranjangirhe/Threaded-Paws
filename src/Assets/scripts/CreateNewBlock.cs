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
    public GameObject errorMsgHolder;
    public Text txtErrorMsg;
    ToolBoxValues manager;

    private AudioSource audioSource;
    private AudioClip audioClip;


    public void NewActionBlock()
    {

        //active tab

        int activeTab = Int32.Parse(Regex.Match(GameObject.Find("TabParent").transform.GetChild(GameObject.Find("TabParent").transform.childCount - 1).gameObject.name, @"\d+").Value);
        manager = GameObject.Find("_SCRIPTS_").GetComponent<ExecuteThreadsLevel>().threads[activeTab].toolBoxValues;
        int cardCount = -100;
        try
        {
            cardCount = (int)manager.GetType().GetField(this.transform.name).GetValue(manager);
        }
        catch { }
        if (cardCount > 0 || cardCount==-100)
        {


            DeleteOtherClone();
            GameObject newActionBox = (GameObject)Instantiate(prefab, transform.position, transform.rotation); //typically returns an Object (not GameObject)
            newActionBox.name = this.transform.name;
            newActionBox.transform.SetParent(this.transform);
            newActionBox.transform.localScale = Vector3.one;
            newActionBox.transform.GetChild(0).GetComponentInChildren<Text>().text = this.GetComponentInChildren<Text>().text;
            newActionBox.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        }
        

    }
    public void ErrorSound()
    {
        if(gameObject.GetComponent<CanvasGroup>().alpha<1)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            showError("You don\'t have any more of those left!");
        }

    }
    private void DeleteOtherClone()
    {
        int parentChildCount = gameObject.transform.parent.childCount;
        for(int i = parentChildCount - 1; i>=0;i--)
        {
            int childCount = gameObject.transform.parent.GetChild(i).childCount;
            for (int j = childCount - 1; j >= 0; j--)
            {
                if (gameObject.transform.parent.GetChild(i).GetChild(j).name.Equals(gameObject.transform.parent.GetChild(i).name))
                {
                    Destroy(gameObject.transform.parent.GetChild(i).GetChild(j).gameObject);
                }

            }

        }
    }


    public void showError(string msg)
    {
        errorMsgHolder.SetActive(true);
        txtErrorMsg.enabled = true;
        txtErrorMsg.text = msg;
    }

   

    // Use this for initialization
    void Start () {
        
        audioClip = Resources.Load<AudioClip>("audio/error");
        audioSource = GameObject.Find("_SCRIPTS_").GetComponent<AudioSource>();
    }


}
