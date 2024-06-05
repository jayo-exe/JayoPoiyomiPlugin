using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq.Expressions;
using VNyanInterface;
using JayoPoiyomiPlugin.VNyanPluginHelper;

namespace JayoPoiyomiPlugin
{
    public class JayoPoiyomiPlugin : MonoBehaviour, VNyanInterface.IButtonClickedHandler, VNyanInterface.ITriggerHandler
    {
        public GameObject windowPrefab;
        public GameObject window;

        public MainThreadDispatcher mainThread;

        private string[] sep;
        private VNyanHelper _VNyanHelper;
        private GameObject lastAvatar;
        private List<Material> materials;
        private Dictionary<string,Dictionary<string, Dictionary<string, string>>> propData;

        private AnimatedPropertiesList PropertiesList;
        private GameObject instructionsTab;
        private GameObject instructionsButton;
        private GameObject propertyListTab;
        private GameObject propertyListButton;


        public void Start()
        {
            
        }

        public void Update()
        {
            if (PropertiesList == null) return;
            findPoiyomiMaterials();
        }

        public void triggerCalled(string triggerName)
        {
            if (!triggerName.StartsWith("_xjp_")) return;

            try
            {
                _VNyanHelper.setVNyanParameterString("_xjp_last_trigger", triggerName);
                string[] triggerParts = triggerName.Split(sep, StringSplitOptions.None);
                string actionName = triggerParts.Length > 0 ? triggerParts[0] : "";
                string propName = triggerParts.Length > 1 ? triggerParts[1] : "";
                string args = triggerParts.Length > 2 ? triggerParts[2] : "";

                Debug.Log($"Trigger Details. action: {actionName} | prop: {propName} | args: {args}");
                switch (actionName)
                {
                    case "_xjp_refetch":
                        findPoiyomiMaterials(true);
                        break;
                    case "_xjp_setfloat":
                        setPoiyomiFloat(_VNyanHelper.parseStringArgument(propName), _VNyanHelper.parseFloatArgument(args));
                        break;
                    case "_xjp_settexscale":
                        string[] tileValues = _VNyanHelper.parseStringArgument(args).Split(new string[] { "," }, StringSplitOptions.None);
                        Vector2 newScaleValue = new Vector2(1, 1);
                        if (tileValues.Length == 2)
                        {
                            float x = _VNyanHelper.parseFloatArgument(tileValues[0]);
                            float y = _VNyanHelper.parseFloatArgument(tileValues[1]);
                            newScaleValue = new Vector2(x, y);
                        }
                        setPoiyomiTextureScale(_VNyanHelper.parseStringArgument(propName), newScaleValue);
                        break;
                    case "_xjp_settexoffset":
                        string[] locValues = _VNyanHelper.parseStringArgument(args).Split(new string[] { "," }, StringSplitOptions.None);
                        Vector2 newOffsetValue = new Vector2(1, 1);
                        if (locValues.Length == 2)
                        {
                            float x = _VNyanHelper.parseFloatArgument(locValues[0]);
                            float y = _VNyanHelper.parseFloatArgument(locValues[1]);
                            newOffsetValue = new Vector2(x, y);
                        }
                        setPoiyomiTextureOffset(_VNyanHelper.parseStringArgument(propName), newOffsetValue);
                        break;
                    case "_xjp_setvector":
                        string[] vecValues = _VNyanHelper.parseStringArgument(args).Split(new string[] { "," }, StringSplitOptions.None);
                        Vector4 newVectorValue = new Vector4(0, 0, 0, 0);
                        if (vecValues.Length >= 2)
                        {
                            newVectorValue.x = _VNyanHelper.parseFloatArgument(vecValues[0]);
                            newVectorValue.y = _VNyanHelper.parseFloatArgument(vecValues[1]);
                        }
                        if (vecValues.Length >= 3)
                        {
                            newVectorValue.z = _VNyanHelper.parseFloatArgument(vecValues[2]);
                        }
                        if (vecValues.Length >= 4)
                        {
                            newVectorValue.w = _VNyanHelper.parseFloatArgument(vecValues[3]);
                        }
                        setPoiyomiVector(_VNyanHelper.parseStringArgument(propName), newVectorValue);
                        break;
                    case "_xjp_setint":
                        setPoiyomiInt(_VNyanHelper.parseStringArgument(propName), (int)_VNyanHelper.parseFloatArgument(args));
                        break;
                    case "_xjp_setcolor":
                        string[] colorValues = _VNyanHelper.parseStringArgument(args).Split(new string[] { "," }, StringSplitOptions.None);
                        Color newColorValue = new Color();
                        if (colorValues.Length == 4)
                        {
                            newColorValue.r = _VNyanHelper.parseFloatArgument(colorValues[0]);
                            newColorValue.g = _VNyanHelper.parseFloatArgument(colorValues[1]);
                            newColorValue.b = _VNyanHelper.parseFloatArgument(colorValues[2]);
                            newColorValue.a = _VNyanHelper.parseFloatArgument(colorValues[3]);
                        }

                        setPoiyomiColor(_VNyanHelper.parseStringArgument(propName), newColorValue);
                        break;
                    case "_xjp_setcolorhex":
                        Color newHexColorValue = new Color();

                        ColorUtility.TryParseHtmlString(_VNyanHelper.parseStringArgument(args), out newHexColorValue);

                        setPoiyomiColor(_VNyanHelper.parseStringArgument(propName), newHexColorValue);
                        break;
                    default:
                        break;
                }
            } catch(Exception e)
            {
                Debug.Log($"Unable to process trigger in JayoPoiyomiPlugin: {e.Message} ; {e.StackTrace}");
            }
            
        }

