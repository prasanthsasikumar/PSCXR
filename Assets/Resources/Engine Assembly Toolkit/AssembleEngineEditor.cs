using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EngineAssembleScript))]
public class AssembleEngineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EngineAssembleScript myScript = (EngineAssembleScript)target;
        /*if (GUILayout.Button("(Dis)Assemble Engine"))
        {
            myScript.AssembleEngine();
        }*/
        if (GUILayout.Button("Set Step Index"))
        {
            myScript.ClearStepIndexOfChildren();
        }
        if (GUILayout.Button("Play Step"))
        {
            myScript.PlayStep();
        }
        if (GUILayout.Button("Make Owner"))
        {
            myScript.MakeOwner();
        }
        if (GUILayout.Button("Releave Ownership"))
        {
            myScript.ReleaveOwnership();
        }
    }
}
