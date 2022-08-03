using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Samples;
using Ubiq.XR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRQuestionnaireToolkit;

public class GameLogic : MonoBehaviour, INetworkObject, INetworkComponent
{
    public bool owner = false;
    public int participantID;

    public enum Condition {HeartRate, CognitiveLoad, Attention, ALL}
    public Condition condition;
    public AudioClip notification, success;

    [System.Serializable]
    public struct Instruction
    {
        //public int id;
        public string instructionText;
        public Texture instructionImage;
        public List<GameObject> gameObjects;
        public GameObject tool;
    }
    public List<Instruction> instructionsList = new List<Instruction>();
    public NetworkId Id { get; private set; }

    [System.Serializable]
    private class State
    {
        public string functionName, stringValue;
        public int value;
        public float timestamp;
    }
    private State state = new State();
    private NetworkContext context;
    private InstructionProvider instructionProvider;
    private string currentScene="";
    private GameObject tools, engine, avatarManager, avatarSample, mirror, dartBoard;
    private GameObject DeviceStatus, MindIndex, RawSignals, FeatureIndex;
    private GameObject hearRateHUD, CognitiveLoadHUD, AttentionHUD;
    private Caliberation caliberationScript;
    private SimpleCSVLogger simpleCSV;
    private StudySetup studySetup;
    private FadeIn fadeQuad;

    private void Awake()
    {
        tools = GameObject.Find("Tools");
        engine = GameObject.Find("V8Diesel_Assemble");
        avatarManager = GameObject.Find("Avatar Manager");
        DeviceStatus = GameObject.Find("DeviceStatusCanvas");
        MindIndex = GameObject.Find("MindIndexesCanvas");
        RawSignals = GameObject.Find("RawSignalsCanvas");
        FeatureIndex = GameObject.Find("FeatureIndexesCanvas");
        avatarSample = GameObject.Find("Avatar_Shieh_V2");
        mirror = GameObject.Find("Objects placed in front of user");
        dartBoard = GameObject.Find("DartBoard");
        caliberationScript = GameObject.FindObjectOfType<Caliberation>();
        simpleCSV= GameObject.FindObjectOfType<SimpleCSVLogger>();
        fadeQuad = GameObject.FindObjectOfType<FadeIn>();
    }
    private void Start()
    {
        Id = NetworkScene.ObjectIdFromName(this);
        context = NetworkScene.Register(this);
        mirror.SetActive(false);
    }
    private void Update()
    {
        if(currentScene != SceneManager.GetActiveScene().name)
        {
            currentScene = SceneManager.GetActiveScene().name;
            switch (currentScene)
            {
                case "Caliberation":
                        CaliberationSceneSetup();
                        break;
                case "MainScene":
                        MainSceneSetup();
                        break;
                case "questionnaire":
                    QuestionnaireSetup();
                    break;
                default: Debug.Log("Wrong Scene Name");
                        break;
            }
        }
    }

    public void CreateRoom()
    {
        GameObject.FindObjectOfType<RoomClient>().Join(name: $"Editor Room {IdGenerator.GenerateUnique().ToString()}", publish: true);
    }
    public void JoinRoom()
    {
        if (owner) return;

        Debug.Log("Joining room : " + GameObject.FindObjectOfType<RoomClient>().Room.JoinCode);
        GameObject.FindObjectOfType<RoomClient>().Join(GameObject.FindObjectOfType<RoomClient>().Room.JoinCode);
    }
    public void CaliberationSceneSetup()
    {
        tools.SetActive(false);
        engine.SetActive(false);
        avatarManager.SetActive(false);
        avatarSample.SetActive(true);
        dartBoard.SetActive(false);
        simpleCSV.SetCondition("CaliberationScene");
    }
    public void MainSceneSetup()
    {
        instructionProvider = Object.FindObjectOfType<InstructionProvider>(true);
        hearRateHUD = Object.FindObjectOfType<HeartRateHUD>(true).gameObject;
        CognitiveLoadHUD = Object.FindObjectOfType<CognitiveLoadHUD>(true).gameObject;
        AttentionHUD = Object.FindObjectOfType<AttentionHUD>(true).gameObject;
        tools.SetActive(true);
        engine.SetActive(true);
        avatarManager.SetActive(true);
        avatarSample.SetActive(false);
        mirror.SetActive(false);
        EnableEEGUI(false);
        ActivateHUD(condition);
        simpleCSV.AddMarker("MainScene");
        fadeQuad.Start();
    }
    public void QuestionnaireSetup()
    {
        studySetup = Object.FindObjectOfType<StudySetup>(true);
        studySetup.ParticipantId = participantID.ToString();
        studySetup.Condition = condition.ToString();
    }
    public void ChangeScene(string sceneName)
    {
        string pattern = "(?<=[.!?])";
        string marker = Regex.Split(Regex.Split(sceneName, pattern)[0], "/")[2];
        Object.FindObjectOfType<RoomSceneManager>().ChangeScene(sceneName);
        this.GetComponent<SimpleCSVLogger>().AddMarker(marker);
        simpleCSV.SetCondition(condition.ToString());
        if(owner) CallRemoteFunction("ChangeScene", 0, sceneName);
    }

