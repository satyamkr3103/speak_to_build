using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    public float lifeTime = 20f;
    public bool isBeingPlaced;
    private float timer;

    private Renderer rend;

    private void Start()
    {
        timer = lifeTime;

        rend = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        if (!isBeingPlaced)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 5f)
        {
            if (rend != null)
            {
                float alpha =
                    Mathf.PingPong(Time.time * 5f, 1f);

                Color c =
                    rend.material.color;

                c.a = alpha;

                rend.material.color = c;
            }
        }

        if (timer <= 0)
        {
            RuntimeObject runtime =
    GetComponent<RuntimeObject>();

            if (runtime != null)
            {
                WorldObjectManager
                    .Instance
                    .Remove(
                        runtime.objectName);
            }
            Destroy(gameObject);
        }
    }
}