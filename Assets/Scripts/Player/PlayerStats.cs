using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaRegenPerSecond = 15f;
    public float regenDelay = 1f;
    private float currentStamina;
    private float regenTimer = 0f;
    public float jumpStaminaCost = 15f;

    [Header("UI")]
    public Image healthFill;
    public Image staminaFill;

    // Публичные свойства
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
        // Если игра на паузе
        if (Time.timeScale == 0f) return;

        if (regenTimer > 0f)
            regenTimer -= Time.deltaTime;

        UpdateUI();
    }

    public void TakeDamage(float dmg)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0f, maxHealth);
        UpdateUI();

        if (currentHealth <= 0f)
            Die();
    }

    public bool TrySpendStamina(float amount)
    {
        if (currentStamina < amount) return false; // если не хватает
        currentStamina -= amount;
        regenTimer = regenDelay;
        UpdateUI();
        return true;
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateUI();
    }

    public void DrainStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0f, maxStamina);
        regenTimer = regenDelay;
    }

    public void RegenerateStamina(float deltaTime)
    {
        if (regenTimer > 0f) return;
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

    void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}