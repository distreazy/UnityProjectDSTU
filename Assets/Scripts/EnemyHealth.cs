using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Juice")]
    public Renderer enemyRenderer;
    public float knockbackForce = 5f;
    private Color originalColor;

    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject deathParticles;

    void Start()
    {
        currentHealth = maxHealth;
        if (enemyRenderer != null) originalColor = enemyRenderer.material.color;
    }

    public void TakeDamage(float amount, Vector3 hitDirection)
    {
        currentHealth -= amount;

        // Вспышка красным
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = Color.red;
            Invoke(nameof(ResetColor), 0.1f);
        }

        // Отбрасывание
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.AddForce(hitDirection * knockbackForce, ForceMode.Impulse);

        if (currentHealth <= 0) Die();
    }

    void ResetColor()
    {
        if (enemyRenderer != null) enemyRenderer.material.color = originalColor;
    }

    void Die()
    {
        // Ищем менеджер игры на сцене и сообщаем о смерти врага
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyKilled();
        }

        if (deathParticles != null)
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        // ... остальной код ...

        Destroy(gameObject); // Уничтожаем врага
    }
}