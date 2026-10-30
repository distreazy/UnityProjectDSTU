using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Health Regen")] // НОВОЕ
    public float healthRegenPerSecond = 10f; // Сколько ХП восстанавливается в секунду
    public float healthRegenDelay = 5f;      // Через сколько секунд после урона начнется реген
    private float healthRegenTimer = 0f;     // Таймер, который считает время до регена

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

        // --- ЛОГИКА РЕГЕНЕРАЦИИ ЗДОРОВЬЯ ---
        if (currentHealth < maxHealth)
        {
            if (healthRegenTimer > 0f)
            {
                // Если таймер еще тикает (мы недавно получали урон) — уменьшаем его
                healthRegenTimer -= Time.deltaTime;
            }
            else
            {
                // Если таймер дошел до нуля — регенерируем ХП
                currentHealth = Mathf.Clamp(currentHealth + healthRegenPerSecond * Time.deltaTime, 0f, maxHealth);
                UpdateUI(); // Обновляем полоску на экране
            }
        }
        // -----------------------------------

        // --- СТАРАЯ ЛОГИКА СТАМИНЫ ---
        if (staminaRegenTimer > 0f)
            staminaRegenTimer -= Time.deltaTime;

        UpdateUI();
    }

    public void TakeDamage(float dmg)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0f, maxHealth);

        // НОВОЕ: При получении урона сбрасываем таймер регена
        healthRegenTimer = healthRegenDelay;

        UpdateUI();

        if (currentHealth <= 0f)
            Die();
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

    // Этот метод пытается потратить стамину (для прыжка). 
    // Если стамины хватает — тратит и возвращает true. Если нет — возвращает false.
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
        // (Смерть обрабатывается через GameManager, который следит за CurrentHealth)
    }
}