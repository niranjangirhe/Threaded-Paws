using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelmaploader : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool AllLevelUnlock = true;
    private int NoOfLevel;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;
    private GameObject btn;
    [TextArea(5, 20)] [SerializeField] private string TutDescription;
    [TextArea(5, 20)] [SerializeField] private List<string> LevelDescription = new List<string>();
    [SerializeField] private Text description;

    void Start()
    {
        NoOfLevel = LevelDescription.Count;
        int levelCount = 0;
        try
        {
            levelCount = PlayerPrefs.GetInt("Won");
        }
        catch { }
        

        GameObject tutorialTab = Instantiate(prefab);
        tutorialTab.transform.SetParent(parent, false);
        tutorialTab.name = "Tut 1";
        tutorialTab.GetComponent<Image>().color = new Color32(105, 191, 255,255);
        tutorialTab.transform.Find("ActionText").GetComponent<Text>().text = "Tutorial";
           
       
        for (int i = 0; i < NoOfLevel; i++)
        {
           
            GameObject levelTab = Instantiate(prefab);
            levelTab.transform.SetParent(parent, false);
            levelTab.name = "Level " + (i+1);
            levelTab.transform.Find("ActionText").GetComponent<Text>().text = "Level " + (i + 1);
        }
        StartCoroutine(LoadDiscription());
    }
    IEnumerator LoadDiscription()
    {
        yield return new WaitForSeconds(0.25f);

        
        if (GameObject.Find("DropAreaThread").transform.childCount > 0)
        {
            
            string name = GameObject.Find("DropAreaThread").transform.GetChild(0).name;
            description.text = name[0] == 'T' ? TutDescription : LevelDescription[Int32.Parse(Regex.Match(name, @"\d+").Value)-1];
        }
        StartCoroutine(LoadDiscription());
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
