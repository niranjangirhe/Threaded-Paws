using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

[System.Serializable]
public class ToolBoxValues
{
	public int CheckInBox;
	public int CutBox;
	public int DryBox;
	public int WashBox;
	public int ResourceBox;
	public int CheckOutBox;
	public int ReturnBox;
	public int GroomBox;

	public Text txt_checkinLeft_thread;
	public Text txt_cutLeft_thread;
	public Text txt_dryLeft_thread;
	public Text txt_washLeft_thread;
	public Text txt_resourcesLeft_thread;
	public Text txt_checkoutLeft_thread;
	public Text txt_returnLeft_thread;
	public Text txt_groomLeft_thread;
	
	public void updateValues()
	{

		txt_checkinLeft_thread.text = "x " + CheckInBox;
		txt_cutLeft_thread.text = "x " + CutBox;
		txt_washLeft_thread.text = "x " + WashBox;
		txt_dryLeft_thread.text = "x " + DryBox;
		txt_resourcesLeft_thread.text = "x " + ResourceBox;
		txt_checkoutLeft_thread.text = "x " + CheckOutBox;
		txt_returnLeft_thread.text = "x " + ReturnBox;
		txt_groomLeft_thread.text = "x " + GroomBox;
	}
}