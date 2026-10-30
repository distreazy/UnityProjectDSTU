using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;

    void Start()
    {
        Destroy(gameObject, 3f); // Удаляем пулю через 3 секунды
    }

    void OnTriggerEnter(Collider other)
    {
        // Если попали в игрока
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null) stats.TakeDamage(damage);

            Destroy(gameObject); // Уничтожаем пулю
        }

        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}