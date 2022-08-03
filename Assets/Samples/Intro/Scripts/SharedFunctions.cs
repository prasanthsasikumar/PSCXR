using System;
using Ubiq.Messaging;
using Ubiq.XR;
using UnityEngine;
using UnityEngine.Events;

namespace Ubiq.Samples
{
    public class SharedFunctions : MonoBehaviour, INetworkObject, INetworkComponent
    {

        public bool owner = false;
        public bool functionCalled = false;
        public GameLogic gameLogic;

        private NetworkContext context;
        public float lastTimestamp;

        public UnityEvent fadeToBlack, showScene;

        [Serializable]
        private class State
        {
            public string functionName;
            public int value;
            public float timestamp;
        }
        private State state = new State();


        public NetworkId Id { get; private set; }
        public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
        {
            owner = false;
            JsonUtility.FromJsonOverwrite(message.ToString(), state);
            switch (state.functionName)
            {
                case "fadeToBlack": 
                    fadeToBlack.Invoke();
                    break;
                case "ShowInstruction":
                    gameLogic.ShowInstruction(true);
                    break;
                case "HideInstruction":
                    gameLogic.ShowInstruction(false);
                    break;
                case "ExecuteInstruction":
                    gameLogic.ExecuteInstruction(state.value, true);
                    break;
                default:
                    print("Incorrect intelligence level.");
                    break;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Id = NetworkScene.ObjectIdFromName(this);
            context = NetworkScene.Register(this);
            gameLogic = UnityEngine.Object.FindObjectOfType<GameLogic>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!functionCalled)
            {
                CallRemoteFunction("fadeToBlack", 0);
                functionCalled = true;
            }
        }

        public void CallRemoteFunction(string functionName, int value)
        {
            if (!owner)
            {
                Debug.Log("Only owner can call a remote function");
                return;
            }

            state.functionName = functionName;
            state.timestamp = Time.deltaTime;
            state.value = value;
            context.Send(ReferenceCountedSceneGraphMessage.Rent(JsonUtility.ToJson(state)));
        }
    }
}
