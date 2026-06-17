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

    public async Task<SearchResultsBundle>
        Search(string objectName)
    {
        SearchResultsBundle bundle =
            new SearchResultsBundle();

        foreach (var provider in providers)
        {
            SearchResultsBundle providerBundle =
                await provider.Search(
                    objectName);

            if (providerBundle != null &&
                providerBundle.results.Count > 0)
            {
                bundle.results.AddRange(
                    providerBundle.results);
            }
        }

        return bundle.results.Count > 0
            ? bundle
            : null;
    }
}