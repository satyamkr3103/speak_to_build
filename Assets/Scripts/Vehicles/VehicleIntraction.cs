using UnityEngine;
using UnityEngine.InputSystem;
public class VehicleInteraction : MonoBehaviour
{
    private Vehicle currentVehicle;

    public float interactionRange = 3f;

    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (currentVehicle == null)
                TryEnterVehicle();
            else
                ExitVehicle();
        }
    }

    void TryEnterVehicle()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                interactionRange);

        foreach (Collider hit in hits)
        {
            Vehicle vehicle =
                hit.GetComponent<Vehicle>();

            if (vehicle != null)
            {
                EnterVehicle(vehicle);
                break;
            }
        }
    }

    void EnterVehicle(Vehicle vehicle)
    {
        currentVehicle = vehicle;

        vehicle.isControlled = true;

        GetComponent<PlayerController3D>().enabled = false;

        Renderer renderer =
            GetComponentInChildren<Renderer>();

        if (renderer != null)
            renderer.enabled = false;

        Camera.main
            .GetComponent<IsometricCamera>()
            .SetTarget(vehicle.transform);
    }

    void ExitVehicle()
    {
        transform.position =
            currentVehicle.transform.position +
            currentVehicle.transform.right * 2f;

        currentVehicle.isControlled = false;

        Renderer renderer =
            GetComponentInChildren<Renderer>();

        if (renderer != null)
            renderer.enabled = true;

        GetComponent<PlayerController3D>().enabled = true;

        Camera.main
            .GetComponent<IsometricCamera>()
            .SetTarget(transform);

        currentVehicle = null;
    }
}