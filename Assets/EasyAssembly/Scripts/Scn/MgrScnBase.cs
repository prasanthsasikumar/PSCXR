using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MgrScnBase : MonoBehaviour
{

    public GameObject ARGORoot = null;

    public GameObject ARGO = null;



    public void TurnLeft()
    {
        ARGO.transform.Rotate(new Vector3(0f,1f,0f),Space.Self);
    }

    public void TurnRight()
    {
        ARGO.transform.Rotate(new Vector3(0f, -1f, 0f), Space.Self);
    }

    public void UpGO()
    {
        
        ARGO.transform.DOLocalMoveY(ARGO.transform.localPosition.y + 0.1f, 0.5f);
    }

    public void DownGO()
    {
        ARGO.transform.DOLocalMoveY(ARGO.transform.localPosition.y - 0.1f, 0.5f);
    }
}
