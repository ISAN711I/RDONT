using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [Header("Health")]
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthFill;
    public TMP_Text healthText;

    [Header("Stamina")]
    public Slider staminaSlider;
    public Gradient staminaGradient;
    public Image staminaFill;
    public TMP_Text staminaText;

    // Health Methods
    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        healthFill.color = healthGradient.Evaluate(1f);
        UpdateHealthText(health);
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
        UpdateHealthText(health);
    }

    private void UpdateHealthText(float currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(healthSlider.maxValue)}";
        }
    }

    // Stamina Methods
    public void SetMaxStamina(float stamina)
    {
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
        staminaFill.color = staminaGradient.Evaluate(1f);
        UpdateStaminaText(stamina);
    }

    public void SetStamina(float stamina)
    {
        staminaSlider.value = stamina;
        staminaFill.color = staminaGradient.Evaluate(staminaSlider.normalizedValue);
        UpdateStaminaText(stamina);
    }

    private void UpdateStaminaText(float currentStamina)
    {
        if (staminaText != null)
        {
            staminaText.text = $"Stamina: {Mathf.RoundToInt(currentStamina)}/{Mathf.RoundToInt(staminaSlider.maxValue)}";
        }
    }
}
