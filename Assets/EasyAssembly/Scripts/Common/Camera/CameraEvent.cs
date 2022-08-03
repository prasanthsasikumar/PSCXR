using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCenter
{
    public class CameraEvent : MonoBehaviour
    {

        public delegate void CameraReachAngleHandler(int angleType, bool reach);

        public static event CameraReachAngleHandler CameraReachAngleEvent;

        public static void RaiseCameraReachAngle(int angleType, bool reach)
        {
            if (CameraReachAngleEvent!=null) 
            {
                CameraReachAngleEvent(angleType,reach);
            }
        }

    }
}

