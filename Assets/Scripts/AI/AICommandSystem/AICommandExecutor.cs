using UnityEngine;

public class AICommandExecutor :
    MonoBehaviour
{
    public static AICommandExecutor Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async void Execute(
        AICommand command)
    {
        switch (command.action.ToLower())
        {
            case "spawn":

                await AISpawnManager
                    .Instance
                    .SpawnObject(
                        command.targetObject,
                        Vector3.zero);

                break;

            case "delete":

                DeleteObject(
                    command.targetObject);

                break;
            case "move":

                MoveObject(
                    command.targetObject,
                    command.value);

                break;
            case "rotate":

                RotateObject(
                    command.targetObject,
                    command.value);

                break;

            case "scale":

                ScaleObject(
                    command.targetObject,
                    command.value);

                break;
            case "movenear":

                MoveNear(
                    command.targetObject,
                    command.referenceObject,
                    command.value);

                break;
            case "moveto":

                MoveTo(
                    command.targetObject,
                    command.referenceObject);

                break;
        }
    }
    void MoveNear(
    string target,
    string reference,
    float distance)
    {
        RuntimeObject targetObj =
            WorldObjectManager
            .Instance
            .Find(target);

        RuntimeObject referenceObj =
            WorldObjectManager
            .Instance
            .Find(reference);

        if (targetObj == null)
        {
            Debug.LogWarning(
                "Target Not Found");

            return;
        }

        if (referenceObj == null)
        {
            Debug.LogWarning(
                "Reference Not Found");

            return;
        }

        Vector3 offset =
    Random.insideUnitSphere;

        offset.y = 0;

        offset =
            offset.normalized *
            distance;

        targetObj.transform.position =
            referenceObj.transform.position +
            offset;
    }

    void MoveTo(
    string target,
    string reference)
    {
        RuntimeObject targetObj =
            WorldObjectManager.Instance
            .Find(target);

        RuntimeObject referenceObj =
            WorldObjectManager.Instance
            .Find(reference);

        if (targetObj == null ||
           referenceObj == null)
        {
            return;
        }

        targetObj.transform.position =
            referenceObj.transform.position;
    }
    void ScaleObject(
    string objectName,
    float factor)
    {
        RuntimeObject obj =
            WorldObjectManager
            .Instance
            .Find(objectName);

        if (obj == null)
            return;

        obj.transform.localScale *=
            factor;
    }

    void RotateObject(
        string objectName,
        float angle)
    {
        RuntimeObject obj =
            WorldObjectManager
            .Instance
            .Find(objectName);

        if (obj == null)
        {
            Debug.LogWarning(
                "Object Not Found");

            return;
        }

        obj.transform.Rotate(0, angle, 0);
    }
    void MoveObject(
        string objectName,
        float distance)
    {
        RuntimeObject obj =
            WorldObjectManager
            .Instance
            .Find(objectName);

        if (obj == null)
        {
            Debug.LogWarning(
                "Object Not Found");

            return;
        }

        obj.transform.position +=
            Vector3.forward *
            distance;
    }
    void DeleteObject(
        string objectName)
    {
        RuntimeObject obj =
            WorldObjectManager
            .Instance
            .Find(
                objectName);

        if (obj == null)
        {
            Debug.LogWarning(
                "Object Not Found");

            return;
        }

        Destroy(
            obj.gameObject);
    }
}