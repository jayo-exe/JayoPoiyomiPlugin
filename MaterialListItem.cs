using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JayoPoiyomiPlugin
{
    class MaterialListItem : MonoBehaviour
    {
        public GameObject nameTextObject;

        private string materialName;

        public void PrepareUI(string matName)
        {
            materialName = matName;
            nameTextObject.GetComponent<Text>().text = materialName;
        }
    }
}