    public void ExecuteRemoteInstruction(int instructionNumber)
    {
        if(instructionNumber!=(instructionsList.Count-1))
        ExecuteInstruction(instructionNumber, false);

        CallRemoteFunction("ShowInstruction", 0, "");
        CallRemoteFunction("ExecuteInstruction", instructionNumber, "");
    }
    public void ExecuteInstruction(int instructionNumber, bool showInstruction)
    {
        //call the remote instruction Client
        if (owner) CallRemoteFunction("HideInstruction", instructionNumber, "");

        if (instructionNumber == 4 || instructionNumber == 5 || instructionNumber == 6 || instructionNumber == 7 ) Object.FindObjectOfType<LiftUpEngine>().LiftUP(true);
        else Object.FindObjectOfType<LiftUpEngine>().LiftUP(false);

        ShowInstruction(showInstruction);
        instructionProvider.SetInstruction(instructionsList[instructionNumber].instructionText, instructionsList[instructionNumber].instructionImage);
        if (instructionsList[instructionNumber].tool)
        {
            foreach (GameObject obj in instructionsList[instructionNumber].gameObjects)
            {
                if (obj.GetComponent<BoxCollider>()) obj.GetComponent<BoxCollider>().isTrigger = true;
                else if (obj.GetComponent<MeshCollider>())
                {
                    obj.GetComponent<MeshCollider>().convex = true;
                    obj.GetComponent<MeshCollider>().isTrigger = true;
                }
                instructionsList[instructionNumber].tool.GetComponent<TriggerAction>().AssemblyPartNames.Add(obj.name);
            }
        }
        else //No tools provided. Enabling part to be grababble by hand
        {
            foreach (GameObject obj in instructionsList[instructionNumber].gameObjects)
            {
                if (obj.GetComponent<SharedComponent>()) obj.GetComponent<SharedComponent>().canGrab = true;
            }
        }
        if (instructionNumber == (instructionsList.Count - 1)) this.GetComponent<AudioSource>().clip = success;
        else this.GetComponent<AudioSource>().clip = notification;
        this.GetComponent<AudioSource>().Play();
    }

