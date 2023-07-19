using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class CameraController : MonoBehaviour, ILoadableData
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CameraController - (0.3)
        //State: Fully Functional
        //This code represents an camera controller of an Real Time Strategy game.

        #region - Camera Settings Data -
        [Header("Movment Data")]
        [SerializeField] private float cameraMovmentSpeed = 1f;
        [SerializeField] private float cameraMovmentSmoothing = 5f;
        [SerializeField] private Vector2 cameraRange = new (100, 100);

        [Header("Rotation Data")]
        [SerializeField] private float cameraRotationSpeed = 15f;
        [SerializeField] private float cameraRotationSmoothing;

        [Header("Zooom Data")]
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float zoomSmoothing = 5f;

        [SerializeField] private Vector2 zoomRange = new (30f, 70f);
        #endregion

        #region - Dependencies and Data -
        [Header("Dependencies")]
        [SerializeField] private Transform cameraHolder;
        private Vector3 cameraDirection => transform.InverseTransformDirection(cameraHolder.forward);
        private Vector3 targetZoomPosition;

        private float zoomInputValue;

        private Vector3 targetPosition;
        #endregion

        #region - Camera Rotation -
        private float targetAngle;
        private float currentAngle;
        private Vector3 startPos;

        private Vector3 input;

        [SerializeField] private bool DrawGizmos = false;
        private InputManager inputAsset => InputManager.Instance;
        #endregion

        //================================Methods================================//

        #region - BuiltIn Methods -
        private void Awake()
        {
            targetPosition = transform.position;
            targetAngle = transform.eulerAngles.y;
            currentAngle = targetAngle;
            targetZoomPosition = cameraHolder.localPosition;
            startPos = transform.position;

            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
            InputHandle();
            CameraMove();
            CameraRotation();
            CameraZoom();
        }
        #endregion

        #region - Camera Input Calculation - 
        private void InputHandle()
        {
            #region - Rotation Input -
            //The below statements detects if the player has the right mouse button pressed, if is true, the camera rotation input angle value is calculated and the mouse cursor is disable.

            if (inputAsset.rightClick)
            {
                targetAngle += inputAsset.Look.x * cameraRotationSpeed;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else Cursor.lockState = CursorLockMode.None;// -> If the right mouse button is not pressed, the cursor is activated again.
            #endregion

            
            zoomInputValue = Input.GetAxisRaw("Mouse ScrollWheel");// -> The zoom input value is get from the InputManager.
            
            //The below statement get the complete input values from the input manager and makes an new 3D vector that holds all the normalized input vector value.
            Vector3 right = transform.right * inputAsset.Move.x;
            Vector3 forward = transform.forward * inputAsset.Move.y;

            input = (forward + right).normalized;
        }
        #endregion

        #region - Camera Move System -
        private void CameraMove()
        { 
            //This method get the camera movment input and using an camera movment speed, moves the camera in the world, considering an area range as limiter.
            Vector3 nextTargetPosition = targetPosition + input * cameraMovmentSpeed;
            if (IsInMoveBounds(nextTargetPosition)) targetPosition = nextTargetPosition;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraMovmentSmoothing);
        }
        private bool IsInMoveBounds(Vector3 position)
        {
            //This method return if the vector passed as an argument is an valid position in the movment bounds.
            return position.x > -cameraRange.x && 
                   position.x < cameraRange.x && 
                   position.z > -cameraRange.y && 
                   position.z < cameraRange.y;
        }
        #endregion

        #region - Camera Rotation System -
        private void CameraRotation()
        {
            //This method get the rotation input and apply on the camera rotation using an Lerp Method to interpolates.
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * cameraRotationSmoothing);
            transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
        }
        #endregion

        #region - Camera Zoom System -
        private void CameraZoom()
        {
            //This method get the camera zoom input and using an camera zoom speed, moves the camera in the world, considering the zoom range as an limiter.
            Vector3 nextTargetPosition = targetZoomPosition + cameraDirection * (zoomInputValue * zoomSpeed);
            if (IsInZoomBounds(nextTargetPosition)) targetZoomPosition = nextTargetPosition;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, targetZoomPosition, Time.deltaTime * zoomSmoothing);
        }
        //The below method return if the vector passed as an argument is an valid zoom in the zoom range bounds.
        private bool IsInZoomBounds(Vector3 position) => position.magnitude > zoomRange.x && position.magnitude < zoomRange.y;
        #endregion

        #region - Camera Range Debug -
        private void OnDrawGizmos()
        {
            //This method draw an gizmos to debug the camera range.
            if (DrawGizmos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position, 5f);
                Gizmos.DrawWireCube(startPos, new Vector3(cameraRange.x * 2f, 5f, cameraRange.y * 2f));
            }
        }
        #endregion

        #region - Data Load -
        public void Load(GameStateData data)
        {
            transform.position = data.cameraData.Position;
            transform.rotation = data.cameraData.Rotation;
            transform.localScale = data.cameraData.Scale;
            Debug.Log("Loading Data!");
        }
        #endregion
    }
}