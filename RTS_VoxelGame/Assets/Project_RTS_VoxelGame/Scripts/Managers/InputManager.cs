using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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


        private PlayerInput playerInput => GetComponent<PlayerInput>();

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public float scrollValue { get; private set; }

        private InputActionMap currentMap;

        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction scrollAction;

        void Start()
        {
            Instance = this;

            currentMap = playerInput.currentActionMap;
            moveAction = currentMap.FindAction("Move");
            lookAction = currentMap.FindAction("Look");
            scrollAction = currentMap.FindAction("ZoomScroll");

            moveAction.performed += onMove;
            lookAction.performed += onLook;
            scrollAction.performed += onScroll;

            moveAction.canceled += onMove;
            lookAction.canceled += onLook;
            scrollAction.canceled += onScroll;
        }
        private void onLook(InputAction.CallbackContext context) => Look = context.ReadValue<Vector2>();
        private void onMove(InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
        private void onScroll(InputAction.CallbackContext context) => scrollValue = context.ReadValue<float>();
    }
}