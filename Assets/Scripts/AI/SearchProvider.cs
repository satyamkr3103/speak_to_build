using System.Threading.Tasks;
using UnityEngine;

public class SearchProvider : MonoBehaviour
{
    public static SearchProvider Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<ModelSearchResult>
        Search(string objectName)
    {
        Debug.Log(
            "Searching for: " +
            objectName);

        await Task.Delay(500);

        return GetMockResult(
            objectName);
    }

    ModelSearchResult GetMockResult(
        string objectName)
    {
        objectName =
            objectName.ToLower();

        if(objectName == "dragon")
        {
            return new ModelSearchResult
            {
                objectName = "dragon",
                category = "creature",
                downloadUrl =
                    "https://example.com/dragon.glb"
            };
        }

        if(objectName == "lion")
        {
            return new ModelSearchResult
            {
                objectName = "lion",
                category = "creature",
                downloadUrl =
                    "https://example.com/lion.glb"
            };
        }

        if(objectName == "car")
        {
            return new ModelSearchResult
            {
                objectName = "car",
                category = "vehicle",
                downloadUrl =
                    "https://example.com/car.glb"
            };
        }

        return null;
    }
}