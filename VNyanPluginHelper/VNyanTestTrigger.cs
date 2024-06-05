using System;
using System.Collections.Generic;
using System.Text;
using VNyanInterface;
using UnityEngine;

namespace JayoPoiyomiPlugin.VNyanPluginHelper
{
    
    class VNyanTestTrigger: MonoBehaviour, ITriggerInterface
    {
        private VNyanTestTrigger _instance;
        private Action<string> triggerFired;
        private Queue<string> triggerQueue = new Queue<string>();

        public void registerTriggerListener(ITriggerHandler triggerHandler)
        {
            triggerFired += triggerHandler.triggerCalled;
        }

        public void callTrigger(string triggerName)
        {
            Debug.Log($"Enqueueing trigger {triggerName}");
            if (_instance == null)
            {
                return;
            }

            lock (triggerQueue)
            {
                triggerQueue.Enqueue(triggerName);
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Update()
        {
            if (_instance == null)
            {
                return;
            }

            lock (triggerQueue)
            {
                while (triggerQueue.Count > 0)
                {
                    triggerFired.Invoke(triggerQueue.Dequeue());
                }
            }
        }
    }
}
