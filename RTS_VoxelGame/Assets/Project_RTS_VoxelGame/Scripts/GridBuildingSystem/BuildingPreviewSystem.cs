using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class BuildingPreviewSystem : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //BuildingPreviewSystem - (0.3)
        //State: Functional
        //This code represents an build preview system, that shows an structure ghost preview on an building placement process.

        #region - Singleton Pattern -
        public static BuildingPreviewSystem Instance;
        private void Awake() => Instance = this;
        #endregion

        #region - Preview Data -
        private float previewYOffset = 0.06f;

        [Header("Preview Data")]
        [SerializeField] private GameObject cellIndicator;
        [SerializeField] private Material previewMaterialPrefab;

        private GameObject previewObject;
        private Material previewMaterialInstance;

        private Renderer cellIndicatorRenderer;
        public Vector3 currentEulerAngles;

        public ObjectData currentData;
        #endregion

        //================================Methods================================//

        #region - BuildIn Methods -
        private void Start()
        {
            //This method execute some action to get all needed data to the preview system functionality, like, instatiates an new material prefab, an deactivates the cellIndicator for certify that he gonna be active only in the preview interaction.
            cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>(); 
            previewMaterialInstance = new Material(previewMaterialPrefab);
            cellIndicator.SetActive(false);
        }
        #endregion

        #region - Placement Preview Show Interaction Start -
        public void ShowPlacementPreview(ObjectData objectData)
        {
            //This method instatiate the preview object and calls the cursor object set, also the method activate the cell indicator.

            previewObject = Instantiate(objectData.prefab);
            currentEulerAngles = previewObject.transform.eulerAngles;

            SetPreview(previewObject);
            SetCursor(objectData.Size);
            cellIndicator.SetActive(true);
            currentData = objectData;
        }
        #endregion

        #region - Placement Preview System -
        private void SetPreview(GameObject previewObject)
        {
            //This method gets the preview object and travels for every material that this object have on his Renderer, changing this material to an new material instance copy.

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
            //This method sets the object preview cursor size to the object size. Also the method verifies if the size is greater than zero, so the system can proceed, if is not, the system will do nothing.

            if (Size.x > 0 && Size.y > 0)
            {
                cellIndicator.transform.localScale = new Vector3(Size.x, 1, Size.y);
                cellIndicatorRenderer.material.mainTextureScale = Size;
            }
        }
        private void RotatePreviewObject()
        {
            cellIndicator.transform.eulerAngles += new Vector3(0, 360 / 4, 0);
            currentEulerAngles = previewObject.transform.eulerAngles += new Vector3(0, 360 / 4, 0);

            if (currentEulerAngles.y == 90) currentData.Size = new Vector2Int(currentData.Size.y, currentData.Size.x);
            else currentData.Size = new Vector2Int(currentData.Size.x, currentData.Size.y);
        }
        #endregion

        #region - Preview System Deactivating -
        public void DisablePlacementPreview()
        {
            //This method simply deactivate the cellIndicator and destroy the object preview, so deactivating the entire Placement Preview System.
            cellIndicator.SetActive(false);
            if (previewObject != null) Destroy(previewObject);
        }
        #endregion

        #region - Preview System Remove Interaction -
        internal void StartRemovePreview()
        {
            //This method starts the preview system, but this method is exclusive to the remove building state.
            cellIndicator.SetActive(true);
            SetCursor(Vector2Int.one);
            CursorFeedbackColorSet(false);
        }
        #endregion

        #region - Cursor and Mouse Update -
        public void UpdatePosition(Vector3 position, bool validity)
        {
            /*This method updates the preview object and cursor position, also updating they feedback color considering they actual validity.
             * Also the method prevent the preview object executing if the system doesn't have an valid preview object reference, so preventing any further bugs.
             */
            if (previewObject != null)
            {
                MovePreview(position);
                PreviewFeedbackColorSet(validity);
            }
            MoveCursor(position);
            CursorFeedbackColorSet(validity);
        }
        private void Update()
        {
            if (ModeManager.Instance.IsOnState(ModeType.BuildPlacementMode) && Input.GetKeyDown(KeyCode.R)) RotatePreviewObject();
        }

        //The below methods sets the preview object position and the cursor position to the position passed as an argument. 
        private void MovePreview(Vector3 position) => previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
        private void MoveCursor(Vector3 position) => cellIndicator.transform.position = position;
        #endregion

        #region - Placement Preview Color Feedback -
        private void PreviewFeedbackColorSet(bool validity)
        {
            /* This method change the preview object main color considering his validity, also the color is passed with an low alpha value, making it an "Ghost" 
             * object copy.
             */

            Color c = validity ? Color.white : Color.red;
            c.a = 0.5f;
            previewMaterialInstance.color = c;
        }
        private void CursorFeedbackColorSet(bool validity)
        {
            /* This method change the cursor object main color considering his validity, also the color is passed with an low alpha value.
             */

            Color c = validity ? Color.white : Color.red;

            c.a = 0.5f;
            cellIndicatorRenderer.material.color = c;
        }
        #endregion
    }
}