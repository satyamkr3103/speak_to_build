using System.Collections.Generic;
using UnityEngine;

public class ModelRepository : MonoBehaviour
{
    public static ModelRepository Instance;

    private void Awake()
    {
        Instance = this;
    }

    Dictionary<string, string> models =
        new Dictionary<string, string>()
    {
        {
            "dragon",
            "YOUR_DRAGON_URL"
        },
        {
            "lion",
            "YOUR_LION_URL"
        },
        {
            "car",
            "YOUR_CAR_URL"
        },
        {
            "boat",
            "YOUR_BOAT_URL"
        }
    };

    public bool HasModel(string name)
    {
        return models.ContainsKey(
            name.ToLower());
    }

    public string GetUrl(string name)
    {
        return models[
            name.ToLower()];
    }
}