using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

/// <summary>
/// Assembly Manager
/// </summary>
public class MgrAssemblyStep : MonoBehaviour
{

    public AssemblyHint HintSys = new AssemblyHint();

    public bool IfHide = false;

    public ProgressState ProgState=ProgressState.Forward;

    public Transform WholeTra=null;

    public List<AnimStepUnit> AllSteps=new List<AnimStepUnit>();

    public int CurrentStepIndex = 0;

    public bool IfAlowedSinglePlay = false;

    public static MgrAssemblyStep Instance;

    private bool isKeyHeld = false;
    private float lastKeyPressTime = 0f;

    public void  Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        iniAnimSteps();
    }

    public void AssembleEngine()
    {
        iniAnimSteps();
        ChangeProgState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeProgState();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            PlayNextStep();
            lastKeyPressTime = Time.time;
        }
    }


    public void ChangeProgState() {

        if (ProgState == ProgressState.Reverse)
        {
            ProgState = ProgressState.Forward;
        }
        else if (ProgState == ProgressState.Forward)
        {
            ProgState = ProgressState.Reverse;
        }

        AssemblyStepPart[] _parts = WholeTra.GetComponentsInChildren<AssemblyStepPart>();

        for (int i = 0; i < _parts.Length; i++)
        {

            AssemblyStepPart _part = _parts[i];

            _part.StepIndex = (AllSteps.Count - 1) - _part.StepIndex;
        }

        AllSteps = new List<AnimStepUnit>();

        List<int> _indexAlreadExist = new List<int>();

        for (int i = 0; i < _parts.Length; i++)
        {
            
            AssemblyStepPart _part = _parts[i];

            int _index = _part.StepIndex;

            if (_indexAlreadExist.Contains(_index))
            {
                for (int j = 0; j < AllSteps.Count; j++)
                {
                    AnimStepUnit _stepUnit = AllSteps[j];
                    int _stepInde = _stepUnit.StepIndex;
                    if (_stepInde == _index)
                    {
                        _stepUnit.StepParts.Add(_part);
                    }
                }

            }
            else 
            {

                _indexAlreadExist.Add(_index);
                AnimStepUnit _stepUnit = new AnimStepUnit();

                _stepUnit.StepIndex = _index;
                _stepUnit.StepParts.Add(_part);

                AllSteps.Add(_stepUnit);
            }
        }

        ResetAllParts();
    }


    private void iniAnimSteps()
    {

        AssemblyStepPart[] _parts= WholeTra.GetComponentsInChildren<AssemblyStepPart>();       

        List<int> _indexAlreadExist = new List<int>();

        for (int i = 0; i < _parts.Length; i++)
        {
            AssemblyStepPart _part = _parts[i];
            int _index = _part.StepIndex;

            if (_indexAlreadExist.Contains(_index)) 
            {
                for (int j = 0; j < AllSteps.Count; j++) 
                {
                    AnimStepUnit _stepUnit = AllSteps[j];
                    int _stepInde = _stepUnit.StepIndex;
                    if (_stepInde== _index)
                    {
                        _stepUnit.StepParts.Add(_part);
                        break;
                    }
                }

            }
            else 
            {

                _indexAlreadExist.Add(_index); 
                AnimStepUnit _stepUnit= new AnimStepUnit();

                _stepUnit.StepIndex = _index;
                _stepUnit.StepParts.Add(_part);

                AllSteps.Add(_stepUnit);
            }
        }

       
    }

    public void ResetForBtn()
    {
        bool _bl = ResetAllParts();

        if (!_bl)
        {
            return;
        }
    }


    public bool ResetAllParts()
    {

        
        for (int i = 0; i < AllSteps.Count; i++)
        {
            if (AllSteps[i].CurretState == StepState.Active)
            {
                HintSys.HintPartInProgress();
                return false;
            }

            for (int j = 0; j < AllSteps[i].StepParts.Count; j++)
            {
                AssemblyStepPart _part = AllSteps[i].StepParts[j];

                if (_part.CurrentState == PartState.Active)
                {
                    HintSys.HintPartInProgress();
                    return false;
                }
            }
        }

        if (ProgState == ProgressState.Forward)
        {
            CurrentStepIndex = 0;
        } else if (ProgState == ProgressState.Reverse)
        {
            CurrentStepIndex = AllSteps.Count - 1;
        }

        for (int i = 0; i < AllSteps.Count; i++)
        {
            AllSteps[i].CurretState = StepState.Unactive;

            for (int j = 0; j < AllSteps[i].StepParts.Count; j++)
            {
                AssemblyStepPart _part = AllSteps[i].StepParts[j];

                _part.ResetState(ProgState);

                if (IfHide)
                {                   
                    if (ProgState == ProgressState.Forward)
                    {
                        _part.HidePart();
                    }
                    
                    else if (ProgState == ProgressState.Reverse)
                    {
                        _part.ShowPart();
                    }
                }             
            }
        }

        return true;
    }


    public void PlayByStep(int stepIndex) {


        if (stepIndex > CurrentStepIndex)
        {
            HintSys.HintWrongStep();
        }
        else if (stepIndex < CurrentStepIndex)
        {
            HintSys.HintRepeetDoneStep();
        }
        else
        {
            HintSys.HintCorrectStep();
            PlayNextStep();
        }


    }


    public void PlayNextStep()
    {
        if (ProgState==ProgressState.Forward) 
        {
            if (CurrentStepIndex > 0)
            {
                int _previousIndex = CurrentStepIndex - 1;

                AnimStepUnit _step = getStepUnitByIndex(_previousIndex);

                if (_step == null)
                {
                    HintSys.HintPreviousStepNull();
                    return;
                }

                StepState _previousStepState = _step.CurretState;

                if (_previousStepState != StepState.Finish)
                {
                    return;
                }
            }
            else if (CurrentStepIndex < 0)
            {
                HintSys.HintIndexNumWrong();
                return;
            }
        }
        else if (ProgState==ProgressState.Reverse) 
        {

            if (CurrentStepIndex < (AllSteps.Count - 1))
            {
                int _previousIndex = CurrentStepIndex + 1;

                AnimStepUnit _step = getStepUnitByIndex(_previousIndex);

                if (_step == null)
                {
                    HintSys.HintPreviousStepNull();
                    return;
                }

                StepState _previousStepState = _step.CurretState;

                if (_previousStepState != StepState.Finish)
                {
                    return;
                }
            }
            else if (CurrentStepIndex > (AllSteps.Count - 1))
            {
                HintSys.HintIndexNumWrong();
                return;
            }
        }



        AnimStepUnit _currentstep = getStepUnitByIndex(CurrentStepIndex);

        if (_currentstep == null)
        {
            HintSys.HintNoMoreStep();
            return;
        }
        if (_currentstep.CurretState != StepState.Unactive)
        {
            HintSys.HintCurrentStepWrong();
            return;
        }

        _currentstep.CurretState = StepState.Active;

        List<AssemblyStepPart> _stepParts = _currentstep.StepParts;
        
        float _longestTime = 0;

        for (int i = 0; i < _stepParts.Count; i++)
        {
            AssemblyStepPart _part = _stepParts[i];
            float _partTime = _part.AnimTime;
            if (_longestTime< _partTime) 
            {
                _longestTime = _partTime;
            }
            _part.AcivePart(ProgState);

            if (IfHide)
            {

                if (ProgState == ProgressState.Forward)
                {
                    _part.ShowPart();
                }

                else if (ProgState == ProgressState.Reverse)
                {
                    _part.HidePart();
                }
            }
        }
        
        StartCoroutine(countStep(_longestTime, _currentstep));


    }
    

    public bool IfPartCanSingleActive(AssemblyStepPart part)
    {
        if (IfAlowedSinglePlay==false) 
        {
            HintSys.HintPartModeNotActive();
            return false;            
        }

        if (part.CurrentState!= PartState.Unactive) 
        {
            HintSys.HintRepeatDonePart();
            return false;
        }

        int _partIndex = part.StepIndex;

        if (ProgState == ProgressState.Forward) 
        {

            if (_partIndex > 0)
            {
                int _previousIndex = _partIndex - 1;

                AnimStepUnit _previousStep = getStepUnitByIndex(_previousIndex);

                if (_previousStep == null)
                {
                    HintSys.HintLastStepWrong();
                    return false;
                }

                StepState _previousStepState = _previousStep.CurretState;

                if (_previousStepState == StepState.Finish)
                {
                    HintSys.HintPartSuc();
                    return true;
                }
                else 
                {
                    for (int i = 0; i < _previousStep.StepParts.Count; i++)
                    {
                        if (_previousStep.StepParts[i].CurrentState != PartState.Finish)
                        {
                            HintSys.HintStepWrong();
                            return false;
                        }
                    }

                    _previousStep.CurretState = StepState.Finish;

                    HintSys.HintPartSuc();
                    return true;
                }
            }
            else if (_partIndex < 0) 
            {
                HintSys.HintIndexNumWrong();
                return false;
            }
            else if (_partIndex == 0)
            {
                HintSys.HintPartSuc();
                return true;
            }
        }
        else if (ProgState == ProgressState.Reverse)
        {

            if (_partIndex <(AllSteps.Count - 1) )
            {

                int _previousIndex = _partIndex + 1;

                AnimStepUnit _previousStep = getStepUnitByIndex(_previousIndex);

                if (_previousStep == null)
                {
                    HintSys.HintLastStepWrong();
                    return false;
                }

                StepState _previousStepState = _previousStep.CurretState;

                if (_previousStepState == StepState.Finish)
                {
                    HintSys.HintPartSuc();
                    return true;
                }
                else 
                {
                    for (int i = 0; i < _previousStep.StepParts.Count; i++)
                    {
                        if (_previousStep.StepParts[i].CurrentState != PartState.Finish)
                        {
                            HintSys.HintStepWrong();
                            return false;
                        }
                    }

                    _previousStep.CurretState = StepState.Finish;

                    HintSys.HintPartSuc();
                    return true;
                }
            }
            else if (_partIndex > (AllSteps.Count - 1)) 
            {
                HintSys.HintIndexNumWrong();
                return false;
            }
            else if (_partIndex ==(AllSteps.Count - 1)) 
            {
                HintSys.HintPartSuc();
                return true;
            }
        }



        HintSys.HintUnKownFail();
        return false;
    }

    private IEnumerator countStep(float stepTime, AnimStepUnit stepUnit)
    {
        
        yield return new WaitForSeconds(stepTime);

        stepUnit.CurretState = StepState.Finish;

    }


    public void HandleSinglePartFinishEvent(int index)
    {

        AnimStepUnit _currentStep = getStepUnitByIndex(index);

        for (int i = 0; i < _currentStep.StepParts.Count; i++)
        {
            if (_currentStep.StepParts[i].CurrentState != PartState.Finish)
            {
                return;
            }
        }

        _currentStep.CurretState = StepState.Finish;

        if (ProgState==ProgressState.Forward)
        {

            CurrentStepIndex++;
        }
        else if (ProgState == ProgressState.Reverse)
        {

            CurrentStepIndex--;
        }

    }


    private AnimStepUnit getStepUnitByIndex(int index)
    {
        for (int i = 0; i < AllSteps.Count; i++)
        {
            AnimStepUnit _step = AllSteps[i];
            int _stepIndex = _step.StepIndex;
            if (_stepIndex==index)
            {
                return _step;
            }
        }
        return null;
    }
}
