using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchTab : MonoBehaviour
{

    public int index;
    public int totalCount;
    private AudioSource audioSource;
    private AudioClip audioClip;
    void Start()
    {
        audioClip = Resources.Load<AudioClip>("audio/selectTab");
        audioSource = GameObject.Find("_SCRIPTS_").GetComponent<AudioSource>();

    }
    public void SwitchTabBtn()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        //update toolbox value (UI)
        GameObject.Find("_SCRIPTS_").GetComponent<ExecuteThreadsLevel3_5>().updateValues(index);
        //To Switch Tab
        GameObject.Find("Tab" + index).transform.SetAsLastSibling();
        for (int i = 0; i < totalCount; i++)
        {
            GameObject.Find("Label" + i).transform.GetChild(0).GetComponent<Image>().color = Color.white;
            GameObject.Find("Label" + i).transform.GetChild(0).Find("Text (TMP)").gameObject.SetActive(false);
            GameObject.Find("Label" + i).GetComponent<LayoutElement>().preferredWidth = 0;
            try { 
                GameObject.Find("Canvas").transform.Find("AgendaPanel").Find("AgendaTickParent").Find("AgendaTick" + i).GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 0); 
            }
            catch
            {
                Debug.Log("Idiot");
            }

        }
        GameObject.Find("Label" + index).transform.GetChild(0).GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 1);
        GameObject.Find("Label" + index).transform.GetChild(0).Find("Text (TMP)").gameObject.SetActive(true);
        GameObject.Find("Label" + index).GetComponent<LayoutElement>().preferredWidth = 1000;

        //To Switch Agenda Tab
        try
        {
            Transform agendaTick = GameObject.Find("Canvas").transform.Find("AgendaPanel").Find("AgendaTickParent").Find("AgendaTick" + index).transform;
            Transform agenda = GameObject.Find("Canvas").transform.Find("AgendaPanel").Find("AgendaParent").Find("Agenda" + index).transform;
            agenda.SetAsLastSibling();
            agendaTick.GetComponent<Image>().color = agendaTick.GetChild(0).GetComponent<Image>().color;
        }
        catch 
        {
            Debug.Log("Riddhi");
        }
       

    }

}
