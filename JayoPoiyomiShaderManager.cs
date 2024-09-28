using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq.Expressions;
using VNyanInterface;
using JayoPoiyomiPlugin.VNyanPluginHelper;
using JayoPoiyomiPlugin.LerpManager;



namespace JayoPoiyomiPlugin
{

    public class JayoPoiyomiShaderManager : MonoBehaviour, VNyanInterface.ITriggerHandler
    {

        public GameObject windowPrefab;
        public GameObject window;

        public MainThreadDispatcher mainThread;
        public JayoPoiyomiLerpManager lerpManager;
        public Dictionary<string, string> HexToDec;

        
        public event Action<ShaderPropertyListData> PropertiesListUpdated;

        private string[] sep;
        private VNyanHelper _VNyanHelper;
        private GameObject lastAvatar;
        private List<Material> materials;
        private ShaderPropertyListData propData;

        public void Awake()
        {
            sep = new string[] { ";;" };
            _VNyanHelper = new VNyanHelper();
            lerpManager = gameObject.AddComponent<JayoPoiyomiLerpManager>();
            lerpManager.IntLerpCalculated += setPoiyomiInt;
            lerpManager.FloatLerpCalculated += setPoiyomiFloat;
            lerpManager.ColorLerpCalculated += setPoiyomiColor;
            lerpManager.Vector4LerpCalculated += setPoiyomiVector;
            lerpManager.TextureScaleLerpCalculated += setPoiyomiTextureScale;
            lerpManager.TextureOffsetLerpCalculated += setPoiyomiTextureOffset;
        }

        public void Update()
        {
            findPoiyomiMaterials();
        }

        public void triggerCalled(string triggerName, int value1, int value2, int value3, string text1, string text2, string text3)
        {
            if (!triggerName.StartsWith("_xjp_")) return;

            //check if this is using the legacy structured trigger names to pass arguments, and pass the the legacy hander if so
            if(triggerName.Contains(";;"))
            {
                handleLegacyTriggers(triggerName);
                return;
            }

            switch (triggerName)
            {
                case "_xjp_refetch":
                    findPoiyomiMaterials(true);
                    break;
                case "_xjp_setfloat":
                    setPoiyomiFloat(_VNyanHelper.parseStringArgument(text1), _VNyanHelper.parseFloatArgument(text2), value1);
                    break;
                case "_xjp_settexscale":
                    string[] tileValues = _VNyanHelper.parseStringArgument(text2).Split(new string[] { "," }, StringSplitOptions.None);
                    Vector2 newScaleValue = new Vector2(1, 1);
                    if (tileValues.Length == 2)
                    {
                        float x = _VNyanHelper.parseFloatArgument(tileValues[0]);
                        float y = _VNyanHelper.parseFloatArgument(tileValues[1]);
                        newScaleValue = new Vector2(x, y);
                    }
                    setPoiyomiTextureScale(_VNyanHelper.parseStringArgument(text1), newScaleValue, value1);
                    break;
                case "_xjp_settexoffset":
                    string[] locValues = _VNyanHelper.parseStringArgument(text2).Split(new string[] { "," }, StringSplitOptions.None);
                    Vector2 newOffsetValue = new Vector2(1, 1);
                    if (locValues.Length == 2)
                    {
                        float x = _VNyanHelper.parseFloatArgument(locValues[0]);
                        float y = _VNyanHelper.parseFloatArgument(locValues[1]);
                        newOffsetValue = new Vector2(x, y);
                    }
                    setPoiyomiTextureOffset(_VNyanHelper.parseStringArgument(text1), newOffsetValue, value1);
                    break;
                case "_xjp_setvector":
                    string[] vecValues = _VNyanHelper.parseStringArgument(text2).Split(new string[] { "," }, StringSplitOptions.None);
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
                    setPoiyomiVector(_VNyanHelper.parseStringArgument(text1), newVectorValue, value1);
                    break;
                case "_xjp_setint":
                    setPoiyomiInt(_VNyanHelper.parseStringArgument(text1), (int)_VNyanHelper.parseFloatArgument(text2), value1);
                    break;
                case "_xjp_setcolor":
                    string[] colorValues = _VNyanHelper.parseStringArgument(text2).Split(new string[] { "," }, StringSplitOptions.None);
                    Color newColorValue = new Color();
                    if (colorValues.Length >= 3)
                    {
                        newColorValue.r = _VNyanHelper.parseFloatArgument(colorValues[0]);
                        newColorValue.g = _VNyanHelper.parseFloatArgument(colorValues[1]);
                        newColorValue.b = _VNyanHelper.parseFloatArgument(colorValues[2]);
                    }
                    if (colorValues.Length >= 4)
                    {
                        newColorValue.a = _VNyanHelper.parseFloatArgument(colorValues[3]);
                    }

                    setPoiyomiColor(_VNyanHelper.parseStringArgument(text1), newColorValue, value1);
                    break;
                case "_xjp_setcolorhex":
                    Color newHexColorValue = new Color();
                    ColorUtility.TryParseHtmlString(_VNyanHelper.parseStringArgument(text2), out newHexColorValue);

                    setPoiyomiColor(_VNyanHelper.parseStringArgument(text1), newHexColorValue, value1);
                    break;
                default:
                    break;
            }

        }

