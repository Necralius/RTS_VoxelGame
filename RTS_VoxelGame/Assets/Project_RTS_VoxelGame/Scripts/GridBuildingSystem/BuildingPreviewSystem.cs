using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class BuildingPreviewSystem : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        [SerializeField] private float previewYOffset = 0.06f;

        [SerializeField]
        private GameObject cellIndicator;
        private GameObject previewObject;

        [SerializeField]
        private Material previewMaterialPrefab;
        private Material previewMaterialInstance;

        private Renderer cellIndicatorRenderer;

        private void Start()
        {
            cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>(); 
            previewMaterialInstance = new Material(previewMaterialPrefab);
            cellIndicator.SetActive(false);
        }
        public void ShowPlacementPreview(GameObject Prefab, Vector2Int Size)
        {
            previewObject = Instantiate(Prefab);
            SetPreview(previewObject);
            SetCursor(Size);
            cellIndicator.SetActive(true);
        }


        private void SetPreview(GameObject previewObject)
        {
            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++) materials[i] = previewMaterialInstance;
                renderer.materials = materials;
            }
        }
        private void SetCursor(Vector2Int Size)
        {
            if (Size.x > 0 || Size.y > 0)
            {
                cellIndicator.transform.localScale = new Vector3(Size.x, 1, Size.y);
                cellIndicatorRenderer.material.mainTextureScale = Size;
            }
        }
        public void DisablePlacementPreview()
        {
            cellIndicator.SetActive(false);
            if (previewObject != null) Destroy(previewObject);
        }

        public void UpdatePosition(Vector3 position, bool validity)
        {          
            if (previewObject != null)
            {
                MovePreview(position);
                ApplyFeedbackToPreview(validity);
            }
            MoveCursor(position);
            ApplyFeedbackToCursor(validity);
        }
        private void MovePreview(Vector3 position)
        {
            previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
        }
        private void MoveCursor(Vector3 position)
        {
            cellIndicator.transform.position = position;
        }
        private void ApplyFeedbackToPreview(bool validity)
        {
            Color c = validity ? Color.white : Color.red;
            c.a = 0.5f;
            previewMaterialInstance.color = c;
        }
        private void ApplyFeedbackToCursor(bool validity)
        {
            Color c = validity ? Color.white : Color.red;

            c.a = 0.5f;
            cellIndicatorRenderer.material.color = c;
        }
        internal void ShowPlacementRemovePreview()
        {
            cellIndicator.SetActive(true);
            SetCursor(Vector2Int.one);
            ApplyFeedbackToCursor(false);
        }
    }
}