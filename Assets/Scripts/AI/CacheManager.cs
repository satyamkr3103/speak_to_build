using System.IO;
using UnityEngine;

public class CacheManager : MonoBehaviour
{
    public static CacheManager Instance;

    private string cacheFolder;

    private void Awake()
    {
        Instance = this;

        cacheFolder =
            Path.Combine(
                Application.persistentDataPath,
                "ConvertedObjects");

        if (!Directory.Exists(
            cacheFolder))
        {
            Directory.CreateDirectory(
                cacheFolder);
        }
    }

    public bool HasCachedObject(
        string objectName)
    {
        string path =
            GetCachedPath(
                objectName);

        return File.Exists(path);
    }

    public string GetCachedPath(
        string objectName)
    {
        return Path.Combine(
            cacheFolder,
            objectName.ToLower() + ".glb");
    }

    public void SaveToCache(
        string objectName,
        string glbPath)
    {
        string targetPath =
            GetCachedPath(
                objectName);

        File.Copy(
            glbPath,
            targetPath,
            true);

        Debug.Log(
            "Saved To Cache: " +
            targetPath);
    }
}