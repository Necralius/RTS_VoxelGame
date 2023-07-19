using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace NekraliusDevelopmentStudio
{
    public class InputManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //InputManager - (0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static InputManager Instance;
        void Awake() => Instance = this;
        #endregion

        #region - Structure Placement System
        [Header("Structure Placement System")]
        public Camera mainSceneCamera;
        public LayerMask gridMask;
        private Vector3 lastPosition;

        #region - Structure Placement Actions -
        public event Action OnClicked;
        public event Action OnExit;
        #endregion

        #endregion

        #region - Dependencies -
        private PlayerInput playerInput => GetComponent<PlayerInput>();
        private InputActionMap currentMap;
        #endregion

        #region - Input Values -
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public float scrollValue { get; private set; }
        public bool rightClick { get; private set; }
        public bool returnMode { get; private set; }
        public bool leftShift { get; private set; }
        public bool rotateObject { get; private set; }
        #endregion

        #region - Input Actions -
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction scrollAction;
        private InputAction rightClickAction;
        private InputAction returnModeAction;
        private InputAction leftShiftAction;
        private InputAction rotateActionAction;
        #endregion

        void Start()
        {
            Instance = this;

            currentMap = playerInput.currentActionMap;
            moveAction = currentMap.FindAction("Move");
            lookAction = currentMap.FindAction("Look");
            scrollAction = currentMap.FindAction("ZoomScroll");
            rightClickAction = currentMap.FindAction("RightClick");
            returnModeAction = currentMap.FindAction("CancelPlacement");
            leftShiftAction = currentMap.FindAction("LeftShift");
            rotateActionAction = currentMap.FindAction("RotateObject");

            moveAction.performed += onMove;
            lookAction.performed += onLook;
            scrollAction.performed += onScroll;
            rightClickAction.performed += onRightClick;
            returnModeAction.performed += onCancelPlacement;
            leftShiftAction.performed += onLeftShift;
            rotateActionAction.performed += onObjectRotate;

            moveAction.canceled += onMove;
            lookAction.canceled += onLook;
            scrollAction.canceled += onScroll;
            rightClickAction.canceled += onRightClick;
            returnModeAction.canceled += onCancelPlacement;
            leftShiftAction.canceled += onLeftShift;
            rotateActionAction.canceled += onObjectRotate;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) OnClicked?.Invoke();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (ModeManager.Instance.IsOnState(ModeType.ViewMode))
                {
                    if (GameManager.Instance.gameIsPaused) GameManager.Instance.ResumeGame();
                    else GameManager.Instance.PauseGame();
                }
                else
                {
                    ModeManager.Instance.ResetMode();
                    OnExit?.Invoke();
                }
            }
        }
        private void onLook(InputAction.CallbackContext context) => Look = context.ReadValue<Vector2>();
        private void onMove(InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
        private void onScroll(InputAction.CallbackContext context) => scrollValue = context.ReadValue<float>();
        private void onRightClick(InputAction.CallbackContext context) => rightClick = context.ReadValueAsButton();
        private void onCancelPlacement(InputAction.CallbackContext context) => returnMode = context.ReadValueAsButton();
        private void onLeftShift(InputAction.CallbackContext context) => leftShift = context.ReadValueAsButton();
        private void onObjectRotate(InputAction.CallbackContext context) => rotateObject = context.ReadValueAsButton();
        public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
        public Vector3 GetSelectedMapPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainSceneCamera.nearClipPlane;
            Ray ray = mainSceneCamera.ScreenPointToRay(mousePos);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, gridMask)) lastPosition = hit.point;
            return lastPosition;
        }
    }
}