using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Action
{
    public bool isneeded;
    [HideInInspector] public string name;
    [HideInInspector] public List<string> requirements = new List<string>();

    public Action(string name, List<string> requirements)
    {
        this.name = name;
        this.requirements = requirements;   
    }
}
