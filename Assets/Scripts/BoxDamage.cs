using UnityEngine;

public class BoxDamage : MonoBehaviour
{
    [Header("Настройки урона")]
    public float damageThreshold = 3f; // Минимальная скорость, чтобы нанести урон
    public float damageMultiplier = 2f; // Множитель урона (скорость * множитель)

    private Rigidbody rb;
    public float Weight { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Weight = rb.mass; // Запоминаем вес для расчетов
        }
    }

    // Автоматически вызывается при столкновении
    void OnCollisionEnter(Collision collision)
    {
        // Если летим слишком медленно - урона нет
        if (collision.relativeVelocity.magnitude < damageThreshold) return;

        // Ищем скрипт здоровья у того, во что врезались
        // (Предполагается, что у врагов есть скрипт EnemyHealth)
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            // Формула урона: Скорость столкновения * Множитель * Вес ящика
            float speed = collision.relativeVelocity.magnitude;
            float damage = speed * damageMultiplier * Weight;

            // Наносим урон
            enemy.TakeDamage(damage);

            Debug.Log($"Ящик весом {Weight} врезался на скорости {speed:F1} и нанес {damage:F1} урона!");
        }
    }
}