using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Vehicle))]
public class BicycleController : MonoBehaviour
{
    private Vehicle vehicle;

    private void Awake()
    {
        vehicle = GetComponent<Vehicle>();
    }

    private void Update()
{
    if (!vehicle.isControlled)
        return;

    float move = 0;
    float turn = 0;

    if (Keyboard.current.wKey.isPressed)
        move = 1;

    if (Keyboard.current.sKey.isPressed)
        move = -1;

    if (Keyboard.current.aKey.isPressed)
        turn = -1;

    if (Keyboard.current.dKey.isPressed)
        turn = 1;

    transform.Translate(
        Vector3.forward *
        move *
        vehicle.speed *
        Time.deltaTime);

    transform.Rotate(
        Vector3.up *
        turn *
        100f *
        Time.deltaTime);
}
}