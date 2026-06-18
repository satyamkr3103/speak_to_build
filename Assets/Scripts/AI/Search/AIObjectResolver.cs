using UnityEngine;

public class AIObjectResolver : MonoBehaviour
{
    public static AIObjectResolver Instance;

    private void Awake()
    {
        Instance = this;
    }

    public ObjectData Resolve(string objectName)
    {
        return ObjectDatabase.Instance.GetObject(
            objectName.ToLower().Trim());
    }
}
