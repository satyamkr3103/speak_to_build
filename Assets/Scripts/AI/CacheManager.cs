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
                "DownloadedObjects");

        if (!Directory.Exists(cacheFolder))
        {
            Directory.CreateDirectory(cacheFolder);
        }
        
    }

    public bool HasCachedObject(string objectName)
{
    string path =
        Path.Combine(
            cacheFolder,
            objectName + ".glb");

    if(File.Exists(path))
        return true;

    string streamingPath =
        Path.Combine(
            Application.streamingAssetsPath,
            objectName + ".glb");

    return File.Exists(streamingPath);
}

    public string GetCachedPath(string objectName)
{
    string cachePath =
        Path.Combine(
            cacheFolder,
            objectName + ".glb");

    if(File.Exists(cachePath))
        return cachePath;

    return Path.Combine(
        Application.streamingAssetsPath,
        objectName + ".glb");
}
}