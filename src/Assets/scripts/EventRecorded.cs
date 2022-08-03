using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventRecorded : MonoBehaviour
{

    List<Thread> threads = new List<Thread>();
    ExecuteThreadsLevel exe;
    public int TutLevel;
    [SerializeField] private Animator animator;
    private GameObject agenda;
    private List<int> threadBlockCount = new List<int> { 0, 0 };
    // Start is called before the first frame update
    void Start()
    {
        exe = gameObject.GetComponent<ExecuteThreadsLevel>();
        threads = exe.threads;
        if(TutLevel<=3)
        {
            Transform tabParent = GameObject.Find("TabParent").transform;
            for(int i=0;i< tabParent.childCount;i++)
            {
                tabParent.GetChild(i).GetChild(0).GetComponent<ScrollRect>().vertical = false;
            }
        }
        switch(TutLevel)
        {
            case 1: animator.Play("Tut1S1"); StartCoroutine(Tut1n2Animtions(0)); break;
            case 2: animator.Play("Tut2S1"); StartCoroutine(Tut1n2Animtions(0)); break;
            case 3: animator.Play("Tut3S1"); StartCoroutine(Tut3Animations(0)); 
                agenda = GameObject.Find("Canvas").transform.Find("AgendaPanel").gameObject; break;
        }
        
        
    }

    // Runs after every 0.5 sec
    [Obsolete]
    IEnumerator Tut3Animations(int state)
    {
        
        try
        {
            Debug.Log("Checking");
            if (state == 0)
            {
                if (agenda.activeSelf)
                {
                    state = 1;
                    animator.Play("Tut3S2");
                }

            }
            else if (state == 1)
            {
                if (agenda.transform.GetChild(1).GetChild(0).name == "Agenda0")
                {
                    state = 2;
                    animator.Play("Tut3S3");
                }
                else if (!agenda.activeSelf)
                {
                    state = 0;
                    animator.Play("Tut3S1");
                }

            }
            else if (state == 2)
            {
                if (!agenda.activeSelf)
                {
                    state = 3;
                    animator.Play("Tut3S4");
                }
            }
            else if (state == 3)
            {
                if(agenda.transform.GetChild(1).GetChild(0).name == "Agenda0" && threadBlockCount[1] == 4)
                {
                    state = 3;
                    animator.Play("Tut3S4");
                }
                if(BlockAtPlace(1,3,"BrushBox") && threadBlockCount[1]==5)
                {
                    state = 4;
                    animator.Play("Tut3S5");
                }
                else if(agenda.transform.GetChild(1).GetChild(0).name != "Agenda0")
                {
                    animator.Play("Tut3S8");
                }
                else if(!BlockAtPlace(1, 3, "BrushBox") && threadBlockCount[1] == 5)
                {
                    animator.Play("Tut3S10");
                    Debug.Log("Bad bad");
                }
            }
            else if(state==4)
            {
                if (agenda.transform.GetChild(1).GetChild(0).name == "Agenda1")
                {
                    state = 5;
                    animator.Play("Tut3S9");
                }
            }
            else if (state == 5)
            {
                try
                {
                    if (GameObject.Find("InformationPanel").active)
                    {
                        state = 6;
                        animator.Play("Tut3S3");
                    }
                }
                catch {
                    animator.Play("Tut3S9");
                }
            }
            else if (state == 6)
            {
                try
                {
                    if(GameObject.Find("InformationPanel")==null)
                    {
                        if (threadBlockCount[0] == 3)
                        {
                            state = 7;
                            animator.Play("Tut2S1");
                        }
                        else if(BlockAtPlace(0,1,"ResourceBox"))
                        {
                            state = 8;
                            animator.Play("Tut3S6");
                        }
                        else
                        {
                            animator.Play("Tut3S10");
                            Debug.Log("Bad bad");
                        }
                    }
                }
                catch
                {
                    if (threadBlockCount[0] == 3)
                    {
                        state = 7;
                        animator.Play("Tut2S1");
                    }
                    else if (BlockAtPlace(0, 1, "ResourceBox"))
                    {
                        state = 8;
                        animator.Play("Tut3S6");
                    }
                    {
                        animator.Play("Tut3S10");
                        Debug.Log("Bad bad");
                    }
                }
            }
            else if (state == 7)
            {
                if (exe.GetActionBlocks(threads[0].tabDropArea)[1].name == "ResourceBox")
                {
                    state = 8;
                    animator.Play("Tut3S6");
                }
            }
            else if (state == 8)
            {
                try
                {
                    if (GameObject.Find("Blocker"))
                    {
                        state = 9;
                        animator.Play("Tut3S7");
                        
                    }
                }
                catch { }
            }
            else if (state == 9)
            {
                try
                {
                    string resource = exe.GetActionBlocks(threads[0].tabDropArea)[1].transform.Find("Dropdown").Find("Label").GetComponent<Text>().text;
                    if (resource == "dryer")
                    {
                        state = 10;
                        GameObject.Find("Animator").transform.Find("Image").gameObject.SetActive(false);
                    }
                    else if(resource!="[null]")
                    {
                        try
                        {
                            if (GameObject.Find("Blocker")==null)
                            {
                                state = 8;
                                animator.Play("Tut3S6");

                            }
                        }
                        catch {
                            state = 8;
                            animator.Play("Tut3S6");
                        }
                        
                    }
                }
                catch {
                    
                }
            }
            threadBlockCount = new List<int>();
            foreach (Thread t in threads)
            {
                int count = 0;
                foreach (Transform child in exe.GetActionBlocks(t.tabDropArea))
                {
                    count++;
                }
                threadBlockCount.Add(count);
            }
        }
        catch { }
        yield return new WaitForSeconds(0.5f);
        if (state==0)
            StartCoroutine(Tut3Animations(0));
        else if(state == 1)
            StartCoroutine(Tut3Animations(1));
        else if(state == 2)
            StartCoroutine(Tut3Animations(2));
        else if (state == 3)
            StartCoroutine(Tut3Animations(3));
        else if (state == 4)
            StartCoroutine(Tut3Animations(4));
        else if (state == 5)
            StartCoroutine(Tut3Animations(5));
        else if (state == 6)
            StartCoroutine(Tut3Animations(6));
        else if (state == 7)
            StartCoroutine(Tut3Animations(7));
        else if (state == 8)
            StartCoroutine(Tut3Animations(8));
        else if (state == 9)
            StartCoroutine(Tut3Animations(9));
    }
    bool BlockAtPlace(int thread, int pos, string name)
    {
        return exe.GetActionBlocks(threads[thread].tabDropArea)[pos].name == name;
    }
    // Runs after every 0.5 sec

    IEnumerator Tut1n2Animtions(int i)
    {
        if(i==0)
        {
            if (TutLevel == 1)
            {
                animator.Play("Tut1S1");
            }
        }
        if(i==1)
        {
            if (TutLevel == 1)
            {
                animator.Play("Tut1S2");
            }
            else
                animator.Play("Tut2S1");
        }
        if (i == 2)
        {
            if (TutLevel == 2)
            {
                try
                {
                    if (exe.GetActionBlocks(threads[0].tabDropArea)[1].name == "CheckOutBox")
                        animator.Play("Tut1S2");
                    else
                        animator.Play("Tut2S2");
                }
                catch
                {

                }
            }
        }
        int count = 0;
        foreach (Thread t in threads)
        {
            foreach (Transform child in exe.GetActionBlocks(t.tabDropArea))
            {
                count++;
            }
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Run");
        StartCoroutine(Tut1n2Animtions(count));
    }
}
