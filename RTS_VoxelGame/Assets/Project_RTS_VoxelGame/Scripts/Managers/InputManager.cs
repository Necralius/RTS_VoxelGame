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

        public Camera mainSceneCamera;
        public LayerMask gridMask;
        private Vector3 lastPosition;

        public event Action OnClicked;
        public event Action OnExit;

        private PlayerInput playerInput => GetComponent<PlayerInput>();

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public float scrollValue { get; private set; }
        public bool rightClick { get; private set; }
        public bool cancelPlacement { get; private set; }

        private InputActionMap currentMap;

        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction scrollAction;
        private InputAction rightClickAction;
        private InputAction cancelPlacementAction;

        void Start()
        {
            Instance = this;

            currentMap = playerInput.currentActionMap;
            moveAction = currentMap.FindAction("Move");
            lookAction = currentMap.FindAction("Look");
            scrollAction = currentMap.FindAction("ZoomScroll");
            rightClickAction = currentMap.FindAction("RightClick");
            cancelPlacementAction = currentMap.FindAction("CancelPlacement");

            moveAction.performed += onMove;
            lookAction.performed += onLook;
            scrollAction.performed += onScroll;
            rightClickAction.performed += onRightClick;
            cancelPlacementAction.performed += onCancelPlacement;

            moveAction.canceled += onMove;
            lookAction.canceled += onLook;
            scrollAction.canceled += onScroll;
            rightClickAction.canceled += onRightClick;
            cancelPlacementAction.canceled += onCancelPlacement;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) OnClicked?.Invoke();
            if (cancelPlacement) OnExit?.Invoke();
        }
        public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
        private void onLook(InputAction.CallbackContext context) => Look = context.ReadValue<Vector2>();
        private void onMove(InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
        private void onScroll(InputAction.CallbackContext context) => scrollValue = context.ReadValue<float>();
        private void onRightClick(InputAction.CallbackContext context) => rightClick = context.ReadValueAsButton();
        private void onCancelPlacement(InputAction.CallbackContext context) => cancelPlacement = context.ReadValueAsButton();

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