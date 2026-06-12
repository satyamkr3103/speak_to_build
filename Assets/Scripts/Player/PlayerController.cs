using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -20f;

    private CharacterController controller;
    private PlayerInputActions input;

    private Vector2 moveInput;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        input = new PlayerInputActions();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        Move();
        JumpAndGravity();
    }

    void Move()
    {
        Vector3 moveDirection =
            new Vector3(moveInput.x, 0, moveInput.y);

        if (moveDirection.magnitude > 0.1f)
        {
            float speed = input.Player.Sprint.IsPressed()
                ? sprintSpeed
                : moveSpeed;

            controller.Move(moveDirection.normalized * speed * Time.deltaTime);

            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    void JumpAndGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        if (input.Player.Jump.triggered && controller.isGrounded)
        {
            verticalVelocity =
                Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        controller.Move(
            Vector3.up * verticalVelocity * Time.deltaTime);
    }
}