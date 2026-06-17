using System.Threading.Tasks;
using UnityEngine;

public class VoiceBuildManager :
    MonoBehaviour
{
    public static VoiceBuildManager
        Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task ProcessVoice()
    {
        byte[] wav =
            VoiceRecorder.Instance
                .StopRecording();

        if (wav == null)
            return;

        string text =
            await GoogleSTTManager
                .Instance
                .Transcribe(wav);

        Debug.Log(
            "Recognized: " +
            text);

        if (string.IsNullOrEmpty(text))
            return;

        string objectName =
            await GroqManager.Instance
                .ExtractObjectName(
                    text);

        Debug.Log(
            "Object: " +
            objectName);

        await AISpawnManager.Instance
            .SpawnObject(
                objectName,
                Vector3.zero);
    }
}