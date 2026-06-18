using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public float gridSize = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        float x =
            Mathf.Round(position.x / gridSize) * gridSize;

        float z =
            Mathf.Round(position.z / gridSize) * gridSize;

        return new Vector3(x, 0, z);
    }
}