using UnityEngine;

[CreateAssetMenu(
    fileName = "ObjectData",
    menuName = "TextToBuild/Object Data")]
public class ObjectData : ScriptableObject
{
    public string objectName;
    public string description;
    public int energyCost;

    public float lifeTime;

    public Sprite icon;
    public GameObject blueprintPrefab;
    public GameObject recipePrefab;

    public bool isVehicle;

    public bool isCreature;

    public bool isHazard;
}
