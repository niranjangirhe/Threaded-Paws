using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Action
{
    public bool isneeded;
    [HideInInspector] public string name;
    private float cost;
    private List<string> requirements = new List<string>();

    public Action(string name, List<string> requirements, float cost)
    {
        this.name = name;
        this.requirements = requirements;
        this.cost = cost;
    }
    public List<string> GetRequirementList()
    {
        return requirements;
    }
    public float GetCost()
    {
        return cost;
    }
}
