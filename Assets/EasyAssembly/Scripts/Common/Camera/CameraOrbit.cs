using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;


[Serializable]  
public struct CameraParameter
{
    public bool limitXAngle;

    public float minXAngle;
    public float maxXAngle;

    public bool limitYAngle;

    public float minYAngle;
    public float maxYAngle;

    public float orbitSensitive;

    public float mouseMoveRotio;


    public CameraParameter(bool limitXAngle = true, 
        float minXAngle = 0,
        float maxXAngle=80f,
        bool limitYAngle=false,
        float minYAngle=0f,
        float maxYAngle=90f,
        float orbitSensitive=10f,
        float mouseMoveRotio=0.3f)
    {
        this.limitXAngle = limitXAngle;
        this.minXAngle = minXAngle;
        this.maxXAngle = maxXAngle;
        this.limitYAngle = limitYAngle;
        this.minYAngle = minYAngle;
        this.maxYAngle = maxYAngle;
        this.orbitSensitive = orbitSensitive;
        this.mouseMoveRotio = mouseMoveRotio;
    }


}



public class CameraOrbit : MonoBehaviour {

    private Vector3 lastMousePos;

    private Vector3 targetEulerAngle;

    private Vector3 eulerAngle;


    public CameraParameter freeOrbitParameter;

    private CameraParameter cureentCameraParameter;


    public Transform cameraRootTf;

    public Transform cameraTf;


    private float cameraDistance;

    private float targetCameraDistance;

    private float lastTouchDistance;

    public float minDistance = 30f;

    public float maxDistance = 100f;

    public float mouseScroollRatio =2f;

    public float zoomSensitive = 5f;


    private Quaternion originalRotate;

    
    public float[] yMinAngles;

    public float[] yMaxAngles;

    public bool[] isAlreadyFire;


    void Start()
    {
        originalRotate = cameraRootTf.rotation;
        cameraDistance = cameraTf.localPosition.z;
        targetCameraDistance = cameraDistance;
        cureentCameraParameter = freeOrbitParameter;

        isAlreadyFire = new bool[yMinAngles.Length];
    }

    void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("点到了UI");
            return;
        }


        Oribit();
        Zoom();
     
    }
    
    private void Oribit()
    {

        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            targetEulerAngle.x += -1*(Input.mousePosition.y-lastMousePos.y)*cureentCameraParameter.mouseMoveRotio;
            targetEulerAngle.y += (Input.mousePosition.x - lastMousePos.x) * cureentCameraParameter.mouseMoveRotio;
            
            if (cureentCameraParameter.limitXAngle)
            {
                targetEulerAngle.x = Mathf.Clamp(targetEulerAngle.x,cureentCameraParameter.minXAngle,cureentCameraParameter.maxXAngle);
            }

            if (cureentCameraParameter.limitYAngle)
            {
                targetEulerAngle.y = Mathf.Clamp(targetEulerAngle.y,cureentCameraParameter.minYAngle,cureentCameraParameter.maxYAngle);
            }

            lastMousePos = Input.mousePosition;
        }
        
        if (Input.touchCount<2)
        {
            eulerAngle = Vector3.Lerp(eulerAngle, targetEulerAngle, Time.fixedDeltaTime * cureentCameraParameter.orbitSensitive);

            cameraRootTf.rotation = originalRotate * Quaternion.Euler(eulerAngle);
        }

        FireEvent(cameraRootTf.localEulerAngles.y);
    }
    
    private void Zoom()
    {
        if (Input.touchCount<2) 
        {
            if (Input.GetAxis("Mouse ScrollWheel")!=0) 
            {
                cameraDistance = -cameraTf.localPosition.z;
                targetCameraDistance = cameraDistance - Input.GetAxis("Mouse ScrollWheel") * cameraDistance * mouseScroollRatio;
                targetCameraDistance = Mathf.Clamp(targetCameraDistance,minDistance,maxDistance);
            }
        }
        else
        {
            if (Input.GetTouch(1).phase==TouchPhase.Began)  
            {
                lastTouchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            }
            if (Input.GetTouch(1).phase == TouchPhase.Moved||Input.GetTouch(0).phase==TouchPhase.Moved)

            {
                cameraDistance = -cameraTf.localPosition.z;
                targetCameraDistance = cameraDistance - (Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) - lastTouchDistance) * mouseScroollRatio;
                targetCameraDistance = Mathf.Clamp(targetCameraDistance, minDistance, maxDistance);
                lastMousePos = Input.mousePosition;

            }
        }
        if (Mathf.Abs(targetCameraDistance-cameraDistance)>0.1f) 
        {
            cameraDistance = Mathf.Lerp(cameraDistance, targetCameraDistance, Time.fixedDeltaTime * zoomSensitive);
            cameraTf.localPosition = new Vector3(0f,0f,-cameraDistance);
        }
    }
    
    private void FireEvent(float yAngle)
    {
        for (int i = 0; i < yMinAngles.Length; i++)
        {
            if (yAngle>yMinAngles[i]&&yAngle<yMaxAngles[i])
            {
                if (!isAlreadyFire[i])
                {
                    EventCenter.CameraEvent.RaiseCameraReachAngle(i,true);
                    isAlreadyFire[i] = true;
                }
            }
            else
            {
                if (isAlreadyFire[i])
                {
                    EventCenter.CameraEvent.RaiseCameraReachAngle(i,false);
                    isAlreadyFire[i] = false;
                }
            }
        }
    }
}
