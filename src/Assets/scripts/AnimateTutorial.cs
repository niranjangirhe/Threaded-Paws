using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimateTutorial : MonoBehaviour
{
    [SerializeField] private Animator exeAnimator;
    [SerializeField] private Animator inAnimator;
    [SerializeField] private AnimationClip exeAnim;
    [SerializeField] private AnimationClip inAnim;
    [SerializeField] private GameObject exeObj;
    [SerializeField] private GameObject inObj;
    AudioSource bgSong;
    LogManager lm;

    // Start is called before the first frame update
    void Start()
    {
        lm =  GameObject.Find("Logging").GetComponent<LogManager>();
        lm.tempUnlocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(AnimateS1(0f));
        bgSong = GameObject.Find("Logging").GetComponent<AudioSource>();
    }

    private void Update()
    {
       ExecuteThreadsLevel.ReduceSound(bgSong, 0.1f, 0.5f);
        if (lm.tempUnlocked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    IEnumerator AnimateS1(float sec)
    {
        yield return new WaitForSeconds(sec);
        exeObj.SetActive(true);
        inObj.SetActive(false);
        exeAnimator.Play("ForeGroundMover");
        StartCoroutine(AnimateS2(exeAnim.length));
        //StartCoroutine(LoadScene(exeAnim.length));
    }
    IEnumerator AnimateS2(float sec)
    {
        yield return new WaitForSeconds(sec);
        inObj.SetActive(true);
        exeObj.SetActive(false);
        inAnimator.Play("Interior");
        StartCoroutine(LoadScene(inAnim.length));
    }
    IEnumerator LoadScene(float sec)
    {
        yield return new WaitForSeconds(sec);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Tut 1");
    }
}
