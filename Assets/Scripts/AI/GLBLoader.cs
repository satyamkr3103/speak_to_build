using System.Threading.Tasks;
using UnityEngine;
using GLTFast;

public class GLBLoader : MonoBehaviour
{
    public static GLBLoader Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<GameObject> LoadGLB(string path)
    {
        GltfImport gltf = new GltfImport();

        bool success =
            await gltf.Load(path);

        if (!success)
        {
            Debug.LogError(
                "Failed to load GLB");

            return null;
        }

        GameObject root =
            new GameObject("LoadedGLB");

        await gltf.InstantiateMainSceneAsync(
            root.transform);

        return root;
    }
}