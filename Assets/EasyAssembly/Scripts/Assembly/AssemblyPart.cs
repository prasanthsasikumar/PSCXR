using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Specific parts involved in assembly
/// </summary>
public class AssemblyPart : MonoBehaviour
{

    private Color _OriColor;

    private Color _ChangeColor = new Color(244f/255f,160f/ 255f, 34f/ 255f, 1f);

    private void Start()
    {
        _OriColor = GetComponent<Renderer>().material.color;
    }

    private void OnMouseEnter()
    {
        //GetRay();
    }

    private void OnMouseExit()
    {
        //OffRay();
    }

    private void OnMouseDown()
    {
        ActivePart();
    }

    public string GetRay()
    {
        string _partNm = string.Empty;

        GetComponent<Renderer>().material.color = _ChangeColor;

        MeshFilter[] _mF = transform.GetComponentsInChildren<MeshFilter>();
        if (_mF != null )
        {
            for (int i = 0; i < _mF.Length; i++)
            {
                _mF[i].GetComponent<Renderer>().material.color = _ChangeColor;
            }
        }

        _partNm = transform.parent.GetComponent<AssemblyStepPart>().PartNm;

        Nor_Assembly.Instance.RegReceiveMessage("", _partNm);


        return _partNm;
    }

    public string GetNm()
    {
        string _partNm = string.Empty;
       
        _partNm = transform.parent.GetComponent<AssemblyStepPart>().PartNm;

        return _partNm;
    }

    public void OffRay()
    {
        GetComponent<Renderer>().material.color = _OriColor;

        MeshFilter[] _mF = transform.GetComponentsInChildren<MeshFilter>();
        if (_mF != null)
        {
            for (int i = 0; i < _mF.Length; i++)
            {
                _mF[i].GetComponent<Renderer>().material.color = _OriColor;
            }
        }

        Nor_Assembly.Instance.RegReceiveMessage("", string.Empty);

    }


    public void ActivePart()
    {

        if (transform.parent.GetComponent<AssemblyStepPart>())
        {

            AssemblyStepPart _stepPart = transform.parent.GetComponent<AssemblyStepPart>();

            bool _ifCanActive = MgrAssemblyStep.Instance.IfPartCanSingleActive(_stepPart);

            if (_ifCanActive)
            {
                _stepPart.SingleActivePart();
            }
            else
            {
                Debug.Log("wrong step");
            }
        }
    }
}
