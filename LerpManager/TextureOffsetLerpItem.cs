﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JayoPoiyomiPlugin.LerpManager
{
    class TextureOffsetLerpItem : ILerpItem
    {
        public event Action<string, Vector2, int> LerpCalculated;
        public string propertyName { get; set; }
        public float lerpTime { get; set; }
        public float currentLerpTime { get; set; }
        public Vector2 startValue { get; set; }
        public Vector2 currentValue { get; set; }
        public Vector2 targetValue { get; set; }

        public void DoLerp()
        {
            float lerpFactor = Math.Min((currentLerpTime / lerpTime), 1.0f);
            currentValue = Vector2.Lerp(startValue, targetValue, lerpFactor);
            LerpCalculated.Invoke(propertyName, currentValue, 0);
        }
    }

    
}
