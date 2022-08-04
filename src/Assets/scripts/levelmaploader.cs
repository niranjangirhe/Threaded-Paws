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
    private int NoOfTutLevel;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;
    private GameObject btn;
    [TextArea(5, 20)] [SerializeField] private List<string> TutDescription = new List<string>();
    [TextArea(5, 20)] [SerializeField] private List<string> LevelDescription = new List<string>();
    [SerializeField] private Text description;

    void Start()
    {
        NoOfLevel = LevelDescription.Count;
        NoOfTutLevel = TutDescription.Count;
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
        StartCoroutine(LoadDiscription());
    }
    IEnumerator LoadDiscription()
    {
        yield return new WaitForSeconds(0.25f);

        Debug.Log("Hi");
        if (GameObject.Find("DropAreaThread").transform.childCount > 0)
        {
            Debug.Log("Back");
            string name = GameObject.Find("DropAreaThread").transform.GetChild(0).name;
            description.text = name[0] == 'T' ? TutDescription[Int32.Parse(Regex.Match(name, @"\d+").Value)-1] : LevelDescription[Int32.Parse(Regex.Match(name, @"\d+").Value)-1];
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
