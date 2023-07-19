using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class MenuManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //MenuManager - (Version 0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static MenuManager Instance { get; private set; }
        void Awake() => Instance = this;
        #endregion

        [SerializeField] private List<MenuAsset> menuList;

        public void OpenMenu(MenuAsset asset)
        {
            foreach(var menu in menuList)
            {
                if (menu.Equals(asset)) menu.OpenMenu();
                else menu.CloseMenu();
            }
        }
        public void OpenMenu(string menuName)
        {
            foreach (var menu in menuList)
            {
                if (menu.menuName.Equals(menuName)) menu.OpenMenu();
                else menu.CloseMenu();
            }
        }
    }
}