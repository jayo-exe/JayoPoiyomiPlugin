using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JayoPoiyomiPlugin
{
    class AnimatedPropertiesList : MonoBehaviour
    {
        public GameObject MaterialItemPrefab;
        public GameObject ListItemPrefab;
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> PropData;

        public void ClearList()
        {
            var children = new List<GameObject>();
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }
            children.ForEach(child => Destroy(child));
        }
        public void RebuildList()
        {
            ClearList();
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach(KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> materialItem in PropData)
            {
                GameObject matLabel = GameObject.Instantiate(MaterialItemPrefab);
                matLabel.transform.SetParent(transform);
                matLabel.GetComponent<MaterialListItem>().PrepareUI(materialItem.Key);


                foreach (KeyValuePair< string, Dictionary<string, string>> propertyItem in materialItem.Value)
                {
                    GameObject newLabel = GameObject.Instantiate(ListItemPrefab);
                    newLabel.transform.SetParent(transform);
                    newLabel.GetComponent<AnimatedPropertyListItem>().PrepareUI(propertyItem.Value["name"], propertyItem.Value["type"]);
                }
            }
        }


    }
}
