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
        if (this.transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >1.0f)
        {
           
            
            SceneManager.LoadScene("Level 5");
        }
    }
}
  
   