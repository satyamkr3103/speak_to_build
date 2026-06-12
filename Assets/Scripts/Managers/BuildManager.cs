using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    public GameObject bridgeBlueprint;
    public GameObject bicyclePrefab;

    private GameObject currentBlueprint;
    public ConstructionBuilder builder;

    public BridgeRecipe bridgeRecipe;

    void Update()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            Instantiate(
                bicyclePrefab,
                new Vector3(2, 1, 2),
                Quaternion.identity);
        }
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            SpawnBlueprint();
        }
        if (
    currentBlueprint != null &&
    Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceObject();
        }
    }

    void PlaceObject()
    {
        ObjectData bridgeData =
            ObjectDatabase.Instance.GetObject("bridge");

        if (!EnergyManager.Instance.SpendEnergy(
    bridgeData.energyCost))
        {
            Debug.Log("Not enough energy!");
            return;
        }

        StartCoroutine(
    builder.BuildBridge(
        currentBlueprint.transform.position,
        currentBlueprint.transform.rotation,
        bridgeRecipe));

        Destroy(currentBlueprint);

        currentBlueprint = null;
    }
    void SpawnBlueprint()
    {
        if (currentBlueprint != null)
            Destroy(currentBlueprint);

        currentBlueprint =
            Instantiate(bridgeBlueprint);
    }
}