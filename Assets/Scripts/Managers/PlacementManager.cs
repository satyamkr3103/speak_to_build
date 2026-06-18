using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float gridSize = 1f;
    [SerializeField]
    private LayerMask placementBlockMask;
    private bool lastCanPlace = true;
    private bool canPlace = true;
    private GameObject currentObject;

    private bool isPlacing;

    private float currentRotation;
    private float heightOffset = 0f;
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
        SetLayerRecursively(
    currentObject,
    LayerMask.NameToLayer(
        "PreviewObject"));

        isPlacing = true;

        currentRotation = 0f;

        heightOffset = 0f;
        canPlace = true;
        lastCanPlace = true;

        // UpdatePreviewColor();

        ObjectLifetime runtime =
    obj.GetComponent<ObjectLifetime>();

        if (runtime != null)
        {
            runtime.isBeingPlaced = true;
        }
    }

    private void Update()
    {
        if (!isPlacing ||
            currentObject == null)
            return;

        HandleMovement();

        CheckPlacement();

        if (lastCanPlace != canPlace)
        {
            // UpdatePreviewColor();

            lastCanPlace = canPlace;
        }

        HandleRotation();

        HandleHeight();

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

            Bounds bounds =
    CalculateBounds(
        currentObject);

            float lowestPoint =
    GetLowestPoint(
        currentObject);

            pos.y +=
                -lowestPoint +
                heightOffset;

            currentObject.transform.position =
                pos;
        }
    }

    float GetLowestPoint(
    GameObject obj)
    {
        Renderer[] renderers =
            obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return 0f;

        float lowest =
            float.MaxValue;

        foreach (Renderer r in renderers)
        {
            if (r.bounds.min.y <
                lowest)
            {
                lowest =
                    r.bounds.min.y;
            }
        }

        return lowest -
            obj.transform.position.y;
    }

    Bounds CalculateBounds(
    GameObject obj)
    {
        Renderer[] renderers =
            obj.GetComponentsInChildren
            <Renderer>();

        if (renderers.Length == 0)
        {
            return new Bounds(
                obj.transform.position,
                Vector3.one);
        }

        Bounds bounds =
            renderers[0].bounds;

        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(
                r.bounds);
        }

        return bounds;
    }
    void HandleHeight()
    {
        if (keyboard == null)
            return;

        if (keyboard.qKey.wasPressedThisFrame)
        {
            heightOffset += 0.5f;
        }

        if (keyboard.eKey.wasPressedThisFrame)
        {
            heightOffset -= 0.5f;
        }
    }

    void HandleRotation()
    {
        if (keyboard == null)
            return;

        if (keyboard.zKey.wasPressedThisFrame)
        {
            currentRotation =
    (currentRotation + 90f) % 360f;

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

        if (mouse.leftButton.wasPressedThisFrame
    && canPlace)
        {
            isPlacing = false;
            ObjectLifetime lifetime =
    currentObject.GetComponent<ObjectLifetime>();

            if (lifetime != null)
            {
                lifetime.isBeingPlaced = false;
            }
            SetLayerRecursively(currentObject, LayerMask.NameToLayer("Placeable"));

            // ResetPreviewColor(currentObject);
            RuntimeObject runtime =
    currentObject.GetComponent
    <RuntimeObject>();

            if (runtime != null)
            {
                WorldObjectManager
                    .Instance
                    .Register(runtime);
            }
            currentObject = null;

            Debug.Log(
                "Placement Confirmed");
        }
    }
    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child
            in obj.transform)
        {
            SetLayerRecursively(
                child.gameObject,
                layer);
        }
    }
    void HandleCancel()
    {
        if (mouse == null)
            return;

        if (mouse.rightButton.wasPressedThisFrame)
        {
            // ResetPreviewColor(currentObject);
            Destroy(
                currentObject);

            currentObject = null;

            isPlacing = false;

            Debug.Log(
                "Placement Cancelled");
        }
    }
    void CheckPlacement()
    {
        Bounds bounds =
            CalculateBounds(
                currentObject);

        Collider[] hits =
            Physics.OverlapBox(
                bounds.center,
                bounds.extents * 0.95f,
                currentObject.transform.rotation,
                placementBlockMask);

        canPlace = true;

        foreach (var hit in hits)
        {
            if (hit.transform.IsChildOf(
                currentObject.transform))
            {
                continue;
            }

            canPlace = false;
            break;
        }
    }
    //     void UpdatePreviewColor()
    //     {
    //         Renderer[] renderers =
    //             currentObject
    //             .GetComponentsInChildren<Renderer>();
    //             Debug.Log(
    //     "Preview Color Update: " +
    //     canPlace);

    //         foreach (Renderer r in renderers)
    //         {
    //             Debug.Log(
    //     r.material.shader.name);
    //             Debug.Log(
    //     r.material.HasProperty("_Color"));
    //             r.material = new Material(
    //     Shader.Find("Universal Render Pipeline/Lit"));

    // r.material.color =
    //     canPlace
    //     ? Color.green
    //     : Color.red;

    //         }
    //     }
    // void ResetPreviewColor(GameObject obj)
    // {
    //     Renderer[] renderers =
    //         obj.GetComponentsInChildren<Renderer>();

    //     foreach (Renderer r in renderers)
    //     {
    //         if (!r.material.HasProperty("_Color"))
    //             continue;

    //         Color c =
    //             r.material.color;

    //         c.r = 1;
    //         c.g = 1;
    //         c.b = 1;
    //         c.a = 1;

    //         r.material.color = c;
    //     }
    // }
    private void OnDrawGizmos()
    {
        if (currentObject == null)
            return;

        Bounds bounds =
            CalculateBounds(
                currentObject);

        Gizmos.color =
            canPlace
            ? Color.green
            : Color.red;

        Gizmos.matrix =
            Matrix4x4.TRS(
                bounds.center,
                currentObject.transform.rotation,
                Vector3.one);

        Gizmos.DrawWireCube(
            Vector3.zero,
            bounds.size);
    }
}