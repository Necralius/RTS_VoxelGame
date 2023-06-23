using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NekraliusDevelopmentStudio
{
    public class ResourceOrderView : MonoBehaviour, IPointerClickHandler
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //ResourceOrderView - (0.1)
        //State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        private TextMeshProUGUI resourceName;
        private Image resourceImage;
        private UnityEvent currentOrder;

        public void OnPointerClick(PointerEventData eventData)
        {
            currentOrder.Invoke();
        }

        public void SetUp(string name, Sprite resourceIcon, UnityEvent orderEvent)
        {
            resourceName = GetComponentInChildren<TextMeshProUGUI>();
            resourceImage = GetComponentInChildren<Image>();

            resourceName.text = name;
            resourceImage.sprite = resourceIcon;
            currentOrder = orderEvent;
        }
    }
}