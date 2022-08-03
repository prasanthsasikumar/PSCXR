using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Assembly part unit 
/// Control assembly animation
/// </summary>
public class AssemblyStepPart : MonoBehaviour
{

    public string PartNm = string.Empty;


    public int StepIndex=0;

    public PartState CurrentState = PartState.Unactive;

    public bool IfFinishStep=false;

    public Transform AssemblyPart = null;

    public Transform StartTra=null;

    public Transform EndTra=null;

    public AssemblyStepPart RelationNextStepPart = null;

    public float AnimTime = 0;

    public AxialDir MoveDir = AxialDir.Axial_Y;

    public float MoveDis = 0;

    public AxialDir RoDir = AxialDir.Axial_Y;

    public float RoSpeed = 0;

    public bool IfNextPartPlay = false;

    public Tweener Tw = null;

    private Vector3 endLocalPos = new Vector3();

    private float currentRoSpeed = 0f;



    public void Start()
    {
        currentRoSpeed = RoSpeed;

        if (EndTra==null)
        {
            switch (MoveDir)
            {
                case AxialDir.Axial_X:
                    endLocalPos = AssemblyPart.localPosition + new Vector3(MoveDis, 0f, 0f);
                    break;
                case AxialDir.Axial_Y:
                    endLocalPos = AssemblyPart.localPosition + new Vector3(0f, MoveDis, 0f);
                    break;
                case AxialDir.Axial_Z:
                    endLocalPos = AssemblyPart.localPosition + new Vector3(0f, 0f, MoveDis);
                    break;
                default:
                    break;
            }
        }
    }


    public void ResetState(ProgressState state) {

        if (Tw != null)
        {
            Tw.Kill();
            Tw = null;
        }
        CurrentState = PartState.Unactive;      

        IfNextPartPlay = false;

        if (state == ProgressState.Forward)
        {
            SetToForwardState();
        }
        else if (state == ProgressState.Reverse)
        {

            SetToReverseState();
        }
    }


    public void SetToForwardState() {

        currentRoSpeed = RoSpeed;
        AssemblyPart.localPosition = StartTra.localPosition;

       
    }


    public void SetToReverseState()
    {
        currentRoSpeed = RoSpeed*-1f;

        if (EndTra!=null)
        {
            AssemblyPart.localPosition = EndTra.localPosition;
        }
        else
        {
            AssemblyPart.localPosition = endLocalPos;                           
        }

    }


    public void AcivePart(ProgressState state)
    {
        if (CurrentState != PartState.Unactive) 
        {
            return;
        }
        
        CurrentState = PartState.Active;

        if (state==ProgressState.Forward)
        {
            if (EndTra == null)
            {

                Tw = AssemblyPart.DOLocalMove(endLocalPos, AnimTime);

            }
            else
            {
                Tw = AssemblyPart.DOLocalMove(EndTra.localPosition, AnimTime);
            }
        }
        else if (state == ProgressState.Reverse)
        {
            currentRoSpeed = RoSpeed * -1f;
            Tw = AssemblyPart.DOLocalMove(StartTra.localPosition, AnimTime);
        }


        
        Tw.SetEase(Ease.Linear);

        StartCoroutine(_CountAnimTime());
    }


    public void ShowPart()
    {
        AssemblyPart.gameObject.SetActive(true);

    }


    public void HidePart()
    {
        StartCoroutine(DelayHide());
    }


    private IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(AnimTime);

        AssemblyPart.gameObject.SetActive(false);

    }


    public void SingleActivePart()
    {
        IfNextPartPlay = true;
        if (CurrentState != PartState.Unactive)
        {
            return;
        }

        CurrentState = PartState.Active;

        if (MgrAssemblyStep.Instance.ProgState == ProgressState.Forward) {

            if (EndTra == null)
            {
                Tw = AssemblyPart.DOLocalMove(endLocalPos, AnimTime);
            }
            else
            {

                Tw = AssemblyPart.DOLocalMove(EndTra.localPosition, AnimTime);
            }
        }
        else if (MgrAssemblyStep.Instance.ProgState == ProgressState.Reverse)
        {
            currentRoSpeed = RoSpeed * -1f;
            Tw = AssemblyPart.DOLocalMove(StartTra.localPosition, AnimTime);
        }

        


        Tw.SetEase(Ease.Linear);
 
        StartCoroutine(_CountAnimTime());
    }


    void Update()
    {

        if (CurrentState == PartState.Active)
        {
            if (RoSpeed == 0)
            {
                return;
            }
            else
            {
                switch (RoDir)
                {
                    case AxialDir.Axial_X:
                        AssemblyPart.Rotate(new Vector3(currentRoSpeed, 0, 0));
                    break;
                    case AxialDir.Axial_Y:
                       AssemblyPart.Rotate(new Vector3(0, currentRoSpeed, 0));
                    break;
                    case AxialDir.Axial_Z:
                        AssemblyPart.Rotate(new Vector3(0, 0, currentRoSpeed));
                    break;
                    default:
                    break;
                }
                
            }            
        }
    }


    private IEnumerator _CountAnimTime()
    {

        yield return new WaitForSeconds(AnimTime);

        CurrentState = PartState.Finish;

        //处理单个部件的装配完成事件
        MgrAssemblyStep.Instance.HandleSinglePartFinishEvent(StepIndex);

        if (IfNextPartPlay&& RelationNextStepPart != null)
        {
            RelationNextStepPart.AcivePart(MgrAssemblyStep.Instance.ProgState);
        }
    }

    private IEnumerator _SingleCountAnimTime()
    {

        yield return new WaitForSeconds(AnimTime);

        CurrentState = PartState.Finish;
        
        if (IfNextPartPlay && RelationNextStepPart != null)
        {
            RelationNextStepPart.SingleActivePart();
        }
    }
}
