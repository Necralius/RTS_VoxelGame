//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Project_RTS_VoxelGame/InputSystem/InputMaps.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputMaps : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaps()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaps"",
    ""maps"": [
        {
            ""name"": ""KeyboardMap"",
            ""id"": ""5a8d4240-44b9-4e7d-99cd-6c53309f47f5"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""54e95697-a93d-4e87-9221-708e48eb1256"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""7d205621-ccf2-4a37-9f3f-31f9bc808396"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ZoomScroll"",
                    ""type"": ""Value"",
                    ""id"": ""3fd1b271-ad78-4394-8ff5-b31e307ceee7"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CancelPlacement"",
                    ""type"": ""Button"",
                    ""id"": ""18f38c5b-29c4-475a-ae6a-ee86937dc88a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""4ce8e7cd-d5f2-47a0-bb03-44d4731d2a94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftShift"",
                    ""type"": ""Button"",
                    ""id"": ""348bcafc-a526-41fb-9f8a-49697e973d4c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f650fda5-e36f-4fa2-9cf3-1821c771e14a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d4d64799-c42d-4ac1-886d-76c0ac0a86d1"",
                    ""path"": ""<Mouse>/delta/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8776ca20-dfa1-4851-9eba-c28c04915598"",
                    ""path"": ""<Mouse>/delta/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7559e6c0-e646-4804-b80c-03a4de3f74f9"",
                    ""path"": ""<Mouse>/delta/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bad13f8c-80e0-4da3-8710-0367ae2bd690"",
                    ""path"": ""<Mouse>/delta/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""25d70d56-bb38-4a37-8a28-f459c08751e7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""03860198-35e7-4a2f-9002-07c56dd07acd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0e27778b-a097-4d05-bb0f-d129e700cc77"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""18790edd-73d0-465f-86f5-8f443d5c9e89"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ba0750a4-d9b3-4417-81bc-53dc02a53da5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""c6ac598a-edbc-40b6-9c40-41cd97ef2c41"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomScroll"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f999f468-0386-4874-81d8-857d7cbf4696"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1366214e-0398-4b58-a407-3687dd9fc2e6"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3d1c2124-6cd1-48a9-b28b-0fc8ae663be7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CancelPlacement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4840890f-91c9-4d01-bf8b-2cf4dc0cac39"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11e88d12-94b4-43db-98da-40f749427d6b"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftShift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // KeyboardMap
        m_KeyboardMap = asset.FindActionMap("KeyboardMap", throwIfNotFound: true);
        m_KeyboardMap_Look = m_KeyboardMap.FindAction("Look", throwIfNotFound: true);
        m_KeyboardMap_Move = m_KeyboardMap.FindAction("Move", throwIfNotFound: true);
        m_KeyboardMap_ZoomScroll = m_KeyboardMap.FindAction("ZoomScroll", throwIfNotFound: true);
        m_KeyboardMap_CancelPlacement = m_KeyboardMap.FindAction("CancelPlacement", throwIfNotFound: true);
        m_KeyboardMap_RightClick = m_KeyboardMap.FindAction("RightClick", throwIfNotFound: true);
        m_KeyboardMap_LeftShift = m_KeyboardMap.FindAction("LeftShift", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // KeyboardMap
    private readonly InputActionMap m_KeyboardMap;
    private IKeyboardMapActions m_KeyboardMapActionsCallbackInterface;
    private readonly InputAction m_KeyboardMap_Look;
    private readonly InputAction m_KeyboardMap_Move;
    private readonly InputAction m_KeyboardMap_ZoomScroll;
    private readonly InputAction m_KeyboardMap_CancelPlacement;
    private readonly InputAction m_KeyboardMap_RightClick;
    private readonly InputAction m_KeyboardMap_LeftShift;
    public struct KeyboardMapActions
    {
        private @InputMaps m_Wrapper;
        public KeyboardMapActions(@InputMaps wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_KeyboardMap_Look;
        public InputAction @Move => m_Wrapper.m_KeyboardMap_Move;
        public InputAction @ZoomScroll => m_Wrapper.m_KeyboardMap_ZoomScroll;
        public InputAction @CancelPlacement => m_Wrapper.m_KeyboardMap_CancelPlacement;
        public InputAction @RightClick => m_Wrapper.m_KeyboardMap_RightClick;
        public InputAction @LeftShift => m_Wrapper.m_KeyboardMap_LeftShift;
        public InputActionMap Get() { return m_Wrapper.m_KeyboardMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardMapActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardMapActions instance)
        {
            if (m_Wrapper.m_KeyboardMapActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnLook;
                @Move.started -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnMove;
                @ZoomScroll.started -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnZoomScroll;
                @ZoomScroll.performed -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnZoomScroll;
                @ZoomScroll.canceled -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnZoomScroll;
                @CancelPlacement.started -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnCancelPlacement;
                @CancelPlacement.performed -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnCancelPlacement;
                @CancelPlacement.canceled -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnCancelPlacement;
                @RightClick.started -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnRightClick;
                @LeftShift.started -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnLeftShift;
                @LeftShift.performed -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnLeftShift;
                @LeftShift.canceled -= m_Wrapper.m_KeyboardMapActionsCallbackInterface.OnLeftShift;
            }
            m_Wrapper.m_KeyboardMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @ZoomScroll.started += instance.OnZoomScroll;
                @ZoomScroll.performed += instance.OnZoomScroll;
                @ZoomScroll.canceled += instance.OnZoomScroll;
                @CancelPlacement.started += instance.OnCancelPlacement;
                @CancelPlacement.performed += instance.OnCancelPlacement;
                @CancelPlacement.canceled += instance.OnCancelPlacement;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @LeftShift.started += instance.OnLeftShift;
                @LeftShift.performed += instance.OnLeftShift;
                @LeftShift.canceled += instance.OnLeftShift;
            }
        }
    }
    public KeyboardMapActions @KeyboardMap => new KeyboardMapActions(this);
    public interface IKeyboardMapActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnZoomScroll(InputAction.CallbackContext context);
        void OnCancelPlacement(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnLeftShift(InputAction.CallbackContext context);
    }
}
