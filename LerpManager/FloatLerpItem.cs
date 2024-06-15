using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JayoPoiyomiPlugin.LerpManager
{
    class FloatLerpItem : ILerpItem
    {
        public JayoPoiyomiPlugin plugin { get; set; }
        public string propertyName { get; set; }
        public float lerpTime { get; set; }
        public float currentLerpTime { get; set; }

        public float startValue { get; set; }
        public float currentValue { get; set; }
        public float targetValue { get; set; }

        public void DoLerp()
        {
            float lerpFactor = Math.Min((currentLerpTime / lerpTime), 1.0f);
            currentValue = Mathf.Lerp(startValue, targetValue, lerpFactor);
            plugin.setPoiyomiFloat(propertyName, currentValue, 0);
        }
    }
}
