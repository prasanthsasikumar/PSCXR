using System.Collections;
using System.Collections.Generic;
using Ubiq.Samples;
using UnityEngine;

public class EngineAssembleScript : MonoBehaviour
{
    public GameObject scn_manager, assemblyPart;
    public int stepIndex = 0;

   public void AssembleEngine()
    {
        scn_manager.GetComponent<MgrAssemblyStep>().AssembleEngine();
    }

    public void ClearStepIndexOfChildren()
    {
        AssemblyStepPart[] _parts = assemblyPart.GetComponentsInChildren<AssemblyStepPart>();
        foreach(AssemblyStepPart part in _parts)
        {
            part.StepIndex = stepIndex;
        }
    }

    public void PlayStep()
    {
        assemblyPart.GetComponent<AssemblyStepPart>().AcivePart(ProgressState.Reverse);
    }

    public void MakeOwner()
    {
        GameObject[] tools = GameObject.FindGameObjectsWithTag("tool");
        foreach(GameObject tool in tools)
        {
            tool.GetComponent<SharedComponent>().owner = true;
        }
        GameObject.Find("V8Diesel_Assemble").GetComponent<SharedComponent>().owner = true;
    }

    public void ReleaveOwnership()
    {
        GameObject[] tools = GameObject.FindGameObjectsWithTag("tool");
        foreach (GameObject tool in tools)
        {
            tool.GetComponent<SharedComponent>().owner = false;
        }
        GameObject.Find("V8Diesel_Assemble").GetComponent<SharedComponent>().owner = false;
    }
}
