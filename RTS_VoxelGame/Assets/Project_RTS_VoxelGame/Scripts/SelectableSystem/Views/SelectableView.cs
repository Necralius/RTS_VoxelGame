using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NekraliusDevelopmentStudio
{
    public class SelectableView : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //SelectableView - (0.3)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static SelectableView Instance;
        private void Awake() => Instance = this;
        #endregion

        public TMP_InputField nameInput;
        public TextMeshProUGUI selectableDescription;
        public TextMeshProUGUI itensSelected;

        private SelectableResource selectable;
        [SerializeField] private GameObject viewCont;

        public void EditingName() => selectable.SelectableName = nameInput.text;

        private void OnEnable()
        {
            if (selectable == null) return;
            nameInput.text = selectable.SelectableName;
            selectableDescription.text = selectable.SelectableDescription;
        }
        public void DeactiveView()
        {
            viewCont.SetActive(false);
            selectable = null;
        }
        public void ActivateView(SelectableResource selectableModel)
        {
            selectable = selectableModel;
            UpdateView();
        }
        public void UpdateView()
        {
            viewCont.SetActive(SelectionManager.Instance.selectedTable.Count > 0);
            if (SelectionManager.Instance.selectedTable.Count > 0)
            {
                nameInput.text = selectable.SelectableName;
                selectableDescription.text = selectable.SelectableDescription;
                itensSelected.gameObject.SetActive(SelectionManager.Instance.selectedTable.Count >= 1);
                itensSelected.text = string.Format("X{0}", SelectionManager.Instance.selectedTable.Count);
            }
        }
    }
}