using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class Selectable : MonoBehaviour, ISelectable
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //Selectable - (0.2)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public SelectableResourceModel resource;
        public GameObject visualObject;

        public bool isSelected = false;

        private void Start()
        {           
            resource = GetComponent<SelectableResourceModel>();
        }
        public void OnSelect()
        {
            visualObject.SetActive(true);
            isSelected = true;
        }

        public void OnDeselect(bool deselectAll)
        {
            visualObject.SetActive(false);

            SelectableView.Instance.UpdateView();
            isSelected = false;
        }
    }
}