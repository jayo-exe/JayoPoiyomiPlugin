using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JayoPoiyomiPlugin
{
    class IntLerpItem : ILerpItem
    {
        public JayoPoiyomiPlugin plugin { get; set; }
        public string propertyName { get; set; }
        public float lerpTime { get; set; }
        public float currentLerpTime { get; set; }

        public int startValue { get; set; }
        public int currentValue { get; set; }
        public int targetValue { get; set; }

        public void DoLerp()
        {
            float lerpFactor = Math.Min((currentLerpTime / lerpTime), 1.0f);
            currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, lerpFactor));
            plugin.setPoiyomiInt(propertyName, currentValue, 0);
        }
    }
}
