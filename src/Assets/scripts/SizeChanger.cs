using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeChanger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject DropAreaTool;
    [SerializeField]
    private GameObject ToolValueParent;

    void Start()
    {
        if (!GameObject.Find("_SCRIPTS_").GetComponent<ExecuteThreadsLevel>().IsDataRace())
        {
            Destroy(GameObject.Find("ReadBox"));
            Destroy(GameObject.Find("CalculateBox"));
            Destroy(GameObject.Find("WriteBox"));
            Destroy(GameObject.Find("ReadLeft1"));
            Destroy(GameObject.Find("CalculateLeft1"));
            Destroy(GameObject.Find("WriteLeft1"));
            Destroy(GameObject.Find("CashParent"));
        }
        Invoke("LateStart", 1);
    }
    void LateStart()
    {
        
        //to change size of scrollbox and toolvalueparent to dropareatool
        float masterRectHeight = DropAreaTool.GetComponent<RectTransform>().rect.height;
        RectTransform scrollBoxRect = gameObject.GetComponent<RectTransform>();
        RectTransform toolValueRect = ToolValueParent.GetComponent<RectTransform>();

        //assign
        scrollBoxRect.sizeDelta = new Vector2(scrollBoxRect.sizeDelta.x, masterRectHeight);
        toolValueRect.sizeDelta = new Vector2(toolValueRect.sizeDelta.x, masterRectHeight);
    }
}
