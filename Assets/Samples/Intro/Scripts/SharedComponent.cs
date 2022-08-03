using System;
using Ubiq.Messaging;
using Ubiq.XR;
using UnityEngine;

namespace Ubiq.Samples
{
    public class SharedComponent : MonoBehaviour, INetworkObject, INetworkComponent, IGraspable, IUseable
    {

        public bool owner = false;
        public bool canGrab = false;

        private NetworkContext context;
        private Transform follow;
        private bool attached = false;

        private float lastGrabTimestamp, timebetweenClicks = .5f;

        [Serializable]
        private class State
        {
            public TransformMessage transform;
            public bool lit;
        }
        private State state = new State();

        public void Grasp(Hand controller)
        {
            if (!canGrab) return;

            if(Time.time - lastGrabTimestamp < timebetweenClicks)
            {
                follow = null;
                return;
            }

            follow = controller.transform;
            owner = true;

            lastGrabTimestamp = Time.time;
        }

        public void Release(Hand controller)
        {
            if (!canGrab) return;


            attached = !attached;
            if (!attached)
            { 
                follow = null;
            }
            //follow = null;
        }

        public void Use(Hand controller)
        {
            state.lit = true;
        }

        public void UnUse(Hand controller)
        {
            state.lit = false;
        }


        public NetworkId Id { get; private set; }


        public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
        {
            owner = false;
            JsonUtility.FromJsonOverwrite(message.ToString(), state);
        }

        // Start is called before the first frame update
        void Start()
        {
            Id = NetworkScene.ObjectIdFromName(this);
            context = NetworkScene.Register(this);
            FindRecursive(this.transform);
            if (this.tag == "tool") canGrab = true;
        }

        void FindRecursive(Transform trans)
        {
            foreach (Transform child in trans)
            {
                if (child.gameObject.GetComponent<AssemblyPart>())
                {
                    if (child.gameObject.GetComponent<SharedComponent>()) return;

                    child.gameObject.AddComponent<SharedComponent>();
                    if (owner) child.gameObject.GetComponent<SharedComponent>().owner = true;
                }
                if (child.childCount > 0)
                {
                    FindRecursive(child);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (owner)
            {
                if (follow != null)
                {
                    transform.position = follow.transform.position;
                    transform.rotation = follow.transform.rotation;
                }
                else
                {
                }

                state.transform.position = transform.localPosition;
                state.transform.rotation = transform.localRotation;

                context.Send(ReferenceCountedSceneGraphMessage.Rent(JsonUtility.ToJson(state)));
            }
            else
            {
                transform.localPosition = state.transform.position;
                transform.localRotation = state.transform.rotation;
            }
        }
    }
}
