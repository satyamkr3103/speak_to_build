using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadManager : MonoBehaviour
{
    public static DownloadManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<string> DownloadModel(
        string objectName,
        string downloadUrl)
    {
        string savePath =
            CacheManager.Instance
            .GetCachedPath(objectName);

        UnityWebRequest request =
            UnityWebRequest.Get(downloadUrl);

        var operation =
            request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if(request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                request.error);

            return null;
        }

        File.WriteAllBytes(
            savePath,
            request.downloadHandler.data);

        Debug.Log(
            "Downloaded: " +
            savePath);

        return savePath;
    }
}