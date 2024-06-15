using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JayoPoiyomiPlugin.LerpManager
{
    class ColorLerpItem : ILerpItem
    {
        public JayoPoiyomiPlugin plugin { get; set; }
        public string propertyName { get; set; }
        public float lerpTime { get; set; }
        public float currentLerpTime { get; set; }
        public Color startValue { get; set; }
        public Color currentValue { get; set; }
        public Color targetValue { get; set; }

        public void DoLerp()
        {
            float lerpFactor = Math.Min((currentLerpTime / lerpTime), 1.0f);
            currentValue = Color.Lerp(startValue, targetValue, lerpFactor);
            plugin.setPoiyomiColor(propertyName, currentValue, 0);
        }
    }

}
