using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class InGameMenuManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static InGameMenuManager Instance;
        void Awake() => Instance = this;
        #endregion

        public List<InGameMenuView> allInGameMenus = new List<InGameMenuView>();

        public void CloseAllMenus() => allInGameMenus.ForEach(e => e.CloseMenu());
    }
}