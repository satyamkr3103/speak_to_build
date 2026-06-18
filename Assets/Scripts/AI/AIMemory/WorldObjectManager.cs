using System.Collections.Generic;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private Dictionary<string,
        RuntimeObject> worldObjects =
            new Dictionary<string,
                RuntimeObject>();

    public void Register(
        RuntimeObject runtime)
    {
        string key =
            runtime.objectName
            .ToLower()
            .Trim();

        worldObjects[key] =
            runtime;

        Debug.Log(
            "Registered: " +
            key);
    }

    public RuntimeObject Find(
        string objectName)
    {
        string key =
            objectName
            .ToLower()
            .Trim();

        if (worldObjects.TryGetValue(
            key,
            out RuntimeObject obj))
        {
            return obj;
        }

        return null;
    }

    public void Remove(
        string objectName)
    {
        string key =
            objectName
            .ToLower()
            .Trim();

        if (worldObjects.ContainsKey(
            key))
        {
            worldObjects.Remove(
                key);

            Debug.Log(
                "Removed: " +
                key);
        }
    }
    public void PrintAllObjects()
    {
        Debug.Log(
            "===== WORLD OBJECTS =====");

        foreach (var pair
            in worldObjects)
        {
            Debug.Log(pair.Key);
        }
    }
    public List<RuntimeObject>
        GetAllObjects()
    {
        return new List<RuntimeObject>(
            worldObjects.Values);
    }
}
