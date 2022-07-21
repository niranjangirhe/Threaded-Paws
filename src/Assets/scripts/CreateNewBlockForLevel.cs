using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewBlockForLevel : MonoBehaviour
{
    [SerializeField] private GameObject clone;
    public void OnMouseEnter()
    {
        DeleteAllClone();
        MakeMyClone();
    }
    void DeleteAllClone()
    {
        int parentChildCount = gameObject.transform.parent.childCount;
        for (int i = parentChildCount - 1; i >= 0; i--)
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
    void MakeMyClone()
    {
        GameObject Temp = Instantiate(clone,gameObject.transform);
        Temp.name = gameObject.name;
        Temp.transform.GetChild(0).GetComponent<Text>().text = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        Temp.transform.SetParent(gameObject.transform);
    }
}
