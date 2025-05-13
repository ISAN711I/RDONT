using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public HealthBar healthBar;
    public float regen = 0;
    public int revives;

    public bool canbehit = false;

    [SerializeField] private AudioClip healSound;
    
    [Header("Screen Shake")]
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.2f;

    public CinemachineImpulseSource cinemachinthing;

    public InventoryObject inventory;

    void Start()
    {
        cinemachinthing = GetComponent<CinemachineImpulseSource>();



        for (int i = 0; i < inventory.container.Count; i++)
        {
            int amt = inventory.container[i].amount;
            ItemData mine = inventory.container[i].item;

            maxHealth += mine.hp * amt;
            maxHealth *= 1 + amt * mine.hp_mult;

            regen += amt*mine.regen;
            revives += amt * mine.revives;
        }

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += regen * Time.deltaTime;
        }
        healthBar.SetHealth(currentHealth);
    }
    public void TakeDamage(float damage)
    {
        if(canbehit == true) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        
        cinemachinthing.GenerateImpulse();
        if (currentHealth <= 0)
        {
            Die();
        }
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        healthBar.SetHealth(currentHealth);
        
        if (healSound != null)
        {
            AudioSource.PlayClipAtPoint(healSound, transform.position);
        }
        
        Debug.Log($"Player healed! Current health: {currentHealth}");
    }

    void Die()
    {
        if (revives > 0)
        {
            currentHealth = maxHealth;
            revives -= 1;
        }
        else
        {
            SceneManager.LoadScene(2);
            Destroy(gameObject);
            Debug.Log("Player died!");
        }
    }

}