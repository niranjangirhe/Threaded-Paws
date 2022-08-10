using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewBlockForLevel : MonoBehaviour
{
    [SerializeField] private GameObject clone;
    [TextArea(5, 20)][SerializeField] public List<string> tutdesc = new List<string>();
    [TextArea(5, 20)][SerializeField] public List<string> lvldesc = new List<string>();
    public string nameOfLevel;
    private int levelnum;
    private string description;
    private Text DescriptionBox;

    private levelmaploader lvlmap;

    private void Start()
    {
        lvlmap = (levelmaploader) GameObject.Find("_SCRIPTS_").GetComponent(typeof(levelmaploader));
        tutdesc = GameObject.Find("_SCRIPTS_").GetComponent<levelmaploader>().TutDescription;
        lvldesc = GameObject.Find("_SCRIPTS_").GetComponent<levelmaploader>().LevelDescription;
        DescriptionBox = GameObject.Find("Description").GetComponent<Text>();
    }
    public void OnMouseEnter()
    {
        DeleteAllClone();
        MakeMyClone();
        if (!lvlmap.filled)
        {
            DisplayDescription(gameObject.name);

        }
    }
    void DisplayDescription(string nameOfLevel)
    {
        //Debug.Log(tutdesc.Count);
        Debug.Log(nameOfLevel[nameOfLevel.Length - 1]);
        levelnum = (int)char.GetNumericValue(nameOfLevel[nameOfLevel.Length - 1]) - 1;
        
        if (nameOfLevel[0] == 'L')
        {
            description = lvldesc[levelnum];
        }
        else
        {
            description = tutdesc[levelnum];
        }

        DescriptionBox.text = description;

    }
    void DeleteAllClone()
    {
        int parentChildCount = gameObject.transform.parent.childCount;
        for (int i = parentChildCount - 1; i >= 0; i--)
        {
            int childCount = gameObject.transform.parent.GetChild(i).childCount;
            for (int j = childCount - 1; j >= 0; j--)
            {
                string myName = gameObject.name;
                string parentName = gameObject.transform.parent.GetChild(i).name;
                string childName = gameObject.transform.parent.GetChild(i).GetChild(j).name;
                if (parentName.Equals(childName) && !childName.Equals(myName))
                {
                    Destroy(gameObject.transform.parent.GetChild(i).GetChild(j).gameObject);
                }

            }

        }
    }
    void MakeMyClone()
    {
        bool shouldCreate = true;
        int childCount = gameObject.transform.childCount;
        for(int i=0;i<childCount;i++)
        {
            if(gameObject.transform.GetChild(i).name.Equals(gameObject.name))
            {
                shouldCreate = false;
            }
        }
        if (shouldCreate)
        {
            GameObject Temp = Instantiate(clone, gameObject.transform);
            Temp.name = gameObject.name;
            Temp.transform.GetChild(0).GetComponent<Text>().text = gameObject.transform.GetChild(0).GetComponent<Text>().text;
            Temp.transform.SetParent(gameObject.transform);
        }
    }
}
