using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SketchfabProvider :
    IModelSearchProvider
{
    private string apiToken =
    APIKeys.SketchfabToken;

    public async Task<ModelSearchResult>
        Search(string objectName)
    {
        string url =
            $"https://api.sketchfab.com/v3/search?type=models&q={objectName}&downloadable=true";

        UnityWebRequest request =
            UnityWebRequest.Get(url);

        request.SetRequestHeader(
            "Authorization",
            "Token " + apiToken);

        var operation =
            request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                request.error);

            return null;
        }

        string json =
            request.downloadHandler.text;

        SketchfabSearchResponse response =
            JsonConvert.DeserializeObject<
                SketchfabSearchResponse>(
                    json);

        if (response == null)
        {
            Debug.LogError(
                "Parse Failed");

            return null;
        }

        foreach (var model in response.results)
        {
            if (model.isDownloadable)
            {
                Debug.Log(
                    "Found Downloadable: " +
                    model.name);

                string downloadInfo =
                    await GetDownloadUrl(
                        model.uid);

                System.IO.File.WriteAllText(
    Application.dataPath +
    "/downloadResponse.json",
    downloadInfo);

                Debug.Log(
                    "Saved Download Response");
                return new ModelSearchResult
                {
                    objectName = objectName,
                    modelName = model.name,
                    uid = model.uid,
                    category = "Unknown",
                    downloadUrl = downloadInfo
                };
            }
        }

        Debug.LogError(
            "No downloadable Sketchfab model found");

        return null;
    }

    public async Task<string>
        GetDownloadUrl(string uid)
    {
        string url =
            $"https://api.sketchfab.com/v3/models/{uid}/download";

        UnityWebRequest request =
            UnityWebRequest.Get(url);

        request.SetRequestHeader(
            "Authorization",
            "Token " + apiToken);

        var operation =
            request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                request.error);

            Debug.LogError(
                request.downloadHandler.text);

            return null;
        }

        string json =
    request.downloadHandler.text;

        SketchfabDownloadResponse response =
            JsonConvert.DeserializeObject<
                SketchfabDownloadResponse>(
                    json);

        if (response == null)
        {
            Debug.LogError(
                "Download Parse Failed");

            return null;
        }

        if (response.source == null)
        {
            Debug.LogError(
                "No source file found");

            return null;
        }

        return response.source.url;
    }
}