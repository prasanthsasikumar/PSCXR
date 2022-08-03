using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The animation step unit controls all parts in a step
/// </summary>
[System.Serializable]
public class AnimStepUnit
{
    /// <summary>
    /// 该步骤中所有的部件
    /// </summary>
    public List<AssemblyStepPart> StepParts = new List<AssemblyStepPart>();

    /// <summary>
    /// All components in this step
    /// </summary>
    public int StepIndex = 0;


    public bool IfNextAuto = false;

    /// <summary>
    /// The overall status of the step
    /// </summary>
    public StepState CurretState = StepState.Unactive;
}
