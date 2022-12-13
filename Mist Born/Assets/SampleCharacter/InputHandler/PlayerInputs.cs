// GENERATED AUTOMATICALLY FROM 'Assets/SampleCharacter/InputHandler/PlayerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""c4728b38-6645-466f-aa50-b2bfce785c6e"",
            ""actions"": [
                {
                    ""name"": ""jump"",
                    ""type"": ""Button"",
                    ""id"": ""025497d1-fe37-4929-8a58-d40262b89f2c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""fcd57bb5-3c94-42fc-8eb7-4c4d77542d8d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""roll"",
                    ""type"": ""Button"",
                    ""id"": ""2ac2d882-120e-44fa-bc2e-7156f09e9b1a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""dash"",
                    ""type"": ""Button"",
                    ""id"": ""e7f2f3ce-1fb4-43be-b16a-eac6ed40dcfd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""light_attack"",
                    ""type"": ""Button"",
                    ""id"": ""1e52b2cf-b779-4110-9b02-1e0ba5e6d43a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""heavy_attack"",
                    ""type"": ""Button"",
                    ""id"": ""39b23e22-6501-4a39-9233-a6cc39bc4d80"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f94c66b6-fe50-499f-b50d-355b9435bcef"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0670fb61-1fcb-408c-b76d-4a31c3c1c1cd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""52430f6f-c67d-4d1a-af28-4a4625af0e6b"",
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
                    ""id"": ""62a3068b-0888-4117-8f67-72e51d469444"",
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
                    ""id"": ""848a421d-8e93-4c89-b39b-d44cf3bccab7"",
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
                    ""id"": ""de16a17b-a298-48a0-960d-c3b97b00993f"",
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
                    ""id"": ""96e4b551-a11d-46d3-b9cd-65a0a489f973"",
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
                    ""id"": ""8b2a3cca-b05e-4664-8852-f634d4887ef4"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb1c2102-c122-4a3d-8ce8-a4bb5aa99240"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7ebb3cf-119c-4765-bba3-ada27fd7f629"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""534f72af-8eff-446d-8971-9dd5e8ed00df"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f05bfe8-843e-4a81-a1b1-cb70942c00c8"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9b471f53-6698-4396-bdb1-4fa55535e453"",
                    ""path"": ""<XInputController>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""light_attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4e24e1a-9f56-411d-99d5-695dea07a75e"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""heavy_attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_jump = m_Player.FindAction("jump", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_roll = m_Player.FindAction("roll", throwIfNotFound: true);
        m_Player_dash = m_Player.FindAction("dash", throwIfNotFound: true);
        m_Player_light_attack = m_Player.FindAction("light_attack", throwIfNotFound: true);
        m_Player_heavy_attack = m_Player.FindAction("heavy_attack", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_jump;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_roll;
    private readonly InputAction m_Player_dash;
    private readonly InputAction m_Player_light_attack;
    private readonly InputAction m_Player_heavy_attack;
    public struct PlayerActions
    {
        private @PlayerInputs m_Wrapper;
        public PlayerActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @jump => m_Wrapper.m_Player_jump;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @roll => m_Wrapper.m_Player_roll;
        public InputAction @dash => m_Wrapper.m_Player_dash;
        public InputAction @light_attack => m_Wrapper.m_Player_light_attack;
        public InputAction @heavy_attack => m_Wrapper.m_Player_heavy_attack;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @roll.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @roll.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @roll.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @light_attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLight_attack;
                @light_attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLight_attack;
                @light_attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLight_attack;
                @heavy_attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeavy_attack;
                @heavy_attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeavy_attack;
                @heavy_attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeavy_attack;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @jump.started += instance.OnJump;
                @jump.performed += instance.OnJump;
                @jump.canceled += instance.OnJump;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @roll.started += instance.OnRoll;
                @roll.performed += instance.OnRoll;
                @roll.canceled += instance.OnRoll;
                @dash.started += instance.OnDash;
                @dash.performed += instance.OnDash;
                @dash.canceled += instance.OnDash;
                @light_attack.started += instance.OnLight_attack;
                @light_attack.performed += instance.OnLight_attack;
                @light_attack.canceled += instance.OnLight_attack;
                @heavy_attack.started += instance.OnHeavy_attack;
                @heavy_attack.performed += instance.OnHeavy_attack;
                @heavy_attack.canceled += instance.OnHeavy_attack;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnLight_attack(InputAction.CallbackContext context);
        void OnHeavy_attack(InputAction.CallbackContext context);
    }
}
