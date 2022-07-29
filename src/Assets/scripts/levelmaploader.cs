using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelmaploader : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool AllLevelUnlock = true;
    [SerializeField] private int NoOfLevel;
    [SerializeField] private int NoOfTutLevel;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;
    private GameObject btn;

    void Start()
    {
        
        int levelCount = 0;
        try
        {
            levelCount = PlayerPrefs.GetInt("Won");
        }
        catch { }
        for (int i = 0; i < NoOfTutLevel; i++)
        {

            GameObject levelTab = Instantiate(prefab);
            levelTab.transform.SetParent(parent, false);
            levelTab.name = "Tut " + (i + 1);
            levelTab.GetComponent<Image>().color = new Color32(105, 191, 255,255);
            levelTab.transform.Find("ActionText").GetComponent<Text>().text = "Tutorial " + (i + 1);
           
        }
        for (int i = 0; i < NoOfLevel; i++)
        {
           
            GameObject levelTab = Instantiate(prefab);
            levelTab.transform.SetParent(parent, false);
            levelTab.name = "Level " + (i+1);
            levelTab.transform.Find("ActionText").GetComponent<Text>().text = "Level " + (i + 1);
        }

    }

    // Update is called once per frame
    public void startLevel()
    {
        SceneManager.LoadScene(GameObject.Find("DropAreaThread").transform.GetChild(0).name); 
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
