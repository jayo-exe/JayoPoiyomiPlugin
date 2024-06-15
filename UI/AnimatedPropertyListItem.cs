using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JayoPoiyomiPlugin.UI
{
    public class AnimatedPropertyListItem : MonoBehaviour
    {
        public GameObject nameTextObject;
        public GameObject typeTextObject;
        public GameObject copyButtonObject;

        private string propertyName;
        private string propertyType;

        public void PrepareUI(string propName, string propType)
        {
            propertyName = propName;
            propertyType = propType;
            nameTextObject.GetComponent<Text>().text = propertyName;
            typeTextObject.GetComponent<Text>().text = propertyType;
            copyButtonObject.GetComponent<Button>().onClick.AddListener(() => { CopyProperty(); });
        }


        //copy the currently-selected trigger name to the clipboard
        public void CopyProperty()
        {
            GUIUtility.systemCopyBuffer = propertyName;
        }


    }
}
