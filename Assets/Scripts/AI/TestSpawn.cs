using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    async void Start()
    {
        await AISpawnManager.Instance
            .SpawnObject(
                "spaceship",
                Vector3.zero);
    }
}