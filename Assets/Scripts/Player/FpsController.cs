using UnityEngine;

namespace DeltaFire.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FpsController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float moveSpeed = 5.5f;
        [SerializeField] private float lookSensitivity = 2.2f;
        [SerializeField] private float gravity = -18f;
        [SerializeField] private float jumpHeight = 1.1f;

        private CharacterController controller;
        private float pitch;
        private float verticalVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (!playerCamera) playerCamera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            Look();
            Move();
        }

        private void Look()
        {
            if (Input.touchCount > 0) return;
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;
            transform.Rotate(Vector3.up * mouseX);
            pitch = Mathf.Clamp(pitch - mouseY, -88f, 88f);
            if (playerCamera) playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void Move()
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            input = Vector3.ClampMagnitude(input, 1f);
            Vector3 motion = transform.TransformDirection(input) * moveSpeed;

            if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
            if (controller.isGrounded && Input.GetButtonDown("Jump")) verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            verticalVelocity += gravity * Time.deltaTime;
            motion.y = verticalVelocity;
            controller.Move(motion * Time.deltaTime);
        }
    }
}
