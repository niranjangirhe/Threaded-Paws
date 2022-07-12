using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMeIfPointerOut : MonoBehaviour
{
    // Start is called before the first frame update
    public void DeleteMe()
    {
        if(ToolBox.onToolBox)
            Destroy(gameObject);
    }
}
