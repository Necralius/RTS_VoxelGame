using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class MenuAsset : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)


        public string menuName;
        public bool isOpen;

        public void OpenMenu()
        {
            isOpen = true;
            gameObject.SetActive(true);
        }
        public void CloseMenu()
        {
            isOpen = false;
            gameObject.SetActive(false);
        }

    }
}