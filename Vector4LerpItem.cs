using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JayoPoiyomiPlugin
{
    class Vector4LerpItem: ILerpItem
    {
        public JayoPoiyomiPlugin plugin { get; set; }
        public string propertyName { get; set; }
        public float lerpTime { get; set; }
        public float currentLerpTime { get; set; }

        public Vector4 startValue { get; set; }
        public Vector4 currentValue { get; set; }
        public Vector4 targetValue { get; set; }

        public void DoLerp()
        {
            float lerpFactor = Math.Min((currentLerpTime / lerpTime), 1.0f);
            currentValue = Vector4.Lerp(startValue, targetValue, lerpFactor);
            plugin.setPoiyomiVector(propertyName, currentValue, 0);
        }
    }
}