    public void ActivateHUD(Condition condition)
    {
        hearRateHUD.SetActive(false);
        CognitiveLoadHUD.SetActive(false);
        AttentionHUD.SetActive(false);

        switch (condition)
        {
            case Condition.HeartRate:
                hearRateHUD.SetActive(true);
                condition = Condition.HeartRate;
                if (owner) CallRemoteFunction("ActivateHUD", 0, "");
                break;
            case Condition.CognitiveLoad:
                CognitiveLoadHUD.SetActive(true);
                condition = Condition.CognitiveLoad;
                if (owner) CallRemoteFunction("ActivateHUD", 1, "");
                break;
            case Condition.Attention:
                AttentionHUD.SetActive(true);
                condition = Condition.Attention;
                if (owner) CallRemoteFunction("ActivateHUD", 2, "");
                break;
            case Condition.ALL:
                hearRateHUD.SetActive(true);
                CognitiveLoadHUD.SetActive(true);
                AttentionHUD.SetActive(true);
                condition = Condition.ALL;
                if (owner) CallRemoteFunction("ActivateHUD", 3, "");
                break;
        }
        simpleCSV.SetCondition(condition.ToString());
    }
    public void ShowHideAvatar()
    {
        if (avatarManager.activeSelf) avatarManager.SetActive(false);
        else avatarManager.SetActive(true);

        if (owner) CallRemoteFunction("ShowHideAvatar", 0, "");
    }
    public string[] GetInstructionList()
    {
        string[] instructionNames = new string[instructionsList.Count];
        for (int i = 0; i < instructionsList.Count; i++)
        {
            instructionNames[i] = instructionsList[i].instructionText;
        }
        return instructionNames;
    }
    public void SwitchToLeftHand()
    {        
        foreach (SwitchHands switchHand in Object.FindObjectsOfType<SwitchHands>())
        {
            switchHand.transform.localPosition = switchHand.leftHand;
        }
    }
    public void SwitchToRightHand()
    {
        foreach (SwitchHands switchHand in Object.FindObjectsOfType<SwitchHands>())
        {
            switchHand.transform.localPosition = switchHand.rightHand;
        }
    }
    public void ShowInstruction(bool value)
    {
        if (value) instructionProvider.ShowInstruction();
        else instructionProvider.HideInstruction();
    }
    public void EnableEyeGaze(bool value)
    {
        if (value)
        {
            GameObject.Find("EyeGaze").GetComponent<LineRenderer>().enabled = true;
            CallRemoteFunction("EnableEyeGaze", 0, "");
        }
        else
        {
            GameObject.Find("EyeGaze").GetComponent<LineRenderer>().enabled = false;
            CallRemoteFunction("DisableEyeGaze", 0, "");
        }
    }
    public void StartLoggingCSV()
    {
        string fileName = participantID.ToString() + "-" + condition.ToString() + "-" + System.DateTime.Now.ToString("HH-mm-ss-") + "-";
        this.GetComponent<SimpleCSVLogger>().StartLogging(fileName);
        if(owner) CallRemoteFunction("StartLogging", 0, "");
    }
    public void StopLoggingCSV()
    {
        this.GetComponent<SimpleCSVLogger>().StopLogging();
        if (owner) CallRemoteFunction("StopLogging", 0, "");
    }
    public void UpdateParticipantDetails(int participantID, string stringCondition)
    {
        this.participantID = participantID;
        this.condition = (Condition)System.Enum.Parse(typeof(Condition), stringCondition);
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        owner = false;
        JsonUtility.FromJsonOverwrite(message.ToString(), state);
        switch (state.functionName)
        {
            case "ChangeScene":
                ChangeScene(state.stringValue);
                break;
            case "ShowInstruction":
                ShowInstruction(true);
                break;
            case "HideInstruction":
                ShowInstruction(false);
                ExecuteInstruction(state.value, false);
                break;
            case "ExecuteInstruction":
                ExecuteInstruction(state.value, true);
                break;
            case "StartLogging":
                StartLoggingCSV();
                break;
            case "StopLogging":
                StopLoggingCSV();
                break;
            case "EnableEyeGaze":
                EnableEyeGaze(true);
                break;
            case "DisableEyeGaze":
                EnableEyeGaze(false);
                break;
            case "ActivateEyeCalib":
                ActivateEyeCalib();
                break;
            case "DeActivateEyeCalib":
                DeActivateEyeCalib();
                break;
            case "EnableDOM":
                EnableDOM(true);
                break;
            case "EnableEEGUI":
                EnableEEGUI(true);
                break;
            case "EnableNBack":
                ShowNback();
                break;
            case "StartNBack":
                StartNback();
                break;
            case "SetNBack":
                SetNback(state.value);
                break;
            case "ResetNBack":
                ResetNBack();
                break;
            case "StartSampling":
                StartSampling();
                break;
            case "CalculateLow":
                CalculateLow();
                break;
            case "CalculateMedium":
                CalculateMedium();
                break;
            case "CalculateHigh":
                CalculateHigh();
                break;
            case "ShowHideAvatar":
                ShowHideAvatar();
                break;
            case "ActivateHUD":
                if (state.value == 0) ActivateHUD(Condition.HeartRate);
                else if (state.value == 1) ActivateHUD(Condition.CognitiveLoad);
                else if (state.value == 2) ActivateHUD(Condition.Attention);
                else if (state.value == 3) ActivateHUD(Condition.ALL);
                break;
            case "UpdateParticipantDetails":
                UpdateParticipantDetails(state.value, state.stringValue);
                break;
            default:
                print("Incorrect Choice");
                break; 
        }

    }
    public void CallRemoteFunction(string functionName, int value, string stringValue)
    {
        if (!owner)
        {
            Debug.Log("Only owner can call a remote function");
            return;
        }
        state.functionName = functionName;
        state.timestamp = Time.deltaTime;
        state.value = value;
        state.stringValue = stringValue;
        context.Send(ReferenceCountedSceneGraphMessage.Rent(JsonUtility.ToJson(state)));
    }

