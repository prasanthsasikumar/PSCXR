using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftUpEngine : MonoBehaviour
{
    public Vector3 liftUpPos, LiftDownPos;
    public void LiftUP(bool value)
    {
        if (value) this.transform.SetPositionAndRotation(liftUpPos, this.transform.rotation);
        else this.transform.SetPositionAndRotation(LiftDownPos, this.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
