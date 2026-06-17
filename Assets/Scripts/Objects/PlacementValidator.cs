using UnityEngine;

public class PlacementValidator : MonoBehaviour
{
    public LayerMask blockingLayers;

    public bool IsValidPlacement()
    {
        Collider[] hits =
            Physics.OverlapBox(
                transform.position,
                transform.localScale * 0.45f,
                transform.rotation,
                blockingLayers);

        foreach (Collider hit in hits)
        {
            if (hit.transform == transform)
                continue;

            return false;
        }

        return true;
    }
}