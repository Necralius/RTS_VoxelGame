using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class SelectableResourceModel : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //SelectableResourceModel - (0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)


        public string SelectableName = "Item_Name";
        [TextArea(3, 10)]
        public string SelectableDescription = "Item_Description";

        public bool isSelected = false;

        public List<OrderType> orders;

        public void ExecuteOrder(OrderType order)
        {
            orders.Find(o => o == order).orderAction.Invoke();
        }
    }
}