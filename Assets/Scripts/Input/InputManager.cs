using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace HorrorGame.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public Vector2 Run { get; private set; }

        private InputActionMap currentMap;
        private InputAction moveAction;

        private InputAction lookAction;

        private void Awake()
        {
            currentMap = playerInput.currentActionMap;
            moveAction = currentMap.FindAction("Move");
            lookAction = currentMap.FindAction("Look");

            moveAction.performed += onMove;
            lookAction.performed += onLook;

            moveAction.canceled += onMove;
            lookAction.canceled += onLook;
        }
        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void onLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        void OnEnable()
        {
            currentMap.Enable();
        }

        void OnDisable()
        {
            currentMap.Disable();
        }


    }
}

