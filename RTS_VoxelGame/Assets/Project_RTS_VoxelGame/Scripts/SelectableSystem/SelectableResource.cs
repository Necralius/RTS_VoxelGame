using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class SelectableResource : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public string SelectableName = "Item_Name";
        [TextArea(3, 10)]
        public string SelectableDescription = "Item_Description";

        [HideInInspector] public float itemMinQuantity;
        [HideInInspector] public float itemMaxQuantity;

        public int quantity;
        
        public bool GetWithRandomFactor;
    }
}