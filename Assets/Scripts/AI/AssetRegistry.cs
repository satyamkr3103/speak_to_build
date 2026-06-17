using System.Collections.Generic;
using UnityEngine;

public class AssetRegistry : MonoBehaviour
{
    public static AssetRegistry Instance;
    [System.Serializable]
    public class RegisteredAsset
    {
        public string objectName;
        public GameObject prefab;
    }
    public RegisteredAsset[] assets;
    private Dictionary<string, GameObject>
        localPrefabs =
        new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;

        foreach (var asset in assets)
        {
            RegisterPrefab(
                asset.objectName.ToLower(),
                asset.prefab);
        }
    }

    public void RegisterPrefab(
        string objectName,
        GameObject prefab)
    {
        localPrefabs[objectName] = prefab;
    }

    public bool HasPrefab(string objectName)
    {
        return localPrefabs.ContainsKey(objectName);
    }

    public GameObject GetPrefab(
        string objectName)
    {
        return localPrefabs[objectName];
    }
}
