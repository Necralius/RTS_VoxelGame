using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NekraliusDevelopmentStudio
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class Drp_IsOnAsset : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public int selectedOption;

        [SerializeField] private List<GameObject> objectsToInteract;
        private TMP_Dropdown currentDropdown;
        [SerializeField] private UnityEvent OnChangeOrder;


        private void Start()
        {
            currentDropdown = GetComponent<TMP_Dropdown>();
            currentDropdown.onValueChanged.AddListener(delegate { UpdateState(); });
        }
        public void UpdateState()
        {
            OnChangeOrder.Invoke();
            foreach (GameObject obj in objectsToInteract) obj.SetActive(currentDropdown.value == selectedOption);
        }
    }
}