using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Nor_Assembly : MonoBehaviour
{
    public static Nor_Assembly Instance=null;

    public Text Tx_CurrentPartCtt = null;

    private MgrScn_Assembly mgrScn = null;


    [SerializeField]
    private RectTransform rtraSteps = null;

    [SerializeField]
    private RectTransform rtraStepsUnfold = null;


    [SerializeField]
    private RectTransform rtraStepsFold = null;

    private float stepsAnimTime = 0.6f;



    public Button Btn_AutoNext=null;

    public Button Btn_Reset = null;

    public Button Btn_Unfold = null;

    public Button Btn_Fold = null;



    public Button Btn_Camshaftpart = null;

    public Button Btn_Crankshaftpart = null;

    public Button Btn_Crankcasepart = null;

    public Button Btn_CrankcaseScrews = null;

    public Button Btn_RodApart = null;

    public Button Btn_RodBpart = null;

    public Button Btn_RodBpartScrews = null;

    public Button Btn_OilPanpart = null;

    public Button Btn_OilPanpartScrews = null;

    public Button Btn_PushRod = null;

    public Button Btn_GearCrankshafToCamshaft = null;

    public Button Btn_Free_Wheelpart = null;

    public Button Btn_Free_WheelpartScrews = null;


    void Awake()
    {
        Instance = this;

        regBtns();
    }

    private void OnEnable()
    {
        mgrScn = GameObject.FindGameObjectWithTag(Const_SYS.TAG_MGRSCN).GetComponent<MgrScn_Assembly>();

        foldStepsPn();
    }



    void regBtns()
    {
        Btn_AutoNext.onClick.AddListener(delegate() {
            mgrScn.PlayNext();
        });

        Btn_Reset.onClick.AddListener(delegate () {
            mgrScn.ResetAll();
        });

        Btn_Unfold.onClick.AddListener(delegate () {
            unFoldStepsPn();
        });
        Btn_Fold.onClick.AddListener(delegate () {
            foldStepsPn();
        });



        Btn_Camshaftpart.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(0);
        });
        Btn_Crankshaftpart.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(1);
        });
        Btn_Crankcasepart.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(2);
        });
        Btn_CrankcaseScrews.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(3);
        });
        Btn_RodApart.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(4);
        });
        Btn_RodBpart.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(5);
        });
        Btn_RodBpartScrews.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(6);
        });
        Btn_OilPanpart.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(7);
        });
        Btn_OilPanpartScrews.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(8);
        });
        Btn_PushRod.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(9);
        });
        Btn_GearCrankshafToCamshaft.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(10);
        });
        Btn_Free_Wheelpart.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(11);
        });

        Btn_Free_WheelpartScrews.onClick.AddListener(delegate () {
            mgrScn.PlayByStep(12);
        });

    }

    public void RegReceiveMessage(string key,object value)
    {

            string _partNm = value.ToString();

            if (string.IsNullOrEmpty(_partNm))
            {
                _partNm = "none";
            }

            Tx_CurrentPartCtt.text = _partNm;
    }


    private void unFoldStepsPn() {

        rtraSteps.DOAnchorPos(rtraStepsUnfold.anchoredPosition, stepsAnimTime);
    }

    private void foldStepsPn()
    {
        rtraSteps.DOAnchorPos(rtraStepsFold.anchoredPosition, stepsAnimTime);
    }
}
