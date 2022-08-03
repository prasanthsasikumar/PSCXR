using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.XR;


namespace Ubiq.Avatars
{

    public class AvatarHintFloat : MonoBehaviour, IAvatarHintProvider<float>
    {
        public AvatarHints.NodeFloat node;
        HandController controller;
        PhysiologicalDataController physiologicalDataController;

        void OnEnable()
        {
            AvatarHints.AddProvider(node, this);
            if (node == AvatarHints.NodeFloat.LeftHandGrip || node == AvatarHints.NodeFloat.RightHandGrip)
            {
                controller = GetComponent<HandController>();
            }else if (node == AvatarHints.NodeFloat.HearRate || node == AvatarHints.NodeFloat.Cognitive_load || node == AvatarHints.NodeFloat.Attention)
            {
                physiologicalDataController = GetComponent<PhysiologicalDataController>();
            }
        }

        void OnDisable()
        {
            AvatarHints.RemoveProvider(node, this);
        }

        public float Provide()
        {
            if (controller != null)
            {
                if(node == AvatarHints.NodeFloat.LeftHandGrip || node == AvatarHints.NodeFloat.RightHandGrip)
                {
                    return controller.GripValue;
                }
                else
                {
                    return 0.0f;
                }                
            } else if(physiologicalDataController != null)
            {                
                if (node == AvatarHints.NodeFloat.HearRate)
                {
                    return physiologicalDataController.heartRate;
                }
                else if (node == AvatarHints.NodeFloat.Cognitive_load)
                {
                    return physiologicalDataController.cognitive_load;
                }
                else if (node == AvatarHints.NodeFloat.Attention)
                {
                    return physiologicalDataController.attention;
                }
                else
                {
                    return 0.0f;
                }
            }
            else
            {
                return 0.0f;
            }
        }
    }

}

