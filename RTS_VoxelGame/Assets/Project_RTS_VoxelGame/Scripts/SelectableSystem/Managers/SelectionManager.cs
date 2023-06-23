using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NekraliusDevelopmentStudio
{
    public class SelectionManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //SelectionManager - (0.6)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static SelectionManager Instance;
        private void Awake() => Instance = this;
        #endregion

        public SerializableDictionary<int, GameObject> selectedTable = new SerializableDictionary<int, GameObject>();
        
        private InputManager inputManager => InputManager.Instance;

        private void AddSelected(GameObject selectedObject)
        {
            int id = selectedObject.GetInstanceID();

            if (!(selectedTable.ContainsKey(id)))
            {
                Selectable selectableObject = selectedObject.GetComponent<Selectable>();
                selectableObject.OnSelect();

                selectedTable.Add(id, selectedObject);

                SelectableView.Instance.ActivateView(selectableObject.resource);
            }
        }

        public void Deselect(int id)
        {
            selectedTable[id].GetComponent<Selectable>().OnDeselect(false);
            selectedTable.Remove(id);
            SelectableView.Instance.UpdateView();
        }
        public void DeselectAll()
        {
            foreach(KeyValuePair<int, GameObject> pair in selectedTable) if (pair.Value != null) selectedTable[pair.Key].GetComponent<Selectable>().OnDeselect(true);
            selectedTable.Clear();
            SelectableView.Instance.UpdateView();
        }

        public void SelectAction()
        {
            if (ModeManager.Instance.IsOnState(ModeType.ViewMode))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (inputManager.IsPointerOverUI()) return;
                    Ray ray = inputManager.mainSceneCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, 50000.0f))
                    {
                        if (hit.transform.GetComponent<Selectable>())
                        {
                            if (inputManager.leftShift)
                            {
                                if (hit.transform.GetComponent<Selectable>().isSelected) Deselect(hit.transform.gameObject.GetInstanceID());
                                else AddSelected(hit.transform.gameObject);
                            }
                            else
                            {
                                DeselectAll();
                                AddSelected(hit.transform.gameObject);
                            }
                        }
                        else if (!inputManager.leftShift) DeselectAll();
                    }
                }
            }
        }
        private void Update() => SelectAction();
    }
}