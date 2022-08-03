 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

   public class CommonHintDlg : MonoBehaviour
{
                   List<string> FilterMessage = new List<string>();
                   List<string> Message = new List<string>();
                   List<GameObject> Cache = new List<GameObject>();
    
                   List<string> FilterMessageOK = new List<string>();
                   List<string> MessageOK = new List<string>();
                   List<GameObject> CacheOK = new List<GameObject>();

    public GameObject UI_HintFrame_Green=null;
    public GameObject UI_HintFrame_Red=null;

    public static CommonHintDlg Instance;

    void Awake()
    {
        Instance = this;
        GameApp.Instance.MgrHintScript = this;
    }

    //public void OpenHintGetAch(string achID) {
    //    UIManager.GetInstance().ShowUI("UI_Pop_GetAchShow");
    //    UIManager.GetInstance().SendMessage("UI_Pop_GetAchShow", "",achID);
    //}
    

    public void OpenHintBoxOK(string msg)
    {
                 if (FilterMessageOK.Contains(msg))
            return;
                 MessageOK.Add(msg);
        UpdateBoxOK();
    }

    void UpdateBoxOK()
    {
                 if (MessageOK.Count == 0)
            return;
                 GameObject go;

                 if (CacheOK.Count == 0)
        {
            //string _path = "UI_Prefabs/Common/UI_HintFrame_Green";
            //GameObject _objHint = Resources.Load<GameObject>(_path);
            go = Instantiate(UI_HintFrame_Green);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

        }
        else
        {
                         go = CacheOK[0];
            CacheOK.RemoveAt(0);
                         go.SetActive(true);
        }

        Vector4 _oldColor = go.GetComponent<Image>().color;
                 go.GetComponent<Image>().color = new Color(_oldColor.x, _oldColor.y, _oldColor.z, 0.6f);
                 Text text = go.transform.Find("Text").GetComponent<Text>();
        string msg = MessageOK[0];
                 text.text = msg;
                 FilterMessageOK.Add(msg);
                 StartCoroutine(CloseBoxOK(go, msg));
        MessageOK.RemoveAt(0);
        if (MessageOK.Count > 0)
            UpdateBoxOK();
    }

    IEnumerator CloseBoxOK(GameObject go, string hint)
    {
                 yield return new WaitForSeconds(1.3f);
        FilterMessageOK.Remove(hint);
        for (int i = 19; i > -1; i--)
        {
                         yield return new WaitForFixedUpdate();

            Vector4 _oldColor = go.GetComponent<Image>().color;
                         go.GetComponent<Image>().color = new Color(_oldColor.x, _oldColor.y, _oldColor.z, i / 20f);
        }
                 go.SetActive(false);
        CacheOK.Add(go);
             }
    

    
    public void OpenHintBox(string msg)
    {
                 if (FilterMessage.Contains(msg))
            return;
                 Message.Add(msg);
        UpdateBox();
    }

    void UpdateBox()
    {
                 if (Message.Count == 0)
            return;
                 GameObject go;
                 if (Cache.Count == 0)
        {
            //             string _path = "UI_Prefabs/Common/UI_HintFrame";
            //GameObject _objHint = Resources.Load<GameObject>(_path);
            go = Instantiate(UI_HintFrame_Red);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }
        else
        {
                         go = Cache[0];
            Cache.RemoveAt(0);
                         go.SetActive(true);
        }

        Vector4 _oldColor = go.GetComponent<Image>().color;
                 go.GetComponent<Image>().color = new Color(_oldColor.x, _oldColor.y, _oldColor.z, 0.6f);
                 Text text = go.transform.Find("Text").GetComponent<Text>();
        string msg = Message[0];
                 text.text = msg;
                 FilterMessage.Add(msg);
                 StartCoroutine(CloseBox(go, msg));
        Message.RemoveAt(0);
        if (Message.Count > 0)
            UpdateBox();
    }

    IEnumerator CloseBox(GameObject go, string hint)
    {
                 yield return new WaitForSeconds(2.0f);
        FilterMessage.Remove(hint);
        for (int i = 19; i > -1; i--)
        {
                         yield return new WaitForFixedUpdate();

            Vector4 _oldColor = go.GetComponent<Image>().color;
                         go.GetComponent<Image>().color = new Color(_oldColor.x, _oldColor.y, _oldColor.z, i / 20f);
        }
                 go.SetActive(false);
        Cache.Add(go);
             }

}
