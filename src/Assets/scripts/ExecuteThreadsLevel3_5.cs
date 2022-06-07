using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;

public class ExecuteThreadsLevel3_5 : MonoBehaviour
{



    [System.Serializable]
    public class Thread
    {
        //This is list of ticks(UI) which can be set on/off
        public List<GameObject> ticks;
        public List<GameObject> innerTicks;

        //ThreadData
        public WorkList workList = new WorkList();


        //Tab (Grandparent of blocks)
        public GameObject layoutPanel;
        public GameObject tabDropArea;

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
    }


    public List<Thread> threads;
    private dropDownManager dropDownManager = new dropDownManager();








    // --- IMAGE SIMULATION ---

    public GameObject scrollRect;

    public GameObject simulationImagePrefab;
    public GameObject simulationErrorPrefab;
    public Text stepsIndicator;


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

    bool stop;
    bool err;
    bool paused;
    bool lost;


    string returnErrMsg = "> ERROR: You are trying to return a resource you don't have.";
    string acquireErrMsg = "> ERROR: You are trying to acquire a resource you already have.";

    void Start()
    {

        //Initilize a Random dogs from each halves;
        UnityEngine.Object[] s = Resources.LoadAll("sprites/dogs", typeof(Sprite));
        int len = s.Length;
        threads[0].dogSprite = (Sprite)s[Random.Range(0, len / 2 - 1)];
        threads[1].dogSprite = (Sprite)s[Random.Range(len / 2, len - 1)];

        //Initilize a Random worker from each halves;
        s = Resources.LoadAll("sprites/workers", typeof(Sprite));
        len = s.Length;
        threads[0].workerSprite = (Sprite)s[Random.Range(0, len / 2 - 1)];
        threads[1].workerSprite = (Sprite)s[Random.Range(len / 2, len - 1)];


        //Fill needsto Dict
        foreach (Thread t in threads)
        {
            System.Reflection.FieldInfo[] varWorklist = t.workList.GetType().GetFields();
            foreach (System.Reflection.FieldInfo v in varWorklist)
            {
                t.needsTo.Add(v.Name, ((Action)v.GetValue(t.workList)).isneeded);
            }
        }
        manager = GameObject.Find("_SCRIPTS_").GetComponent<ToolboxManager>();
        disablePanel = GameObject.Find("DisablePanel");
        bar = GameObject.Find("RadialProgressBar").GetComponent<ProgressBar>();
        try
        {
            simulationScrollRect = scrollRect.transform.GetComponent<ScrollRect>();
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

    private Transform[] GetActionBlocks(GameObject tabDropArea)
    {

        //get children in drop area for thread
        int childCount = tabDropArea.transform.childCount;

        Transform[] threadChildren = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {

            threadChildren[i] = tabDropArea.transform.GetChild(i);
        }

        return threadChildren;
    }

    public void Awake()
    {
        ApplyTicks();
    }
    public void ExecuteThreads()
    {

        //-------- UI Updates and Logging --------
        LogManager.instance.logger.sendChronologicalLogs("RunLevel03Thread", "", LogManager.instance.UniEndTime().ToString());
        scrollToTop();
        clearAllClones();
        clearVerticalLayouts();
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
        // switch to stop button
        runButton.transform.SetAsFirstSibling();
        try
        {
            // disable all other functionalities
            disablePanel.SetActive(true);
        }
        catch
        {
            Debug.Log("Cannot enable DisablePanel");
        }




        //-------- Reseting previous Sim --------
        foreach (Thread t in threads)
        {
            t.isCheckedIn = false;
            t.isCheckedOut = false;
            t.currIndex = 0;
            t.canPrint = true;

            System.Reflection.FieldInfo[] varWorklist = t.workList.GetType().GetFields();
            foreach (System.Reflection.FieldInfo v in varWorklist)
            {
                t.did[v.Name] = false;
            }
            foreach (string key in dropDownManager.options)
            {
                t.hasItems[key] = false;
            }

            //Gettings block from FE
            t.blocks = GetActionBlocks(t.tabDropArea);

            t.simBlocks = new List<SimBlock>();
            t.simulationImages = new List<GameObject>();

            //General Variables
            stop = false;
            err = false;
            paused = false;
            lost = false;
        }


        //------------- Extract block sequence from FE ---------
        foreach (Thread t in threads)
        {
            int i = 0;
            foreach (Transform child in t.blocks)
            {

                if (child.GetComponent<Draggable>().typeOfItem == Draggable.Type.ACTION)
                {


                    // action block is a GET action
                    if (t.blocks[i].transform.GetComponentInChildren<Text>().text == "get")
                    {

                        string resource = t.blocks[i].transform.Find("Dropdown").Find("Label").GetComponent<Text>().text;
                        if (resource == "[null]")
                        {
                            terminateSimulation("Please select a resource to acquire in thread 1.");
                            manager.showError("Please select a resource to acquire in thread 1.");
                            return;

                        }
                        else
                        {
                            t.simBlocks.Add(new SimBlock(SimBlock.ACQUIIRE, resource));
                            //blocks_names_t1.Add("[thread 1] acquire ( " + resource + " );");
                            InputWorkerData inpt = new InputWorkerData { action = resource, typeOf = "Acquire" };
                            LogManager.instance.logger.sendInputWorkerOne(resource, "Acquire", LogManager.instance.UniEndTime().ToString());
                            GameLogData.inputList_t1.Add(inpt);
                            //						LogData.inputList_t1.Add ("Acquire: " + resource);

                            i++;

                            // create new object from prefab
                            GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = t.workerSprite;
                            newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/acquire");

                            Sprite item;

                            item = Resources.Load<Sprite>("sprites/items/" + resource);
                            newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                            newItem.transform.Find("ActionText").GetComponent<Text>().text = "get(" + resource + ");";
                            t.simulationImages.Add(newItem);
                        }

                        // action block is a RETURN action
                    }
                    else if (t.blocks[i].transform.GetComponentInChildren<Text>().text == "ret")
                    {

                        string resource = t.blocks[i].transform.Find("Dropdown").Find("Label").GetComponent<Text>().text;

                        if (resource == "[null]")
                        {
                            terminateSimulation("Please select a resource to return in thread 1.");
                            manager.showError("Please select a resource to return in thread 1.");
                            return;
                        }
                        else
                        {
                            t.simBlocks.Add(new SimBlock(SimBlock.RETURN, resource));
                            //blocks_names_t1.Add("[thread 1] return ( " + resource + " );");
                            InputWorkerData inpt = new InputWorkerData { action = resource, typeOf = "Return" };
                            LogManager.instance.logger.sendInputWorkerOne(resource, "Return", LogManager.instance.UniEndTime().ToString());
                            GameLogData.inputList_t1.Add(inpt);
                            //						LogData.inputList_t1.Add ("Return: " + resource);

                            i++;

                            // create new object from prefab
                            GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = t.workerSprite;
                            newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/return");

                            Sprite item;
                            item = Resources.Load<Sprite>("sprites/items/" + resource);
                            newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                            newItem.transform.Find("ActionText").GetComponent<Text>().text = "return(" + resource + ");";
                            t.simulationImages.Add(newItem);

                        }

                    }
                    else
                    {

                        String action = t.blocks[i].transform.GetComponentInChildren<Text>().text;
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
                            t.simBlocks.Add(new SimBlock(SimBlock.CHECKIN, ""));
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = t.workerSprite;
                            newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = t.dogSprite;
                            newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/acquire");

                        }
                        else if (action == "checkout")
                        {

                            // Debug.Log ("CHECKING OUT");
                            t.simBlocks.Add(new SimBlock(SimBlock.CHECKOUT, ""));
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = t.workerSprite;
                            newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = t.dogSprite;
                            newItem.transform.Find("AcqRet").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/return");

                        }
                        else
                        {

                            // create new object from prefab (single action)
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = t.dogSprite;
                            t.simBlocks.Add(new SimBlock(SimBlock.WORK, action));
                            Sprite item = Resources.Load<Sprite>("sprites/actions/" + action);
                            newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                        }
                        newItem.transform.Find("ActionText").GetComponent<Text>().text = action + ";";
                        t.simulationImages.Add(newItem);

                    }
                }
            }
        }
        if (threads[0].blocks.Length < 1)
        {

            manager.showError("There are no actions to run in thread 1.");
            terminateSimulation("There are no actions to run in thread 1.");
            return;
        }

        if (threads[1].blocks.Length < 1)
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
            StartCoroutine(printThreads(threads[0].simulationImages, threads[1].simulationImages, 5));
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
                   
                    g.SetActive(((Action)t.workList.GetType().GetField(g.name).GetValue(t.workList)).isneeded) ;
                    
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

                    g.SetActive(((Action)t.workList.GetType().GetField(g.name).GetValue(t.workList)).isneeded);

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
        threads[0].currIndex = 0;
        threads[1].currIndex = 0;

        threads[0].canPrint = true;
        threads[1].canPrint = true;

        int j = 0;

        while ((threads[0].currIndex < threads[0].simBlocks.Count) || (threads[1].currIndex < threads[1].simBlocks.Count))
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
                    //Debug.Log("spd: " + b1[threads[0].t_curr_index]);

                    // {"[null]", "brush" ,"clippers" , "cond.", "dryer", "scissors", "shampoo", "station", "towel"};

                    if (threads[0].simBlocks[threads[0].currIndex].type == SimBlock.ACQUIIRE)
                    {

                        // acquiring resource
                        switch (threads[0].simBlocks[threads[0].currIndex].name)
                        {

                            case "brush":

                                if (!threads[0].hasItems["brush"] && threads[1].hasItems["brush"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/brush");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for brush...</color>";
                                    newItem.transform.SetParent(threads[0].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[0].canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "brush");
                                    threads[0].canPrint = true;

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
                                    newItem.transform.SetParent(threads[0].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[0].canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "clippers");
                                    threads[0].canPrint = true;

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
                                    newItem.transform.SetParent(threads[0].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[0].canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "cond.");
                                    threads[0].canPrint = true;

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
                                    newItem.transform.SetParent(threads[0].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[0].canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "dryer");
                                    threads[0].canPrint = true;

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
                                    newItem.transform.SetParent(threads[0].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[0].canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "scissors");
                                    threads[0].canPrint = true;

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
                                    newItem.transform.SetParent(threads[0].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[0].canPrint = false;

                                }
                                else
                                {
                                    int output = acquire(ref threads[0].hasItems, "shampoo");
                                    threads[0].canPrint = true;

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
                                    newItem.transform.SetParent(threads[0].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[0].canPrint = false;

                                }
                                else
                                {
                                    int output = acquire(ref threads[0].hasItems, "station");
                                    threads[0].canPrint = true;

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
                                    newItem.transform.SetParent(threads[0].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[0].canPrint = false;

                                }
                                else
                                {

                                    int output = acquire(ref threads[0].hasItems, "towel");
                                    threads[0].canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, 1);
                                    }
                                }

                                break;
                        }

                    }
                    else if (threads[0].simBlocks[threads[0].currIndex].type == SimBlock.RETURN)
                    {

                        // returning resource
                        switch (threads[0].simBlocks[threads[0].currIndex].name)
                        {

                            case "brush":

                                int output1 = return_res(ref threads[0].hasItems, "brush");

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
                    else if (threads[0].simBlocks[threads[0].currIndex].type == SimBlock.WORK)
                    {
                        if (threads[0].simBlocks[threads[0].currIndex].name == "Cut")
                        {

                            if (!threads[0].hasItems["brush"] || !threads[0].hasItems["scissors"])
                            {

                                String actionText = s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s1[threads[0].currIndex].transform.SetParent(threads[0].layoutPanel.transform);
                                s1[threads[0].currIndex].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't cut without a brush and some scissors.", 1);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform cut
                                threads[0].did["Cut"] = true;
                            }
                        }
                        else if (threads[0].simBlocks[threads[0].currIndex].name == "Dry")
                        {

                            if (!threads[0].hasItems["station"] || !threads[0].hasItems["dryer"] || !threads[0].hasItems["towel"])
                            {

                                String actionText = s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s1[threads[0].currIndex].transform.SetParent(threads[0].layoutPanel.transform);
                                s1[threads[0].currIndex].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't dry without a station, a dryer and a towel.", 1);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform dry
                                threads[0].did["Dry"] = true;
                            }

                        }
                        else if (threads[0].simBlocks[threads[0].currIndex].name == "Wash")
                        {

                            if (!threads[0].hasItems["station"] || !threads[0].hasItems["shampoo"] || !threads[0].hasItems["towel"] || !threads[0].hasItems["cond."])
                            {

                                String actionText = s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s1[threads[0].currIndex].transform.SetParent(threads[0].layoutPanel.transform);
                                s1[threads[0].currIndex].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't wash without a station, shampoo, conditioner, and a towel.", 1);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform wash
                                threads[0].did["Wash"] = true;
                            }

                        }
                        else if (threads[0].simBlocks[threads[0].currIndex].name == "Groom")
                        {

                            if (!threads[0].hasItems["brush"] || !threads[0].hasItems["clippers"])
                            {

                                String actionText = s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s1[threads[0].currIndex].transform.SetParent(threads[0].layoutPanel.transform);
                                s1[threads[0].currIndex].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't groom without a brush and some nail clippers.", 1);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform groom
                                threads[0].did["Groom"] = true;
                            }

                        }
                    }
                    else if (threads[0].simBlocks[threads[0].currIndex].type == SimBlock.CHECKIN)
                    {

                        if (threads[0].isCheckedIn)
                        {

                            String actionText = s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                            s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s1[threads[0].currIndex].transform.SetParent(threads[0].layoutPanel.transform);
                            s1[threads[0].currIndex].transform.localScale = Vector3.one;

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
                    else if (threads[0].simBlocks[threads[0].currIndex].type == SimBlock.CHECKOUT)
                    {
                        foreach (KeyValuePair<string, bool> b in threads[0].needsTo)
                        {
                            Debug.Log(b.Key + b.Value + "<----");
                        }
                        if ((threads[0].needsTo["Cut"] && !threads[0].did["Cut"]) || (threads[0].needsTo["Dry"] && !threads[0].did["Dry"]) || (threads[0].needsTo["Wash"] && !threads[0].did["Wash"]) || (threads[0].needsTo["Groom"] && !threads[0].did["Groom"]))
                        {

                            String actionText = s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                            s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s1[threads[0].currIndex].transform.SetParent(threads[0].layoutPanel.transform);
                            s1[threads[0].currIndex].transform.localScale = Vector3.one;
                            scrollToBottom();

                            resError("> ERROR: Seems like worker 1 didn't fulfill all of the customer's requests. Please try again.", 1);
                            scrollToBottom();

                        }
                        else if (threads[0].hasItems["brush"] || threads[0].hasItems["clippers"] || threads[0].hasItems["cond."] || threads[0].hasItems["dryer"] || threads[0].hasItems["scissors"] || threads[0].hasItems["shampoo"] || threads[0].hasItems["station"] || threads[0].hasItems["towel"])
                        {

                            String actionText = s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                            s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s1[threads[0].currIndex].transform.SetParent(threads[0].layoutPanel.transform);
                            s1[threads[0].currIndex].transform.localScale = Vector3.one;

                            resError("> ERROR: You need to return all the resources you acquired before checking out.", 1);
                            scrollToBottom();

                        }
                        else if (threads[0].isCheckedOut)
                        {

                            String actionText = s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                            s1[threads[0].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s1[threads[0].currIndex].transform.SetParent(threads[0].layoutPanel.transform);
                            s1[threads[0].currIndex].transform.localScale = Vector3.one;

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

                    if (threads[0].canPrint)
                    {

                        if (!err)
                        {
                            s1[threads[0].currIndex].transform.SetParent(threads[0].layoutPanel.transform);
                            s1[threads[0].currIndex].transform.localScale = Vector3.one;
                        }
                        threads[0].currIndex++;
                    }
                    scrollToBottom();

                }
                catch { }

                scrollToBottom();

                // ------------------------------  THREAD 2 ------------------------------

                try
                {

                    // {"[null]", "brush" ,"clippers" , "cond.", "dryer", "scissors", "shampoo", "station", "towel"};

                    if (threads[1].simBlocks[threads[1].currIndex].type == SimBlock.ACQUIIRE)
                    {

                        // acquiring resource
                        switch (threads[1].simBlocks[threads[1].currIndex].name)
                        {

                            case "brush":

                                if (!threads[1].hasItems["brush"] && threads[0].hasItems["brush"])
                                { // need to wait for resource

                                    GameObject newItem = Instantiate(simulationImagePrefab) as GameObject;
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/brush");
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting for brush...</color>";
                                    newItem.transform.SetParent(threads[1].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[1].canPrint = false;

                                }
                                else
                                {

                                    int output1 = acquire(ref threads[1].hasItems, "brush");
                                    threads[1].canPrint = true;

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
                                    newItem.transform.SetParent(threads[1].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[1].canPrint = false;

                                }
                                else
                                {

                                    int output2 = acquire(ref threads[1].hasItems, "clippers");
                                    threads[1].canPrint = true;

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
                                    newItem.transform.SetParent(threads[1].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[1].canPrint = false;

                                }
                                else
                                {

                                    int output3 = acquire(ref threads[1].hasItems, "cond.");
                                    threads[1].canPrint = true;

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
                                    newItem.transform.SetParent(threads[1].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[1].canPrint = false;

                                }
                                else
                                {

                                    int output4 = acquire(ref threads[1].hasItems, "dryer");
                                    threads[1].canPrint = true;

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
                                    newItem.transform.SetParent(threads[1].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[1].canPrint = false;

                                }
                                else
                                {

                                    int output5 = acquire(ref threads[1].hasItems, "scissors");
                                    threads[1].canPrint = true;

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
                                    newItem.transform.SetParent(threads[1].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[1].canPrint = false;

                                }
                                else
                                {

                                    int output6 = acquire(ref threads[1].hasItems, "shampoo");
                                    threads[1].canPrint = true;

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
                                    newItem.transform.SetParent(threads[1].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[1].canPrint = false;

                                }
                                else
                                {

                                    int output7 = acquire(ref threads[1].hasItems, "station");
                                    threads[1].canPrint = true;

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
                                    newItem.transform.SetParent(threads[1].layoutPanel.transform);
                                    newItem.transform.localScale = Vector3.one;
                                    scrollToBottom();

                                    threads[1].canPrint = false;

                                }
                                else
                                {
                                    int output8 = acquire(ref threads[1].hasItems, "towel");
                                    threads[1].canPrint = true;

                                    if (output8 < 0)
                                    {
                                        resError(acquireErrMsg, 2);
                                    }
                                }

                                break;
                        }

                    }
                    else if (threads[1].simBlocks[threads[1].currIndex].type == SimBlock.RETURN)
                    {

                        // returning resource
                        switch (threads[1].simBlocks[threads[1].currIndex].name)
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
                    else if (threads[1].simBlocks[threads[1].currIndex].type == SimBlock.WORK)
                    {
                        if (threads[1].simBlocks[threads[1].currIndex].name == "Cut")
                        {

                            if (!threads[1].hasItems["brush"] || !threads[1].hasItems["scissors"])
                            {

                                String actionText = s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s2[threads[1].currIndex].transform.SetParent(threads[1].layoutPanel.transform);
                                s2[threads[1].currIndex].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't cut without a brush and some scissors.", 2);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform cut
                                threads[1].did["Cut"] = true;
                            }

                        }
                        else if (threads[1].simBlocks[threads[1].currIndex].name == "Dry")
                        {

                            if (!threads[1].hasItems["station"] || !threads[1].hasItems["dryer"] || !threads[1].hasItems["towel"])
                            {

                                String actionText = s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s2[threads[1].currIndex].transform.SetParent(threads[1].layoutPanel.transform);
                                s2[threads[1].currIndex].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't dry without a station, a dryer and a towel.", 2);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform dry
                                threads[1].did["Dry"] = true;
                            }

                        }
                        else if (threads[1].simBlocks[threads[1].currIndex].name == "Wash")
                        {

                            if (!threads[1].hasItems["station"] || !threads[1].hasItems["shampoo"] || !threads[1].hasItems["towel"] || !threads[1].hasItems["cond."])
                            {

                                String actionText = s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s2[threads[1].currIndex].transform.SetParent(threads[1].layoutPanel.transform);
                                s2[threads[1].currIndex].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't wash without a station, shampoo, conditioner, and a towel.", 2);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform wash
                                threads[1].did["Wash"] = true;
                            }

                        }
                        else if (threads[1].simBlocks[threads[1].currIndex].name == "Groom")
                        {

                            if (!threads[1].hasItems["brush"] || !threads[1].hasItems["clippers"])
                            {

                                String actionText = s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                s2[threads[1].currIndex].transform.SetParent(threads[1].layoutPanel.transform);
                                s2[threads[1].currIndex].transform.localScale = Vector3.one;

                                resError("> ERROR: You can't groom without a brush and some nail clippers.", 2);
                                scrollToBottom();

                            }
                            else
                            {

                                // perform groom
                                threads[1].did["Groom"] = true;
                            }

                        }
                    }
                    else if (threads[1].simBlocks[threads[0].currIndex].type == SimBlock.CHECKIN)
                    {

                        if (threads[1].isCheckedIn)
                        {

                            String actionText = s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                            s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s2[threads[1].currIndex].transform.SetParent(threads[1].layoutPanel.transform);
                            s2[threads[1].currIndex].transform.localScale = Vector3.one;

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
                    else if (threads[1].simBlocks[threads[0].currIndex].type == SimBlock.CHECKOUT)
                    {

                        if ((threads[1].needsTo["Cut"] && !threads[1].did["Cut"]) || (threads[1].needsTo["Dry"] && !threads[1].did["Dry"]) || (threads[1].needsTo["Wash"] && !threads[1].did["Wash"]) || (threads[1].needsTo["Groom"] && !threads[1].did["Groom"]))
                        {

                            String actionText = s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                            s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s2[threads[1].currIndex].transform.SetParent(threads[1].layoutPanel.transform);
                            s2[threads[1].currIndex].transform.localScale = Vector3.one;
                            scrollToBottom();

                            resError("> ERROR: Seems like worker 2 didn't fulfill all of the customer's requests. Please try again.", 2);
                            scrollToBottom();

                        }
                        else if (threads[1].hasItems["brush"] || threads[1].hasItems["clippers"] || threads[1].hasItems["cond."] || threads[1].hasItems["dryer"] || threads[1].hasItems["scissors"] || threads[1].hasItems["shampoo"] || threads[1].hasItems["station"] || threads[1].hasItems["towel"])
                        {

                            String actionText = s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                            s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s2[threads[1].currIndex].transform.SetParent(threads[1].layoutPanel.transform);
                            s2[threads[1].currIndex].transform.localScale = Vector3.one;

                            resError("> ERROR: You need to return all the resources you acquired before checking out.", 2);
                            scrollToBottom();

                        }
                        else if (threads[1].isCheckedOut)
                        {

                            String actionText = s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                            s2[threads[1].currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                            s2[threads[1].currIndex].transform.SetParent(threads[1].layoutPanel.transform);
                            s2[threads[1].currIndex].transform.localScale = Vector3.one;

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

                    if (threads[1].canPrint)
                    {
                        if (!err)
                        {
                            s2[threads[1].currIndex].transform.SetParent(threads[1].layoutPanel.transform);
                            s2[threads[1].currIndex].transform.localScale = Vector3.one;
                        }

                        threads[1].currIndex++;
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
            newItemParent = threads[0].layoutPanel.transform;
        else
            newItemParent = threads[1].layoutPanel.transform;

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

        //threads[0].layoutPanel
        foreach (Transform child in threads[0].layoutPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //threads[1].layoutPanel
        foreach (Transform child in threads[1].layoutPanel.transform)
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