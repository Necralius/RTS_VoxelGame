using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NekraliusDevelopmentStudio
{
    public class ButtonInteractor : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        [Header("Button Orders")]
        public UnityEvent SelectedOrder;
        public UnityEvent ClickedOrder;

        public void Selected() => SelectedOrder.Invoke();

        public void Clicked() => ClickedOrder.Invoke();
    }
}