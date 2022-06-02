using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;

public class ExecuteThreadsLevel3_5 : MonoBehaviour
{


    //----- Niranjan Variables ------

    [System.Serializable]
    public class Thread
    {
        //This is list of ticks(UI) which can be set on/off
        public List<GameObject> ticks;
        public List<GameObject> innerTicks;

        //ThreadData (Niranjan's Datastructure (New oops))
        public WorkList workList = new WorkList();
        public List<SimBlock> simBlocks = new List<SimBlock>();

        //to get block from FE
        [HideInInspector] public Transform[] blocks;

        //Tab (Grandparent of blocks)
        public GameObject tab;

        //stores the bool for all works
        [HideInInspector] public bool isCheckedIn;
        [HideInInspector] public bool isCheckedOut;
        [HideInInspector] public Dictionary<string, bool> hasItems = new Dictionary<string, bool>();
        [HideInInspector] public Dictionary<string, bool> needsTo = new Dictionary<string, bool>();
        [HideInInspector] public Dictionary<string, bool> did = new Dictionary<string, bool>();
    }


    public List<Thread> threads;
    private dropDownManager dropDownManager = new dropDownManager();








    // --- IMAGE SIMULATION ---

    public GameObject scrollRect;

    public GameObject simulationImagePrefab;
    public GameObject simulationErrorPrefab;
    public GameObject layoutPanel1;
    public GameObject layoutPanel2;
    public Text stepsIndicator;

    //Privatized (Dogs and workers will be randomly picked. Done to reduce script attachments)
    private Sprite dogSprite1;
    private Sprite dogSprite2;
    private Sprite workerSprite1;
    private Sprite workerSprite2;

    //Remove as it had no use
    //private Sprite displayErrorSprite;
    
    // GameObject contentContainer;

    // ------------------------

    ToolboxManager manager;
    GameObject disablePanel;
    ProgressBar bar;
    ScrollRect simulationScrollRect;

    public GameObject runButton;
    public GameObject stopButton;


    Transform[] blocks_t1;
    Transform[] blocks_t2;

    bool stop;
    bool err;
    bool paused;
    bool lost;


    bool t1_has_brush;
    bool t1_has_clippers;
    bool t1_has_conditioner;
    bool t1_has_dryer;
    bool t1_has_scissors;
    bool t1_has_shampoo;
    bool t1_has_station;
    bool t1_has_towel;


    bool t2_has_brush;
    bool t2_has_clippers;
    bool t2_has_conditioner;
    bool t2_has_dryer;
    bool t2_has_scissors;
    bool t2_has_shampoo;
    bool t2_has_station;
    bool t2_has_towel;


    bool t1_did_cut;
    bool t1_did_dry;
    bool t1_did_wash;
    bool t1_did_groom;
    
    bool t2_needs_cut;
    bool t2_needs_dry;
    bool t2_needs_wash;
    bool t2_needs_groom;
    bool t2_did_cut;
    bool t2_did_dry;
    bool t2_did_wash;
    bool t2_did_groom;
    bool t1_checkedin;
    bool t1_checkedout;
    bool t2_checkedin;
    bool t2_checkedout;

    string returnErrMsg = "> ERROR: You are trying to return a resource you don't have.";
    string acquireErrMsg = "> ERROR: You are trying to acquire a resource you already have.";

    void Start()
    {

        //Initilize a Random dogs from each halves;
        UnityEngine.Object[] s = Resources.LoadAll("sprites/dogs", typeof(Sprite));
        int len = s.Length;
        dogSprite1 = (Sprite)s[Random.Range(0, len / 2 - 1)];
        dogSprite2 = (Sprite)s[Random.Range(len / 2, len - 1)];

        //Initilize a Random worker from each halves;
        s = Resources.LoadAll("sprites/workers", typeof(Sprite));
        len = s.Length;
        workerSprite1 = (Sprite)s[Random.Range(0, len / 2 - 1)];
        workerSprite2 = (Sprite)s[Random.Range(len / 2, len - 1)];

        //Initilize display error image (no need if project doesn't have error)
        //displayErrorSprite = Resources.Load<Sprite>("sprites/error");

        //fill needsToDictionary from inspector.
        foreach (Thread t in threads)
        {
            System.Reflection.FieldInfo[] varWorklist = t.workList.GetType().GetFields();
            foreach (System.Reflection.FieldInfo v in varWorklist)
            {
                t.needsTo.Add(v.Name, (bool)v.GetValue(t.workList));
                t.did.Add(v.Name, false);
            }
            foreach(string key in dropDownManager.options)
            {
                t.hasItems.Add(key, false);
            }
        }




        stop = false;
        err = false;
        paused = false;
        lost = false;

        manager = GameObject.Find("_SCRIPTS_").GetComponent<ToolboxManager>();
        // timer = GameObject.FindObjectOfType<Timer> ();
        disablePanel = GameObject.Find("DisablePanel");
        bar = GameObject.Find("RadialProgressBar").GetComponent<ProgressBar>();
        try
        {
            simulationScrollRect = scrollRect.transform.GetComponent<ScrollRect>();
            // contentContainer = layoutPanel1.transform.parent.gameObject;
        }
        catch { }

        try
        {
            disablePanel.SetActive(false);
        }
        catch
        {
            Debug.Log("Disable Panel can't be found.");
        }
    }

    private Transform[] GetActionBlocks_MultiThreads(String tabNum)
    {

        //get children in drop area for thread

        string path = "";

        if (tabNum == "1")
            path = "Tab1/ScrollRect/Holder/DropAreaThread1";
        else
            path = "Tab2/ScrollRect/Holder/DropAreaThread2";

        Debug.Log("children (T" + tabNum + "): " + GameObject.Find(path).transform.childCount);
        int childCount = GameObject.Find(path).transform.childCount;

        Transform[] threadChildren = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {

            threadChildren[i] = GameObject.Find(path).transform.GetChild(i);
        }

        return threadChildren;
    }

