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
        ModelMemory memory =
    ModelMemoryDatabase
        .Instance
        .GetBestMemory(
            objectName);

        if (memory != null &&
           File.Exists(
               memory.glbPath))
        {
            Debug.Log(
                "Memory Hit: " +
                memory.modelName);

            GameObject obj =
                await GLBLoader.Instance
                    .LoadGLB(
                        memory.glbPath);

            if (obj != null)
            {
                obj.transform.position =
                    position;

                AutoSetupManager.Instance
                    .SetupObject(
                        obj,
                        objectName);

                PlacementManager.Instance
    .BeginPlacement(
        obj);

                return obj;
            }
        }
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

            PlacementManager.Instance
    .BeginPlacement(
        obj);

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

            PlacementManager.Instance
    .BeginPlacement(
        obj);

return obj;
        }
        Debug.Log("Checking SearchProvider...");
        SearchResultsBundle bundle =
            await SearchProvider.Instance
                .Search(objectName);

        if (bundle != null &&
            bundle.results.Count > 0)
        {
            foreach (
                ModelSearchResult result
                in bundle.results)
            {
                Debug.Log(
                    "Trying: " +
                    result.modelName);

                string path =
                    await DownloadManager.Instance
                        .DownloadAndExtract(
                            objectName,
                            result.downloadUrl);

                if (path == null)
                {
                    Debug.LogWarning(
                        "Download Failed");

                    continue;
                }

                GameObject obj =
                    await GLBLoader.Instance
                        .LoadGLB(path);

                if (obj == null)
                {
                    Debug.LogWarning(
                        "Load Failed");

                    continue;
                }

                obj.transform.position =
                    position;

                AutoSetupManager.Instance
                    .SetupObject(
                        obj,
                        objectName);

                Debug.Log(
                    "SUCCESS: " +
                    result.modelName);
                ModelMemoryDatabase
    .Instance
    .RecordSuccess(
        objectName,
        result.uid,
        result.modelName,
        path);

                ModelSuccessCache.Instance
                    .RecordSuccess(result.uid);

                PlacementManager.Instance
    .BeginPlacement(
        obj);

return obj;
            }
        }

        return null;
    }
}