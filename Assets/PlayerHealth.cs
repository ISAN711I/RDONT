using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public HealthBar healthBar;

    public bool canbehit = false;

    [SerializeField] private AudioClip healSound;
    
    [Header("Screen Shake")]
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.2f;

    public CinemachineImpulseSource cinemachinthing;

    void Start()
    {
        cinemachinthing = GetComponent<CinemachineImpulseSource>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
        SceneManager.LoadScene(2);
        Destroy(gameObject);
        Debug.Log("Player died!");
    }

}