    public void Awake()
    {
        ApplyTicks();
    }
    public void ExecuteThreads()
    {
        LogManager.instance.logger.sendChronologicalLogs("RunLevel03Thread", "", LogManager.instance.UniEndTime().ToString());


        scrollToTop();

        clearAllClones();
        clearVerticalLayouts();

        //t1_did_cut = false;
        //t1_did_dry = false;
        //t1_did_wash = false;
        //t1_did_groom = false;

        //t2_did_cut = false;
        //t2_did_dry = false;
        //t2_did_wash = false;
        //t2_did_groom = false;


        // ----- SET UP FOR LOLA AND ROCKY, CUSTOMERS FOR LEVEL 3 -----

        /*
        t1_needs_cut = true;
        t1_needs_dry = false;
        t1_needs_wash = true;
        t1_needs_groom = false;

        t2_needs_cut = true;
        t2_needs_dry = true;
        t2_needs_wash = false;
        t2_needs_groom = false;
        */

        // ------ START EXECUTE THREADS -------

        try
        {
            GameObject.Find("InformationPanel").SetActive(false);
        }
        catch { }

        try
        {

            GameObject.Find("AgendaPanel").SetActive(false);
        }
        catch { }

        stop = false;
        err = false;
        paused = false;
        lost = false;



        //t1_has_brush = false;
        //t1_has_clippers = false;
        //t1_has_conditioner = false;
        //t1_has_dryer = false;
        //t1_has_scissors = false;
        //t1_has_shampoo = false;
        //t1_has_station = false;
        //t1_has_towel = false;

        //t2_has_brush = false;
        //t2_has_clippers = false;
        //t2_has_conditioner = false;
        //t2_has_dryer = false;
        //t2_has_scissors = false;
        //t2_has_shampoo = false;
        //t2_has_station = false;
        //t2_has_towel = false;



        try
        {
            // disable all other functionalities
            disablePanel.SetActive(true);
        }
        catch
        {
            Debug.Log("Cannot enable DisablePanel");
        }

        // switch to stop button
        runButton.transform.SetAsFirstSibling();

        
        foreach (Thread t in threads)
        {

            //--- Reseting previous Sim ---
            t.isCheckedIn = false;
            t.isCheckedOut = false;
            foreach(string key in dropDownManager.options)
            {
                t.hasItems[key] = false;
            }
            foreach (string key in dropDownManager.options)
            {
                t.did[key] = false;
            }


            //Gettings block from FE
            t.blocks = GetActionBlocks_MultiThreads("1");
        }

        // ------------------------ READING THREAD 1 ------------------------

        // int thread1_whilesChildren = 0;

        // retrieving the objects (blocks) current in thread 1

        blocks_t1 = GetActionBlocks_MultiThreads("1"); 
            

        // this structure will store the text lines to display
        //List<string> blocks_names_t1 = new List<string>();
        List<GameObject> simulationImagesToDisplay_T1 = new List<GameObject>();


        int i = 0;
        foreach (Transform child in blocks_t1)
        {

            if (child.GetComponent<Draggable>().typeOfItem == Draggable.Type.ACTION)
            {

                //Debug.Log ("TYPE ACTION");

                // action block is a GET action
                if (blocks_t1[i].transform.GetComponentInChildren<Text>().text == "get")
                {

                    string resource = blocks_t1[i].transform.Find("Dropdown").Find("Label").GetComponent<Text>().text;

                    if (resource == "[null]")
                    {
                        terminateSimulation("Please select a resource to acquire in thread 1.");
                        manager.showError("Please select a resource to acquire in thread 1.");
                        return;

                    }
                    else
                    {
                        threads[0].simBlocks.Add(new SimBlock(SimBlock.ACQUIIRE,resource));
                        //blocks_names_t1.Add("[thread 1] acquire ( " + resource + " );");
                        InputWorkerData inpt = new InputWorkerData { action = resource, typeOf = "Acquire" };
                        LogManager.instance.logger.sendInputWorkerOne(resource, "Acquire", LogManager.instance.UniEndTime().ToString());
                        GameLogData.inputList_t1.Add(inpt);
                        //						LogData.inputList_t1.Add ("Acquire: " + resource);

                        i++;

                        // create new object from prefab
                        GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = workerSprite1;
                        newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/acquire");

                        Sprite item;
                        
                        item = Resources.Load<Sprite>("sprites/items/"+resource);
                        /*
                        if (resource == "brush")
                            item = itemsSprites[0];
                        else if (resource == "clippers")
                            item = itemsSprites[1];
                        else if (resource == "cond.")
                            item = itemsSprites[2];
                        else if (resource == "dryer")
                            item = itemsSprites[3];
                        else if (resource == "scissors")
                            item = itemsSprites[4];
                        else if (resource == "shampoo")
                            item = itemsSprites[5];
                        else if (resource == "station")
                            item = itemsSprites[6];
                        else if (resource == "towel")
                            item = itemsSprites[7];
                        else
                            item = displayErrorSprite;
                        */
                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                        newItem.transform.Find("ActionText").GetComponent<Text>().text = "get(" + resource + ");";
                        simulationImagesToDisplay_T1.Add(newItem);
                    }

                    // action block is a RETURN action
                }
                else if (blocks_t1[i].transform.GetComponentInChildren<Text>().text == "ret")
                {

                    string resource = blocks_t1[i].transform.Find("Dropdown").Find("Label").GetComponent<Text>().text;

                    if (resource == "[null]")
                    {
                        terminateSimulation("Please select a resource to return in thread 1.");
                        manager.showError("Please select a resource to return in thread 1.");
                        return;
                    }
                    else
                    {
                        threads[0].simBlocks.Add(new SimBlock(SimBlock.RETURN, resource));
                        //blocks_names_t1.Add("[thread 1] return ( " + resource + " );");
                        InputWorkerData inpt = new InputWorkerData { action = resource, typeOf = "Return" };
                        LogManager.instance.logger.sendInputWorkerOne(resource, "Return", LogManager.instance.UniEndTime().ToString());
                        GameLogData.inputList_t1.Add(inpt);
                        //						LogData.inputList_t1.Add ("Return: " + resource);

                        i++;

                        // create new object from prefab
                        GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = workerSprite1;
                        newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/return");

                        Sprite item;
                        item = Resources.Load<Sprite>("sprites/items/" + resource);
                        /*
                        if (resource == "brush")
                            item = itemsSprites[0];
                        else if (resource == "clippers")
                            item = itemsSprites[1];
                        else if (resource == "cond.")
                            item = itemsSprites[2];
                        else if (resource == "dryer")
                            item = itemsSprites[3];
                        else if (resource == "scissors")
                            item = itemsSprites[4];
                        else if (resource == "shampoo")
                            item = itemsSprites[5];
                        else if (resource == "station")
                            item = itemsSprites[6];
                        else if (resource == "towel")
                            item = itemsSprites[7];
                        else
                            item = displayErrorSprite;
                        */
                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                        newItem.transform.Find("ActionText").GetComponent<Text>().text = "return(" + resource + ");";
                        simulationImagesToDisplay_T1.Add(newItem);

                    }

                }
                else
                {

                    String action = blocks_t1[i].transform.GetComponentInChildren<Text>().text;
                    //blocks_names_t1.Add("[thread 1] " + action + ";");
                    InputWorkerData inpt = new InputWorkerData { action = action, typeOf = "Action" };
                    LogManager.instance.logger.sendInputWorkerOne(action, "Action", LogManager.instance.UniEndTime().ToString());
                    GameLogData.inputList_t1.Add(inpt);
                    //					LogData.inputList_t1.Add ("Action: " + action);

                    i++;

                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;

                    if (action == "checkin")
                    {

                        // Debug.Log ("CHECKING IN");
                        threads[0].simBlocks.Add(new SimBlock(SimBlock.CHECKIN, ""));
                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = workerSprite1;
                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = dogSprite1;
                        newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/acquire");

                    }
                    else if (action == "checkout")
                    {

                        // Debug.Log ("CHECKING OUT");
                        threads[0].simBlocks.Add(new SimBlock(SimBlock.CHECKOUT, ""));
                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = workerSprite1;
                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = dogSprite1;
                        newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/return");

                    }
                    else
                    {

                        // create new object from prefab (single action)
                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = dogSprite1;
                        threads[0].simBlocks.Add(new SimBlock(SimBlock.WORK, action));
                        Sprite item = Resources.Load<Sprite>("sprites/actions/" + action);

                        /*
                        if (action == "cut")
                            item = actionsSprites[2];
                        else if (action == "dry")
                            item = actionsSprites[3];
                        else if (action == "wash")
                            item = actionsSprites[4];
                        else if (action == "groom")
                            item = actionsSprites[5];
                        else
                            item = displayErrorSprite;
                        */

                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                    }
                    newItem.transform.Find("ActionText").GetComponent<Text>().text = action + ";";
                    simulationImagesToDisplay_T1.Add(newItem);

                }

            }
            else if (child.GetComponent<Draggable>().typeOfItem == Draggable.Type.IFSTAT)
            {

                //Debug.Log ("TYPE IFSTAT");

            }
            else if (child.GetComponent<Draggable>().typeOfItem == Draggable.Type.WHILELOOP)
            {

                //Debug.Log ("TYPE WHILELOOP");
            }
        }

        // ------------------------ READING THREAD 2 ------------------------

        // int thread2_whilesChildren = 0;

        // retrieving the objects (blocks) current in thread 1
        blocks_t2 = GetActionBlocks_MultiThreads("2");

        // this structure will store the text lines to display
        //List<string> blocks_names_t2 = new List<string>();
        List<GameObject> simulationImagesToDisplay_T2 = new List<GameObject>();

        i = 0;

        foreach (Transform child in blocks_t2)
        {

            if (child.GetComponent<Draggable>().typeOfItem == Draggable.Type.ACTION)
            {

                //Debug.Log ("TYPE ACTION");

                // action block is a GET action
                if (blocks_t2[i].transform.GetComponentInChildren<Text>().text == "get")
                {

                    string resource = blocks_t2[i].transform.Find("Dropdown").Find("Label").GetComponent<Text>().text;

                    if (resource == "[null]")
                    {

                        terminateSimulation("Please select a resource to acquire in thread 2.");
                        manager.showError("Please select a resource to acquire in thread 2.");
                        return;

                    }
                    else
                    {
                        threads[1].simBlocks.Add(new SimBlock(SimBlock.ACQUIIRE, resource));
                        //blocks_names_t2.Add("[thread 2] acquire ( " + resource + " );");
                        InputWorkerData inpt = new InputWorkerData { action = resource, typeOf = "Acquire" };
                        LogManager.instance.logger.sendInputWorkerTwo(resource, "Acquire", LogManager.instance.UniEndTime().ToString());
                        GameLogData.inputList_t2.Add(inpt);
                        //						LogData.inputList_t2.Add ("Acquire: " + resource);

                        i++;

                        // create new object from prefab
                        GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = workerSprite2;
                        newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/acquire");

                        Sprite item;
                        item = Resources.Load<Sprite>("sprites/items/" + resource);
                        /*
                        if (resource == "brush")
                            item = itemsSprites[0];
                        else if (resource == "clippers")
                            item = itemsSprites[1];
                        else if (resource == "cond.")
                            item = itemsSprites[2];
                        else if (resource == "dryer")
                            item = itemsSprites[3];
                        else if (resource == "scissors")
                            item = itemsSprites[4];
                        else if (resource == "shampoo")
                            item = itemsSprites[5];
                        else if (resource == "station")
                            item = itemsSprites[6];
                        else if (resource == "towel")
                            item = itemsSprites[7];
                        else
                            item = displayErrorSprite;
                        */
                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                        newItem.transform.Find("ActionText").GetComponent<Text>().text = "get(" + resource + ");";
                        simulationImagesToDisplay_T2.Add(newItem);

                    }

                    // action block is a RETURN action
                }
                else if (blocks_t2[i].transform.GetComponentInChildren<Text>().text == "ret")
                {

                    string resource = blocks_t2[i].transform.Find("Dropdown").Find("Label").GetComponent<Text>().text;

                    if (resource == "[null]")
                    {

                        terminateSimulation("Please select a resource to return in thread 2.");
                        manager.showError("Please select a resource to return in thread 2.");
                        return;

                    }
                    else
                    {
                        threads[1].simBlocks.Add(new SimBlock(SimBlock.RETURN, resource));
                        //blocks_names_t2.Add("[thread 2] return ( " + resource + " );");
                        InputWorkerData inpt = new InputWorkerData { action = resource, typeOf = "Return" };
                        LogManager.instance.logger.sendInputWorkerTwo(resource, "Return", LogManager.instance.UniEndTime().ToString());
                        GameLogData.inputList_t2.Add(inpt);
                        //						LogData.inputList_t2.Add ("Return: " + resource);

                        i++;

                        // create new object from prefab
                        GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = workerSprite2;
                        newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/return");

                        Sprite item;
                        item = Resources.Load<Sprite>("sprites/items/" + resource);
                        /*
                        if (resource == "brush")
                            item = itemsSprites[0];
                        else if (resource == "clippers")
                            item = itemsSprites[1];
                        else if (resource == "cond.")
                            item = itemsSprites[2];
                        else if (resource == "dryer")
                            item = itemsSprites[3];
                        else if (resource == "scissors")
                            item = itemsSprites[4];
                        else if (resource == "shampoo")
                            item = itemsSprites[5];
                        else if (resource == "station")
                            item = itemsSprites[6];
                        else if (resource == "towel")
                            item = itemsSprites[7];
                        else
                            item = displayErrorSprite;
                        */
                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                        newItem.transform.Find("ActionText").GetComponent<Text>().text = "return(" + resource + ");";
                        simulationImagesToDisplay_T2.Add(newItem);
                    }

                }
                else
                {

                    String action = blocks_t2[i].transform.GetComponentInChildren<Text>().text;
                    
                    //blocks_names_t2.Add("[thread 2] " + action + ";");
                    InputWorkerData inpt = new InputWorkerData { action = action, typeOf = "Action" };
                    LogManager.instance.logger.sendInputWorkerTwo(action, "Action", LogManager.instance.UniEndTime().ToString());
                    GameLogData.inputList_t2.Add(inpt);
                    //					LogData.inputList_t2.Add ("Action: " + action);

                    i++;

                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;

                    if (action == "checkin")
                    {
                        threads[1].simBlocks.Add(new SimBlock(SimBlock.CHECKIN, ""));
                        //Debug.Log ("CHECKING IN");

                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = workerSprite2;
                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = dogSprite2;
                        newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/acquire");

                    }
                    else if (action == "checkout")
                    {

                        // Debug.Log ("CHECKING OUT");
                        threads[1].simBlocks.Add(new SimBlock(SimBlock.CHECKOUT, ""));
                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = workerSprite2;
                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = dogSprite2;
                        newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/return");

                    }
                    else
                    {

                        // create new object from prefab (single action)
                        newItem.transform.Find("Icon").GetComponent<Image>().sprite = dogSprite2;
                        threads[1].simBlocks.Add(new SimBlock(SimBlock.WORK, action));
                        Sprite item = Resources.Load<Sprite>("sprites/actions/" + action);

                        /*
                        if (action == "cut")
                            item = actionsSprites[2];
                        else if (action == "dry")
                            item = actionsSprites[3];
                        else if (action == "wash")
                            item = actionsSprites[4];
                        else if (action == "groom")
                            item = actionsSprites[5];
                        else
                            item = displayErrorSprite;
                        */

                        newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                    }
                    newItem.transform.Find("ActionText").GetComponent<Text>().text = action + ";";
                    simulationImagesToDisplay_T2.Add(newItem);

                }

            }
            else if (child.GetComponent<Draggable>().typeOfItem == Draggable.Type.IFSTAT)
            {

                //Debug.Log ("TYPE IFSTAT");

            }
            else if (child.GetComponent<Draggable>().typeOfItem == Draggable.Type.WHILELOOP)
            {

                //Debug.Log ("TYPE WHILELOOP");

            }
        }

        if (blocks_t1.Length < 1)
        {

            manager.showError("There are no actions to run in thread 1.");
            terminateSimulation("There are no actions to run in thread 1.");
            return;
        }

        if (blocks_t2.Length < 1)
        {

            manager.showError("There are no actions to run in thread 2.");
            terminateSimulation("There are no actions to run in thread 2.");
            return;
        }

        try
        {
            /*
            if ((blocks_names_t1[0].Substring(11, 7) != "checkin"  ) || (blocks_names_t2[0].Substring(11, 7) != "checkin"  ))
            {
                manager.showError("Remember to always check-in your costumer first!");
                terminateSimulation("Remember to always check-in your costumer first!");
                return;
            }
            */


            //Check If Checkin block is present
            Debug.Log("spd: "+ threads[0].simBlocks[0].type);
            Debug.Log("spd: " + threads[1].simBlocks[0].type);

            if ((threads[0].simBlocks[0].type != SimBlock.CHECKIN ) || (threads[1].simBlocks[0].type != SimBlock.CHECKIN))
            {

                manager.showError("Remember to always check-in your costumer first!");
                terminateSimulation("Remember to always check-in your costumer first!");
                return;
            }

        }
        catch
        {
            Debug.Log("spd: catched");
            manager.showError("Remember to always check-in your costumer first!");
            terminateSimulation("Remember to always check-in your costumer first!");
            return;
        }

        try
        {

            /*
            if ((blocks_names_t1[blocks_names_t1.Count - 1].Substring(11, 8) != "checkout") ||
                (blocks_names_t2[blocks_names_t2.Count - 1].Substring(11, 8) != "checkout"))
            {

                manager.showError("Remember to always check-out your costumer when you're done!");
                terminateSimulation("Remember to always check-out your costumer when you're done!");
                return;
            }*/


            //Check If Checkout block is present
            if ((threads[0].simBlocks.Last().type != SimBlock.CHECKOUT) || (threads[1].simBlocks.Last().type != SimBlock.CHECKOUT))
            {

                manager.showError("Remember to always check-out your costumer when you're done!");
                terminateSimulation("Remember to always check-out your costumer when you're done!");
                return;
            }

        }
        catch
        {

            manager.showError("Remember to always check-out your costumer when you're done!");
            terminateSimulation("Remember to always check-out your costumer when you're done!");
            return;
        }

        if (!err)
        {
            StartCoroutine(printThreads(simulationImagesToDisplay_T1, simulationImagesToDisplay_T2, 5));
        }
    }

