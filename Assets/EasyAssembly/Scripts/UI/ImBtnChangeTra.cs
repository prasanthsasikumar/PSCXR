using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ImBtnChangeTra :MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    
    public GlobleEnum.ARGOChangeTraDir Dir = GlobleEnum.ARGOChangeTraDir.Left;

    private MgrScnBase mgrScn = null;


    private bool ifPress = false;

    public void OnEnable()
    {
        mgrScn = GameObject.FindGameObjectWithTag(Const_SYS.TAG_MGRSCN).GetComponent<MgrScnBase>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        switch (Dir)
        {
            case GlobleEnum.ARGOChangeTraDir.Up:
                mgrScn.UpGO();
            break;
            case GlobleEnum.ARGOChangeTraDir.Down:
                mgrScn.DownGO();
            break;
          
            default:
            break;
        }

        ifPress = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ifPress = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ifPress = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ifPress = false;
        
    }

    public void Update()
    {
        if (ifPress)
        {
            switch (Dir)
            {
                case GlobleEnum.ARGOChangeTraDir.Left:

                    mgrScn.TurnLeft();
                break;
                case GlobleEnum.ARGOChangeTraDir.Right:

                    mgrScn.TurnRight();
                break;
                default:
                break;
            }
        }
    }



}
