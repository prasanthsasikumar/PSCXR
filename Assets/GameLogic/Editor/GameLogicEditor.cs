using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameLogic))]
public class GameLogicEditor : Editor
{
    private static int selectedInstruction = 0, selectedHand = 1, selectedEyeGaze = 1, selectedCsvSave = 1;
    private static int selectedScene = -1, selectedNback = 0, selectedCondition= -1;

    private string[] dominantHand = new string[2];
    private string[] EyeGaze = new string[2];
    private string[] Logging = new string[2];
    private string[] Nback = new string[3];
    private string[] conditions = new string[4];

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        dominantHand[0] = "Left Hand";
        dominantHand[1] = "Right Hand";
        EyeGaze[0] = "Enabled";
        EyeGaze[1] = "Disabled";
        Logging[0] = "Enabled";
        Logging[1] = "Disabled";
        Nback[0] = "1 Back";
        Nback[1] = "2 Back";
        Nback[2] = "3 Back";
        conditions[0] = "HeartRate";
        conditions[1] = "CognitiveLoad";
        conditions[2] = "Attention";
        conditions[3] = "All";

        var component = target as GameLogic;
        var scenenames = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
        var instructions = component.GetInstructionList();

        GUILayout.BeginHorizontal("Network");
        if (GUILayout.Button("Create Room"))
        {
            component.CreateRoom();
        }
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Join Room"))
        {
            component.JoinRoom();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginHorizontal("CalibBox");
        if (GUILayout.Button("Update remote Participant Details"))
        {
            component.CallRemoteFunction("UpdateParticipantDetails", component.participantID + 1, component.condition.ToString());
        }
        if (GUILayout.Button("ShowHideAvatar"))
        {
            component.ShowHideAvatar();
        }
        GUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
        selectedCsvSave = EditorGUILayout.Popup("CSV Logging", selectedCsvSave, Logging);
        if (EditorGUI.EndChangeCheck())
        {
            if (selectedCsvSave == 0) component.StartLoggingCSV();
            else component.StopLoggingCSV();
        }
        EditorGUI.BeginChangeCheck();
        selectedCondition = EditorGUILayout.Popup("Condition", selectedCondition, conditions);
        if (EditorGUI.EndChangeCheck())
        {
            component.ChangeScene(scenenames[0]);
            if (selectedCondition == 0) component.ActivateHUD(GameLogic.Condition.HeartRate);
            else if (selectedCondition == 1) component.ActivateHUD(GameLogic.Condition.CognitiveLoad);
            else if (selectedCondition == 2) component.ActivateHUD(GameLogic.Condition.Attention);
            else if (selectedCondition == 3) component.ActivateHUD(GameLogic.Condition.ALL);
        }

        GUILayout.Space(20);
        GUILayout.BeginHorizontal("CalibBox");
        if (GUILayout.Button("ActivateEyeCalib"))
        {
            component.ActivateEyeCalib();
        }
        if (GUILayout.Button("DeActivateEyeCalib"))
        {
            component.DeActivateEyeCalib();
        }
        GUILayout.EndHorizontal();
                   

        GUILayout.BeginHorizontal("NBack");
        EditorGUI.BeginChangeCheck();
        selectedNback = EditorGUILayout.Popup("SetNback", selectedNback, Nback);
        if (EditorGUI.EndChangeCheck())
        {
            component.SetNback(selectedNback+1);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("NBackContd");
        if (GUILayout.Button("ShowNback"))
        {
            component.ShowNback();
        }
        if (GUILayout.Button("StartNBack"))
        {
            component.StartNback();
        }
        if (GUILayout.Button("ResetNBack"))
        {
            component.ResetNBack();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        if (GUILayout.Button("StartSampling"))
        {
            component.StartSampling();
        }
        GUILayout.BeginHorizontal("EEG");
        if (GUILayout.Button("Calculate LOW"))
        {
            component.CalculateLow();
        }
        if (GUILayout.Button("Calculate Medium"))
        {
            component.CalculateMedium();
        }
        if (GUILayout.Button("Calculate High"))
        {
            component.CalculateHigh();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        selectedInstruction = EditorGUILayout.Popup("Instruction", selectedInstruction, instructions);
        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Update Instruction Locally"))
        {
            component.ExecuteInstruction(selectedInstruction , true);
        }
        if (GUILayout.Button("Update Instruction Remotely"))
        {
            component.ExecuteRemoteInstruction(selectedInstruction);
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("InstructionCompleted"))
        {
            component.ExecuteInstruction(10, true);
            component.ExecuteRemoteInstruction(10);
        }

        GUILayout.Space(40);
        EditorGUI.BeginChangeCheck();
        selectedHand = EditorGUILayout.Popup("Dominent Hand", selectedHand, dominantHand);
        if (EditorGUI.EndChangeCheck())
        {
            if (selectedHand == 0) component.SwitchToLeftHand();
            else component.SwitchToRightHand();
        }

        EditorGUI.BeginChangeCheck();
        selectedEyeGaze = EditorGUILayout.Popup("EyeGaze", selectedEyeGaze, EyeGaze);
        if (EditorGUI.EndChangeCheck())
        {
            if (selectedEyeGaze == 0) component.EnableEyeGaze(true);
            else component.EnableEyeGaze(false);
        }

        GUILayout.BeginHorizontal("Environment");
        if (GUILayout.Button("EnableDOM"))
        {
            component.EnableDOM(true);
        }
        if (GUILayout.Button("EnableEEGUI"))
        {
            component.EnableEEGUI(true);
        }
        GUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        selectedScene = EditorGUILayout.Popup("Change Scene", selectedScene, scenenames);
        if (EditorGUI.EndChangeCheck())
        {
            component.ChangeScene(scenenames[selectedScene]);
        }
    }
}
