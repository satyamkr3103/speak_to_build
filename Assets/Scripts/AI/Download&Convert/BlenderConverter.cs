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
        @"C:\Program Files\Blender Foundation\Blender 4.2\blender.exe";

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

        process.StartInfo.RedirectStandardOutput =
    true;

        process.StartInfo.RedirectStandardError =
            true;

        process.Start();

        await Task.Run(() =>
        {
            process.WaitForExit();
        });

        string output =
    process.StandardOutput.ReadToEnd();

        string error =
            process.StandardError.ReadToEnd();

        Debug.Log(output);

        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError(error);
        }
        Debug.Log(
    "Blender Exit Code: " +
    process.ExitCode);
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