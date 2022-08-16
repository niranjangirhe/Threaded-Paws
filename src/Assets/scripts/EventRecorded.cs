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
    private int blocks;
    private int blocksInThread;
    [SerializeField] private Animator animator;
    private GameObject agenda;
    private List<int> threadBlockCount = new List<int> { 0, 0 };
    private AudioSource[] allAudioSources;
    private CanvasGroup canvasGroup;
    private LogManager logManager;
    private Text returnText;
    private bool animFinish = false;

    // Start is called before the first frame update

    [Obsolete]
    private void Start()
    {
        logManager = GameObject.Find("Logging").GetComponent<LogManager>();
        canvasGroup = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
        returnText = GameObject.Find("ToolValueParent").transform.Find("ReturnLeft1").GetComponent<Text>();
        exe = gameObject.GetComponent<ExecuteThreadsLevel>();
        threads = exe.threads;
        blocksInThread = GameObject.Find("DropAreaThread").transform.childCount;
     
        if (TutLevel<=2)
        {
            Transform tabParent = GameObject.Find("TabParent").transform;
            for(int i=0;i< tabParent.childCount;i++)
            {
                tabParent.GetChild(i).GetChild(0).GetComponent<ScrollRect>().vertical = false;
            }
        }
        switch(TutLevel)
        {
            case 1: animator.Play("Tut1S1"); StartCoroutine(Tut1Animtions(0)); break;
            case 2: animator.Play("Tut3S1"); StartCoroutine(Tut2Animations(0)); 
                agenda = GameObject.Find("Canvas").transform.Find("AgendaPanel").gameObject; break;
            case 3:
                animator.Play("Tut4S1"); StartCoroutine(Tut3Animations(0));
                break;
            case 4:
                animator.Play("Tut5S0");
                //yield return new WaitForSeconds(7f);
                 StartCoroutine(Tut4Animations(-1));
                break;
        }

       
    }
    IEnumerator Tut4Animations(int state)
    {
        LockCursor();

        if (state == -1)
        {
            LockCursor();
            try
            {
                if (GameObject.Find("Animator").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 10)
                {

                    LockCursor();
                    animator.Play("Tut5S1");


                    state = 0;

                }
            }
            catch
            {

                state = 0;
            }


        }
        if (state == 0)
        {
            try
            {
                
                if (GameObject.Find("ToolValueParent").transform.Find("ReadLeft1").GetComponent<Text>().text != "x 1" && BlockAtPlace(0, 5, "ReadBox"))
                {
                    animator.Play("Tut5S2");
                    Debug.Log("State 1");
                    state = 1;
                    
                    //GameObject.Find("Animator").transform.Find("Image").gameObject.SetActive(false);
                }
                else if (!BlockAtPlace(0, 5, "ReadBox") && threadBlockCount[0] == 7)
                {
                    animator.Play("Tut5S5");
                    Debug.Log("Bad bad");
                }
            }
            catch
            {
                state = 1;
            }
        }
        if (state == 1)
        {
            try
            {

                if (GameObject.Find("ToolValueParent").transform.Find("CalculateLeft1").GetComponent<Text>().text != "x 1" && BlockAtPlace(0,6, "CalculateBox"))
                {
                    animator.Play("Tut5S3");
                    Debug.Log("State 2");
                    state = 2;

                    //GameObject.Find("Animator").transform.Find("Image").gameObject.SetActive(false);
                }
                else if (!BlockAtPlace(0, 6, "CalculateBox") && threadBlockCount[0] == 8)
                {
                    animator.Play("Tut5S5");
                    Debug.Log("Bad bad");
                }
            }
            catch
            {
                state = 2;
            }
        }
        if (state == 2)
        {
            try
            {



                if (GameObject.Find("ToolValueParent").transform.Find("WriteLeft1").GetComponent<Text>().text != "x 1" && BlockAtPlace(0, 7, "WriteBox") && !animFinish)
                {
                    animator.Play("Tut5S4");
                    animFinish = true;
                    Debug.Log("State 3");
                    state = 3;

                    //GameObject.Find("Animator").transform.Find("Image").gameObject.SetActive(false);
                }
                else if (!BlockAtPlace(0, 7, "WriteBox") && threadBlockCount[0] == 9)
                {
                    animator.Play("Tut5S5");
                    Debug.Log("Bad bad");
                    
                }
            }
            catch
            {
                state = 3;
            }
        }
        if (state == 3)
        {
            try
            {
                
                if (GameObject.Find("ThreadSimPanel(Clone)").transform.childCount != 0)
                {
                    
                    GameObject.Find("Animator").transform.Find("Image").gameObject.SetActive(false);
                    
                }
            }
            catch
            {
                
            }
        }
        yield return new WaitForSeconds(0.5f);
        if (state == -1)
            StartCoroutine(Tut4Animations(-1));
        if (state == 0)
            StartCoroutine(Tut4Animations(0));
        if (state == 1)
            StartCoroutine(Tut4Animations(1));
        if (state == 2)
            StartCoroutine(Tut4Animations(2));
        if (state == 3)
            StartCoroutine(Tut4Animations(3));

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
    IEnumerator Tut3Animations(int state)
    {
        LockCursor();

        if (state == 0)
        {
            LockCursor();
            try
            {
                if(GameObject.Find("Animator").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >10)
                {
                    
                    LockCursor();
                    animator.Play("Tut4S2");
                    
                    
                    state = 1;

                }
            }
            catch
            {
                
                state = 1;
            }
            

        }
        if (state == 1)
        {
            try
            {
                
                //blocksInThread = GameObject.Find("DropAreaThread").transform.childCount;
                
                if (BlockAtPlace(0, 4, "ReturnBox") && BlockAtPlace(0, 5, "ReturnBox"))
                {
                    animator.Play("Tut4S3");
                    state = 2;
                    
                    //GameObject.Find("Animator").transform.Find("Image").gameObject.SetActive(false);
                }
                else if (GameObject.Find("DropAreaThread").transform.childCount > 6 && (!BlockAtPlace(0, 4, "ReturnBox") || !BlockAtPlace(0, 5, "ReturnBox")))
                {
                    //error in placement
                    animator.Play("Tut4S4");
                }
                
            }
            catch
            {
                state = 2;
            }
        }
        if (state == 2)
        {
            
            try
            {
                
                if (GameObject.Find("ThreadSimPanel(Clone)").transform.childCount != 0)
                {
                    //state = 3;
                    //animator.Play("Tut4S3");
                    GameObject.Find("Animator").transform.Find("Image").gameObject.SetActive(false);
                }
            }
            catch
            {
                //state = 3;
            }
        }
        yield return new WaitForSeconds(0.5f);
        if (state == 0)
            StartCoroutine(Tut3Animations(0));
        if (state == 1)
            StartCoroutine(Tut3Animations(1));
        if (state == 2)
            StartCoroutine(Tut3Animations(2));
    }

    // Runs after every 0.5 sec


    IEnumerator Tut2Animations(int state)
    {
        LockCursor();
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
                    if (GameObject.Find("InformationPanel").activeSelf)
                    {
                        state = 6;
                        animator.Play("Tut3S13");
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
                            animator.Play("Tut3S11");
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
                        animator.Play("Tut3S11");
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
                        animator.Play("Tut3S12");
                        GameObject.Find("Animator").transform.Find("Image").gameObject.SetActive(false);
                    }
                    else if(resource!= "???")
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
            StartCoroutine(Tut2Animations(0));
        else if(state == 1)
            StartCoroutine(Tut2Animations(1));
        else if(state == 2)
            StartCoroutine(Tut2Animations(2));
        else if (state == 3)
            StartCoroutine(Tut2Animations(3));
        else if (state == 4)
            StartCoroutine(Tut2Animations(4));
        else if (state == 5)
            StartCoroutine(Tut2Animations(5));
        else if (state == 6)
            StartCoroutine(Tut2Animations(6));
        else if (state == 7)
            StartCoroutine(Tut2Animations(7));
        else if (state == 8)
            StartCoroutine(Tut2Animations(8));
        else if (state == 9)
            StartCoroutine(Tut2Animations(9));
    }
    bool BlockAtPlace(int thread, int pos, string name)
    {
        return exe.GetActionBlocks(threads[thread].tabDropArea)[pos].name == name;
    }
    // Runs after every 0.5 sec

    IEnumerator Tut1Animtions(int i)
    {
        LockCursor();
            if (i == 0)
            {
                animator.Play("Tut1S1");
            }
            if (i == 1)
            {
                try
                {
                    if (exe.GetActionBlocks(threads[0].tabDropArea)[0].name == "CheckInBox")
                        animator.Play("Tut2S1");
                    else
                        animator.Play("Tut1S1");
                }
                catch { }
            }
            if (i == 2)
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
        
        int count = 0;
        foreach (Thread t in threads)
        {
            foreach (Transform child in exe.GetActionBlocks(t.tabDropArea))
            {
                count++;
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Tut1Animtions(count));
    }
    void LockCursor()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        bool isPlaying=false;
        foreach (AudioSource audioS in allAudioSources)
        {
            
            if (audioS.isActiveAndEnabled && audioS.isPlaying && audioS.transform.name.Contains("Script"))
            {
                isPlaying = true;
                break;
            }

        }
        if (isPlaying)
        {
            //Cursor.lockState = CursorLockMode.Locked;
            toggleClick(false);
        }
        else
        {
            //Cursor.lockState = CursorLockMode.None;
            toggleClick(true);
        }
    }
    void toggleClick(bool b)
    {
        logManager.cursorLocked = !b;
        canvasGroup.interactable = b;
        canvasGroup.blocksRaycasts = b;
    }
}
