using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class DebugExe : MonoBehaviour
{
    public class ExeData
    {
        public bool doIdleBeforeSOI;
        public List<byte> sequence = new List<byte>();
        public List<bool> isIdle = new List<bool>();
    }


    public List<Thread> threads;
    private dropDownManager dropDownManager = new dropDownManager();
    [SerializeField] private bool isRetAllCompulsion;


    // --- For Dynamic UI ---
    private GameObject tab;
    private GameObject label;
    private GameObject agenda;
    private GameObject agendaTick;
    private GameObject board;

    [TextArea(5, 20)][SerializeField] private string descriptionText;
    [TextArea(5, 20)] [SerializeField] private string bubbleText;
    [SerializeField] private int idleMomentPercent;
    [SerializeField] private int stepOfInterest;



    // --- IMAGE SIMULATION ---

    [SerializeField] private GameObject scrollRect;
    [SerializeField] private Transform iconPanel;

    // ---- Simulation ----
    [SerializeField] private int timeout;
    [SerializeField] private int NoOfTestCase;


    // ----- Prefab ------
    private GameObject actionSimulationImagePrefab;
    private GameObject singleSimulationImagePrefab;
    private GameObject simulationErrorPrefab;



    [SerializeField] private Text stepsIndicator;


    // ------- Tool box value text object--------
    [SerializeField] private Text txt_checkinLeft_thread;
    [SerializeField] private Text txt_cutLeft_thread;
    [SerializeField] private Text txt_dryLeft_thread;
    [SerializeField] private Text txt_washLeft_thread;
    [SerializeField] private Text txt_resourcesLeft_thread;
    [SerializeField] private Text txt_checkoutLeft_thread;
    [SerializeField] private Text txt_returnLeft_thread;
    [SerializeField] private Text txt_groomLeft_thread;

    //-------- Audio ----------
    private AudioSource audioSource;
    private AudioClip wonClip, gameoverClip;



    ToolboxManager manager;
    GameObject disablePanel;
    ProgressBar bar;
    ScrollRect simulationScrollRect;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject stopButton;
    [SerializeField] private GameObject nextButton;

    bool stop;
    bool err;
    bool paused;
    bool lost;


    string returnErrMsg = "> ERROR: You are trying to return a resource you don't have.";
    string acquireErrMsg = "> ERROR: You are trying to acquire a resource you already have.";

   

    public void updateValues(int index)
    {

        txt_checkinLeft_thread.text = "x " + threads[index].toolBoxValues.CheckInBox;
        txt_cutLeft_thread.text = "x " + threads[index].toolBoxValues.CutBox;
        txt_washLeft_thread.text = "x " + threads[index].toolBoxValues.WashBox;
        txt_dryLeft_thread.text = "x " + threads[index].toolBoxValues.DryBox;
        txt_resourcesLeft_thread.text = "x " + threads[index].toolBoxValues.ResourceBox;
        txt_checkoutLeft_thread.text = "x " + threads[index].toolBoxValues.CheckOutBox;
        txt_returnLeft_thread.text = "x " + threads[index].toolBoxValues.ReturnBox;
        txt_groomLeft_thread.text = "x " + threads[index].toolBoxValues.BrushBox;

        System.Reflection.FieldInfo[] boxList = threads[index].toolBoxValues.GetType().GetFields();
        foreach (System.Reflection.FieldInfo bl in boxList)
        {
            if ((int)bl.GetValue(threads[index].toolBoxValues) == 0)
            {
                GameObject.Find(bl.Name).GetComponent<CanvasGroup>().alpha = 0.5f;
            }
            else
            {
                GameObject.Find(bl.Name).GetComponent<CanvasGroup>().alpha = 1;
            }
        }
    }
    void Start()
    {
        // -------- Initialize Description text --------
        GameObject.Find("InstructionsPanel").transform.Find("Part2").Find("Background").GetChild(0).GetChild(0).GetComponent<Text>().text = descriptionText;
        GameObject.Find("InstructionsPanel").transform.Find("Part1").Find("SpeechBox").GetChild(0).GetComponent<Text>().text = bubbleText;
        GameObject.Find("InstructionsPanel").transform.Find("Part1").Find("Level").GetComponent<Text>().text = SceneManager.GetActiveScene().name;


        // --------Intialize Prefabs -------
        actionSimulationImagePrefab = Resources.Load<GameObject>("prefabs/ActionSim");
        singleSimulationImagePrefab = Resources.Load<GameObject>("prefabs/singleIconSimulation");
        simulationErrorPrefab = Resources.Load<GameObject>("prefabs/ErrorSimulationImage");

        //Fill needsto Dict and assign individual audiosource 
        int i = 0;
        foreach (Thread t in threads)
        {
            System.Reflection.FieldInfo[] varWorklist = t.workList.GetType().GetFields();
            foreach (System.Reflection.FieldInfo v in varWorklist)
            {
                t.needsTo.Add(v.Name, ((Action)v.GetValue(t.workList)).isneeded);
            }

            //---Assign Audio Source to each thread ----
            t.audioSource = GameObject.Find("Simulation").transform.Find("ScrollRect").Find("Panel").GetChild(i).GetComponent<AudioSource>();
            t.audioSource.volume = 0.5f;
            i++;
        }

        //Assign Values
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
        //--- Set Audio Vars-------
        audioSource = gameObject.GetComponent<AudioSource>();
        wonClip = Resources.Load<AudioClip>("audio/won");
        gameoverClip = Resources.Load<AudioClip>("audio/gameOver");

        tab = Resources.Load<GameObject>("prefabs/Tab");
        agenda = Resources.Load<GameObject>("prefabs/Agenda");
        agendaTick = Resources.Load<GameObject>("prefabs/Tick");
        label = Resources.Load<GameObject>("prefabs/Label");
        board = Resources.Load<GameObject>("prefabs/Board");


        //enable Play button
        CloseBtn(stopButton);
        OpenBtn(playButton);


        int count = 0;
        System.Random r = new System.Random();
        UnityEngine.Object[] s = Resources.LoadAll("sprites/workers", typeof(Sprite));
        foreach (int i in Enumerable.Range(0, s.Length).OrderBy(x => r.Next()))
        {
            if (count < threads.Count)
            {
                threads[count].workerName = s[i].name;
                threads[count].workerSprite = (Sprite)s[i];
            }
            count++;
        }

        count = 0;
        UnityEngine.Object[] d = Resources.LoadAll("sprites/dogs", typeof(Sprite));
        foreach (int i in Enumerable.Range(0, d.Length).OrderBy(x => r.Next()))
        {
            if (count < threads.Count)
            {
                threads[count].dogName = d[i].name;
                threads[count].dogSprite = (Sprite)d[i];
            }
            count++;
        }

        AddTabs();
        updateValues(0);
    }

    private void AddTabs()
    {
        int count=0;


        //Just caution so we won't get null ref error
        GameObject.Find("Canvas").transform.Find("InstructionsPanel").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("AgendaPanel").gameObject.SetActive(true);

        // --------Intialize Prefabs -------
        Transform tabParent = GameObject.Find("TabParent").transform;
        Transform labelParent = GameObject.Find("LabelParent").transform;
        Transform agendaParent = GameObject.Find("AgendaParent").transform;
        Transform agendaTickParent = GameObject.Find("AgendaTickParent").transform;
        Transform boardParent = GameObject.Find("boardParent").transform;
        iconPanel = GameObject.Find("IconPanel").transform;

        GameObject simPanel = Resources.Load<GameObject>("prefabs/ThreadSimPanel");
        GameObject icon = Resources.Load<GameObject>("prefabs/icon");

        foreach (Thread t in threads)
        {
            //------- Add Tab --------
            GameObject tabtemp = Instantiate(tab);
            tabtemp.transform.SetParent(tabParent,false);
            tabtemp.name = "Tab"+count.ToString();


            //------- Add Sim Panel --------
            GameObject layoutTemp = Instantiate(simPanel);
            layoutTemp.transform.SetParent(scrollRect.transform.Find("Panel"), false);
            t.layoutPanel = layoutTemp;


            //------- Add Sim icon --------
            GameObject iconTemp = Instantiate(icon);
            iconTemp.transform.SetParent(iconPanel, false);
            iconTemp.transform.Find("Image").GetComponent<Image>().sprite = t.workerSprite;


            //------- Add Label --------
            GameObject labeltemp = Instantiate(label);
            labeltemp.transform.SetParent(labelParent, false);
            labeltemp.transform.GetChild(0).GetComponent<SwitchTab>().index = count;
            labeltemp.transform.GetChild(0).GetComponent<SwitchTab>().totalCount = threads.Count;    
            labeltemp.name = "Label"+count.ToString();
            labeltemp.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = t.workerName;
            labeltemp.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = t.workerSprite;

            t.tabDropArea = tabtemp.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;

            //------- Add Agenda --------
            GameObject agendatemp = Instantiate(agenda);
            agendatemp.transform.SetParent(agendaParent, false);
            agendatemp.name = "Agenda" + count.ToString();
            agendatemp.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = t.dogSprite;
            agendatemp.transform.Find("Name-fillin").GetComponent<Text>().text = t.dogName;


            //------- Add Agenda Tick --------
            GameObject agendaTicktemp = Instantiate(agendaTick);
            agendaTicktemp.transform.SetParent(agendaTickParent, false);
            agendaTicktemp.name = "AgendaTick" + count.ToString();
            Color c = Random.ColorHSV(0f, 1f, 0.2f, 0.7f, 0.9f, 1f);
            agendatemp.transform.GetChild(0).GetComponent<Image>().color = c;
            if (count == 0)
            {
                agendaTicktemp.GetComponent<Image>().color = c;
            }
            else
            {
                agendaTicktemp.GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 0);
            }
            agendaTicktemp.transform.GetChild(0).GetComponent<Image>().color = c;
            agendaTicktemp.transform.GetChild(1).GetComponent<Image>().sprite = t.workerSprite;
            agendaTicktemp.GetComponent<SwitchTab>().index = count;
            agendaTicktemp.GetComponent<SwitchTab>().totalCount = threads.Count;


            //--------- Add Board ----------
            GameObject boardTemp = Instantiate(board);
            boardTemp.transform.SetParent(boardParent, false);
            boardTemp.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = t.dogSprite;
            boardTemp.transform.GetChild(0).Find("Name-fillin").GetComponent<Text>().text = t.dogName;
            boardTemp.transform.Find("WorkerName").GetComponent<Text>().text = t.workerName;


            // ------- Enable inner and outer tick --------
            System.Reflection.FieldInfo[] varWorklist = t.workList.GetType().GetFields();
            foreach (System.Reflection.FieldInfo v in varWorklist)
            {
                boardTemp.transform.GetChild(0).Find(v.Name).gameObject.SetActive(((Action)v.GetValue(t.workList)).isneeded);
                agendatemp.transform.Find(v.Name).gameObject.SetActive(((Action)v.GetValue(t.workList)).isneeded);
            }

            count++;
        }
        GameObject.Find("Tab0").transform.SetAsLastSibling();
        GameObject.Find("Agenda0").transform.SetAsLastSibling();
        GameObject.Find("Label0").transform.GetChild(0).GetComponent<Image>().color = new Vector4(0.9F, 0.9F, 0.9F, 1);
        GameObject.Find("Label0").transform.GetChild(0).Find("Text (TMP)").gameObject.SetActive(true);
        GameObject.Find("Label0").GetComponent<LayoutElement>().preferredWidth = 1000;
        GameObject.Find("Canvas").transform.Find("AgendaPanel").gameObject.SetActive(false);


        //Disable SimIcons till execute
        iconPanel.gameObject.SetActive(false);
    }

    public void ExecuteThreads()
    {
        if (!isAllowed(playButton))
        {
            return;
        }
       
        //-------- UI Updates and Logging --------
        LogManager.instance.logger.sendChronologicalLogs("RunLevel03Thread", "", LogManager.instance.UniEndTime().ToString());
        scrollToTop();
        scrollToLeft();
        scrollToRight();
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
        CloseBtn(playButton);
        OpenBtn(stopButton);
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
            ResetThreads();

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
                            terminateSimulation(t.workerName + " has not selected resource in get");
                            manager.showError(t.workerName + " has not selected resource in get");
                            return;
                        }
                        else
                        {
                            t.simBlocks.Add(new SimBlock(SimBlock.ACQUIIRE, resource));
                            InputWorkerData inpt = new InputWorkerData { action = resource, typeOf = "Acquire" };
                            LogManager.instance.logger.sendInputWorkerOne(resource, "Acquire", LogManager.instance.UniEndTime().ToString());
                            GameLogData.inputList_t1.Add(inpt);

                            i++;

                            // create new object from prefab
                            GameObject newItem = Instantiate(singleSimulationImagePrefab) as GameObject;
                            newItem.GetComponent<Image>().color = new Color32(239, 71, 239, 141);
                            Sprite item;

                            item = Resources.Load<Sprite>("sprites/items/" + resource);
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = item;
                            newItem.transform.Find("ActionText").GetComponent<Text>().text = "get";
                            t.simulationImages.Add(newItem);
                        }
                        // action block is a RETURN action
                    }
                    else if (t.blocks[i].transform.GetComponentInChildren<Text>().text == "ret")
                    {

                        string resource = t.blocks[i].transform.Find("Dropdown").Find("Label").GetComponent<Text>().text;

                        if (resource == "[null]")
                        {
                            terminateSimulation(t.workerName+" has not selected resource in return");
                            manager.showError(t.workerName + " has not selected resource in return");
                            return;
                        }
                        else
                        {
                            t.simBlocks.Add(new SimBlock(SimBlock.RETURN, resource));
                            InputWorkerData inpt = new InputWorkerData { action = resource, typeOf = "Return" };
                            LogManager.instance.logger.sendInputWorkerOne(resource, "Return", LogManager.instance.UniEndTime().ToString());
                            GameLogData.inputList_t1.Add(inpt);

                            i++;

                            // create new object from prefab
                            GameObject newItem = Instantiate(singleSimulationImagePrefab) as GameObject;
                            newItem.GetComponent<Image>().color = new Color32(144, 208, 113, 141);
                            Sprite item;
                            item = Resources.Load<Sprite>("sprites/items/" + resource);
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = item;
                            newItem.transform.Find("ActionText").GetComponent<Text>().text = "return";
                            t.simulationImages.Add(newItem);

                        }

                    }
                    else
                    {

                        String action = t.blocks[i].transform.GetComponentInChildren<Text>().text;
                        InputWorkerData inpt = new InputWorkerData { action = action, typeOf = "Action" };
                        LogManager.instance.logger.sendInputWorkerOne(action, "Action", LogManager.instance.UniEndTime().ToString());
                        GameLogData.inputList_t1.Add(inpt);

                        i++;


                        GameObject newItem;
                        if (action == "checkin")
                        {
                            newItem = Instantiate(singleSimulationImagePrefab) as GameObject;
                            newItem.GetComponent<Image>().color = new Color32(255, 196, 61, 141);
                            t.simBlocks.Add(new SimBlock(SimBlock.CHECKIN, ""));
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = t.dogSprite;
                            newItem.transform.Find("ActionText").GetComponent<Text>().text = "check-in";


                        }
                        else if (action == "checkout")
                        {
                            newItem = Instantiate(singleSimulationImagePrefab) as GameObject;
                            newItem.GetComponent<Image>().color = new Color32(255, 196, 61, 141);
                            t.simBlocks.Add(new SimBlock(SimBlock.CHECKOUT, ""));
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = t.dogSprite;
                            newItem.transform.Find("ActionText").GetComponent<Text>().text = "check-out";


                        }
                        else
                        {
                            newItem = Instantiate(actionSimulationImagePrefab) as GameObject;
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = t.dogSprite;
                            t.simBlocks.Add(new SimBlock(SimBlock.WORK, action));
                            Sprite item = Resources.Load<Sprite>("sprites/actions/" + action);
                            newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = item;
                            newItem.transform.Find("ActionText").GetComponent<Text>().text = action;
                        }
                        
                        t.simulationImages.Add(newItem);
                    }
                }
            }
            if (t.blocks.Length < 1)
            {

                manager.showError(t.workerName+" does not have any actions");
                terminateSimulation(t.workerName + " does not have any actions");
                return;
            }

            try
            {
                if (t.simBlocks[0].type != SimBlock.CHECKIN)
                {
                    manager.showError(t.workerName + " has not check-in the customer");
                    terminateSimulation(t.workerName + " has not check-in the customer");
                    return;
                }
                if (t.simBlocks[t.simBlocks.Count-1].type != SimBlock.CHECKOUT)
                {
                    manager.showError(t.workerName + " has not check-out the customer");
                    terminateSimulation(t.workerName + " has not check-out the customer");
                    return;
                }
            }
            catch
            {
                
            }
      
        }
        if (!err)
        {
            iconPanel.gameObject.SetActive(true);
            StartCoroutine(printThreads());
        }
    }

    private void ResetThreads()
    {
        foreach (Thread t in threads)
        {
            t.isCheckedIn = false;
            t.isCheckedOut = false;
            t.currIndex = 0;
            t.canPrint = true;
            t.didIdle = false;
        }
    }

    private void OpenBtn(GameObject btn)
    {
        btn.transform.Find("overlay").gameObject.SetActive(false);
        btn.transform.GetComponent<Button>().interactable = true;
    }

    private void CloseBtn(GameObject btn)
    {
        btn.transform.Find("overlay").gameObject.SetActive(true);
        btn.transform.GetComponent<Button>().interactable = false;
    }

    IEnumerator printThreads()
    {

        for(int ittr = 0; ittr< NoOfTestCase;ittr++)
        {
            ExeData exeData = CheckAllPossibility();
            if(exeData == null)
            {
                break;
            }
            Debug.Log(ittr + " TestCase Pass");
        }

        bar.currentAmount = 0;


        foreach (Thread t in threads)
        {
            t.currIndex = 0;
            t.canPrint = true;
        }

        int j = 0;
        bool whileStop = false;

        //SOI: Step of Interest
        bool doIdleBeforeSOI = (Random.Range(0, 100) < stepOfInterest * idleMomentPercent * threads.Count);

        Debug.Log("Do Idle" + doIdleBeforeSOI);
        while (!whileStop)
        {
            
            whileStop = true;
            foreach (Thread t in threads)
            {
                if(t.currIndex<t.simBlocks.Count)
                {
                    whileStop = false;
                    break;
                }
            }

            if (bar.currentAmount < timeout)
            {
                bar.currentAmount += 1;
                bar.LoadingBar.GetComponent<Image>().fillAmount = bar.currentAmount / timeout;

            }
            else
            {

                LogManager.instance.logger.sendChronologicalLogs("Level03Lost", "", LogManager.instance.UniEndTime().ToString());
                manager.gameLost();
                audioSource.clip = gameoverClip;
                audioSource.Play();
                //------- logging -----------
                GameLogData.isLevelCleared = false;
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
                CloseBtn(stopButton);
                OpenBtn(playButton);
                
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
                    CloseBtn(stopButton);
                    OpenBtn(playButton);
                }
                bar.LoadingBar.GetComponent<Image>().fillAmount = 0;
                break;
            }
            else
            {

                stepsIndicator.text = "" + (j + 1);

                System.Random r = new System.Random();
                foreach (int i in Enumerable.Range(0, threads.Count).OrderBy(x => r.Next()))
                {
                    int idleInt = Random.Range(0, 100);
                    Thread t = threads[i];
                    bool isIdle;
                    if (t.currIndex<stepOfInterest && t.currIndex!=0)
                    {
                        if (doIdleBeforeSOI)
                        {
                            if (!t.didIdle)
                            {
                                isIdle = (idleInt < 100 / (stepOfInterest - t.currIndex));
                                Debug.Log("Percentage" + (100 / (stepOfInterest - t.currIndex)));
                                if (isIdle)
                                    t.didIdle = true;
                            }
                            else
                            {
                                isIdle = false;
                            }
                        }
                        else
                        {
                            isIdle = false;
                        }

                    }
                    else
                    {
                        isIdle = (idleInt < idleMomentPercent) && t.currIndex!=0;
                    }
            
                    if (!isIdle)
                    {
                        try
                        {
                            //------------ Acquire -------------
                            if (t.simBlocks[t.currIndex].type == SimBlock.ACQUIIRE)
                            {
                                //If t don't have the obj but some other thread has it then it will return true;
                                if (MeNotSomeOneHas(t.simBlocks[t.currIndex].name, t))
                                {
                                    GameObject newItem = Instantiate(actionSimulationImagePrefab) as GameObject;
                                    newItem.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                                    newItem.transform.Find("ItemAction").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/actions/waiting");
                                    newItem.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/items/" + t.simBlocks[t.currIndex].name);
                                    newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>Waiting...</color>";
                                    newItem.transform.SetParent(t.layoutPanel.transform);
                                    newItem.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                                    scrollToBottom();

                                    t.canPrint = false;
                                }
                                else
                                {
                                    int output = acquire(ref t.hasItems, t.simBlocks[t.currIndex].name);
                                    t.canPrint = true;

                                    if (output < 0)
                                    {
                                        resError(acquireErrMsg, t.layoutPanel); // ERROR: You are trying to acquire a resource you already have.";
                                        t.audioSource.clip = Resources.Load<AudioClip>("audio/error");
                                        t.audioSource.Play();
                                    }
                                }
                            }
                            //------------ Return -------------
                            else if (t.simBlocks[t.currIndex].type == SimBlock.RETURN)
                            {
                                int output1 = return_res(ref t.hasItems, t.simBlocks[t.currIndex].name);

                                if (output1 < 0)
                                {
                                    resError(returnErrMsg, t.layoutPanel);
                                    t.audioSource.clip = Resources.Load<AudioClip>("audio/error");
                                    t.audioSource.Play();
                                }
                            }
                            //------------ Work/Action block -------------
                            else if (t.simBlocks[t.currIndex].type == SimBlock.WORK)
                            {
                                if (IHaveAllThings(t.simBlocks[t.currIndex].name, t))
                                {
                                    t.did[t.simBlocks[t.currIndex].name] = true;
                                    //play music
                                    try
                                    { 
                                        t.audioSource.clip = Resources.Load<AudioClip>("audio/"+ t.simBlocks[t.currIndex].name);
                                        t.audioSource.Play();
                                        StartCoroutine(PauseSound(t.audioSource,2));
                                    }
                                    catch { }
                                }
                                else
                                {
                                    String actionText = t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                    t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                    t.simulationImages[t.currIndex].transform.SetParent(t.layoutPanel.transform);
                                    t.simulationImages[t.currIndex].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                                    resError("> ERROR: You can't " + t.simBlocks[t.currIndex].name.ToLower() + " without " + RequirementList(t.simBlocks[t.currIndex].name, t), t.layoutPanel);
                                    t.audioSource.clip = Resources.Load<AudioClip>("audio/error");
                                    t.audioSource.Play();
                                    scrollToBottom();

                                }
                            }
                            else if (t.simBlocks[t.currIndex].type == SimBlock.CHECKIN)
                            {

                                if (t.isCheckedIn)
                                {

                                    String actionText = t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                    t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                    t.simulationImages[t.currIndex].transform.SetParent(t.layoutPanel.transform);
                                    t.simulationImages[t.currIndex].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                                    resError("> ERROR: You are already checked in. You have to check out before attempting to check in a different customer.", t.layoutPanel);
                                    t.audioSource.clip = Resources.Load<AudioClip>("audio/error");
                                    t.audioSource.Play();
                                    scrollToBottom();

                                }
                                else
                                {

                                    // perform check-in
                                    t.isCheckedIn = true;
                                    t.isCheckedOut = false;
                                }

                            }
                            else if (t.simBlocks[t.currIndex].type == SimBlock.CHECKOUT)
                            {
                                foreach (KeyValuePair<string, bool> k in t.needsTo)
                                {
                                    if (k.Value && !t.did[k.Key])
                                    {
                                        String actionText = t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                        t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                        t.simulationImages[t.currIndex].transform.SetParent(t.layoutPanel.transform);
                                        t.simulationImages[t.currIndex].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                                        scrollToBottom();

                                        resError("> ERROR: Seems like worker 1 didn't fulfill all of the customer's requests. Please try again.", t.layoutPanel);
                                        t.audioSource.clip = Resources.Load<AudioClip>("audio/error");
                                        t.audioSource.Play();
                                        scrollToBottom();
                                        break;
                                    }
                                }
                                if (isRetAllCompulsion)
                                {
                                    foreach (KeyValuePair<string, bool> k in t.hasItems)
                                    {
                                        if (k.Value)
                                        {
                                            String actionText = t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                            t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                            t.simulationImages[t.currIndex].transform.SetParent(t.layoutPanel.transform);
                                            t.simulationImages[t.currIndex].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                                            resError("> ERROR: You need to return all the resources you acquired before checking out.", t.layoutPanel);
                                            t.audioSource.clip = Resources.Load<AudioClip>("audio/error");
                                            t.audioSource.Play();
                                            scrollToBottom();
                                            break;
                                        }
                                    }
                                }
                                else if (t.isCheckedOut)
                                {

                                    String actionText = t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text;
                                    t.simulationImages[t.currIndex].transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + actionText + "</color>";
                                    t.simulationImages[t.currIndex].transform.SetParent(t.layoutPanel.transform);
                                    t.simulationImages[t.currIndex].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                                    resError("> ERROR: You have to check in before attempting to check out a customer.", t.layoutPanel);
                                    t.audioSource.clip = Resources.Load<AudioClip>("audio/error");
                                    t.audioSource.Play();
                                    scrollToBottom();

                                }
                                else
                                {

                                    // perform check-out
                                    t.isCheckedIn = false;
                                    t.isCheckedOut = true;
                                }
                            }

                        }
                        catch { }

                        try
                        {

                            if (t.canPrint)
                            {

                                if (!err)
                                {
                                    t.simulationImages[t.currIndex].transform.SetParent(t.layoutPanel.transform);
                                    t.simulationImages[t.currIndex].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                                }
                                t.currIndex++;
                            }


                        }
                        catch { }
                    }
                    else if(t.currIndex<t.simBlocks.Count)
                    {

                        //---------------- IDLE -----------------
                        try
                        {

                            GameObject newItem = Instantiate(singleSimulationImagePrefab) as GameObject;
                           
                            UnityEngine.Object[] s = Resources.LoadAll("sprites/items/idle", typeof(Sprite));

                            int randomIndex = Random.Range(0, s.Length);
                            newItem.transform.Find("Icon").GetComponent<Image>().sprite = (Sprite)s[randomIndex];
                            newItem.transform.Find("ActionText").GetComponent<Text>().text = "Busy";
                            newItem.transform.SetParent(t.layoutPanel.transform);
                            newItem.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                        }
                        catch
                        {
                            
                        }
                    }


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
            audioSource.clip = wonClip;
            audioSource.Play();
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

    ExeData CheckAllPossibility()
    {
        ResetThreads();
        ExeData exeData = new ExeData();

        float currentAmount = 0;
        

        foreach (Thread t in threads)
        {
            t.currIndex = 0;
            t.canPrint = true;
        }

        int j = 0;
        bool whileStop = false;
        bool lost = false;


        bool doIdleBeforeSOI = (Random.Range(0, 100) < stepOfInterest * idleMomentPercent * threads.Count);
        exeData.doIdleBeforeSOI = doIdleBeforeSOI;
        
        while (!whileStop && !lost)
        {

            whileStop = true;
            foreach (Thread t in threads)
            {
                if (t.currIndex < t.simBlocks.Count)
                {
                    whileStop = false;
                    break;
                }
            }

            if (currentAmount < timeout)
            {
                currentAmount += 1;
            }
            else
            {
                //Send Lost game Posibility
                return exeData;
            }



            System.Random r = new System.Random();
            foreach (int i in Enumerable.Range(0, threads.Count).OrderBy(x => r.Next()))
            {
                exeData.sequence.Add((byte)i);
                int idleInt = Random.Range(0, 100);
                Thread t = threads[i];
                bool isIdle;
                if (t.currIndex < stepOfInterest && t.currIndex != 0)
                {
                    if (doIdleBeforeSOI)
                    {
                        if (!t.didIdle)
                        {
                            isIdle = (idleInt < 100 / (stepOfInterest - t.currIndex));
                            if (isIdle)
                                t.didIdle = true;
                        }
                        else
                        {
                            isIdle = false;
                        }
                    }
                    else
                    {
                        isIdle = false;
                    }

                }
                else
                {
                    isIdle = (idleInt < idleMomentPercent) && t.currIndex != 0;
                }
                exeData.isIdle.Add(isIdle);
                if (!isIdle)
                {

                    //------------ Acquire -------------
                    if (t.simBlocks[t.currIndex].type == SimBlock.ACQUIIRE)
                    {
                        //If I don't have the obj but some other thread has it then it will return true;
                        if (MeNotSomeOneHas(t.simBlocks[t.currIndex].name, t))
                        {

                            //-- Waiting -- 

                            t.canPrint = false;
                        }
                        else
                        {
                            int output = acquire(ref t.hasItems, t.simBlocks[t.currIndex].name);
                            t.canPrint = true;

                            if (output < 0)
                            {

                                // --- You Already have it

                                lost = true;

                            }
                        }
                    }
                    //------------ Return -------------
                    else if (t.simBlocks[t.currIndex].type == SimBlock.RETURN)
                    {
                        int output1 = return_res(ref t.hasItems, t.simBlocks[t.currIndex].name);

                        if (output1 < 0)
                        {
                            //--- You don't have the resource

                            lost = true;

                        }
                    }
                    //------------ Work/Action block -------------
                    else if (t.simBlocks[t.currIndex].type == SimBlock.WORK)
                    {
                        if (IHaveAllThings(t.simBlocks[t.currIndex].name, t))
                        {
                            t.did[t.simBlocks[t.currIndex].name] = true;

                        }
                        else
                        {
                            //don't have things

                            lost = true;

                        }
                    }
                    else if (t.simBlocks[t.currIndex].type == SimBlock.CHECKIN)
                    {

                        if (t.isCheckedIn)
                        {
                            // double checkin

                            lost = true;


                        }
                        else
                        {

                            // perform check-in
                            t.isCheckedIn = true;
                            t.isCheckedOut = false;
                        }

                    }
                    else if (t.simBlocks[t.currIndex].type == SimBlock.CHECKOUT)
                    {
                        foreach (KeyValuePair<string, bool> k in t.needsTo)
                        {
                            if (k.Value && !t.did[k.Key])
                            {

                                // not did all work

                                lost = true;

                            }
                        }
                        if (isRetAllCompulsion)
                        {
                            foreach (KeyValuePair<string, bool> k in t.hasItems)
                            {
                                if (k.Value)
                                {
                                    // not returned all

                                    lost = true;

                                }
                            }
                        }
                        else if (t.isCheckedOut)
                        {
                            // check in before checkout

                            lost = true;


                        }
                        else
                        {

                            // perform check-out
                            t.isCheckedIn = false;
                            t.isCheckedOut = true;
                        }
                    }


                }
            }
            j++; // increment step
        }
        return null;
    }
    private string RequirementList(string name,Thread t)
    {
        string list = "";
        List<string> r = ((Action)t.workList.GetType().GetField(name).GetValue(t.workList)).GetRequirementList();
        for (int i = 0; i < r.Count; i++)
        {
            if (i == r.Count - 1)
            {
                list += r[i] + ".";
            }
            else if (i == r.Count - 2)
            {
                list += r[i] + " and ";
            }
            else
            {
                list += r[i] + ", ";
            }
        }
        return list;
    }

    private bool IHaveAllThings(string name, Thread t)
    {
        bool iDoHave = true;
        foreach (string s in ((Action)t.workList.GetType().GetField(name).GetValue(t.workList)).GetRequirementList())
        {
            if (!t.hasItems[s])
            {
                iDoHave = false;
            }
        }
        return iDoHave;
    }

    //If t don't have the obj but some other thread has it then it will return true;
    private bool MeNotSomeOneHas(string name, Thread me)
    {
        bool someOneHas = false;
        foreach (Thread t in threads)
        {
            if(t!=me && t.hasItems[name])
            {
                someOneHas = true;
            }
        }
        return someOneHas;
    }

    public void terminateSimulation(string error)
    {
        if (!isAllowed(stopButton))
        {
            return;
        }
            
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

        CloseBtn(stopButton);
        OpenBtn(playButton);
        bar.LoadingBar.GetComponent<Image>().fillAmount = 0;

    }
    public void GameOver(GameObject gameOverPanel)
    {     
        LogManager.instance.logger.sendChronologicalMenuLogs("TryAgain", LogManager.instance.UniEndTime().ToString());
        GameLogData.chronologicalLogs.Add("TerminateLevel3: " + LogManager.instance.UniEndTime());
        LogManager.instance.logger.sendChronologicalLogs("TerminateLevel3", "", LogManager.instance.UniEndTime().ToString());

        GameLogData.failedReason = "GameOver";
        LogManager.instance.CreateLogData();

        LogManager.instance.failCount++;

        stepsIndicator.text = "0";

        err = true;
        lost = true;
        stop = true;
        paused = true;

        try
        {
            gameOverPanel.SetActive(false);
            disablePanel.SetActive(false);
        }
        catch
        {
            Debug.Log("Cannot disable DisablePanel.");
        }
        bar.LoadingBar.GetComponent<Image>().fillAmount = 0;
        CloseBtn(stopButton);
        OpenBtn(playButton);
        CloseBtn(nextButton);

    }
    public void Review(GameObject wonPanel)
    {

        GameLogData.chronologicalLogs.Add("TerminateLevel3: " + LogManager.instance.UniEndTime());
        LogManager.instance.logger.sendChronologicalLogs("TerminateLevel3", "", LogManager.instance.UniEndTime().ToString());

        GameLogData.failedReason = "Review";
        LogManager.instance.CreateLogData();

        LogManager.instance.failCount++;

        stepsIndicator.text = "0";

        err = true;
        lost = true;
        stop = true;
        paused = true;

        try
        {
            wonPanel.SetActive(false);
            disablePanel.SetActive(false);
        }
        catch
        {
            Debug.Log("Cannot disable DisablePanel.");
        }
        bar.LoadingBar.GetComponent<Image>().fillAmount = 0;
        CloseBtn(stopButton);
        OpenBtn(playButton);
        OpenBtn(nextButton);

    }
    private bool isAllowed(GameObject g)
    {
        return !g.transform.Find("overlay").gameObject.active;
    }

    void resError(String msg, GameObject layout)
    {

        // display error
        Transform newItemParent = layout.transform;

        GameObject newItem = Instantiate(simulationErrorPrefab) as GameObject;
        newItem.transform.Find("ActionText").GetComponent<Text>().text = "<color=red>" + msg + "</color>";
        newItem.transform.SetParent(newItemParent);
        newItem.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
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
        foreach(Thread t in threads)
        {
            foreach (Transform child in t.layoutPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
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

    void scrollToRight()
    {

        // Debug.Log ("scrollToBottom()");
        Canvas.ForceUpdateCanvases();
        waitOneFrame();
        simulationScrollRect.horizontalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    void scrollToLeft()
    {

        // Debug.Log ("scrollToBottom()");
        Canvas.ForceUpdateCanvases();
        waitOneFrame();
        simulationScrollRect.horizontalNormalizedPosition = 1f;
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
    IEnumerator PauseSound(AudioSource audioSource, float sec)
    {

        yield return new WaitForSeconds(sec);
        StartCoroutine(FadeOut(audioSource, 1+sec/2));
    }
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    IEnumerator waitOneFrame()
    {
        yield return 0;
    }
    
}