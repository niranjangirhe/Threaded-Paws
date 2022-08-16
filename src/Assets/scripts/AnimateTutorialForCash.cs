using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimateTutorialForCash : MonoBehaviour
{
    AudioSource bgSong;

    // Start is called before the first frame update
    private void Start()
    {
        bgSong = GameObject.Find("Logging").GetComponent<AudioSource>();
    }

    private void Update()
    {
        ExecuteThreadsLevel.ReduceSound(bgSong, 0.1f, 0.5f);
        //check if animation is over
        if (this.transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >1.0f)
        {
            SceneManager.LoadScene("Tut 4 extended");
        }
    }
}
  
   