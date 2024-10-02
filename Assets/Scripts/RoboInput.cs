//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/RoboInput.inputactions
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

public partial class @RoboInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @RoboInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""RoboInput"",
    ""maps"": [
        {
            ""name"": ""RoboActions"",
            ""id"": ""54c2ab05-0c8a-4266-bf83-dc18806dad39"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""52b0023f-1730-49cb-9027-4bd060b7f991"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""9cc7c804-e512-4585-85fe-d43a0a8e472e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""931db487-a183-46d8-8ff5-ffffe8167241"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ready"",
                    ""type"": ""Button"",
                    ""id"": ""6baeaf93-ae38-48f2-bb2b-c62b7f660bd1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""cdc062d4-0461-422e-b0d1-ec844956cba0"",
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
                    ""id"": ""a59861af-10d6-46a6-b76f-0eb0a0bb8809"",
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
                    ""id"": ""d8a561c4-40d1-433f-9007-af88c917ee4f"",
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
                    ""id"": ""248531c7-0bd1-4f93-b79a-449e45ee15c5"",
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
                    ""id"": ""b4f5a678-7e0b-4e50-b839-52868905daf0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""abcc2ef3-59f3-45c7-a835-67b1a99c72e2"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a3b85c9-3629-464e-b296-62bb3d02d2f1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02285e40-7f90-4e45-88ac-b20d81e4e9dd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""046cd548-6440-4603-9dff-2d46e7e755e7"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ready"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // RoboActions
        m_RoboActions = asset.FindActionMap("RoboActions", throwIfNotFound: true);
        m_RoboActions_Move = m_RoboActions.FindAction("Move", throwIfNotFound: true);
        m_RoboActions_MousePosition = m_RoboActions.FindAction("MousePosition", throwIfNotFound: true);
        m_RoboActions_Fire = m_RoboActions.FindAction("Fire", throwIfNotFound: true);
        m_RoboActions_Ready = m_RoboActions.FindAction("Ready", throwIfNotFound: true);
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

    // RoboActions
    private readonly InputActionMap m_RoboActions;
    private List<IRoboActionsActions> m_RoboActionsActionsCallbackInterfaces = new List<IRoboActionsActions>();
    private readonly InputAction m_RoboActions_Move;
    private readonly InputAction m_RoboActions_MousePosition;
    private readonly InputAction m_RoboActions_Fire;
    private readonly InputAction m_RoboActions_Ready;
    public struct RoboActionsActions
    {
        private @RoboInput m_Wrapper;
        public RoboActionsActions(@RoboInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_RoboActions_Move;
        public InputAction @MousePosition => m_Wrapper.m_RoboActions_MousePosition;
        public InputAction @Fire => m_Wrapper.m_RoboActions_Fire;
        public InputAction @Ready => m_Wrapper.m_RoboActions_Ready;
        public InputActionMap Get() { return m_Wrapper.m_RoboActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RoboActionsActions set) { return set.Get(); }
        public void AddCallbacks(IRoboActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_RoboActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_RoboActionsActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @MousePosition.started += instance.OnMousePosition;
            @MousePosition.performed += instance.OnMousePosition;
            @MousePosition.canceled += instance.OnMousePosition;
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @Ready.started += instance.OnReady;
            @Ready.performed += instance.OnReady;
            @Ready.canceled += instance.OnReady;
        }

        private void UnregisterCallbacks(IRoboActionsActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @MousePosition.started -= instance.OnMousePosition;
            @MousePosition.performed -= instance.OnMousePosition;
            @MousePosition.canceled -= instance.OnMousePosition;
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @Ready.started -= instance.OnReady;
            @Ready.performed -= instance.OnReady;
            @Ready.canceled -= instance.OnReady;
        }

        public void RemoveCallbacks(IRoboActionsActions instance)
        {
            if (m_Wrapper.m_RoboActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IRoboActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_RoboActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_RoboActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public RoboActionsActions @RoboActions => new RoboActionsActions(this);
    public interface IRoboActionsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnReady(InputAction.CallbackContext context);
    }
}
