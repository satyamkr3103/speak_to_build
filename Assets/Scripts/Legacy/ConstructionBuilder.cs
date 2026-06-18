using System.Collections;
using UnityEngine;

public class ConstructionBuilder : MonoBehaviour
{
    public IEnumerator BuildObject(
        Vector3 position,
        Quaternion rotation,
        GenericRecipe recipe)
    {
        foreach (var piece in recipe.pieces)
        {
            Vector3 spawnPos =
                position + rotation * piece.localPosition;

            Debug.Log("Build Position: " + position);
            Debug.Log("Piece Offset: " + piece.localPosition);
            Debug.Log("Final Spawn: " + spawnPos);

            GameObject obj =
                Instantiate(
                    piece.prefab,
                    spawnPos + Vector3.up * 3f,
                    rotation);

            ObjectLifetime life =
                obj.AddComponent<ObjectLifetime>();

            life.lifeTime = 20f;

            StartCoroutine(
                FlyDown(obj, spawnPos));

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FlyDown(
        GameObject obj,
        Vector3 target)
    {
        float t = 0;

        Vector3 start = obj.transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * 3f;

            obj.transform.position =
                Vector3.Lerp(start, target, t);

            yield return null;
        }
    }
}