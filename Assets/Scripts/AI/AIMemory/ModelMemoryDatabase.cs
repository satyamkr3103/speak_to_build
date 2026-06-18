using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class ModelMemoryDatabase :
    MonoBehaviour
{
    public static
        ModelMemoryDatabase Instance;

    private List<ModelMemory>
        memories =
            new List<ModelMemory>();

    string SavePath =>
        Path.Combine(
            Application.persistentDataPath,
            "ModelMemory.json");

    private void Awake()
    {
        Instance = this;

        Load();
    }

    public ModelMemory
        GetBestMemory(
            string objectName)
    {
        objectName =
            objectName.ToLower();

        ModelMemory best =
            null;

        foreach(var memory in memories)
        {
            if(memory.objectName !=
                objectName)
                continue;

            if(best == null ||
               memory.successScore >
               best.successScore)
            {
                best = memory;
            }
        }

        return best;
    }

    public void RecordSuccess(
        string objectName,
        string uid,
        string modelName,
        string glbPath)
    {
        ModelMemory existing =
            memories.Find(
                x =>
                x.uid == uid);

        if(existing == null)
        {
            existing =
                new ModelMemory();

            existing.objectName =
                objectName;

            existing.uid =
                uid;

            existing.modelName =
                modelName;

            existing.glbPath =
                glbPath;

            existing.successScore =
                0;

            memories.Add(
                existing);
        }

        existing.successScore++;

        Save();

        Debug.Log(
            $"Memory Updated: " +
            $"{existing.modelName} " +
            $"({existing.successScore})");
    }

    void Save()
    {
        string json =
            JsonConvert.SerializeObject(
                memories,
                Formatting.Indented);

        File.WriteAllText(
            SavePath,
            json);
    }

    void Load()
    {
        if(!File.Exists(
            SavePath))
        {
            return;
        }

        string json =
            File.ReadAllText(
                SavePath);

        memories =
            JsonConvert.DeserializeObject
            <List<ModelMemory>>(
                json);

        if(memories == null)
        {
            memories =
                new List<ModelMemory>();
        }
    }
}