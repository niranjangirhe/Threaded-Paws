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
    
    public Action wash = new Action("Wash", new List<string> { "sponge", "shampoo","cond." });
    public Action cut = new Action("Cut", new List<string> { "scissors", "brush", "clippers" });
    public Action brush = new Action("Brush", new List<string> { "spray", "brush" });
    public Action dry = new Action("Dry", new List<string> { "towel", "dryer" });
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
	public int BrushBox;
}
[System.Serializable]
public enum ActionEnum
{
	Cut,
	Wash,
	Brush,
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
	clippers,
	dryer,
    sponge,
    spray
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
    //ThreadData
    public WorkList workList = new WorkList();
    public ToolBoxValues toolBoxValues = new ToolBoxValues();


    //Tab (Grandparent of blocks)
    [HideInInspector] public GameObject layoutPanel;
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