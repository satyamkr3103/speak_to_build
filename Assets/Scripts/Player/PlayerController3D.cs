using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController3D : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float sprintSpeed = 9f;

    [SerializeField]
    float gravity = -20f;

    [SerializeField]
    float jumpHeight = 1.5f;

    private CharacterController controller;

    private Vector3 velocity;

    private bool grounded;

    private Keyboard keyboard;

    private void Start()
    {
        controller =
            GetComponent<CharacterController>();

        keyboard =
            Keyboard.current;
    }

    private void Update()
    {
        grounded =
            controller.isGrounded;

        if (grounded &&
            velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 move =
            Vector2.zero;

        if (keyboard.wKey.isPressed)
            move.y += 1;

        if (keyboard.sKey.isPressed)
            move.y -= 1;

        if (keyboard.aKey.isPressed)
            move.x -= 1;

        if (keyboard.dKey.isPressed)
            move.x += 1;

        Vector3 direction =
            transform.right * move.x +
            transform.forward * move.y;

        float speed =
            keyboard.leftShiftKey.isPressed
            ? sprintSpeed
            : moveSpeed;

        controller.Move(
            direction.normalized *
            speed *
            Time.deltaTime);

        if (
            keyboard.spaceKey.wasPressedThisFrame &&
            grounded)
        {
            velocity.y =
                Mathf.Sqrt(
                    jumpHeight *
                    -2f *
                    gravity);
        }

        velocity.y +=
            gravity *
            Time.deltaTime;

        controller.Move(
            velocity *
            Time.deltaTime);
    }
}