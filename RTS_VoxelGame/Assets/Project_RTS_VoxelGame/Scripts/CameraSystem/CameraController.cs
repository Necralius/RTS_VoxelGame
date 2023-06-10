using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class CameraController : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CameraController - (0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        private InputManager inputAsset => InputManager.Instance;

        [Header("Camera Sensitivity Settings")]
        [SerializeField] private float cameraMovmentSpeed = 1f;
        [SerializeField] private float cameraRotationSpeed = 15f;

        [Header("Other Settings")]
        [SerializeField] private float cameraMovmentSmoothing = 5f;

        #region - Camera Movment -
        [Header("Camera Movment")]
        [SerializeField] private Vector2 cameraRange = new (100, 100);

        private Vector3 targetPosition;
        private Vector3 input;
        #endregion

        #region - Camera Rotation -
        private float targetAngle;
        private float currentAngle;
        #endregion

        #region - Camera Zoom -
        [Header("Camera Zoom Settings")]
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float zoomSmoothing = 5f;
        [SerializeField] private Vector2 zoomRange = new (30f, 70f);

        [Header("Dependencies")]
        [SerializeField] private Transform cameraHolder;
        private Vector3 cameraDirection => transform.InverseTransformDirection(cameraHolder.forward);
        private Vector3 targetZoomPosition;
        private float zoomInputValue;
        #endregion

        private Vector3 startPos;

        private void Awake()
        {
            targetPosition = transform.position;
            targetAngle = transform.eulerAngles.y;
            currentAngle = targetAngle;
            targetZoomPosition = cameraHolder.localPosition;
            startPos = transform.position;
        }
        private void Update()
        {
            InputHandle();
            CameraMove();
            CameraRotation();
            CameraZoom();
        }

        private void InputHandle()
        {
            if (Input.GetMouseButton(1)) targetAngle += inputAsset.Look.x * cameraRotationSpeed;

            zoomInputValue = Input.GetAxisRaw("Mouse ScrollWheel");

            float x = inputAsset.Move.x;
            float z = inputAsset.Move.y;

            Vector3 right = transform.right * x;
            Vector3 forward = transform.forward * z;

            input = (forward + right).normalized;
        }
        private void CameraMove()
        { 
            Vector3 nextTargetPosition = targetPosition + input * cameraMovmentSpeed;
            if (IsInMoveBounds(nextTargetPosition)) targetPosition = nextTargetPosition;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraMovmentSmoothing);
        }
        private void CameraRotation()
        {
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * cameraMovmentSmoothing);
            transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
        }
        private void CameraZoom()
        {
            Vector3 nextTargetPosition = targetZoomPosition + cameraDirection * (zoomInputValue * zoomSpeed);
            if (IsInZoomBounds(nextTargetPosition)) targetZoomPosition = nextTargetPosition;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, targetZoomPosition, Time.deltaTime * zoomSmoothing);
        }
        private bool IsInZoomBounds(Vector3 position) => position.magnitude > zoomRange.x && position.magnitude < zoomRange.y;
        private bool IsInMoveBounds(Vector3 position)
        {
            return position.x > -cameraRange.x && 
                   position.x < cameraRange.x && 
                   position.z > -cameraRange.y && 
                   position.z < cameraRange.y;
        }

        #region - Camera Range Debug -
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 5f);
            Gizmos.DrawWireCube(startPos, new Vector3(cameraRange.x * 2f, 5f, cameraRange.y * 2f));
        }
        #endregion
    }
}