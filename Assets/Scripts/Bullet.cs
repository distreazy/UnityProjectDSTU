using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;

    void Start()
    {
        Destroy(gameObject, 3f); // Удаляем пулю через 3 секунды, чтобы не тормозила игру
    }

    void OnTriggerEnter(Collider other)
    {
        // Если попали в игрока
        if (other.CompareTag("Player"))
        {
            // В нашем скрипте PlayerStats метод называется TakeDamage
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null) stats.TakeDamage(damage);

            Destroy(gameObject); // Уничтожаем пулю
        }
        // Если попали в стену
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}