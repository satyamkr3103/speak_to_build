using System.Collections.Generic;
using UnityEngine;

public class ObjectDatabase : MonoBehaviour
{
    public static ObjectDatabase Instance;

    public List<ObjectData> objects =
        new List<ObjectData>();

    private Dictionary<string, ObjectData> lookup =
        new Dictionary<string, ObjectData>();

    private void Awake()
    {
        Instance = this;

        foreach (var obj in objects)
        {
            lookup[obj.objectName.ToLower()] = obj;
        }
    }

    private void Start()
    {
        ObjectData data =
            GetObject("bridge");

        Debug.Log(data.objectName);
    }

    public ObjectData GetObject(string name)
    {
        name = name.ToLower();

        if (lookup.ContainsKey(name))
            return lookup[name];

        return null;
    }
}