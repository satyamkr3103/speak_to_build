using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float gridSize = 1f;

    private GameObject currentObject;

    private bool isPlacing;

    private float currentRotation;

    private Mouse mouse;
    private Keyboard keyboard;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mouse = Mouse.current;
        keyboard = Keyboard.current;
    }

    public void BeginPlacement(
        GameObject obj)
    {
        currentObject = obj;

        isPlacing = true;

        currentRotation = 0f;
    }

    private void Update()
    {
        if (!isPlacing ||
            currentObject == null)
            return;

        HandleMovement();

        HandleRotation();

        HandlePlacement();

        HandleCancel();
    }

    void HandleMovement()
    {
        if (mouse == null)
            return;

        Ray ray =
            Camera.main.ScreenPointToRay(
                mouse.position.ReadValue());

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            500f,
            groundMask))
        {
            Vector3 pos =
                hit.point;

            pos.x =
                Mathf.Round(
                    pos.x / gridSize)
                * gridSize;

            pos.z =
                Mathf.Round(
                    pos.z / gridSize)
                * gridSize;

            currentObject.transform.position =
                pos;
        }
    }

    void HandleRotation()
    {
        if (keyboard == null)
            return;

        if (keyboard.zKey.wasPressedThisFrame)
        {
            currentRotation += 90f;

            currentObject.transform.rotation =
                Quaternion.Euler(
                    0,
                    currentRotation,
                    0);
        }
    }

    void HandlePlacement()
    {
        if (mouse == null)
            return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            isPlacing = false;

            currentObject = null;

            Debug.Log(
                "Placement Confirmed");
        }
    }

    void HandleCancel()
    {
        if (mouse == null)
            return;

        if (mouse.rightButton.wasPressedThisFrame)
        {
            Destroy(
                currentObject);

            currentObject = null;

            isPlacing = false;

            Debug.Log(
                "Placement Cancelled");
        }
    }
}