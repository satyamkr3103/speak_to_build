using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class AISpawnManager : MonoBehaviour
{
    public static AISpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<GameObject> SpawnObject(
        string objectName,
        Vector3 position)
    {
        objectName =
            objectName.ToLower().Trim();
        Debug.Log("Checking Registry...");
        // Local prefab
        if (AssetRegistry.Instance.HasPrefab(objectName))
        {
            GameObject prefab =
                AssetRegistry.Instance.GetPrefab(
                    objectName);

            GameObject obj =
                Instantiate(
                    prefab,
                    position,
                    Quaternion.identity);

            return obj;
        }
        Debug.Log("Checking Cache...");
        // Cached GLB
        if (CacheManager.Instance.HasCachedObject(
            objectName))
        {
            string path =
                CacheManager.Instance.GetCachedPath(
                    objectName);

            GameObject obj =
                await GLBLoader.Instance.LoadGLB(
                    path);

            if (obj != null)
            {
                obj.transform.position =
                    position;

                AutoSetupManager.Instance
                    .SetupObject(
                        obj,
                        objectName);
            }

            return obj;
        }
        Debug.Log("Checking SearchProvider...");
        ModelSearchResult result =
    await SearchProvider.Instance
        .Search(objectName);

        if (result != null)
        {
            Debug.Log(
        "Download URL Found");

    string path =
        await DownloadManager.Instance
        .DownloadAndExtract(
            result.objectName,
            result.downloadUrl);

    Debug.Log(
        "Extracted Path: " +
        path);

            if (path != null)
            {
                GameObject obj =
                    await GLBLoader.Instance
                    .LoadGLB(path);

                if (obj != null)
                {
                    obj.transform.position =
                        position;

                    AutoSetupManager.Instance
                        .SetupObject(
                            obj,
                            objectName);
                }

                return obj;
            }
        }

        return null;
    }
}