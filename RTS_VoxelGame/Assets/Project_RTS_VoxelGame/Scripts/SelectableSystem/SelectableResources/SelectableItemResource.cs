using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class SelectableItemResource : SelectableResourceModel
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        [HideInInspector] public float itemMinQuantity;
        [HideInInspector] public float itemMaxQuantity;

        public int quantity;
        
        public bool GetWithRandomFactor;
    }
}