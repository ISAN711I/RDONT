using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 5f;
    public float damage = 10f;

    [Header("Visual & Drops")]
    public GameObject enemyparticle;
    public GameObject elfbar;

    [Header("Drop Settings")]
    public float minDropChance = 10f;
    public float dropChanceMultiplier = 2f;

    private GameObject player;
    private Rigidbody2D rb;
    private float scale_value;

    public static float NextGaussianPolar(float mean = 0, float standardDeviation = 1)
    {
        float u, v, s;
        do {
            u = 2.0f * Random.value - 1.0f;
            v = 2.0f * Random.value - 1.0f;
            s = u * u + v * v;
        } while (s >= 1.0f || s == 0f);
        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);
        return u * s * standardDeviation + mean;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Random scale and damage setup
        scale_value = Mathf.Max(0.4f, NextGaussianPolar() + 1);
        damage = scale_value * scale_value + 5.0f;
        transform.localScale = new Vector3(scale_value, scale_value, scale_value);
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Set the Rigidbody2D's velocity directly to move the enemy toward the player
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
