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

    public bool HasStamina => currentStamina > 5f;
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

        // Логика регена здоровья
        if (currentHealth < maxHealth)
        {
            if (healthRegenTimer > 0f)
                healthRegenTimer -= Time.deltaTime;
            else
            {
                currentHealth = Mathf.Clamp(currentHealth + healthRegenPerSecond * Time.deltaTime, 0f, maxHealth);
                UpdateUI();
            }
        }

        // Логика регена стамины
        if (staminaRegenTimer > 0f)
        {
            staminaRegenTimer -= Time.deltaTime;
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina = Mathf.Clamp(currentStamina + staminaRegenPerSecond * Time.deltaTime, 0f, maxStamina);
            UpdateUI();
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0f, maxHealth);
        healthRegenTimer = healthRegenDelay;
        UpdateUI();

        if (currentHealth <= 0f) Die();

        PlayerAudio pAudio = GetComponent<PlayerAudio>();
        if (pAudio != null) pAudio.PlayHurtSound();

        if (CameraShake.Instance != null) CameraShake.Instance.Shake(0.3f, 0.2f);
    }

    public void DrainStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0f, maxStamina);
        staminaRegenTimer = regenDelay;
        UpdateUI();
    }

    public bool TrySpendStamina(float amount)
    {
        if (currentStamina < amount)
            return false;

        currentStamina -= amount;
        staminaRegenTimer = regenDelay;
        UpdateUI();
        return true;
    }

    public void RegenerateStamina(float deltaTime)
    {

    }

    void UpdateUI()
    {
        if (healthFill != null) healthFill.fillAmount = currentHealth / maxHealth;
        if (staminaFill != null) staminaFill.fillAmount = currentStamina / maxStamina;
    }

    void Die()
    {
        Debug.Log("Player died");
    }
}