using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JayoPoiyomiPlugin.LerpManager
{
    interface ILerpItem
    {
        string propertyName {get; set;}
        float lerpTime { get; set; }
        float currentLerpTime { get; set; }

        void DoLerp();
    }
}
