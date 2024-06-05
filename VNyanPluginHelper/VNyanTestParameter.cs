using System;
using System.Collections.Generic;
using System.Text;
using VNyanInterface;
using UnityEngine;

namespace JayoPoiyomiPlugin.VNyanPluginHelper
{
    
    class VNyanTestParameter: MonoBehaviour, IParameterInterface
    {
        private VNyanTestParameter _instance;
        private Dictionary<string, string> VNyanStringParameters = new Dictionary<string, string>();
        private Dictionary<string, float> VNyanFloatParameters = new Dictionary<string, float>();

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

        public void setVNyanParameterString(string parameterName, string value)
        {

            VNyanTestHarness testHarness = GameObject.FindObjectOfType<VNyanTestHarness>();
            VNyanStringParameters[parameterName] = value;
            testHarness.setStringParameter(parameterName, value);
            testHarness.refreshParameterDisplay();
        }

        public string getVNyanParameterString(string parameterName)
        {

            string loadedParameter;
            if (VNyanStringParameters.TryGetValue(parameterName, out loadedParameter))
            {
                return loadedParameter;
            }
            return "";
        }

        public void setVNyanParameterFloat(string parameterName, float value)
        {
            VNyanTestHarness testHarness = GameObject.FindObjectOfType<VNyanTestHarness>();
            VNyanFloatParameters[parameterName] = value;
            testHarness.setFloatParameter(parameterName, value);
            testHarness.refreshParameterDisplay();
        }

        public float getVNyanParameterFloat(string parameterName)
        {
            float loadedParameter;
            if (VNyanFloatParameters.TryGetValue(parameterName, out loadedParameter))
            {
                return loadedParameter;
            }
            return 0.0f;
        }

        public string fillStringWithParameters(string originalString)
        {
            //need to learn the expected output for this
            return "";
        }

    }
}
