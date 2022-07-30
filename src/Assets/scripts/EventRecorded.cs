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
    
    // Start is called before the first frame update
    void Start()
    {
        exe = gameObject.GetComponent<ExecuteThreadsLevel>();
        threads = exe.threads;
        StartCoroutine(checkoneBlock(0));
    }

    // Update is called once per frame

    IEnumerator checkoneBlock(int i)
    {
        if(i==1)
        {
            Debug.Log("Inserted one Block");
            if(TutLevel==1)
            {
                animator.Play("Tut1S2");
            }
        }
        if (i == 2)
        {
            Debug.Log("Inserted one Block");
            if (TutLevel == 2)
            {
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
        yield return new WaitForSeconds(1);
        StartCoroutine(checkoneBlock(count));
    }

}
