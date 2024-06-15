using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JayoPoiyomiPlugin.UI
{
    public class AnimatedPropertiesList : MonoBehaviour
    {
        public GameObject MaterialItemPrefab;
        public GameObject ListItemPrefab;

        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> PropData;
        public string SearchTerm = "";
        public string TypeFilter = "";

        private List<string> CollapsedMaterials;

        public void Awake()
        {
            CollapsedMaterials = new List<string>();
        }

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
            
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> materialItem in PropData)
            {
                bool addedMaterialItem = false;
                GameObject matLabel = null;

                foreach (KeyValuePair< string, Dictionary<string, string>> propertyItem in materialItem.Value)
                {
                    if (SearchTerm != "" && !propertyItem.Value["name"].ToLowerInvariant().Contains(SearchTerm.ToLowerInvariant())) continue;
                    if (TypeFilter != "" && propertyItem.Value["type"] != TypeFilter) continue;

                    if(!addedMaterialItem)
                    {
                        matLabel = GameObject.Instantiate(MaterialItemPrefab);
                        matLabel.transform.SetParent(transform);
                        var materialListItem = matLabel.GetComponent<MaterialListItem>();
                        materialListItem.PrepareUI(materialItem.Key);
                        materialListItem.MaterialListCollapsed += () => { CollapsedMaterials.Add(materialItem.Key); };
                        materialListItem.MaterialListExpanded += () => { CollapsedMaterials.Remove(materialItem.Key); };
                        addedMaterialItem = true;
                    }

                    GameObject newLabel = GameObject.Instantiate(ListItemPrefab);
                    newLabel.transform.SetParent(transform);
                    newLabel.GetComponent<AnimatedPropertyListItem>().PrepareUI(propertyItem.Value["name"], propertyItem.Value["type"]);
                    if(addedMaterialItem && matLabel != null)
                    {
                        matLabel.GetComponent<MaterialListItem>().ChildProperties.Add(newLabel);
                    }
                    
                }

                if (CollapsedMaterials.Contains(materialItem.Key))
                {
                    CollapsedMaterials.Remove(materialItem.Key);
                    matLabel.GetComponent<MaterialListItem>().CollapseChildren();
                }
            }

        }


    }
}
