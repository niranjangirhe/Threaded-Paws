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
[System.Serializable]
public enum ActionEnum
{
	Cut,
	Wash,
	Groom,
	Dry,
	CheckIn,
	CheckOut,
	Return,
	Get
}
[System.Serializable]
public enum ItemsEnum
{
	Null,
	brush,
	scissors,
	towel,
	shampoo,
	conditioner,
	station,
	clippers,
	dryer
}
[System.Serializable]
public class BlockInfo
{
	public ActionEnum actionEnum;
	[Tooltip("Select only for Return and Get")]
	public ItemsEnum itemsEnum;
}

[System.Serializable]
public class Thread
{
    //This is list of ticks(UI) which can be set on/off
    public List<GameObject> ticks;
    private List<GameObject> innerTicks;

    //ThreadData
    public WorkList workList = new WorkList();
    public ToolBoxValues toolBoxValues = new ToolBoxValues();


    //Tab (Grandparent of blocks)
    public GameObject layoutPanel;
    [HideInInspector] public GameObject tabDropArea;

    //Thread personal data
    [HideInInspector] public string workerName;
    [HideInInspector] public string dogName;

    //sprites Dog and worker
    [HideInInspector] public Sprite dogSprite;
    [HideInInspector] public Sprite workerSprite;

    //stores the bool for all works and action
    [HideInInspector] public bool isCheckedIn;
    [HideInInspector] public bool isCheckedOut;
    [HideInInspector] public Dictionary<string, bool> hasItems = new Dictionary<string, bool>();
    [HideInInspector] public Dictionary<string, bool> needsTo = new Dictionary<string, bool>();
    [HideInInspector] public Dictionary<string, bool> did = new Dictionary<string, bool>();

    //variables to store temp data
    [HideInInspector] public int currIndex;
    [HideInInspector] public bool canPrint;
    [HideInInspector] public Transform[] blocks;
    [HideInInspector] public List<SimBlock> simBlocks;
    [HideInInspector] public List<GameObject> simulationImages;

    // ------ for prefill -----


    public List<BlockInfo> blockInfo;
}