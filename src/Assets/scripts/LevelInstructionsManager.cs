using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelInstructionsManager : MonoBehaviour
{

    public GameObject level_part1;
    public GameObject level_part2;

    public GameObject level_disableFunctionality;

    public void level_getPanel1()
    {

        level_part1.SetActive(true);
        level_part2.SetActive(false);
    }

    public void level_getPanel2()
    {

        level_part2.SetActive(true);
        level_part1.SetActive(false);
    }

    public void level_activateAllFunctionality()
    {

        level_disableFunctionality.SetActive(false);
    }

    public void skipInstructions()
    {
        try
        {
            LogManager.instance.logger.sendChronologicalLogs("StartLevel"+gameObject.GetComponent<ExecuteThreadsLevel>().levelNo, "", LogManager.instance.UniEndTime().ToString());
        }
        catch
        {

        }
        level_part1.SetActive(false);
            level_part2.SetActive(false);

            level_activateAllFunctionality();
       
    }

    void Start()
    {
        if (gameObject.GetComponent<ExecuteThreadsLevel>().isTutorial)
            skipInstructions();
        else
        {
            level_part1.SetActive(true);
            level_part2.SetActive(false);
            level_disableFunctionality.SetActive(true);
        }

        
       
        //GameLogData.levelNo = 3;
        
    }
}