    private void ApplyTicks()
    {
        foreach (Thread t in threads)
        {
            
            foreach (GameObject g in t.ticks)
            {              
                try
                {
                   
                    g.SetActive((bool)t.workList.GetType().GetField(g.name).GetValue(t.workList)) ;
                    
                }
                catch(Exception e)
                {
                    
                    Debug.Log(e.Message+" Name The Tick as same as work "+ g.name);
                }
            }
            foreach (GameObject g in t.innerTicks)
            {
                try
                {

                    g.SetActive((bool)t.workList.GetType().GetField(g.name).GetValue(t.workList));

                }
                catch (Exception e)
                {

                    Debug.Log(e.Message + " Name The Tick as same as work " + g.name);
                }
            }
        }
    }

    IEnumerator printThreads(List<GameObject> s1, List<GameObject> s2, int speed)
    {

        // waitOneSecond ();

        // scrollToTop ();

        bar.currentAmount = 0;

        // int step_counter = 1;
        int t1_curr_index = 0;
        int t2_curr_index = 0;

        bool t1_canPrint = true;
        bool t2_canPrint = true;

        int j = 0;

        while ((t1_curr_index < threads[0].simBlocks.Count) || (t2_curr_index < threads[1].simBlocks.Count))
        {

            if (bar.currentAmount < 100)
            {
                bar.currentAmount += speed;
                bar.LoadingBar.GetComponent<Image>().fillAmount = bar.currentAmount / 100;

            }
            else
            {

                LogManager.instance.logger.sendChronologicalLogs("Level03Lost", "", LogManager.instance.UniEndTime().ToString());
                manager.gameLost();

                //logging
                GameLogData.isLevelCleared = false;
                //	LogData.levelSteps = j;
                GameLogData.levelClearedTime = LogManager.instance.EndTimer();
                GameLogData.levelClearAmount = bar.currentAmount;
                GameLogData.failedReason = "Times up! GameLost";
                LogManager.instance.failCount++;
                GameLogData.failedAttempts = LogManager.instance.failCount;
                LogManager.instance.CreateLogData();

                LogManager.instance.isQuitLogNeed = false;

                stop = true;
                paused = true;
                lost = true;

                stopButton.transform.GetComponent<Button>().interactable = false;

                yield return 0;
            }

            if (stop)
            {

                if (!paused)
                {

                    try
                    {

                        disablePanel.SetActive(false);

                    }
                    catch
                    {

                        Debug.Log("Cannot disable DisablePanel");
                    }

                    runButton.transform.SetAsLastSibling();

                }

                bar.LoadingBar.GetComponent<Image>().fillAmount = 0;

                break;
                // yield break;
                // yield return 0;

            }
            else
            {

                stepsIndicator.text = "" + (j + 1);

                // ------------------------------  THREAD 1 ------------------------------

                try
                {
                    //Debug.Log("spd: " + b1[t1_curr_index]);

                    // {"[null]", "brush" ,"clippers" , "cond.", "dryer", "scissors", "shampoo", "station", "towel"};

                    if (threads[0].simBlocks[t1_curr_index].type == SimBlock.ACQUIIRE)
                    {

                        // acquiring resource
                        switch (threads[0].simBlocks[t1_curr_index].name)
                        {

                            case "brush":

                                if (!threads[0].hasItems["brush"] && threads[1].hasItems["brush"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/brush"); 
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for brush...</color>";
                                    newItem.transform.SetParent(layoutPanel1.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t1_canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems,"brush");
                                    t1_canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, 1); // ERROR: You are trying to acquire a resource you already have.";
                                    }
                                }

                                break;

                            case "clippers":

                                if (!threads[0].hasItems["clippers"] && threads[1].hasItems["clippers"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/clippers");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for nail clippers...</color>";
                                    newItem.transform.SetParent(layoutPanel1.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t1_canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "clippers");
                                    t1_canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, 1);
                                    }
                                }

                                break;

                            case "cond.":

                                if (!threads[0].hasItems["cond."] && threads[1].hasItems["cond."])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/cond.");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for conditioner...</color>";
                                    newItem.transform.SetParent(layoutPanel1.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t1_canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "cond.");
                                    t1_canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, 1);
                                    }
                                }

                                break;

                            case "dryer":

                                if (!threads[0].hasItems["dryer"] && threads[1].hasItems["dryer"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/dryer");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for dryer...</color>";
                                    newItem.transform.SetParent(layoutPanel1.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t1_canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "dryer");
                                    t1_canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, 1);
                                    }
                                }

