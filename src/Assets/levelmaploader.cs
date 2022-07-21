using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelmaploader : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool AllLevelUnlock = true;
    [SerializeField] private int NoOfLevel;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;


    void Start()
    {
        
        int levelCount = 0;
        try
        {
            levelCount = PlayerPrefs.GetInt("Won");
        }
        catch { }
        for (int i = 0; i <= NoOfLevel; i++)
        {
           
            GameObject levelTab = Instantiate(prefab);
            levelTab.transform.SetParent(parent, false);
            levelTab.name = "Level" + (i+1);
            levelTab.transform.Find("ActionText").GetComponent<Text>().text = "Level" + (i + 1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
