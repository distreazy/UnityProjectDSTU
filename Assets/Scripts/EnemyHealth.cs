using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} получил {amount:F1} урона! Осталось: {currentHealth}");

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        // Ищем менеджер игры на сцене и сообщаем о смерти врага
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyKilled();
        }

        Destroy(gameObject); // Уничтожаем врага
    }
}