using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventRecorded : MonoBehaviour
{

    List<Thread> threads = new List<Thread>();
    ExecuteThreadsLevel exe;
    [SerializeField] private int TutLevel;
    [SerializeField] private Animator animator;
    private GameObject agenda;

    // Start is called before the first frame update
    void Start()
    {
        exe = gameObject.GetComponent<ExecuteThreadsLevel>();
        threads = exe.threads;
        switch(TutLevel)
        {
            case 1: animator.Play("Tut1S1"); StartCoroutine(Tut1n2Animtions(0)); break;
            case 2: animator.Play("Tut2S1"); StartCoroutine(Tut1n2Animtions(0)); break;
            case 3: animator.Play("Tut3S1"); StartCoroutine(Tut3Animations(0)); 
                agenda = GameObject.Find("Canvas").transform.Find("AgendaPanel").gameObject; break;
        }
        
        
    }

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
    }

    // Update is called once per frame

    IEnumerator Tut1n2Animtions(int i)
    {
        if(i==1)
        {
            if(TutLevel==1)
            {
                animator.Play("Tut1S2");
            }
        }
        if (i == 2)
        {
            if (TutLevel == 2)
            {
                if(exe.GetActionBlocks(threads[0].tabDropArea)[1].name=="CheckOutBox")
                    animator.Play("Tut1S2");
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
        StartCoroutine(Tut1n2Animtions(count));
    }
}
