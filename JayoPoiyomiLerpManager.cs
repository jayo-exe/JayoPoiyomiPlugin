using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace JayoPoiyomiPlugin
{
    public class JayoPoiyomiLerpManager : MonoBehaviour
    {
        public JayoPoiyomiPlugin plugin { get; set; }
        private List<ILerpItem> activeLerps = new List<ILerpItem>();

        public void startLerp(string propertyName, int startValue, int targetValue, float lerpTime)
        {
            IntLerpItem item = new IntLerpItem()
            {
                plugin = plugin,
                propertyName = propertyName,
                startValue = startValue,
                currentValue = startValue,
                targetValue = targetValue,
                lerpTime = lerpTime,
                currentLerpTime = 0f
            };
            activeLerps.Add(item);
        }

        public void startLerp(string propertyName, float startValue, float targetValue, float lerpTime)
        {
            FloatLerpItem item = new FloatLerpItem()
            {
                plugin = plugin,
                propertyName = propertyName,
                startValue = startValue,
                currentValue = startValue,
                targetValue = targetValue,
                lerpTime = lerpTime,
                currentLerpTime = 0f
            };
            activeLerps.Add(item);
        }

        public void startLerp(string propertyName, Color startValue, Color targetValue, float lerpTime)
        {
            ColorLerpItem item = new ColorLerpItem()
            {
                plugin = plugin,
                propertyName = propertyName,
                startValue = startValue,
                currentValue = startValue,
                targetValue = targetValue,
                lerpTime = lerpTime,
                currentLerpTime = 0f
            };
            activeLerps.Add(item);
        }

        public void startLerp(string propertyName, string subType, Vector2 startValue, Vector2 targetValue, float lerpTime)
        {
            if(subType == "scale")
            {
                TextureScaleLerpItem item = new TextureScaleLerpItem()
                {
                    plugin = plugin,
                    propertyName = propertyName,
                    startValue = startValue,
                    currentValue = startValue,
                    targetValue = targetValue,
                    lerpTime = lerpTime,
                    currentLerpTime = 0f
                };
                activeLerps.Add(item);
            } else if (subType == "offset")
            {
                TextureOffsetLerpItem item = new TextureOffsetLerpItem()
                {
                    plugin = plugin,
                    propertyName = propertyName,
                    startValue = startValue,
                    currentValue = startValue,
                    targetValue = targetValue,
                    lerpTime = lerpTime,
                    currentLerpTime = 0f
                };
                activeLerps.Add(item);
            }
            
            
        }

        public void startLerp(string propertyName, Vector4 startValue, Vector4 targetValue, float lerpTime)
        {
            Vector4LerpItem item = new Vector4LerpItem()
            {
                plugin = plugin,
                propertyName = propertyName,
                startValue = startValue,
                currentValue = startValue,
                targetValue = targetValue,
                lerpTime = lerpTime,
                currentLerpTime = 0f
            };
            activeLerps.Add(item);
        }

        private void checkLerps()
        {
            List<ILerpItem> remainingLerps = new List<ILerpItem>();
            foreach (ILerpItem lerpItem in activeLerps)
            {
                
                lerpItem.currentLerpTime += (Time.deltaTime * 1000);
                lerpItem.DoLerp();
                //Debug.Log($"Checking lerp for {lerpItem.propertyName}. Time: {lerpItem.currentLerpTime}");
                if (lerpItem.currentLerpTime < lerpItem.lerpTime)
                {
                    remainingLerps.Add(lerpItem);
                }
            }

            activeLerps = remainingLerps;
        }

        public void Update()
        {
            checkLerps();
        }
    }
}