        private void setPoiyomiFloat(string propName, float? newValue)
        {
            //Debug.Log($"Setting Poiyomi Float Value for {propName} to {newValue}");
            if (newValue == null) return;
            findPoiyomiMaterials();
            foreach(Material material in materials)
            {
                material.SetFloat(propName, (float)newValue);
            }
        }

        private void setPoiyomiInt(string propName, int? newValue)
        {
            //Debug.Log($"Setting Poiyomi Int Value for {propName} to {newValue}");
            if (newValue == null) return;
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                material.SetInt(propName, (int)newValue);
            }
        }

        private void setPoiyomiColor(string propName, Color? newValue)
        {
            //Debug.Log($"Setting Poiyomi Color Value for {propName} to {newValue.ToString()}");
            if (newValue == null) return;
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                material.SetColor(propName, (Color)newValue);
            }
        }

        private void setPoiyomiVector(string propName, Vector4 newValue)
        {
            //Debug.Log($"Setting Poiyomi Vector Value for {propName} to {newValue.x}, {newValue.y}, {newValue.z}, {newValue.w},");
            if (newValue == null) return;
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                material.SetVector(propName, newValue);
            }
        }

        private void setPoiyomiTextureScale(string propName, Vector2 newValue)
        {
            //Debug.Log($"Setting Poiyomi texture Tiling Value for {propName} to {newValue.x}, {newValue.y}");
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                material.SetTextureScale(propName, newValue);
            }
        }

        private void setPoiyomiTextureOffset(string propName, Vector2 newValue)
        {
            //Debug.Log($"Setting Poiyomi texture Offset Value for {propName} to {newValue.x} , {newValue.y}");
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                material.SetTextureOffset(propName, newValue);
            }
        }

        private void findPoiyomiMaterials(bool force = false)
        {
            GameObject avatar = _VNyanHelper.getAvatarObject();
            if(!force)
            {
                if (avatar == null) return;
                if (avatar == lastAvatar) return;
            }
            lastAvatar = avatar;
            materials = new List<Material>();
            propData = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            foreach (Renderer renderer in avatar.GetComponentsInChildren<Renderer>(true))
            {
                foreach (Material material in renderer.sharedMaterials)
                {
                    if (material == null) continue;
                    if (materials.Contains(material)) continue;
                    if (material.shader.name.StartsWith(".poiyomi/") || material.shader.name.StartsWith("Hidden/Locked/.poiyomi/"))
                    {
                        //Debug.Log($"Poiyomi shader found! {material.shader.name} on material {material.name}");
                        materials.Add(material);
                        propData.Add($"{material.name} ({material.GetInstanceID()})", new Dictionary<string, Dictionary<string, string>>());
                        findAnimatedParameters(material);
                    }
                }
            }
            PropertiesList.PropData = propData;
            PropertiesList.RebuildList();
        }

        private List<string> findAnimatedParameters(Material material)
        {
            List<string> parameterList = new List<string>();
            Shader shader = material.shader;
            int propertyCount = shader.GetPropertyCount();
            for (int i = 0; i <= propertyCount - 1; i++)
            {
                if (shader.GetPropertyName(i).EndsWith(material.name))
                {
                    //Debug.Log($"Found shader renamed property {shader.GetPropertyName(i)} | {shader.GetPropertyDescription(i).Split(new String[] { "--" }, StringSplitOptions.None)[0]} ({shader.GetPropertyType(i)})");
                    int nameId = shader.GetPropertyNameId(i);
                    parameterList.Add(shader.GetPropertyName(i));
                    propData[$"{material.name} ({material.GetInstanceID()})"].Add(shader.GetPropertyName(i), new Dictionary<string, string> { 
                        ["name"] = shader.GetPropertyName(i),
                        ["type"] = shader.GetPropertyType(i).ToString(),
                        ["flag"] = "renamed"
                    });
                } else if (material.GetTag($"{shader.GetPropertyName(i)}Animated", false, "") != "")
                {
                    //Debug.Log($"Found animated shader property [{shader.GetPropertyFlags(i)}] {shader.GetPropertyName(i)} | {shader.GetPropertyDescription(i).Split(new String[] { "--" }, StringSplitOptions.None)[0]} ({shader.GetPropertyType(i)})");
                    propData[$"{material.name} ({material.GetInstanceID()})"].Add(shader.GetPropertyName(i), new Dictionary<string, string>
                    {
                        ["name"] = shader.GetPropertyName(i),
                        ["type"] = shader.GetPropertyType(i).ToString(),
                        ["flag"] = "animated"
                    });
                }
            }
            return parameterList;
        }

        public void Awake()
        {

            Debug.Log($"Poiyomi Plugin is Awake!");
            sep = new string[] { ";;" };
            _VNyanHelper = new VNyanHelper();
            Debug.Log($"Loading Settings");
            // Load settings
            loadPluginSettings();

            Debug.Log($"Beginning Plugin Setup");

            mainThread = gameObject.AddComponent<MainThreadDispatcher>();
            _VNyanHelper.registerTriggerListener(this);
            
            try
            {
                window = _VNyanHelper.pluginSetup(this, "Jayo's Poiyomi Plugin", windowPrefab);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }



            // Hide the window by default
            if (window != null)
            {
                propertyListButton = window.transform.Find("Panel/PropertiesButton").gameObject;
                propertyListTab = window.transform.Find("Panel/PropertiesPanel").gameObject;
                instructionsButton = window.transform.Find("Panel/InstructionsButton").gameObject;
                instructionsTab = window.transform.Find("Panel/InstructionsPanel").gameObject;
                window.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                window.SetActive(false);

                try
                {
                    Debug.Log($"Preparing Plugin Window");

                    window.transform.Find("Panel/TitleBar/CloseButton").GetComponent<Button>().onClick.AddListener(() => { closePluginWindow(); });
                    propertyListButton.GetComponent<Button>().onClick.AddListener(() => { tabToPropertyList(); });
                    instructionsButton.GetComponent<Button>().onClick.AddListener(() => { tabToInstructions(); });
                    PropertiesList = window.transform.Find("Panel/PropertiesPanel/Scroll View/Viewport/PropertiesLayoutGroup").gameObject.GetComponent<AnimatedPropertiesList>();
                    findPoiyomiMaterials();
                    tabToPropertyList();
                }
                catch (Exception e)
                {
                    Debug.Log($"Couldn't prepare Plugin Window: {e.Message}");
                }
            }


        }

        private void OnApplicationQuit()
        {
            // Save settings
            savePluginSettings();
        }

        public void loadPluginSettings()
        {
            // Get settings in dictionary
        }

        public void savePluginSettings()
        {

        }

        public void pluginButtonClicked()
        {
            // Flip the visibility of the window when plugin window button is clicked
            if (window != null)
            {
                window.SetActive(!window.activeSelf);
                if (window.activeSelf)
                {
                    window.transform.SetAsLastSibling();
                }
                    window.transform.SetAsLastSibling();
            }

        }

        public void closePluginWindow()
        {
            window.SetActive(false);
        }

        public void tabToInstructions()
        {
            instructionsTab.SetActive(true);
            propertyListTab.SetActive(false);
            instructionsButton.GetComponent<Button>().interactable = false;
            propertyListButton.GetComponent<Button>().interactable = true;
        }

        public void tabToPropertyList()
        {
            instructionsTab.SetActive(false);
            propertyListTab.SetActive(true);
            instructionsButton.GetComponent<Button>().interactable = true;
            propertyListButton.GetComponent<Button>().interactable = false;
        }

    }
}
