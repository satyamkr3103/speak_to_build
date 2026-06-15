using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BlenderConverter : MonoBehaviour
{
    public static BlenderConverter Instance;

    [SerializeField]
    private string blenderPath =
        @"C:\Program Files\Blender Foundation\Blender 5.1\blender.exe";

    private void Awake()
    {
        Instance = this;
    }

    public async Task<string> ConvertToGLB(
        string sourceFile)
    {
        string outputPath =
            Path.ChangeExtension(
                sourceFile,
                ".glb");

        string scriptPath =
            Path.Combine(
                Application.streamingAssetsPath,
                "convert.py");

        Process process =
            new Process();

        process.StartInfo.FileName =
            blenderPath;

        process.StartInfo.Arguments =
            $"--background --python \"{scriptPath}\" -- \"{sourceFile}\" \"{outputPath}\"";

        process.StartInfo.CreateNoWindow =
            true;

        process.StartInfo.UseShellExecute =
            false;

        process.Start();

        await Task.Run(() =>
        {
            process.WaitForExit();
        });

        if (File.Exists(outputPath))
        {
            Debug.Log(
                "GLB Created");

            return outputPath;
        }

        Debug.LogError(
            "Conversion Failed");

        return null;
    }
}