using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimateTutorialForCash : MonoBehaviour
{


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void Update()
    {
        //check if animation is over
        if (this.transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >1.0f)
        {
            SceneManager.LoadScene("Tut 4 extended");
        }
    }
}
  
   