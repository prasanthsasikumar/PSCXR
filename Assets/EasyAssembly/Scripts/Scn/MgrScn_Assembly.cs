using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgrScn_Assembly : MgrScnBase
{

    public MgrAssemblyStep MgrAssembly;


    private void Start()
    {
    
    }
    public void PlayNext()
    {
        MgrAssembly.PlayNextStep();
    }

    public void ResetAll()
    {
        MgrAssembly.ResetAllParts();
    }

    public void PlayByStep(int index)
    {
        MgrAssembly.PlayByStep(index);
    }
}
