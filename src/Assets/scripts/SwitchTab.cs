using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchTab : MonoBehaviour
{

    public int index;
    public int totalCount;
    void Start()
    {

    }
    public void SwitchTabBtn()
    {

        //To Switch Tab
        GameObject.Find("Tab" + index).transform.SetAsLastSibling();
        for (int i = 0; i < totalCount; i++)
        {
            GameObject.Find("Label" + i).transform.GetChild(0).GetComponent<Image>().color = Color.white;
            try { 
                GameObject.Find("AgendaTick" + i).GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 1); 
            }
            catch 
            { }

        }
        GameObject.Find("Label" + index).transform.GetChild(0).GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 1);

        //To Switch Agenda Tab
        try
        {
            Transform agendaTick = GameObject.Find("AgendaTick" + index).transform;
            Transform agenda = GameObject.Find("Agenda" + index).transform;
            agenda.SetAsLastSibling();
            agendaTick.GetComponent<Image>().color = agendaTick.GetChild(0).GetComponent<Image>().color;
        }
        catch { }
       

    }
    // Update is called once per frame
    void Update()
    {

    }
}
