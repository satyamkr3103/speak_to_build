using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera :
    MonoBehaviour
{
    public Transform target;

    public float distance = 5f;

    public float height = 2f;

    public float sensitivity = 2f;

    float yaw;
    float pitch = 15f;

    private Mouse mouse;

    private void Start()
    {
        mouse =
            Mouse.current;

        Cursor.lockState =
            CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector2 delta =
            mouse.delta.ReadValue();

        yaw +=
            delta.x *
            sensitivity;

        pitch -=
            delta.y *
            sensitivity;

        pitch =
            Mathf.Clamp(
                pitch,
                -20,
                80);

        Quaternion rotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0);

        Vector3 offset =
            rotation *
            new Vector3(
                0,
                0,
                -distance);

        transform.position =
            target.position +
            Vector3.up * height +
            offset;

        transform.LookAt(
            target.position +
            Vector3.up * height);

        target.rotation =
            Quaternion.Euler(
                0,
                yaw,
                0);
    }
}