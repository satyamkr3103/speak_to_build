using System.Threading.Tasks;
using UnityEngine;
public class SearchProvider : MonoBehaviour
{
    public static SearchProvider Instance;

    private IModelSearchProvider[]
        providers;

    private void Awake()
    {
        Instance = this;

        providers =
            new IModelSearchProvider[]
            {
                new PolyPizzaProvider(),
                new SketchfabProvider()
            };
    }

    public async Task<ModelSearchResult>
        Search(string objectName)
    {
        foreach(var provider in providers)
        {
            ModelSearchResult result =
                await provider.Search(
                    objectName);

            if(result != null)
                return result;
        }

        return null;
    }
}