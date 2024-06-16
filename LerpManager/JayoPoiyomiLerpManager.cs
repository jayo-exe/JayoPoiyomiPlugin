using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace JayoPoiyomiPlugin.LerpManager
{
    public class JayoPoiyomiLerpManager : MonoBehaviour
    {

        public event Action<string, int, int> IntLerpCalculated;
        public event Action<string, float, int> FloatLerpCalculated;
        public event Action<string, Color, int> ColorLerpCalculated;
        public event Action<string, Vector4, int> Vector4LerpCalculated;
        public event Action<string, Vector2, int> TextureScaleLerpCalculated;
        public event Action<string, Vector2, int> TextureOffsetLerpCalculated;

        private List<ILerpItem> activeLerps = new List<ILerpItem>();

        public void startLerp(string propertyName, int startValue, int targetValue, float lerpTime)
        {
            IntLerpItem item = new IntLerpItem()
            {
                propertyName = propertyName,
                startValue = startValue,
                currentValue = startValue,
                targetValue = targetValue,
                lerpTime = lerpTime,
                currentLerpTime = 0f
            };
            item.LerpCalculated += (p, v, l) => IntLerpCalculated.Invoke(p, v, l);
            activeLerps.Add(item);
        }

        public void startLerp(string propertyName, float startValue, float targetValue, float lerpTime)
        {
            FloatLerpItem item = new FloatLerpItem()
            {
                propertyName = propertyName,
                startValue = startValue,
                currentValue = startValue,
                targetValue = targetValue,
                lerpTime = lerpTime,
                currentLerpTime = 0f
            };
            item.LerpCalculated += (p, v, l) => FloatLerpCalculated.Invoke(p, v, l);
            activeLerps.Add(item);
        }

        public void startLerp(string propertyName, Color startValue, Color targetValue, float lerpTime)
        {
            ColorLerpItem item = new ColorLerpItem()
            {
                propertyName = propertyName,
                startValue = startValue,
                currentValue = startValue,
                targetValue = targetValue,
                lerpTime = lerpTime,
                currentLerpTime = 0f
            };
            item.LerpCalculated += (p, v, l) => ColorLerpCalculated.Invoke(p, v, l);
            activeLerps.Add(item);
        }

        public void startLerp(string propertyName, string subType, Vector2 startValue, Vector2 targetValue, float lerpTime)
        {
            if(subType == "scale")
            {
                TextureScaleLerpItem item = new TextureScaleLerpItem()
                {
                    propertyName = propertyName,
                    startValue = startValue,
                    currentValue = startValue,
                    targetValue = targetValue,
                    lerpTime = lerpTime,
                    currentLerpTime = 0f
                };
                item.LerpCalculated += (p, v, l) => TextureScaleLerpCalculated.Invoke(p, v, l);
                activeLerps.Add(item);
            } else if (subType == "offset")
            {
                TextureOffsetLerpItem item = new TextureOffsetLerpItem()
                {
                    propertyName = propertyName,
                    startValue = startValue,
                    currentValue = startValue,
                    targetValue = targetValue,
                    lerpTime = lerpTime,
                    currentLerpTime = 0f
                };
                item.LerpCalculated += (p, v, l) => TextureOffsetLerpCalculated.Invoke(p, v, l);
                activeLerps.Add(item);
            }
            
            
        }

        public void startLerp(string propertyName, Vector4 startValue, Vector4 targetValue, float lerpTime)
        {
            Vector4LerpItem item = new Vector4LerpItem()
            {
                propertyName = propertyName,
                startValue = startValue,
                currentValue = startValue,
                targetValue = targetValue,
                lerpTime = lerpTime,
                currentLerpTime = 0f
            };
            item.LerpCalculated += (p, v, l) => Vector4LerpCalculated.Invoke(p, v, l);
            activeLerps.Add(item);
        }

        private void checkLerps()
        {
            List<ILerpItem> remainingLerps = new List<ILerpItem>();
            foreach (ILerpItem lerpItem in activeLerps)
            {
                lerpItem.currentLerpTime += (Time.deltaTime * 1000);
                lerpItem.DoLerp();
                if (lerpItem.currentLerpTime >= lerpItem.lerpTime)
                {
                    activeLerps.Remove(lerpItem);
                }
            }
        }

        public void Update()
        {
            checkLerps();
        }
    }
}
