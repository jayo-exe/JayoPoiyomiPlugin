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

        public ShaderPropertyListData PropData;
        public string SearchTerm = "";
        public string TypeFilter = "";

        private List<string> CollapsedMaterials;
        private Dictionary<string, GameObject> MaterialItems;

        public void Awake()
        {
            CollapsedMaterials = new List<string>();
            MaterialItems = new Dictionary<string, GameObject>();
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
            Dictionary<string, ShaderPropertyListItem> materialsToAdd = new Dictionary<string, ShaderPropertyListItem>();
            List<string> materialsToRemove = new List<string>(MaterialItems.Keys);

            foreach (KeyValuePair<string, ShaderPropertyListItem> materialItem in PropData)
            {
                materialsToRemove.Remove(materialItem.Key);
                if (MaterialItems.ContainsKey(materialItem.Key)) continue;
                materialsToAdd.Add(materialItem.Key, materialItem.Value);
            }

            foreach (string materialName in materialsToRemove)
            {
                MaterialItems[materialName].GetComponent<MaterialListItem>().DestroyChildren();
                Destroy(MaterialItems[materialName]);
                MaterialItems.Remove(materialName);
            }

            foreach (KeyValuePair<string, ShaderPropertyListItem> materialItem in materialsToAdd)
            {
                bool addedMaterialItem = false;
                GameObject matLabel = null;

                foreach (KeyValuePair<string, ShaderPropertyDetails> propertyItem in materialItem.Value)
                {
                    if (SearchTerm != "" && !propertyItem.Value["name"].ToLowerInvariant().Contains(SearchTerm.ToLowerInvariant())) continue;
                    if (TypeFilter != "" && propertyItem.Value["type"] != TypeFilter) continue;

                    if (!addedMaterialItem)
                    {
                        matLabel = GameObject.Instantiate(MaterialItemPrefab);
                        matLabel.transform.SetParent(transform);
                        var materialListItem = matLabel.GetComponent<MaterialListItem>();
                        materialListItem.PrepareUI(materialItem.Key);
                        materialListItem.MaterialListCollapsed += () => { CollapsedMaterials.Add(materialItem.Key); };
                        materialListItem.MaterialListExpanded += () => { CollapsedMaterials.Remove(materialItem.Key); };
                        addedMaterialItem = true;
                        MaterialItems.Add(materialItem.Key, matLabel);
                    }

                    GameObject newLabel = GameObject.Instantiate(ListItemPrefab);
                    newLabel.transform.SetParent(transform);
                    newLabel.GetComponent<AnimatedPropertyListItem>().PrepareUI(propertyItem.Value["name"], propertyItem.Value["type"]);
                    if (addedMaterialItem && matLabel != null)
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

            FilterList();
        }

        public void FilterList()
        {

            foreach (KeyValuePair<string, ShaderPropertyListItem> materialItem in PropData)
            {


                foreach (KeyValuePair<string, ShaderPropertyDetails> propertyItem in materialItem.Value)
                {
                    bool itemMatch = true;
                    if (SearchTerm != "" && !propertyItem.Value["name"].ToLowerInvariant().Contains(SearchTerm.ToLowerInvariant())) itemMatch = false;
                    if (TypeFilter != "" && propertyItem.Value["type"] != TypeFilter) itemMatch = false;
                    MaterialItems[materialItem.Key].SetActive(itemMatch);
                }
            }
        }


    }
}
