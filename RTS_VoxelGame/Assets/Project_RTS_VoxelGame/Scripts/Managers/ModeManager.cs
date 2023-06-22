using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class ModeManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern - 
        public static ModeManager Instance;
        void Awake() => Instance = this;
        #endregion

        public ModeType currentMode;

        public void ResetMode() => currentMode = ModeType.ViewMode;
        public void SetMode(ModeType type)
        {
            if (IsOnState(ModeType.ViewMode)) currentMode = type;
        }
        public bool IsOnState(ModeType type) => currentMode.Equals(type);

    }
}