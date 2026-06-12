using UnityEngine;
using UnityEngine.InputSystem;

public class BlueprintPreview : MonoBehaviour
{
    private PlacementValidator validator;
    private Renderer rend;
    private Camera cam;
    private float rotationY = 0f;
    private void Start()
    {
        cam = Camera.main;
        validator = GetComponent<PlacementValidator>();
        rend = GetComponentInChildren<Renderer>();
        Debug.Log(cam);
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            rotationY -= 90f;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            rotationY += 90f;
        }

        if (validator != null)
        {
            bool valid = validator.IsValidPlacement();

            if (valid)
            {
                rend.material.color = new Color(0f, 1f, 0f, 0.5f);
            }
            else
            {
                rend.material.color = new Color(1f, 0f, 0f, 0.5f);
            }
        }
        if (cam == null)
        {
            Debug.LogError("Camera is NULL");
            return;
        }

        if (GridManager.Instance == null)
        {
            Debug.LogError("GridManager Instance is NULL");
            return;
        }

        if (Mouse.current == null)
        {
            Debug.LogError("Mouse.current is NULL");
            return;
        }

        Ray ray =
            cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 snapped =
                GridManager.Instance.SnapToGrid(hit.point);

            transform.position = snapped;
            transform.rotation =
    Quaternion.Euler(
        0,
        rotationY,
        0);
        }
    }
}