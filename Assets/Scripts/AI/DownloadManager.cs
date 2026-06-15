using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadManager : MonoBehaviour
{
    public static DownloadManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<string> DownloadAndExtract(
        string objectName,
        string zipUrl)
    {
        string cacheFolder =
            Path.Combine(
                Application.persistentDataPath,
                "DownloadedObjects");

        if (!Directory.Exists(cacheFolder))
        {
            Directory.CreateDirectory(
                cacheFolder);
        }

        string zipPath =
            Path.Combine(
                cacheFolder,
                objectName + ".zip");

        UnityWebRequest request =
            UnityWebRequest.Get(zipUrl);

        var operation =
            request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                request.error);

            return null;
        }

        File.WriteAllBytes(
            zipPath,
            request.downloadHandler.data);

        Debug.Log(
            "ZIP Downloaded");

        string extractFolder =
            Path.Combine(
                cacheFolder,
                objectName);

        if (Directory.Exists(
            extractFolder))
        {
            Directory.Delete(
                extractFolder,
                true);
        }

        ZipFile.ExtractToDirectory(
            zipPath,
            extractFolder);

        string[] zipFiles =
            Directory.GetFiles(
                extractFolder,
                "*.zip",
                SearchOption.AllDirectories);

        foreach (string nestedZip in zipFiles)
        {
            string nestedFolder =
                Path.Combine(
                    Path.GetDirectoryName(
                        nestedZip),
                    Path.GetFileNameWithoutExtension(
                        nestedZip));

            ZipFile.ExtractToDirectory(
                nestedZip,
                nestedFolder,
                true);

            Debug.Log(
                "Nested ZIP Extracted");
        }

        string[] files =
    Directory.GetFiles(
        extractFolder,
        "*.*",
        SearchOption.AllDirectories);

        foreach (string file in files)
        {
            Debug.Log(file);
        }
        Debug.Log(
            "ZIP Extracted");

        string sourceFile =
            FindSourceModel(
                extractFolder);

        if (sourceFile == null)
        {
            return null;
        }

        if (sourceFile.EndsWith(
    ".glb",
    System.StringComparison.OrdinalIgnoreCase))
        {
            return sourceFile;
        }

        string glbPath =
            await BlenderConverter.Instance
                .ConvertToGLB(
                    sourceFile);

        return glbPath;
    }

    string FindSourceModel(
    string folder)
    {
        string[] files =
            Directory.GetFiles(
                folder,
                "*.*",
                SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string extension =
                Path.GetExtension(
                    file).ToLower();

            if (
                extension == ".glb" ||
                extension == ".gltf" ||
                extension == ".obj" ||
                extension == ".fbx")
            {
                return file;
            }
        }

        Debug.LogError(
            "No source model found");

        return null;
    }
}