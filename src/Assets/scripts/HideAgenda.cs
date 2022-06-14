using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAgenda : MonoBehaviour
{
   public void OnClick()
    {
        Debug.Log("Hidden");
        gameObject.SetActive(false);
    }
}
