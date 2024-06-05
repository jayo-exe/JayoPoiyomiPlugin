using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JayoPoiyomiPlugin
{
    class AnimatedPropertiesList : MonoBehaviour
    {
        public GameObject ListItemPrefab;
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> PropData;

        public void RebuildList()
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach(KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> materialItem in PropData)
            {
                GameObject matLabel = DefaultControls.CreateText(new DefaultControls.Resources());
                Text matText = matLabel.GetComponent<Text>();
                matText.horizontalOverflow = HorizontalWrapMode.Overflow;
                matText.fontStyle = FontStyle.Bold;
                matText.text = materialItem.Key;
                matLabel.transform.SetParent(transform);

                foreach (KeyValuePair< string, Dictionary<string, string>> propertyItem in materialItem.Value)
                {
                    GameObject newLabel = DefaultControls.CreateText(new DefaultControls.Resources());
                    Text newText = newLabel.GetComponent<Text>();
                    newText.horizontalOverflow = HorizontalWrapMode.Overflow;
                    newText.fontSize = 12;
                    newText.GetComponent<RectTransform>().sizeDelta = new Vector2(newText.GetComponent<RectTransform>().sizeDelta.x, 20);
                    newText.text = $"{propertyItem.Value["name"]} ({propertyItem.Value["type"]})";
                    newLabel.transform.SetParent(transform);

                }
            }
        }


    }
}
