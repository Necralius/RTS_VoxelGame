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
        PeasantGoToMode,
    }
    public enum ResourceType
    {
        RawWood = 0,
        RawMetal = 1
    }
    public enum SelectionType
    {
        WoodResource,
        MetalResource,
        PeasantResource
    }
    public enum PeasantType
    {
        CivilEnginner = 0,
        Archer = 1,
        Warrior = 2
    }
    public enum AudioType
    {
        Master,
        Music,
        SoundEffect
    }

    [Serializable]
    public struct BuildTable
    {
        public ResourceType type;
        public int quantity;
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