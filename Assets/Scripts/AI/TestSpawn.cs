using UnityEngine;
using UnityEngine.InputSystem;

public class CommandExecutorTest :
    MonoBehaviour
{
    private Keyboard keyboard;

    private void Start()
    {
        keyboard =
            Keyboard.current;
    }

    private async void Update()
    {
        if (keyboard.jKey
            .wasPressedThisFrame)
        {
            AICommand command =
                await AICommandParser
                .Instance
                .Parse(
                    "spawn dragon");
            
           
            AICommandExecutor
                .Instance
                .Execute(
                    command);
            
        }

        if (keyboard.kKey
            .wasPressedThisFrame)
        {
            AICommand command =
                await AICommandParser
                .Instance
                .Parse(
                    "delete dragon");

            AICommandExecutor
                .Instance
                .Execute(
                    command);
        }

        if (keyboard.lKey
            .wasPressedThisFrame)
        {
            AICommand command =
                await AICommandParser
                .Instance
                .Parse(
                    "move dragon 5");

            AICommandExecutor
                .Instance
                .Execute(
                    command);
        }

        if (keyboard.mKey
            .wasPressedThisFrame)
        {
            AICommand command =
                await AICommandParser
                .Instance
                .Parse(
                    "rotate dragon 90");

            AICommandExecutor
                .Instance
                .Execute(
                    command);
        }

        if (keyboard.nKey
            .wasPressedThisFrame)
        {
            AICommand command =
                await AICommandParser
                .Instance
                .Parse(
                    "scale dragon 2");

            AICommandExecutor
                .Instance
                .Execute(
                    command);
        }
    }
}