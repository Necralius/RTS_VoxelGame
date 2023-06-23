using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NekraliusDevelopmentStudio
{
    public class RTS_Utillity : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //RTS_Utillity - (0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)
    }
    public enum ModeType
    {
        ViewMode,
        BuildPlacementMode,
        PeasantSpawningMode,
        PeasantManagmentMode,
        PeasantGoToMode
    }

    [Serializable]
    public class OrderType
    {
        public string orderName = "Order_Name";

        public Sprite orderIcon;

        [Header("Order Events")]
        public UnityEvent orderAction;
    }
}