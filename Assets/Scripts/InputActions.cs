//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.1.1
//     from Assets/Scripts/InputActions.inputactions
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

public partial class @InputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Gameplay_Battle"",
            ""id"": ""53b0868c-38d0-448e-8d57-3ef36b505eb1"",
            ""actions"": [
                {
                    ""name"": ""Horizontal Axis"",
                    ""type"": ""Value"",
                    ""id"": ""d49ebae0-a82c-47e0-a9e3-e7ca296f893e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Vertical Axis"",
                    ""type"": ""Value"",
                    ""id"": ""0a86aed3-eebf-4c57-8bfa-b56912251883"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""edbf7b38-8517-4f92-9296-7537d3b6b7ae"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Left Mouse Button"",
                    ""type"": ""Button"",
                    ""id"": ""f615477e-0623-457a-873d-0998c1e57374"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Right Mouse Button"",
                    ""type"": ""Button"",
                    ""id"": ""a49ff686-2d6f-43e4-9cef-882066b32638"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""714abc44-6390-4a17-b4ba-1e6b5465bb94"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Axis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""b38d2efa-5e26-4726-ac77-dc798f0784f7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Horizontal Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""a73f9c2e-46df-4f78-a6b6-35224b72d80e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Horizontal Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""5b01bf27-e396-438c-a540-df2263cef2c5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical Axis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a99330de-f906-4d24-b100-a17aec71f917"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Vertical Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""dcc2f4df-5cdc-44d5-85b3-5f147ed6aefc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Vertical Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""40826c31-e90e-424e-ae8f-9a427f4bc455"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b5e7a9c2-52ac-4ce0-8134-4ca8ff9a8f99"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""393290ad-a578-43af-9749-d8384ac27f81"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""74da8f97-8e5c-413f-a636-a4bc7fa03783"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Left Mouse Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce5bb2df-c3c9-4557-a292-5be85c5ff09e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Right Mouse Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay_Battle
        m_Gameplay_Battle = asset.FindActionMap("Gameplay_Battle", throwIfNotFound: true);
        m_Gameplay_Battle_HorizontalAxis = m_Gameplay_Battle.FindAction("Horizontal Axis", throwIfNotFound: true);
        m_Gameplay_Battle_VerticalAxis = m_Gameplay_Battle.FindAction("Vertical Axis", throwIfNotFound: true);
        m_Gameplay_Battle_Scroll = m_Gameplay_Battle.FindAction("Scroll", throwIfNotFound: true);
        m_Gameplay_Battle_LeftMouseButton = m_Gameplay_Battle.FindAction("Left Mouse Button", throwIfNotFound: true);
        m_Gameplay_Battle_RightMouseButton = m_Gameplay_Battle.FindAction("Right Mouse Button", throwIfNotFound: true);
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

    // Gameplay_Battle
    private readonly InputActionMap m_Gameplay_Battle;
    private IGameplay_BattleActions m_Gameplay_BattleActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Battle_HorizontalAxis;
    private readonly InputAction m_Gameplay_Battle_VerticalAxis;
    private readonly InputAction m_Gameplay_Battle_Scroll;
    private readonly InputAction m_Gameplay_Battle_LeftMouseButton;
    private readonly InputAction m_Gameplay_Battle_RightMouseButton;
    public struct Gameplay_BattleActions
    {
        private @InputActions m_Wrapper;
        public Gameplay_BattleActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalAxis => m_Wrapper.m_Gameplay_Battle_HorizontalAxis;
        public InputAction @VerticalAxis => m_Wrapper.m_Gameplay_Battle_VerticalAxis;
        public InputAction @Scroll => m_Wrapper.m_Gameplay_Battle_Scroll;
        public InputAction @LeftMouseButton => m_Wrapper.m_Gameplay_Battle_LeftMouseButton;
        public InputAction @RightMouseButton => m_Wrapper.m_Gameplay_Battle_RightMouseButton;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay_Battle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Gameplay_BattleActions set) { return set.Get(); }
        public void SetCallbacks(IGameplay_BattleActions instance)
        {
            if (m_Wrapper.m_Gameplay_BattleActionsCallbackInterface != null)
            {
                @HorizontalAxis.started -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnHorizontalAxis;
                @HorizontalAxis.performed -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnHorizontalAxis;
                @HorizontalAxis.canceled -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnHorizontalAxis;
                @VerticalAxis.started -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnVerticalAxis;
                @VerticalAxis.performed -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnVerticalAxis;
                @VerticalAxis.canceled -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnVerticalAxis;
                @Scroll.started -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnScroll;
                @LeftMouseButton.started -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnLeftMouseButton;
                @LeftMouseButton.performed -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnLeftMouseButton;
                @LeftMouseButton.canceled -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnLeftMouseButton;
                @RightMouseButton.started -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnRightMouseButton;
                @RightMouseButton.performed -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnRightMouseButton;
                @RightMouseButton.canceled -= m_Wrapper.m_Gameplay_BattleActionsCallbackInterface.OnRightMouseButton;
            }
            m_Wrapper.m_Gameplay_BattleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalAxis.started += instance.OnHorizontalAxis;
                @HorizontalAxis.performed += instance.OnHorizontalAxis;
                @HorizontalAxis.canceled += instance.OnHorizontalAxis;
                @VerticalAxis.started += instance.OnVerticalAxis;
                @VerticalAxis.performed += instance.OnVerticalAxis;
                @VerticalAxis.canceled += instance.OnVerticalAxis;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
                @LeftMouseButton.started += instance.OnLeftMouseButton;
                @LeftMouseButton.performed += instance.OnLeftMouseButton;
                @LeftMouseButton.canceled += instance.OnLeftMouseButton;
                @RightMouseButton.started += instance.OnRightMouseButton;
                @RightMouseButton.performed += instance.OnRightMouseButton;
                @RightMouseButton.canceled += instance.OnRightMouseButton;
            }
        }
    }
    public Gameplay_BattleActions @Gameplay_Battle => new Gameplay_BattleActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    public interface IGameplay_BattleActions
    {
        void OnHorizontalAxis(InputAction.CallbackContext context);
        void OnVerticalAxis(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnLeftMouseButton(InputAction.CallbackContext context);
        void OnRightMouseButton(InputAction.CallbackContext context);
    }
}
