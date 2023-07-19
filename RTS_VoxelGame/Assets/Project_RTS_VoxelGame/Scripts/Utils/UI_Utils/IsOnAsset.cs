using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NekraliusDevelopmentStudio
{
    [RequireComponent(typeof(Toggle))]
    public class IsOnAsset : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        [SerializeField] private List<GameObject> objectsToInteract;
        private Toggle currentToggle;
        [SerializeField] private UnityEvent changeOrder;

        private void Start()
        {
            currentToggle = GetComponent<Toggle>();
            currentToggle.onValueChanged.AddListener(delegate { UpdateState(); });
        }
        public void UpdateState()
        {
            changeOrder.Invoke();
            foreach (GameObject obj in objectsToInteract) obj.SetActive(currentToggle.isOn);
        }
    }
}