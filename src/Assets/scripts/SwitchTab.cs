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
        GameObject.Find("Tab"+index).transform.SetAsLastSibling();
        for(int i=0;i<totalCount;i++)
        {
            GameObject.Find("Label"+i).transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }
        this.GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 1);
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
