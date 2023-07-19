using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class InGameMenuView : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //MenuView - (0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        private Animator anim => GetComponent<Animator>();

        public bool isOpen = false;

        public void InteractWithMenu()
        {
            isOpen = !isOpen;
            anim.SetBool("OpenMenu", isOpen);
        }
        public void CloseMenu()
        {
            isOpen = false;
            anim.SetBool("OpenMenu", false);
        }
    }
}