using HorrorGame.Manager;
using UnityEngine;

namespace HorrorGame.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float animBlendSpeed = 8.9f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundedForce = -2f;

        private float yVelocity;

        [Header("Camera")]
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Transform fpCamera;
        [SerializeField] private float upperLimit = -40f;
        [SerializeField] private float bottomLimit = 70f;
        [SerializeField] private float mouseSensitivity = 21.9f;


        private InputManager inputManager;
        private Animator animator;
        private CharacterController characterController;

        private bool hasAnimator;
        private int xVelHash;
        private int yVelHash;

        private float xRotation = 0f;
        private Vector2 currentVelocity;   // smoothed input for animator

        private Vector3 moveDirection;     // for smooth movement

        void Start()
        {
            // Get components
            hasAnimator = TryGetComponent(out animator) ||
                         (animator = GetComponentInChildren<Animator>()) != null;

            characterController = GetComponent<CharacterController>();
            inputManager = GetComponent<InputManager>();

            if (characterController == null)
            {
                Debug.LogError("CharacterController is missing on the Player!");
            }

            // Animator hashes
            xVelHash = Animator.StringToHash("X_Velocity");
            yVelHash = Animator.StringToHash("Y_Velocity");

            // Cursor setup
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Initial camera setup
            if (fpCamera != null && cameraRoot != null)
                fpCamera.position = cameraRoot.position;
        }

        void Update()
        {
            HandleMovement();
            HandleCamera();
        }

        private void HandleMovement()
        {
            if (!hasAnimator || characterController == null) return;

            Vector2 input = inputManager.Move;

            // Smooth input for animation
            currentVelocity = Vector2.Lerp(currentVelocity, input * walkSpeed,
                                           animBlendSpeed * Time.deltaTime);

            // Ground check
            if (characterController.isGrounded)
            {
                if (yVelocity < 0)
                {
                    // Small downward force keeps player stuck to ground
                    yVelocity = groundedForce;
                }
            }
            else
            {
                // Apply gravity over time
                yVelocity += gravity * Time.deltaTime;
            }

            // Combine horizontal + vertical movement
            Vector3 horizontalMove = transform.TransformDirection(
                new Vector3(currentVelocity.x, 0f, currentVelocity.y));

            Vector3 finalMove = horizontalMove + Vector3.up * yVelocity;

            characterController.Move(finalMove * Time.deltaTime);

            // Update Animator
            animator.SetFloat(xVelHash, currentVelocity.x);
            animator.SetFloat(yVelHash, currentVelocity.y);
        }

        private void HandleCamera()
        {
            if (!hasAnimator) return;

            float mouseX = inputManager.Look.x * mouseSensitivity * Time.deltaTime;
            float mouseY = inputManager.Look.y * mouseSensitivity * Time.deltaTime;

            // Vertical camera rotation (head/camera only)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

            fpCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Horizontal rotation (whole player body)
            transform.Rotate(Vector3.up * mouseX);
        }

        void OnApplicationFocus(bool hasFocus)
        {
            Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !hasFocus;
        }
    }
}