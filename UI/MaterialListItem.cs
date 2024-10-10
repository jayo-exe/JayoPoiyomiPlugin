using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JayoPoiyomiPlugin.UI
{
    public class MaterialListItem : MonoBehaviour
    {
        public GameObject NameTextObject;
        public GameObject CollapseButtonObject;
        public GameObject ExpandButtonObject;
        public List<GameObject> ChildProperties;

        public event Action MaterialListCollapsed;
        public event Action MaterialListExpanded;

        private bool isCollapsed;
        private string materialName;

        public void Awake()
        {
            ChildProperties = new List<GameObject>();
        }

        public void OnDisable()
        {
            foreach (GameObject childProperty in ChildProperties)
            {
                childProperty.SetActive(false);
            }
        }

        public void OnEnable()
        {
            foreach (GameObject childProperty in ChildProperties)
            {
                if (isCollapsed) continue;
                childProperty.SetActive(true);
            }
        }

        public void PrepareUI(string matName)
        {
            materialName = matName;
            NameTextObject.GetComponent<Text>().text = materialName;
            isCollapsed = false;
            CollapseButtonObject.GetComponent<Button>().onClick.AddListener(() => { CollapseChildren(); });
            ExpandButtonObject.GetComponent<Button>().onClick.AddListener(() => { ExpandChildren(); });
            ExpandButtonObject.SetActive(false);
        }

        public void CollapseChildren()
        {
            if (isCollapsed) return;

            foreach(GameObject childProperty in ChildProperties)
            {
                childProperty.SetActive(false);
            }

            isCollapsed = true;
            CollapseButtonObject.SetActive(false);
            ExpandButtonObject.SetActive(true);
            MaterialListCollapsed.Invoke();
        }

        public void ExpandChildren()
        {
            if (!isCollapsed) return;

            foreach (GameObject childProperty in ChildProperties)
            {
                childProperty.SetActive(true);
            }

            isCollapsed = false;
            CollapseButtonObject.SetActive(true);
            ExpandButtonObject.SetActive(false);
            MaterialListExpanded.Invoke();
        }

        public void DestroyChildren()
        {
            foreach (GameObject childProperty in ChildProperties)
            {
                GameObject.Destroy(childProperty);
            }
        }
    }
}
