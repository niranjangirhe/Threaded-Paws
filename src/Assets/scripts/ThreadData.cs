using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *This file store all the related information of the work that barber can do
 *Like the task/work that barber has to do. 
 */


//stores the bool for all works
[System.Serializable]
public class WorkList
{
    public Action Dry = new Action("Dry",new List<string> { "station","towel","dryer"});
    public Action Wash = new Action("Wash", new List<string> { "station", "towel","shampoo","cond." });
    public Action Cut = new Action("Cut", new List<string> { "scissors", "brush" });
    public Action Groom = new Action("Groom", new List<string> { "clippers", "brush" });
}

public class SimBlock
{
    public static byte CHECKIN = 1;
    public static byte CHECKOUT = 2;
    public static byte ACQUIIRE = 3;
    public static byte RETURN = 4;
    public static byte WORK = 5;
    public byte type;
    public string name;

    public SimBlock(byte type, string name)
    {
        this.type = type;
        this.name = name;
    }
}