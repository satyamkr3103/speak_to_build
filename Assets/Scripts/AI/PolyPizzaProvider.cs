using System.Threading.Tasks;
using UnityEngine;

public class PolyPizzaProvider :
    IModelSearchProvider
{
    public async Task<ModelSearchResult>
        Search(string objectName)
    {
        Debug.Log(
            "Searching PolyPizza: " +
            objectName);

        await Task.Delay(100);

        return null;
    }
}