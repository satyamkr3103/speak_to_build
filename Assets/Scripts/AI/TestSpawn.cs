using UnityEngine;
using UnityEngine.InputSystem;

public class TestSpawn : MonoBehaviour
{
    private Keyboard keyboard;

    private void Start()
    {
        keyboard =
            Keyboard.current;
    }

    private void Update()
    {
        if (keyboard == null)
            return;

        if (keyboard.rKey.wasPressedThisFrame)
        {
            VoiceRecorder.Instance
                .StartRecording();
        }

        if (keyboard.tKey.wasPressedThisFrame)
        {
            _ =
                VoiceBuildManager.Instance
                    .ProcessVoice();
        }
    }
}