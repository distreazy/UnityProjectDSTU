using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Health Regen")]
    public float healthRegenPerSecond = 10f;
    public float healthRegenDelay = 5f;
    private float healthRegenTimer = 0f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaRegenPerSecond = 15f;
    public float regenDelay = 1f;
    public float jumpStaminaCost = 15f;
    private float currentStamina;
    private float staminaRegenTimer = 0f;

    [Header("UI")]
    public Image healthFill;
    public Image staminaFill;

    public bool HasStamina => currentStamina > 0f;
    public float CurrentHealth => currentHealth;
    public float CurrentStamina => currentStamina;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        UpdateUI();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (currentHealth < maxHealth)
        {
            if (healthRegenTimer > 0f)
            {
                healthRegenTimer -= Time.deltaTime;
            }
            else
            {
                currentHealth = Mathf.Clamp(currentHealth + healthRegenPerSecond * Time.deltaTime, 0f, maxHealth);
                UpdateUI();
            }
        }

        if (staminaRegenTimer > 0f)
            staminaRegenTimer -= Time.deltaTime;

        UpdateUI();
    }

    public void TakeDamage(float dmg)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0f, maxHealth);

        // При получении урона сбрасываем таймер регена
        healthRegenTimer = healthRegenDelay;

        UpdateUI();

        if (currentHealth <= 0f)
            Die();

        PlayerAudio pAudio = GetComponent<PlayerAudio>();
        if (pAudio != null) pAudio.PlayHurtSound();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateUI();
    }

    public void DrainStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0f, maxStamina);
        staminaRegenTimer = regenDelay;
    }

    public bool TrySpendStamina(float amount)
    {
        if (currentStamina < amount)
            return false; // Не хватает стамины

        currentStamina -= amount;
        staminaRegenTimer = regenDelay; // Сбрасываем таймер восстановления
        UpdateUI();
        return true;
    }

    public void RegenerateStamina(float deltaTime)
    {
        if (staminaRegenTimer > 0f) return;
        currentStamina = Mathf.Clamp(
            currentStamina + staminaRegenPerSecond * deltaTime,
            0f, maxStamina);
    }

    void UpdateUI()
    {
        if (healthFill != null)
            healthFill.fillAmount = currentHealth / maxHealth;

        if (staminaFill != null)
            staminaFill.fillAmount = currentStamina / maxStamina;
    }

    void Die()
    {
        Debug.Log("Player died");
    }
}