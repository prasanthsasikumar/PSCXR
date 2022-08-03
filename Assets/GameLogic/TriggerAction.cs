using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAction : MonoBehaviour
{
    public List<string> AssemblyPartNames = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        if(AssemblyPartNames.Contains(other.gameObject.name))
        other.gameObject.GetComponentInParent<AssemblyStepPart>().SingleActivePart();
    }
}