    public bool IsCaliberationScene()
    {
        if (currentScene == "Caliberation") return true;
        else return false;
    }
    public void ActivateEyeCalib()
    {
        //caliberationScript.ActivateMirror();
        dartBoard.SetActive(true);
        EnableEyeGaze(true);
        simpleCSV.AddMarker("ActivateEyeCalib");
        if (owner) CallRemoteFunction("ActivateEyeCalib", 0, "");
    }
    public void DeActivateEyeCalib()
    {
        //caliberationScript.DeActivateMirror();
        dartBoard.SetActive(false);
        EnableEyeGaze(false);
        simpleCSV.AddMarker("DeactivateEyeCalib");
        if (owner) CallRemoteFunction("DeActivateEyeCalib", 0, "");
    }
    public void EnableDOM(bool value)
    {
        caliberationScript.ShowEnvironment(value);
        if (owner) CallRemoteFunction("EnableDOM", 0, "");
    }
    public void EnableEEGUI(bool value)
    {
        DeviceStatus.SetActive(value);
        MindIndex.SetActive(value);
        FeatureIndex.SetActive(value);
        RawSignals.SetActive(value);
        if (owner) CallRemoteFunction("EnableEEGUI", 0, "");
    }
    public void ShowNback()
    {
        EnableEEGUI(false);
        caliberationScript.ClearUp();
        caliberationScript.ShowNback(true);
        if (owner) CallRemoteFunction("EnableNBack", 0, "");
    }
    public void StartNback()
    {
        caliberationScript.StartNback();
        if (owner) CallRemoteFunction("StartNBack", 0, "");
    }
    public void ResetNBack()
    {
        caliberationScript.ResetNBack();
        if (owner) CallRemoteFunction("ResetNBack", 0, "");
    }
    public void SetNback(int n)
    {
        caliberationScript.SetNback(n);
        if (owner) CallRemoteFunction("SetNBack", n, "");
    }
    public void StartSampling()
    {
        GameObject.FindObjectOfType<PhysiologicalDataController>().keepGSRSampling = true;
        simpleCSV.AddMarker("StartSampling");
        if (owner) CallRemoteFunction("StartSampling", 0, "");
    }
    public void CalculateLow()
    {
        GameObject.FindObjectOfType<PhysiologicalDataController>().low = GameObject.FindObjectOfType<PhysiologicalDataController>().AnalyzeGSRSample();
        simpleCSV.AddMarker("Low " + GameObject.FindObjectOfType<PhysiologicalDataController>().low);
        if (owner) CallRemoteFunction("CalculateLow", 0, "");
    }
    public void CalculateMedium()
    {
        GameObject.FindObjectOfType<PhysiologicalDataController>().medium = GameObject.FindObjectOfType<PhysiologicalDataController>().AnalyzeGSRSample();
        simpleCSV.AddMarker("Medium " + GameObject.FindObjectOfType<PhysiologicalDataController>().medium);
        if (owner) CallRemoteFunction("CalculateMedium", 0, "");
    }
    public void CalculateHigh()
    {
        GameObject.FindObjectOfType<PhysiologicalDataController>().high = GameObject.FindObjectOfType<PhysiologicalDataController>().AnalyzeGSRSample();
        simpleCSV.AddMarker("High " + GameObject.FindObjectOfType<PhysiologicalDataController>().high);
        if (owner) CallRemoteFunction("CalculateHigh", 0, "");
    }
}


//Instructions

//Use the driver tool to remove all eight flywheel bolts
//Grab and remove the flywheel
//Use the driver tool to remove all eight oil pan bolts;
//Use Crowbar to remove the Oil pan
//Use file to seperate the crank case
//Grab and remove the crankshaft
//Grab and remove the camshaft;
//Use the driver tool to remove the lower rods as shown
//Use spanner to remove the following pistons
//Use pliers to remove  pushrods