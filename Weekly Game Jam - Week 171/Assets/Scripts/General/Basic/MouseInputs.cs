// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/General/Basic/MouseInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MouseInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MouseInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MouseInputs"",
    ""maps"": [
        {
            ""name"": ""ActionObjects"",
            ""id"": ""6715a309-4f9b-4c60-8f1a-3582295337bd"",
            ""actions"": [
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a3012f66-7a97-4325-a3d3-d59607245c2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d0dbd4f0-9529-41f3-9f07-c04a19edcabc"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ActionObjects
        m_ActionObjects = asset.FindActionMap("ActionObjects", throwIfNotFound: true);
        m_ActionObjects_LeftClick = m_ActionObjects.FindAction("LeftClick", throwIfNotFound: true);
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

    // ActionObjects
    private readonly InputActionMap m_ActionObjects;
    private IActionObjectsActions m_ActionObjectsActionsCallbackInterface;
    private readonly InputAction m_ActionObjects_LeftClick;
    public struct ActionObjectsActions
    {
        private @MouseInputs m_Wrapper;
        public ActionObjectsActions(@MouseInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftClick => m_Wrapper.m_ActionObjects_LeftClick;
        public InputActionMap Get() { return m_Wrapper.m_ActionObjects; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionObjectsActions set) { return set.Get(); }
        public void SetCallbacks(IActionObjectsActions instance)
        {
            if (m_Wrapper.m_ActionObjectsActionsCallbackInterface != null)
            {
                @LeftClick.started -= m_Wrapper.m_ActionObjectsActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_ActionObjectsActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_ActionObjectsActionsCallbackInterface.OnLeftClick;
            }
            m_Wrapper.m_ActionObjectsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
            }
        }
    }
    public ActionObjectsActions @ActionObjects => new ActionObjectsActions(this);
    public interface IActionObjectsActions
    {
        void OnLeftClick(InputAction.CallbackContext context);
    }
}