        public void handleLegacyTriggers(string triggerName)
        {
            try
            {
                _VNyanHelper.setVNyanParameterString("_xjp_last_trigger", triggerName);
                string[] triggerParts = triggerName.Split(sep, StringSplitOptions.None);
                string actionName = triggerParts.Length > 0 ? triggerParts[0] : "";
                string propName = triggerParts.Length > 1 ? triggerParts[1] : "";
                string args = triggerParts.Length > 2 ? triggerParts[2] : "";
                int lerpTime = triggerParts.Length > 3 ? Int32.Parse(triggerParts[3]) : 0;

                //Debug.Log($"Trigger Details. action: {actionName} | prop: {propName} | args: {args}");
                switch (actionName)
                {
                    case "_xjp_refetch":
                        findPoiyomiMaterials(true);
                        break;
                    case "_xjp_setfloat":
                        setPoiyomiFloat(_VNyanHelper.parseStringArgument(propName), _VNyanHelper.parseFloatArgument(args), lerpTime);
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
                        setPoiyomiTextureScale(_VNyanHelper.parseStringArgument(propName), newScaleValue, lerpTime);
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
                        setPoiyomiTextureOffset(_VNyanHelper.parseStringArgument(propName), newOffsetValue, lerpTime);
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
                        setPoiyomiVector(_VNyanHelper.parseStringArgument(propName), newVectorValue, lerpTime);
                        break;
                    case "_xjp_setint":
                        setPoiyomiInt(_VNyanHelper.parseStringArgument(propName), (int)_VNyanHelper.parseFloatArgument(args), lerpTime);
                        break;
                    case "_xjp_setcolor":
                        string[] colorValues = _VNyanHelper.parseStringArgument(args).Split(new string[] { "," }, StringSplitOptions.None);
                        Color newColorValue = new Color();
                        if (colorValues.Length >= 3)
                        {
                            newColorValue.r = _VNyanHelper.parseFloatArgument(colorValues[0]);
                            newColorValue.g = _VNyanHelper.parseFloatArgument(colorValues[1]);
                            newColorValue.b = _VNyanHelper.parseFloatArgument(colorValues[2]);
                        }
                        if (colorValues.Length >= 4)
                        {
                            newColorValue.a = _VNyanHelper.parseFloatArgument(colorValues[3]);
                        }

                        setPoiyomiColor(_VNyanHelper.parseStringArgument(propName), newColorValue, lerpTime);
                        break;
                    case "_xjp_setcolorhex":
                        Color newHexColorValue = new Color();

                        ColorUtility.TryParseHtmlString(_VNyanHelper.parseStringArgument(args), out newHexColorValue);

                        setPoiyomiColor(_VNyanHelper.parseStringArgument(propName), newHexColorValue, lerpTime);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Unable to process trigger in JayoPoiyomiPlugin: {e.Message} ; {e.StackTrace}");
            }
        }

        public void setPoiyomiFloat(string propName, float newValue, int lerpTime)
        {
            //Debug.Log($"Setting Poiyomi Float Value for {propName} to {newValue} over {lerpTime}ms");
            if (newValue == null) return;
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                if (lerpTime > 0)
                {
                    lerpManager.startLerp(propName, material.GetFloat(propName), newValue, (float)lerpTime);
                }
                else
                {
                    material.SetFloat(propName, (float)newValue);
                }

            }
        }

        public void setPoiyomiInt(string propName, int newValue, int lerpTime)
        {
            //Debug.Log($"Setting Poiyomi Int Value for {propName} to {newValue}");
            if (newValue == null) return;
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                if (lerpTime > 0)
                {
                    lerpManager.startLerp(propName, material.GetInt(propName), newValue, (float)lerpTime);
                }
                else
                {
                    material.SetInt(propName, (int)newValue);
                }
            }
        }

