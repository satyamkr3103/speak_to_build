using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    public float lifeTime = 20f;

    private float timer;

    private Renderer rend;

    private void Start()
    {
        timer = lifeTime;

        rend = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 5f)
        {
            if(rend != null)
            {
                float alpha =
                    Mathf.PingPong(Time.time * 5f, 1f);

                Color c =
                    rend.material.color;

                c.a = alpha;

                rend.material.color = c;
            }
        }

        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}