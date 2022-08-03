using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SimpleCSVLogger : MonoBehaviour
{
    public string fileName="pscvr.csv";
    public enum PhysiologicalDataType {
        Timestamp, HeartRate, CognitiveLoad, Attention, Relaxation, leftPupilDialation, 
        rightPupilDialation, collidingObject, Condition,
        AF3, AF4, FP1, FP2, AF7, AF8
    }
    [System.Serializable]
    public struct PhysiologicalValues
    {
        public PhysiologicalDataType dataType;
        public float value;
        public string stringValue;

        public PhysiologicalValues(PhysiologicalDataType dataType, float value, string stringValue)
        {
            this.dataType = dataType;
            this.value = value;
            this.stringValue = stringValue;
        }
    }
    public List<PhysiologicalValues> physiologicalData = new List<PhysiologicalValues>();


    private bool isWriting = false;
    private StreamWriter outStream;

    void Update()
    {
        if (!isWriting) return;

        string content = "";
        for (int i = 0; i < physiologicalData.Count; i++)
        {
            string value = "";
            //switch (physiologicalData[i].dataType)
            //{
            //    case PhysiologicalDataType.Timestamp:
            //        value = DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond;
            //        break;
            //    case PhysiologicalDataType.HeartRate:
            //        value = physiologicalData[i].value.ToString();
            //        break;
            //    case PhysiologicalDataType.CognitiveLoad:
            //        value = physiologicalData[i].value.ToString();
            //        break;
            //    case PhysiologicalDataType.Attention:
            //        value = physiologicalData[i].value.ToString();
            //        break;
            //    case PhysiologicalDataType.leftPupilDialation:
            //        value = physiologicalData[i].value.ToString();
            //        break;
            //    case PhysiologicalDataType.rightPupilDialation:
            //        value = physiologicalData[i].value.ToString();
            //        break;
            //    case PhysiologicalDataType.collidingObject:
            //        value = physiologicalData[i].stringValue.ToString();
            //        break;
            //    case PhysiologicalDataType.Condition:
            //        value = physiologicalData[i].stringValue.ToString();
            //        break;
            //    case PhysiologicalDataType.AF3:
            //        value = physiologicalData[i].stringValue.ToString();
            //        break;
            //}

            if (physiologicalData[i].dataType == PhysiologicalDataType.Timestamp) value = DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond;
            else if (physiologicalData[i].dataType == PhysiologicalDataType.HeartRate || physiologicalData[i].dataType == PhysiologicalDataType.CognitiveLoad || physiologicalData[i].dataType == PhysiologicalDataType.Attention || physiologicalData[i].dataType == PhysiologicalDataType.Relaxation) value = physiologicalData[i].value.ToString();
            else if (physiologicalData[i].dataType == PhysiologicalDataType.leftPupilDialation || physiologicalData[i].dataType == PhysiologicalDataType.rightPupilDialation) value = physiologicalData[i].value.ToString();
            else value = physiologicalData[i].stringValue;

            content = content + value + ",";
        }
        outStream.WriteLine(content);
    }

    public void SetEyeTrackingData(float[] pupilDialation, string collidingObject)
    {
        for(int i=0; i<physiologicalData.Count; i++)
        {
            if(physiologicalData[i].dataType == PhysiologicalDataType.leftPupilDialation)
            {
                physiologicalData[i] = new PhysiologicalValues(physiologicalData[i].dataType, pupilDialation[0], "");
            }
            else if (physiologicalData[i].dataType == PhysiologicalDataType.rightPupilDialation)
            {
                physiologicalData[i] = new PhysiologicalValues(physiologicalData[i].dataType, pupilDialation[1], "");
            }
            else if (physiologicalData[i].dataType == PhysiologicalDataType.collidingObject)
            {
                physiologicalData[i] = new PhysiologicalValues(physiologicalData[i].dataType, 0, collidingObject);
            }
        }
    }
    public void SetPhysiologicalData(float heartRate, float cognitiveLoad, float attention, float relaxation)
    {
        for (int i = 0; i < physiologicalData.Count; i++)
        {
            if (physiologicalData[i].dataType == PhysiologicalDataType.HeartRate)
            {
                physiologicalData[i] = new PhysiologicalValues(physiologicalData[i].dataType, heartRate, "");
            }
            else if (physiologicalData[i].dataType == PhysiologicalDataType.CognitiveLoad)
            {
                physiologicalData[i] = new PhysiologicalValues(physiologicalData[i].dataType, cognitiveLoad, "");
            }
            else if (physiologicalData[i].dataType == PhysiologicalDataType.Attention)
            {
                physiologicalData[i] = new PhysiologicalValues(physiologicalData[i].dataType, attention, "");
            }
            else if (physiologicalData[i].dataType == PhysiologicalDataType.Relaxation)
            {
                physiologicalData[i] = new PhysiologicalValues(physiologicalData[i].dataType, relaxation, "");
            }
        }
    }
    public void SetCondition(string condition)
    {
        for (int i = 0; i < physiologicalData.Count; i++)
        {
            if (physiologicalData[i].dataType == PhysiologicalDataType.Condition)
            {
                physiologicalData[i] = new PhysiologicalValues(physiologicalData[i].dataType, 0, condition);
            }
        }

    }
    public void SetEEGValue(double value, PhysiologicalDataType dataType)
    {
        for (int i = 0; i < physiologicalData.Count; i++)
        {
            if (physiologicalData[i].dataType == dataType)
                physiologicalData[i] = new PhysiologicalValues(physiologicalData[i].dataType, 0, value.ToString());
        }

    }
    public void StartLogging(string fileId)
    {
        if (isWriting)
        {
            return;
        }

        if(fileId == null)
            outStream = System.IO.File.CreateText(Application.dataPath+"/../CSV/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-")+fileName);
        else
            outStream = System.IO.File.CreateText(Application.dataPath + "/../CSV/" + fileId + fileName);
        // create cols
        string headers = "";
        foreach (PhysiologicalValues physiologicalValue in physiologicalData)
        {
            headers += physiologicalValue.dataType.ToString() + ",";
        }
        outStream.WriteLine(headers);
        isWriting = true;
        Debug.Log("Started CSV logging");
    }
    public void AddMarker(string markerString)
    {
        if (!isWriting) return;

        string rowContent = "";
        foreach (PhysiologicalValues physiologicalValue in physiologicalData)
        {
            rowContent = rowContent + markerString + ",";
        }
        outStream.WriteLine(rowContent);
    }
    public void StopLogging()
    {
        if(isWriting) outStream.Close();
        isWriting = false;
        Debug.Log("Terminated CSV logging");
    }
    private void OnApplicationQuit()
    {
        StopLogging();
    }
}
