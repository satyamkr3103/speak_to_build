using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class ModelSuccessCache :
    MonoBehaviour
{
    public static ModelSuccessCache Instance;

    private Dictionary<string, int>
        successScores =
            new Dictionary<string, int>();

    string SavePath =>
        Path.Combine(
            Application.persistentDataPath,
            "modelScores.json");

    private void Awake()
    {
        Instance = this;

        Load();
    }

    public void RecordSuccess(
        string uid)
    {
        if (!successScores.ContainsKey(uid))
        {
            successScores[uid] = 0;
        }

        successScores[uid]++;

        Save();

        Debug.Log(
            "Success Score: " +
            uid +
            " = " +
            successScores[uid]);
    }

    public int GetScore(
        string uid)
    {
        if (successScores.TryGetValue(
            uid,
            out int score))
        {
            return score;
        }

        return 0;
    }

    void Save()
    {
        string json =
            JsonConvert.SerializeObject(
                successScores,
                Formatting.Indented);

        File.WriteAllText(
            SavePath,
            json);
    }

    void Load()
    {
        if (!File.Exists(
            SavePath))
        {
            return;
        }

        string json =
            File.ReadAllText(
                SavePath);

        successScores =
            JsonConvert.DeserializeObject<
                Dictionary<string, int>>(
                    json);

        if (successScores == null)
        {
            successScores =
                new Dictionary<string, int>();
        }
    }
}
