using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    public GameObject bridgeBlueprint;
    public GameObject bicyclePrefab;

    private GameObject currentBlueprint;
    public ConstructionBuilder builder;
    private ObjectData selectedObject;

    void Start()
    {
        selectedObject =
            ObjectDatabase.Instance.GetObject("bridge");
    }
    void Update()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            Instantiate(
                bicyclePrefab,
                new Vector3(2, 1, 2),
                Quaternion.identity);
        }
        if (currentBlueprint != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Destroy(currentBlueprint);
            currentBlueprint = null;
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            selectedObject =
                ObjectDatabase.Instance.GetObject("bridge");

            Debug.Log("Bridge Selected");
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            selectedObject =
                ObjectDatabase.Instance.GetObject("ladder");

            Debug.Log("Ladder Selected");
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Debug.Log("Spawn Ladder");
        }
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            SpawnBlueprint();
        }
        if (
    currentBlueprint != null &&
    Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlacementValidator validator =
    currentBlueprint.GetComponent<PlacementValidator>();

            if (validator == null ||
                validator.IsValidPlacement())
            {
                PlaceObject();
            }
        }
    }

    void PlaceObject()
    {
        if (selectedObject == null)
        {
            Debug.Log("No object selected!");
            return;
        }

        ObjectData buildData =
    selectedObject;

        if (!EnergyManager.Instance.SpendEnergy(
    buildData.energyCost))
        {
            Debug.Log("Not enough energy!");
            return;
        }

        GenericRecipe recipe =
    buildData.recipePrefab.GetComponent<GenericRecipe>();

        if (recipe == null)
        {
            Debug.LogError("Recipe missing GenericRecipe component!");
            return;
        }

        StartCoroutine(
            builder.BuildObject(
                currentBlueprint.transform.position,
                currentBlueprint.transform.rotation,
                recipe));

        Destroy(currentBlueprint);

        currentBlueprint = null;
    }
    void SpawnBlueprint()
    {
        if (currentBlueprint != null)
            Destroy(currentBlueprint);

        if (selectedObject == null)
        {
            Debug.LogError("No object selected!");
            return;
        }

        currentBlueprint =
            Instantiate(selectedObject.blueprintPrefab);
    }
}