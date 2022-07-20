using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelmaploader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject levelTabPrefab = Resources.Load<GameObject>("prefabs/LevelTile");
        Transform parent = GameObject.Find("Parent").transform;
        int levelCount = 0;
        try
        {
            levelCount = PlayerPrefs.GetInt("Won");
        }
        catch { }
        for (int i = 0; i <= levelCount; i++)
        {
            GameObject levelTab = Instantiate(levelTabPrefab);
            levelTab.transform.SetParent(parent, false);
            levelTab.name = "Level" + (i+1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
