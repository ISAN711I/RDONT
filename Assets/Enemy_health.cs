using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    [Header("Health")]
    public bool diesondeath = true;
    public float health;
    public bool invulnerable;

    [Header("Death Effects")]
    public GameObject deatheffect;

    [Header("General Drop")]
    public GameObject drop;
    [Range(0f, 1f)]
    public float dropChance = 0.5f;

    [Header("Coin Drop Settings")]
    public GameObject coinPrefab;
    [Range(0f, 1f)]
    public float coinDropChance = 0.8f;
    public int minCoins = 1;
    public int maxCoins = 5;

    [Header("Death Sounds")]
    public AudioClip[] deathSounds;
    public float deathSoundVolume = 1f;

    [Header("Hit Sounds")]
    public AudioClip[] hitSounds; // New: Hit sound clips
    public float hitSoundVolume = 1f; // New: Hit sound volume

    void Update()
    {
        if (health <= 0 &&diesondeath == true)
        {
            // Spawn death effect
            if (deatheffect != null)
                Instantiate(deatheffect, transform.position, Quaternion.identity);

            // Drop a power-up or item
            if (drop != null && Random.value < dropChance)
                Instantiate(drop, transform.position, Quaternion.identity);

            // Drop coins
            if (coinPrefab != null && Random.value < coinDropChance)
            {
                int coinAmount = Random.Range(minCoins, maxCoins + 1);

                for (int i = 0; i < coinAmount; i++)
                {
                    Vector2 offset = Random.insideUnitCircle * 0.5f;
                    Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0f);
                    Instantiate(coinPrefab, spawnPos, Quaternion.identity);
                }
            }

            // Play random death sound
            if (deathSounds.Length > 0)
            {
                AudioClip clip = deathSounds[Random.Range(0, deathSounds.Length)];
                AudioSource.PlayClipAtPoint(clip, transform.position, deathSoundVolume);
            }

            // Destroy the enemy

            swaaerh.enemiesAlive--;
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!invulnerable)
        {
            health -= damage;

            // Play random hit sound
            if (hitSounds.Length > 0)
            {
                AudioClip clip = hitSounds[Random.Range(0, hitSounds.Length)];
                AudioSource.PlayClipAtPoint(clip, transform.position, hitSoundVolume);
            }
        }
    }
}
