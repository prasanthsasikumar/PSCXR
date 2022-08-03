using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssemblyStepTool : EditorWindow
{
    [MenuItem("Tools/Industrial Kit/Assembly")] 

    static void AddWindow()
    {
        Rect _rect = new Rect(0, 0, 380, 210);
        AssemblyStepTool window = (AssemblyStepTool)EditorWindow.GetWindowWithRect(typeof(AssemblyStepTool), _rect, true, "Assembly");
        window.Show();
    }

    GameObject[] selectedGameObjects;


    [InitializeOnLoadMethod]
    public void Awake()
    {
        OnSelectionChange();    
    }

    void OnGUI()
    {
        GUIStyle text_style1 = new GUIStyle();
        text_style1 = GUI.skin.button;
        text_style1.fontSize = 12;
        text_style1.alignment = TextAnchor.MiddleCenter;
        text_style1.normal.textColor = new Color(0, 0, 0);
        text_style1.border = GUI.skin.button.border;

        EditorGUILayout.BeginVertical();
        
        GUILayout.Label("");


        if (GUI.Button(new Rect(30, 30, 320, 30), "Create Assembly Manager"))
        {

            if (GameObject.Find("Mgr_RetarderStep"))
            {
                this.ShowNotification(new GUIContent("Already exists"));
                return;
            }

            EditorApplication.delayCall += CreateRetarderMgr;
        }

        
        GUILayout.Label("");


        if (GUI.Button(new Rect(30, 90, 320, 30), "Single Part With Start And End Pos"))
        {

            if (selectedGameObjects.Length < 1)
            {

                this.ShowNotification(new GUIContent("Select at least one game object"));
                return;
            }
            else if (selectedGameObjects.Length == 2)
            {

                int _ruleStartNum = 0;

                for (int i = 0; i < selectedGameObjects.Length; i++)
                {
                    if (selectedGameObjects[i].name == "Start")
                    {
                        _ruleStartNum++;
                    }
                }

                if (_ruleStartNum != 1)
                {

                    this.ShowNotification(new GUIContent("Pay attention to the naming rules"));
                    return;
                }
            }
            else if (selectedGameObjects.Length == 3)
            {

                int _ruleStartNum = 0;
                int _ruleEndNum = 0;
                for (int i = 0; i < selectedGameObjects.Length; i++)
                {
                    if (selectedGameObjects[i].name == "Start")
                    {
                        _ruleStartNum++;
                    }
                    else if (selectedGameObjects[i].name == "End")
                    {
                        _ruleEndNum++;
                    }
                }

                if (_ruleStartNum != 1 || _ruleEndNum != 1)
                {

                    this.ShowNotification(new GUIContent("Pay attention to the naming rules"));
                    return;
                }
            }
            else if (selectedGameObjects.Length > 3)
            {


                this.ShowNotification(new GUIContent("Select up to three game objects"));
                return;

            }


            if (!GameObject.Find("Mgr_AssemblyStep"))
            {
                CreateRetarderMgr();
            }


            Transform _part = null;
            Transform _start = null;
            Transform _end = null;

            for (int i = 0; i < selectedGameObjects.Length; i++)
            {
                if (selectedGameObjects[i].name == "Start")
                {
                    _start = selectedGameObjects[i].transform;
                }
                else if (selectedGameObjects[i].name == "End")
                {
                    _end = selectedGameObjects[i].transform;
                }
                else
                {
                    _part = selectedGameObjects[i].transform;
                }
            }

            if (selectedGameObjects.Length == 1)
            {
                _start = new GameObject("Start").transform;
                _start.position = _part.position;
                _start.rotation = _part.rotation;
            }

            GameObject _partUnit = new GameObject(_part.name + "part");

            AssemblyStepPart _unitScript = _partUnit.AddComponent<AssemblyStepPart>();

            _partUnit.transform.position = _part.position;

            _partUnit.transform.SetParent(_part.parent);

            if (_start != null)
            {
                _start.SetParent(_partUnit.transform);
            }
            if (_end != null)
            {
                _end.SetParent(_partUnit.transform);
            }
            if (_part != null)
            {
                _part.SetParent(_partUnit.transform);
            }

            if (!_part.GetComponent<AssemblyPart>())
            {
                _part.gameObject.AddComponent<AssemblyPart>();
            }

            if (!_part.GetComponent<MeshCollider>())
            {
                _part.gameObject.AddComponent<MeshCollider>();
            }


            _unitScript.StartTra = _start;
            _unitScript.EndTra = _end;
            _unitScript.AssemblyPart = _part;


            AssetDatabase.Refresh();
        }

        GUILayout.Label("");

        if (GUI.Button(new Rect(30, 150, 320, 30), "Multiple Parts Without Start And End Pos"))
        {

            if (selectedGameObjects.Length < 1)
            {

                this.ShowNotification(new GUIContent("Select at least one game object"));
                return;
            }
            else if (selectedGameObjects.Length > 50)
            {


                this.ShowNotification(new GUIContent("Select up to 50 game objects"));
                return;

            }


            if (!GameObject.Find("Mgr_AssemblyStep"))
            {
                CreateRetarderMgr();
            }

            for (int i = 0; i < selectedGameObjects.Length; i++)
            {
                Transform _part = null;
                _part = selectedGameObjects[i].transform;

                Transform _start = null;
                _start = new GameObject("Start").transform;
                _start.position = _part.position;
                _start.rotation = _part.rotation;

                GameObject _partUnit = new GameObject(_part.name + "part");



                AssemblyStepPart _unitScript = _partUnit.AddComponent<AssemblyStepPart>();

                _partUnit.transform.position = _part.position;

                _partUnit.transform.rotation = _part.rotation;

                _partUnit.transform.SetParent(_part.parent);

                if (_start != null)
                {
                    _start.SetParent(_partUnit.transform);
                }
                if (_part != null)
                {
                    _part.SetParent(_partUnit.transform);
                }

                if (!_part.GetComponent<AssemblyPart>())
                {
                    _part.gameObject.AddComponent<AssemblyPart>();
                }

                if (!_part.GetComponent<MeshCollider>())
                {
                    _part.gameObject.AddComponent<MeshCollider>();
                }


                _unitScript.StartTra = _start;
                _unitScript.AssemblyPart = _part;
            }

            AssetDatabase.Refresh();
        }

        EditorGUILayout.EndVertical();
    }


    void CreateRetarderMgr()
    {
        GameObject _mgrRetarder = new GameObject("Mgr_AssemblyStep");
        _mgrRetarder.AddComponent<MgrAssemblyStep>();
    }

    void OnInspectorUpdate()
    {

        this.Repaint();
    }

    void OnSelectionChange()
    {

        selectedGameObjects = Selection.gameObjects;

    }
}
