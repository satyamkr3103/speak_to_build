using UnityEngine;
using UnityEngine.InputSystem;

public class BuildCameraController :
    MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 20f;

    [SerializeField]
    float zoomSpeed = 10f;

    [SerializeField]
    float minZoom = 5f;

    [SerializeField]
    float maxZoom = 50f;

    private Camera cam;

    private Mouse mouse;

    private Keyboard keyboard;

    private void Start()
    {
        cam =
            GetComponent<Camera>();

        mouse =
            Mouse.current;

        keyboard =
            Keyboard.current;
    }

    private void Update()
    {

        if (!CameraModeManager
            .Instance
            .IsBuildMode())
            return;

        HandleZoom();
        HandlePan();
        HandleRotation();
    }

    void HandleZoom()
    {
        float scroll =
            mouse.scroll.ReadValue().y;

        if (scroll == 0)
            return;

        cam.orthographicSize -=
            scroll *
            0.01f *
            zoomSpeed;

        cam.orthographicSize =
            Mathf.Clamp(
                cam.orthographicSize,
                minZoom,
                maxZoom);
    }

    void HandlePan()
    {
        if (!mouse.middleButton
            .isPressed)
            return;

        Vector2 delta =
            mouse.delta.ReadValue();

        Vector3 move =
            new Vector3(
                -delta.x,
                0,
                -delta.y);

        transform.position +=
            move *
            Time.deltaTime *
            moveSpeed;
    }

    void HandleRotation()
    {
        if (keyboard.xKey.wasPressedThisFrame)
        {
            Debug.Log("Rotate Left");

            transform.Rotate(
                Vector3.up,
                -45f,
                Space.World);
        }

        if (keyboard.cKey.wasPressedThisFrame)
        {
            Debug.Log("Rotate Right");

            transform.Rotate(
                Vector3.up,
                45f,
                Space.World);
        }
    }
}