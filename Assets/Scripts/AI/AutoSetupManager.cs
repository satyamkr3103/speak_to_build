using UnityEngine;

public class AutoSetupManager : MonoBehaviour
{
    public static AutoSetupManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetupObject(
        GameObject obj,
        string objectName)
    {
        NormalizeScale(obj, objectName);

        AddCollider(obj);

        AddLifetime(obj);

        DetectCategory(
            obj,
            objectName);
    }

    void NormalizeScale(
        GameObject obj,
        string objectName)
    {
        Bounds bounds =
            CalculateBounds(obj);

        float currentHeight =
            bounds.size.y;

        if (currentHeight <= 0.01f)
            return;

        float targetHeight =
            GetTargetHeight(objectName);

        float scaleFactor =
            targetHeight / currentHeight;

        obj.transform.localScale *=
            scaleFactor;
    }

    float GetTargetHeight(
        string objectName)
    {
        string lower =
            objectName.ToLower();

        if (lower.Contains("dragon"))
            return 5f;

        if (lower.Contains("lion"))
            return 2f;

        if (lower.Contains("tiger"))
            return 2f;

        if (lower.Contains("car"))
            return 2f;

        if (lower.Contains("truck"))
            return 3f;

        if (lower.Contains("bicycle"))
            return 1.5f;

        if (lower.Contains("bike"))
            return 1.5f;

        if (lower.Contains("boat"))
            return 3f;

        if (lower.Contains("tree"))
            return 6f;

        if (lower.Contains("castle"))
            return 10f;

        return 3f;
    }

    void AddCollider(GameObject obj)
    {
        if (obj.GetComponentInChildren<Collider>() != null)
            return;

        AddBestCollider(obj);
    }

    void AddBestCollider(GameObject obj)
    {
        Bounds bounds =
            CalculateBounds(obj);

        float x = bounds.size.x;
        float y = bounds.size.y;
        float z = bounds.size.z;

        // Tall objects
        if (y > x * 2f && y > z * 2f)
        {
            obj.AddComponent<CapsuleCollider>();
            return;
        }

        // Roughly spherical objects
        if (
            Mathf.Abs(x - z) < 0.5f &&
            Mathf.Abs(y - z) < 0.5f)
        {
            obj.AddComponent<SphereCollider>();
            return;
        }

        // Complex models
        MeshFilter meshFilter =
            obj.GetComponentInChildren<MeshFilter>();

        if (meshFilter != null)
        {
            MeshCollider mesh =
                obj.AddComponent<MeshCollider>();

            mesh.sharedMesh =
                meshFilter.sharedMesh;

            mesh.convex = true;

            return;
        }

        // Fallback
        obj.AddComponent<BoxCollider>();
    }

    void DetectCategory(
        GameObject obj,
        string objectName)
    {
        RuntimeObject runtime =
            obj.GetComponent<RuntimeObject>();

        if (runtime == null)
        {
            runtime =
                obj.AddComponent<RuntimeObject>();
        }

        runtime.objectName =
            objectName;

        string lower =
            objectName.ToLower();

        // Vehicles
        if (
            lower.Contains("car") ||
            lower.Contains("truck") ||
            lower.Contains("bike") ||
            lower.Contains("bicycle") ||
            lower.Contains("boat") ||
            lower.Contains("motorcycle") ||
            lower.Contains("tank") ||
            lower.Contains("helicopter") ||
            lower.Contains("plane") ||
            lower.Contains("airplane") ||
            lower.Contains("jet") ||
            lower.Contains("ship")
        )
        {
            runtime.category =
                ObjectCategory.Vehicle;

            SetupVehicle(obj);

            return;
        }

        // Creatures
        if (
            lower.Contains("dragon") ||
            lower.Contains("lion") ||
            lower.Contains("tiger") ||
            lower.Contains("wolf") ||
            lower.Contains("bear") ||
            lower.Contains("horse")
        )
        {
            runtime.category =
                ObjectCategory.Creature;

            return;
        }

        runtime.category =
            ObjectCategory.Generic;
    }

    void SetupVehicle(GameObject obj)
    {
        Rigidbody rb =
            obj.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb =
                obj.AddComponent<Rigidbody>();
        }

        rb.mass = 200f;

        Debug.Log(
            "Vehicle Setup Complete");
    }

    Bounds CalculateBounds(GameObject obj)
    {
        Renderer[] renderers =
            obj.GetComponentsInChildren<Renderer>();

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
            bounds.Encapsulate(r.bounds);
        }

        return bounds;
    }

    void AddLifetime(GameObject obj)
    {
        if (obj.GetComponent<ObjectLifetime>())
            return;

        ObjectLifetime life =
            obj.AddComponent<ObjectLifetime>();

        life.lifeTime = 30f;
    }
}