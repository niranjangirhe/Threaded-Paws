using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTutorial : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animate("ForeGroundMover", 2f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Animate(string name, float sec)
    {
        yield return new WaitForSeconds(sec);
        animator.Play(name);
    }
}
