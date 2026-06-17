using UnityEngine;
using UnityEngine.InputSystem;

public class CameraModeManager : MonoBehaviour
{
    public static CameraModeManager Instance;

    [Header("References")]
    public Camera mainCamera;

    public ThirdPersonCamera thirdPersonCamera;

    public Transform player;

    [Header("Ortho Settings")]
    public float orthoHeight = 30f;

    public float orthoSize = 15f;

    private bool buildMode;

    private Keyboard keyboard;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        keyboard =
            Keyboard.current;

        EnterThirdPersonMode();
    }

    private void Update()
    {
        if (keyboard == null)
            return;

        if (keyboard.tabKey.wasPressedThisFrame)
        {
            buildMode =
                !buildMode;

            if (buildMode)
            {
                EnterBuildMode();
            }
            else
            {
                EnterThirdPersonMode();
            }
        }

    }

    void EnterBuildMode()
    {
        mainCamera.orthographic =
            true;

        mainCamera.orthographicSize =
            orthoSize;

        thirdPersonCamera.enabled =
            false;

        Vector3 pos =
            player.position;

        pos.y =
            orthoHeight;

        mainCamera.transform.position =
            pos;

        mainCamera.transform.rotation =
            Quaternion.Euler(
                90,
                0,
                0);

        Debug.Log(
            "Build Mode");
    }

    void EnterThirdPersonMode()
    {
        mainCamera.orthographic =
            false;

        thirdPersonCamera.enabled =
            true;

        Debug.Log(
            "Third Person Mode");
    }


    public bool IsBuildMode()
    {
        return buildMode;
    }
}