                                break;

                            case "scissors":

                                if (!threads[0].hasItems["scissors"] && threads[1].hasItems["scissors"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/scissors");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for scissors...</color>";
                                    newItem.transform.SetParent(layoutPanel1.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t1_canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "scissors");
                                    t1_canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, 1);
                                    }
                                }

                                break;

                            case "shampoo":

                                if (!threads[0].hasItems["shampoo"] && threads[1].hasItems["shampoo"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/shampoo");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for shampoo...</color>";
                                    newItem.transform.SetParent(layoutPanel1.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t1_canPrint = false;

                                }
                                else
                                {
                                    int output = acquire(ref threads[0].hasItems, "shampoo");
                                    t1_canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, 1);
                                    }
                                }

                                break;

                            case "station":

                                if (!threads[0].hasItems["station"] && threads[1].hasItems["station"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/station");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for station...</color>";
                                    newItem.transform.SetParent(layoutPanel1.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t1_canPrint = false;

                                }
                                else
                                {
                                    int output = acquire(ref threads[0].hasItems, "station");
                                    t1_canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, 1);
                                    }
                                }

                                break;

                            case "towel":

                                if (!threads[0].hasItems["towel"] && threads[1].hasItems["towel"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/towel");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for towel...</color>";
                                    newItem.transform.SetParent(layoutPanel1.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t1_canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "towel");
                                    t1_canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, 1);
                                    }
                                }

                                break;
                        }

                    }
                    else if (threads[0].simBlocks[t1_curr_index].type == SimBlock.RETURN)
                    {

                        // returning resource
                        switch (threads[0].simBlocks[t1_curr_index].name)
                        {

                            case "brush":

                                int output1 = return_res(ref threads[0].hasItems,"brush");

                                if (output1 < 0)
                                {
                                    resError(returnErrMsg, 1);
                                }

                                break;

                            case "clippers":

                                int output2 = return_res(ref threads[0].hasItems, "clippers");

                                if (output2 < 0)
                                {
                                    resError(returnErrMsg, 1);
                                }

                                break;

                            case "cond.":

                                int output3 = return_res(ref threads[0].hasItems, "cond.");

                                if (output3 < 0)
                                {
                                    resError(returnErrMsg, 1);
                                }

                                break;

                            case "dryer":

                                int output4 = return_res(ref threads[0].hasItems, "dryer");

                                if (output4 < 0)
                                {
                                    resError(returnErrMsg, 1);
                                }

                                break;

                            case "scissors":

                                int output5 = return_res(ref threads[0].hasItems, "scissors");

                                if (output5 < 0)
                                {
                                    resError(returnErrMsg, 1);
                                }

                                break;

                            case "shampoo":

                                int output6 = return_res(ref threads[0].hasItems, "shampoo");

                                if (output6 < 0)
                                {
                                    resError(returnErrMsg, 1);
                                }

                                break;

                            case "station":

                                int output7 = return_res(ref threads[0].hasItems, "station");

                                if (output7 < 0)
                                {
                                    resError(returnErrMsg, 1);
                                }

                                break;

                            case "towel":

                                int output8 = return_res(ref threads[0].hasItems, "towel");

                                if (output8 < 0)
                                {
                                    resError(returnErrMsg, 1);
                                }

                                break;
                        }

                    }
                    else if(threads[0].simBlocks[t1_curr_index].type == SimBlock.WORK)
                    {
                        if (threads[0].simBlocks[t1_curr_index].name == "Cut")
                        {

                            if (!threads[0].hasItems["brush"] || !threads[0].hasItems["scissors"])
                            {

                                String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                                s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
                                s1[t1_curr_index].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't cut without a brush and some scissors.", 1);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform cut
                                threads[0].did["Cut"] = true;
                                t1_did_cut = true;
                            }
                        }
                        else if (threads[0].simBlocks[t1_curr_index].name == "Dry")
                        {

                            if (!threads[0].hasItems["station"] || !threads[0].hasItems["dryer"] || !threads[0].hasItems["towel"])
                            {

                                String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                                s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
                                s1[t1_curr_index].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't dry without a station, a dryer and a towel.", 1);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform dry
                                threads[0].did["Dry"] = true;
                                t1_did_dry = true;
                            }

                        }
                        else if (threads[0].simBlocks[t1_curr_index].name == "Wash")
                        {

                            if (!threads[0].hasItems["station"] || !threads[0].hasItems["shampoo"] || !threads[0].hasItems["towel"] || !threads[0].hasItems["cond."])
                            {

                                String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                                s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
                                s1[t1_curr_index].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't wash without a station, shampoo, conditioner, and a towel.", 1);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform wash
                                threads[0].did["Wash"] = true;
                                t1_did_wash = true;
                            }

                        }
                        else if (threads[0].simBlocks[t1_curr_index].name == "Groom")
                        {

                            if (!threads[0].hasItems["brush"] || !threads[0].hasItems["clippers"])
                            {

                                String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                                s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
                                s1[t1_curr_index].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't groom without a brush and some nail clippers.", 1);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform groom
                                threads[0].did["Groom"] = true;
                                t1_did_groom = true;
                            }

                        }
                    }
                    else if (threads[0].simBlocks[t1_curr_index].type == SimBlock.CHECKIN)
                    {

                        if (threads[0].isCheckedIn)
                        {

                            String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                            s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
                            s1[t1_curr_index].transform.localScale = Vector3.one;

                            resError("> ERROR: You are already checked in. You have to check out before attempting to check in a different customer.", 1);
                            scrollToBottom();

                        }
                        else
                        {

                            // perform check-in
                            threads[0].isCheckedIn = true;
                            threads[0].isCheckedOut = false;
                        }

                    }
                    else if (threads[0].simBlocks[t1_curr_index].type == SimBlock.CHECKOUT)
                    {
                        foreach(KeyValuePair<string,bool> b in threads[0].needsTo)
                        {
                            Debug.Log(b.Key + b.Value + "<----");
                        }
                        if ((threads[0].needsTo["Cut"] && !threads[0].did["Cut"]) || (threads[0].needsTo["Dry"] && !threads[0].did["Dry"]) || (threads[0].needsTo["Wash"] && !threads[0].did["Wash"]) || (threads[0].needsTo["Groom"] && !threads[0].did["Groom"]))
                        {

                            String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                            s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
                            s1[t1_curr_index].transform.localScale = Vector3.one;
                            scrollToBottom();

                            resError("> ERROR: Seems like worker 1 didn't fulfill all of the customer's requests. Please try again.", 1);
                            scrollToBottom();

                        }
                        else if (threads[0].hasItems["brush"] || threads[0].hasItems["clippers"] || threads[0].hasItems["cond."] || threads[0].hasItems["dryer"] || threads[0].hasItems["scissors"] || threads[0].hasItems["shampoo"] || threads[0].hasItems["station"] || threads[0].hasItems["towel"])
                        {

                            String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                            s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
                            s1[t1_curr_index].transform.localScale = Vector3.one;

                            resError("> ERROR: You need to return all the resources you acquired before checking out.", 1);
                            scrollToBottom();

                        }
                        else if (threads[0].isCheckedOut)
                        {

                            String actionText = s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                            s1[t1_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
                            s1[t1_curr_index].transform.localScale = Vector3.one;

                            resError("> ERROR: You have to check in before attempting to check out a customer.", 1);
                            scrollToBottom();

                        }
                        else
                        {

                            // perform check-out
                            threads[0].isCheckedIn = false;
                            threads[0].isCheckedOut = true;
                        }
                    }

                }
                catch { }

                try
                {

                    if (t1_canPrint)
                    {

                        if (!err)
                        {
                            s1[t1_curr_index].transform.SetParent(layoutPanel1.transform);
                            s1[t1_curr_index].transform.localScale = Vector3.one;
                        }
                        t1_curr_index++;
                    }
                    scrollToBottom();

                }
                catch { }

                scrollToBottom();

                // ------------------------------  THREAD 2 ------------------------------

                try
                {

                    // {"[null]", "brush" ,"clippers" , "cond.", "dryer", "scissors", "shampoo", "station", "towel"};

                    if (threads[1].simBlocks[t1_curr_index].type == SimBlock.ACQUIIRE)
                    {

                        // acquiring resource
                        switch (threads[1].simBlocks[t1_curr_index].name)
                        {

                            case "brush":

                                if (!threads[1].hasItems["brush"] && threads[0].hasItems["brush"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/brush");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for brush...</color>";
                                    newItem.transform.SetParent(layoutPanel2.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t2_canPrint = false;

                                }
                                else
                                {

                                    int output1 = acquire(ref threads[1].hasItems, "brush");
                                    t2_canPrint = true;

                                    if (output1 < 0)
                                    {
                                        resError(acquireErrMsg, 2);
                                    }
                                }

                                break;

                            case "clippers":

                                if (!threads[1].hasItems["clippers"] && threads[0].hasItems["clippers"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/clippers");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for nail clippers...</color>";
                                    newItem.transform.SetParent(layoutPanel2.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t2_canPrint = false;

                                }
                                else
                                {

                                    int output2 = acquire(ref threads[1].hasItems, "clippers");
                                    t2_canPrint = true;

                                    if (output2 < 0)
                                    {
                                        resError(acquireErrMsg, 2);
                                    }
                                }

                                break;

                            case "cond.":

                                if (!threads[1].hasItems["cond."] && threads[0].hasItems["cond."])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/cond.");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for conditioner...</color>";
                                    newItem.transform.SetParent(layoutPanel2.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t2_canPrint = false;

                                }
                                else
                                {

                                    int output3 = acquire(ref threads[1].hasItems, "cond.");
                                    t2_canPrint = true;

                                    if (output3 < 0)
                                    {
                                        resError(acquireErrMsg, 2);
                                    }
                                }

                                break;

                            case "dryer":

                                if (!threads[1].hasItems["dryer"] && threads[0].hasItems["dryer"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/dryer");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for dryer...</color>";
                                    newItem.transform.SetParent(layoutPanel2.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t2_canPrint = false;

                                }
                                else
                                {

                                    int output4 = acquire(ref threads[1].hasItems, "dryer");
                                    t2_canPrint = true;

                                    if (output4 < 0)
                                    {
                                        resError(acquireErrMsg, 2);
                                    }
                                }

                                break;

                            case "scissors":

                                if (!threads[1].hasItems["scissors"] && threads[0].hasItems["scissors"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/scissors");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for scissors...</color>";
                                    newItem.transform.SetParent(layoutPanel2.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t2_canPrint = false;

                                }
                                else
                                {

                                    int output5 = acquire(ref threads[1].hasItems, "scissors");
                                    t2_canPrint = true;

                                    if (output5 < 0)
                                    {
                                        resError(acquireErrMsg, 2);
                                    }
                                }

                                break;

                            case "shampoo":

                                if (!threads[1].hasItems["shampoo"] && threads[0].hasItems["shampoo"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/shampoo");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for shampoo...</color>";
                                    newItem.transform.SetParent(layoutPanel2.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t2_canPrint = false;

                                }
                                else
                                {

                                    int output6 = acquire(ref threads[1].hasItems, "shampoo");
                                    t2_canPrint = true;

                                    if (output6 < 0)
                                    {
                                        resError(acquireErrMsg, 2);
                                    }
                                }

                                break;

                            case "station":

                                if (!threads[1].hasItems["station"] && threads[0].hasItems["station"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/station");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for station...</color>";
                                    newItem.transform.SetParent(layoutPanel2.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t2_canPrint = false;

                                }
                                else
                                {

                                    int output7 = acquire(ref threads[1].hasItems, "station");
                                    t2_canPrint = true;

                                    if (output7 < 0)
                                    {
                                        resError(acquireErrMsg, 2);
                                    }
                                }

                                break;

                            case "towel":

                                if (!threads[1].hasItems["towel"] && threads[0].hasItems["towel"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/towel");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for towel...</color>";
                                    newItem.transform.SetParent(layoutPanel2.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    t2_canPrint = false;

                                }
                                else
                                {
                                    int output8 = acquire(ref threads[1].hasItems, "towel");
                                    t2_canPrint = true;

                                    if (output8 < 0)
                                    {
                                        resError(acquireErrMsg, 2);
                                    }
                                }

                                break;
                        }

                    }
                    else if (threads[1].simBlocks[t1_curr_index].type == SimBlock.RETURN)
                    {

                        // returning resource
                        switch (threads[1].simBlocks[t1_curr_index].name)
                        {

                            case "brush":

                                int output1 = return_res(ref threads[1].hasItems, "brush");

                                if (output1 < 0)
                                {
                                    resError(returnErrMsg, 2);
                                }

                                break;

                            case "clippers":

                                int output2 = return_res(ref threads[1].hasItems, "clippers");

                                if (output2 < 0)
                                {
                                    resError(returnErrMsg, 2);
                                }

                                break;

                            case "cond.":

                                int output3 = return_res(ref threads[1].hasItems, "cond.");

                                if (output3 < 0)
                                {
                                    resError(returnErrMsg, 2);
                                }

                                break;

                            case "dryer":

                                int output4 = return_res(ref threads[1].hasItems, "dryer");

                                if (output4 < 0)
                                {
                                    resError(returnErrMsg, 2);
                                }

                                break;

                            case "scissors":

                                int output5 = return_res(ref threads[1].hasItems, "scissors");

                                if (output5 < 0)
                                {
                                    resError(returnErrMsg, 2);
                                }

                                break;

                            case "shampoo":

                                int output6 = return_res(ref threads[1].hasItems, "shampoo");

                                if (output6 < 0)
                                {
                                    resError(returnErrMsg, 2);
                                }

                                break;

                            case "station":

                                int output7 = return_res(ref threads[1].hasItems, "station");

                                if (output7 < 0)
                                {
                                    resError(returnErrMsg, 2);
                                }

                                break;

                            case "towel":

                                int output8 = return_res(ref threads[1].hasItems, "towel");

                                if (output8 < 0)
                                {
                                    resError(returnErrMsg, 2);
                                }

                                break;
                        }

                    }
                    else if(threads[1].simBlocks[t1_curr_index].type == SimBlock.WORK)
                    {
                        if (threads[1].simBlocks[t1_curr_index].name == "Cut")
                        {

                            if (!threads[1].hasItems["brush"] || !threads[1].hasItems["scissors"])
                            {

                                String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                                s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
                                s2[t2_curr_index].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't cut without a brush and some scissors.", 2);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform cut
                                threads[1].did["Cut"] = true;
                                t2_did_cut = true;
                            }

                        }
                        else if (threads[1].simBlocks[t1_curr_index].name == "Dry")
                        {

                            if (!threads[1].hasItems["station"] || !threads[1].hasItems["dryer"] || !threads[1].hasItems["towel"])
                            {

                                String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                                s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
                                s2[t2_curr_index].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't dry without a station, a dryer and a towel.", 2);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform dry
                                threads[1].did["Dry"] = true;
                                t2_did_dry = true;
                            }

                        }
                        else if (threads[1].simBlocks[t1_curr_index].name == "Wash")
                        {

                            if (!threads[1].hasItems["station"] || !threads[1].hasItems["shampoo"] || !threads[1].hasItems["towel"] || !threads[1].hasItems["cond."])
                            {

                                String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                                s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
                                s2[t2_curr_index].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't wash without a station, shampoo, conditioner, and a towel.", 2);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform wash
                                threads[1].did["Wash"] = true;
                                t2_did_wash = true;
                            }

                        }
                        else if (threads[1].simBlocks[t1_curr_index].name == "Groom")
                        {

                            if (!threads[1].hasItems["brush"] || !threads[1].hasItems["clippers"])
                            {

                                String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                                s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
                                s2[t2_curr_index].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't groom without a brush and some nail clippers.", 2);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform groom
                                threads[1].did["Groom"] = true;
                                t2_did_groom = true;
                            }

                        }
                    }
                    else if (threads[1].simBlocks[t1_curr_index].type == SimBlock.CHECKIN)
                    {

                        if (threads[1].isCheckedIn)
                        {

                            String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                            s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
                            s2[t2_curr_index].transform.localScale = Vector3.one;

                            resError("> ERROR: You are already checked in. You have to check out before attempting to check in a different customer.", 2);
                            scrollToBottom();

                        }
                        else
                        {

                            // perform check-in
                            threads[1].isCheckedIn = true;
                            threads[1].isCheckedOut = false;
                        }

                    }
                    else if (threads[1].simBlocks[t1_curr_index].type == SimBlock.CHECKOUT)
                    {

                        if ((threads[1].needsTo["Cut"] && !threads[1].did["Cut"]) || (threads[1].needsTo["Dry"] && !threads[1].did["Dry"]) || (threads[1].needsTo["Wash"] && !threads[1].did["Wash"]) || (threads[1].needsTo["Groom"] && !threads[1].did["Groom"]))
                        {

                            String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                            s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
                            s2[t2_curr_index].transform.localScale = Vector3.one;
                            scrollToBottom();

                            resError("> ERROR: Seems like worker 2 didn't fulfill all of the customer's requests. Please try again.", 2);
                            scrollToBottom();

                        }
                        else if (threads[1].hasItems["brush"] || threads[1].hasItems["clippers"] || threads[1].hasItems["cond."] || threads[1].hasItems["dryer"] || threads[1].hasItems["scissors"] || threads[1].hasItems["shampoo"] || threads[1].hasItems["station"] || threads[1].hasItems["towel"])
                        {

                            String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                            s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
                            s2[t2_curr_index].transform.localScale = Vector3.one;

                            resError("> ERROR: You need to return all the resources you acquired before checking out.", 2);
                            scrollToBottom();

                        }
                        else if (threads[1].isCheckedOut)
                        {

                            String actionText = s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text;
                            s2[t2_curr_index].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
                            s2[t2_curr_index].transform.localScale = Vector3.one;

                            resError("> ERROR: You have to check in before attempting to check out a customer.", 2);
                            scrollToBottom();

                        }
                        else
                        {

                            // perform check-out
                            threads[1].isCheckedIn = false;
                            threads[1].isCheckedOut = true;
                        }
                    }

                }
                catch { }

                try
                {

                    if (t2_canPrint)
                    {
                        if (!err)
                        {
                            s2[t2_curr_index].transform.SetParent(layoutPanel2.transform);
                            s2[t2_curr_index].transform.localScale = Vector3.one;
                        }

                        t2_curr_index++;
                    }

                }
                catch
                {
                    scrollToBottom();
                }

                j++; // increment step
                yield return new WaitForSeconds(1);
                scrollToBottom();
            }
        }

        if (!lost)
        {
            LogManager.instance.logger.sendChronologicalLogs("Level03Won", "", LogManager.instance.UniEndTime().ToString());
            manager.gameWon();
            Debug.Log("Finished in " + j + " steps.");

            //logging
            GameLogData.isLevelCleared = true;
            //	LogData.levelSteps = j;
            GameLogData.levelClearedTime = LogManager.instance.EndTimer();
            GameLogData.levelClearAmount = bar.currentAmount;
            GameLogData.failedReason = "Passed";
            GameLogData.failedAttempts = LogManager.instance.failCount;
            GameLogData.infoButtonCount = LogManager.instance.infoCount;
            GameLogData.agendaButtonCount = LogManager.instance.agendaCount;
            LogManager.instance.CreateLogData();

            LogManager.instance.isQuitLogNeed = false;
        }

        Canvas.ForceUpdateCanvases();
        scrollToBottom();
    }

    public void terminateSimulation(string error)
    {
        GameLogData.chronologicalLogs.Add("TerminateLevel3: " + LogManager.instance.UniEndTime());
        LogManager.instance.logger.sendChronologicalLogs("TerminateLevel3", "", LogManager.instance.UniEndTime().ToString());

        GameLogData.failedReason = error;
        LogManager.instance.CreateLogData();

        LogManager.instance.failCount++;

        stepsIndicator.text = "0";

        err = true;
        lost = true;
        stop = true;
        paused = true;

        try
        {
            disablePanel.SetActive(false);
        }
        catch
        {
            Debug.Log("Cannot disable DisablePanel.");
        }

        runButton.transform.SetAsLastSibling();
        bar.LoadingBar.GetComponent<Image>().fillAmount = 0;

        // StartCoroutine (waitOneSecond());
        // scrollToBottom ();
    }

    void resError(String msg, int thread_num)
    {

        // display error
        Transform newItemParent;

        if (thread_num == 1)
            newItemParent = layoutPanel1.transform;
        else
            newItemParent = layoutPanel2.transform;

        GameObject newItem = Instantiate(simulationErrorPrefab) as GameObject;
        newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + msg + "</color>";
        // newItem.transform.parent = newItemParent;
        newItem.transform.SetParent(newItemParent);
        newItem.transform.localScale = Vector3.one;
        // scrollToBottom ();

        // terminate simulation
        terminateSimulation(msg);
    }

    int acquire(ref Dictionary<string,bool> dict, string name)
    {

        if (dict[name])
        {

            err = true;
            lost = true;
            stop = true;
            paused = true;

            return -1;

        }
        else
        {

            dict[name] = true;
            return 0;
        }

    }

    int return_res(ref Dictionary<string, bool> dict, string name)
    {

        if (!dict[name])
        {

            err = true;
            lost = true;
            stop = true;
            paused = true;

            return -1;

        }
        else
        {
            dict[name] = false;

            return 0;
        }
    }

    void clearVerticalLayouts()
    {
        stepsIndicator.text = "0";

        //layoutPanel1
        foreach (Transform child in layoutPanel1.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //layoutPanel2
        foreach (Transform child in layoutPanel2.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void scrollToBottom()
    {

        // Debug.Log ("scrollToBottom()");
        Canvas.ForceUpdateCanvases();
        waitOneFrame();
        simulationScrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    void scrollToTop()
    {

        // Debug.Log ("scrollToBottom()");
        Canvas.ForceUpdateCanvases();
        waitOneFrame();
        simulationScrollRect.verticalNormalizedPosition = 1f;
        Canvas.ForceUpdateCanvases();
    }

    void clearAllClones()
    {

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {

            if (obj.transform.name == "SimulationImage(Clone)")
                GameObject.Destroy(obj);
        }
    }

    IEnumerator waitOneFrame()
    {
        yield return 0;
    }
}