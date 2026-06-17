using System.IO;
using UnityEngine;

public class VoiceRecorder : MonoBehaviour
{
    public static VoiceRecorder Instance;

    private AudioClip clip;

    private string device;

    private void Awake()
    {
        Instance = this;
    }

    public void StartRecording()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError(
                "No microphone found");

            return;
        }

        device =
            Microphone.devices[0];

        clip =
            Microphone.Start(
                device,
                false,
                10,
                16000);

        Debug.Log(
            "Recording Started");
    }

    public byte[] StopRecording()
    {
        if (clip == null)
            return null;

        Microphone.End(device);

        byte[] wav =
            WavUtility.FromAudioClip(
                clip);

        Debug.Log(
            "Recording Stopped");

        return wav;
    }
}
