using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ubiq.XR
{
    public class PhysiologicalDataController : PhysiologicalData
    {
        public UnityEvent<float, float, float, float> logValues;

        public List<float> gsrValues = new List<float>();
        public bool keepGSRSampling = false;
        public float low=0, medium=0.5f, high=1;

        private void Update()
        {
            //Cognitive load calculation comes up here
            if(keepGSRSampling) gsrValues.Add(this.cognitive_load);

            NormalizeValues();

            logValues.Invoke(heartRate, gsrRaw, attention, relaxation);
        }

        public float AnalyzeGSRSample()
        {
            float average = 0f;
            foreach(float gsrValue in gsrValues)
            {
                average = average + gsrValue;
            }
            average = average / gsrValues.Count;
            gsrValues = new List<float>();
            keepGSRSampling = false;
            return average;
        }

        public void NormalizeValues()
        {
            heartRate = heartRateRaw;
            if (heartRate < 30) heartRate = 30;
            else if (heartRate > 199) heartRate = 199;

            float[] cognitiveValues = new float[3];
            cognitiveValues[0] = gsrRaw - low;
            cognitiveValues[1] = gsrRaw - medium;
            cognitiveValues[2] = gsrRaw - high;


            float lowestDiff = Mathf.Infinity;
            for(int i = 0; i<cognitiveValues.Length;i++)
            {
                if (cognitiveValues[i] < 0) { cognitiveValues[i] = -cognitiveValues[i]; }
                if (cognitiveValues[i] < lowestDiff)
                {
                    lowestDiff = cognitiveValues[i];
                    if (i == 2) cognitive_load = .99f;
                    else if (i == 1) cognitive_load = .5f;
                    else if (i == 0) cognitive_load = 0f;
                }
            }

            attention = attentionRaw;
        }
    }
}