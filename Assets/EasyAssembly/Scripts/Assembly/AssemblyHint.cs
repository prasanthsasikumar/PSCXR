using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// hint class
/// </summary>
[Serializable]
public class AssemblyHint 
{

    public bool IfHint = true;


    public bool IfHintPartInProgress = true;

    public bool IfUnKownFail = true;

    public string StrPartInProgress = "An animation is playing";
    public string StrUnKownFail = "failed for unknown reason";

    public void HintPartInProgress()
    {
        if (!IfHint) { return; }
        if (!IfHintPartInProgress) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrPartInProgress);
    }

    public void HintUnKownFail()
    {
        if (!IfHint) { return; }
        if (!IfUnKownFail) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrUnKownFail);
    }



    public bool IfHintWrongStep = true;

    public bool IfHintRepeatDoneStep = true;

    public bool IfHintCorrectStep = true;

    public string StrWrongStep = "wrong step";
    public string StrRepeetDoneStep = "already finished";
    public string StrCorrectStep = "step correct";

    public void HintWrongStep()
    {
        if (!IfHint){ return; }
        if (!IfHintWrongStep){ return;}

        GameApp.Instance.MgrHintScript.OpenHintBox(StrWrongStep);
    }

    public void HintRepeetDoneStep()
    {
        if (!IfHint) { return; }
        if (!IfHintRepeatDoneStep) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrRepeetDoneStep);
    }

    public void HintCorrectStep()
    {
        if (!IfHint) { return; }
        if (!IfHintCorrectStep) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBoxOK(StrCorrectStep);
    }

    public bool IfHintPreviousStepNull = true;

    public bool IfHintIndexNumWrong = true;

    public bool IfHintNoMoreStep = true;

    public bool IfHintCurrentStepWrong = true;

    public string StrPreviousStepNull = "previous animation is null";
    public string StrIndexNumWrong = "step index is wrong";
    public string StrNoMoreStep = "next step is null";
    public string StrCurrentStepWrong = "current step is wrong";


    public void HintPreviousStepNull()
    {
        if (!IfHint) { return; }
        if (!IfHintPreviousStepNull) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrPreviousStepNull);
    }

    public void HintIndexNumWrong() {
        if (!IfHint) { return; }
        if (!IfHintIndexNumWrong) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrIndexNumWrong);
    }

    public void HintNoMoreStep()
    {
        if (!IfHint) { return; }
        if (!IfHintNoMoreStep) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrNoMoreStep);
    }

    public void HintCurrentStepWrong()
    {
        if (!IfHint) { return; }
        if (!IfHintCurrentStepWrong) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrCurrentStepWrong);
    }



    public bool IfPartModeNotActive = true;

    public bool IfRepeatDonePart = true;

    public bool IfLastStepWrong = true;

    public bool IfStepFinish = true;

    public bool IfStepWrong = true;

    public bool IfPartSuc = true;

    public string StrPartModeNotActive = "single part mode not been activated";
    public string StrRepeatDonePart = "already finished";
    public string StrLastStepWrong = "previous setp is wrong";
    public string StrStepFinish = "finish a step";
    public string StrStepWrong = "wrong step";
    public string StrPartSuc = "success";


    public void HintPartModeNotActive()
    {
        if (!IfHint) { return; }
        if (!IfPartModeNotActive) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrPartModeNotActive);
    }

    public void HintRepeatDonePart()
    {
        if (!IfHint) { return; }
        if (!IfRepeatDonePart) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrRepeatDonePart);
    }

    public void HintLastStepWrong() {
        if (!IfHint) { return; }
        if (!IfLastStepWrong) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrLastStepWrong);
    }

    public void HintStepFinish()
    {
        if (!IfHint) { return; }
        if (!IfStepFinish) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBoxOK(StrStepFinish);
    }


    public void HintStepWrong()
    {
        if (!IfHint) { return; }
        if (!IfStepWrong) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBox(StrStepWrong);
    }

    public void HintPartSuc()
    {
        if (!IfHint) { return; }
        if (!IfPartSuc) { return; }

        GameApp.Instance.MgrHintScript.OpenHintBoxOK(StrPartSuc);
    }



}