        public void setPoiyomiColor(string propName, Color newValue, int lerpTime)
        {
            //Debug.Log($"Setting Poiyomi Color Value for {propName} to {newValue.ToString()}");
            if (newValue == null) return;
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                if (lerpTime > 0)
                {
                    lerpManager.startLerp(propName, material.GetColor(propName), newValue, (float)lerpTime);
                }
                else
                {
                    material.SetColor(propName, (Color)newValue);
                }

            }
        }

        public void setPoiyomiVector(string propName, Vector4 newValue, int lerpTime)
        {
            //Debug.Log($"Setting Poiyomi Vector Value for {propName} to {newValue.x}, {newValue.y}, {newValue.z}, {newValue.w},");
            if (newValue == null) return;
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                if (lerpTime > 0)
                {
                    lerpManager.startLerp(propName, material.GetVector(propName), newValue, (float)lerpTime);
                }
                else
                {
                    material.SetVector(propName, newValue);
                }

            }
        }

        public void setPoiyomiTextureScale(string propName, Vector2 newValue, int lerpTime)
        {
            //Debug.Log($"Setting Poiyomi texture Tiling Value for {propName} to {newValue.x}, {newValue.y}");
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                if (lerpTime > 0)
                {
                    lerpManager.startLerp(propName, "scale", material.GetTextureScale(propName), newValue, (float)lerpTime);
                }
                else
                {
                    material.SetTextureScale(propName, newValue);
                }

            }
        }

        public void setPoiyomiTextureOffset(string propName, Vector2 newValue, int lerpTime)
        {
            //Debug.Log($"Setting Poiyomi texture Offset Value for {propName} to {newValue.x} , {newValue.y}");
            findPoiyomiMaterials();
            foreach (Material material in materials)
            {
                if (lerpTime > 0)
                {
                    lerpManager.startLerp(propName, "offset", material.GetTextureOffset(propName), newValue, (float)lerpTime);
                }
                else
                {
                    material.SetTextureOffset(propName, newValue);
                }
            }
        }

        public void findPoiyomiMaterials(bool force = false)
        {
            GameObject avatar = _VNyanHelper.getAvatarObject();
            if (!force)
            {
                if (avatar == null) return;
                if (avatar == lastAvatar) return;
            }
            lastAvatar = avatar;

            materials = new List<Material>();
            propData = new ShaderPropertyListData();

            foreach (Renderer renderer in GameObject.FindObjectsOfType<Renderer>(true))
            {
                foreach (Material material in renderer.sharedMaterials)
                {
                    //Debug.Log($"Checking Material {material.name}");
                    if (material == null) continue;
                    if (materials.Contains(material)) continue;
                    if (material.shader.name.StartsWith(".poiyomi/") || material.shader.name.StartsWith("Hidden/Locked/.poiyomi/"))
                    {
                        //Debug.Log($"Poiyomi shader found! {material.shader.name} on material {material.name}");
                        materials.Add(material);
                        propData.Add($"{material.name} ({material.GetInstanceID()})", new ShaderPropertyListItem());
                        findAnimatedParameters(material);
                    }
                }
            }

            PropertiesListUpdated.Invoke(propData);
        }

        private List<string> findAnimatedParameters(Material material)
        {
            List<string> parameterList = new List<string>();
            Shader shader = material.shader;
            int propertyCount = shader.GetPropertyCount();
            for (int i = 0; i <= propertyCount - 1; i++)
            {
                //Debug.Log($"Checking shader property {shader.GetPropertyName(i)} | {shader.GetPropertyDescription(i).Split(new String[] { "--" }, StringSplitOptions.None)[0]} ({shader.GetPropertyType(i)})");
                if (shader.GetPropertyName(i).EndsWith(material.name.Replace(" ","")))
                {
                    //Debug.Log($"Found shader renamed property {shader.GetPropertyName(i)} | {shader.GetPropertyDescription(i).Split(new String[] { "--" }, StringSplitOptions.None)[0]} ({shader.GetPropertyType(i)})");
                    int nameId = shader.GetPropertyNameId(i);
                    parameterList.Add(shader.GetPropertyName(i));
                    propData[$"{material.name} ({material.GetInstanceID()})"].Add(shader.GetPropertyName(i), new ShaderPropertyDetails
                    {
                        ["name"] = shader.GetPropertyName(i),
                        ["type"] = shader.GetPropertyType(i).ToString(),
                        ["flag"] = "renamed"
                    });
                }
                else if (material.GetTag($"{shader.GetPropertyName(i)}Animated", false, "") != "")
                {
                    //Debug.Log($"Found animated shader property [{shader.GetPropertyFlags(i)}] {shader.GetPropertyName(i)} | {shader.GetPropertyDescription(i).Split(new String[] { "--" }, StringSplitOptions.None)[0]} ({shader.GetPropertyType(i)})");
                    propData[$"{material.name} ({material.GetInstanceID()})"].Add(shader.GetPropertyName(i), new ShaderPropertyDetails
                    {
                        ["name"] = shader.GetPropertyName(i),
                        ["type"] = shader.GetPropertyType(i).ToString(),
                        ["flag"] = "animated"
                    });
                }
            }
            return parameterList;
        }

    }
}
