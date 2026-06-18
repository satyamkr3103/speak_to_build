using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SketchfabProvider :
    IModelSearchProvider
{
    private string apiToken =
    APIKeys.SketchfabToken;

    public async Task<SearchResultsBundle>
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

        List<SearchCandidate>
            candidates =
                new List<SearchCandidate>();

        foreach (var model in response.results)
        {
            if (!model.isDownloadable)
                continue;

            List<string> tagList =
    new List<string>();

            if (model.tags != null)
            {
                foreach (var tag in model.tags)
                {
                    tagList.Add(tag.name);
                }
            }

            candidates.Add(
                new SearchCandidate
                {
                    uid = model.uid,
                    name = model.name,
                    likes = model.likeCount,
                    views = model.viewCount,
                    description = model.description,
                    tags = tagList
                });
        }

        if (candidates.Count == 0)
        {
            Debug.LogError(
                "No downloadable Sketchfab model found");

            return null;
        }

        List<SearchCandidate>
    filtered =
        new List<SearchCandidate>();

        foreach (var candidate in candidates)
        {
            bool valid =
                await GroqModelFilter.Instance
                    .IsValidCandidate(
                        objectName,
                        candidate);

            if (valid)
            {
                filtered.Add(
                    candidate);
            }
        }

        if (filtered.Count == 0)
        {
            Debug.LogError(
                "No Sketchfab candidates passed Groq filtering");

            return null;
        }

        filtered.Sort(
    (a, b) =>
    {
        int scoreA =
            a.likes * 2 +
            a.views / 1000 +
            ModelSuccessCache
                .Instance
                .GetScore(a.uid) * 1000;

        int scoreB =
            b.likes * 2 +
            b.views / 1000 +
            ModelSuccessCache
                .Instance
                .GetScore(b.uid) * 1000;

        return scoreB.CompareTo(scoreA);
    });

        if (filtered.Count > 8)
        {
            filtered =
                filtered.GetRange(
                    0,
                    8);
        }

        Debug.Log("=== Candidates ===");

        for (int i = 0; i < filtered.Count; i++)
        {
            Debug.Log(
                $"{i}: {filtered[i].name}");
        }

        Debug.Log(
            "=== FILTERED ===");

        foreach (var item in filtered)
        {
            Debug.Log(
                item.name);
        }

        SearchResultsBundle bundle =
            new SearchResultsBundle();

        for (int i = 0;
            i < filtered.Count;
            i++)
        {
            SearchCandidate candidate =
                filtered[i];

            string downloadUrl =
                await GetDownloadUrl(
                    candidate.uid);

            if (downloadUrl == null)
                continue;

            bundle.results.Add(
                new ModelSearchResult
                {
                    objectName =
                        objectName,

                    modelName =
                        candidate.name,

                    uid =
                        candidate.uid,

                    downloadUrl =
                        downloadUrl
                });
        }

        return bundle;
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