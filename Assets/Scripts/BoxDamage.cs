using UnityEngine;

public class BoxDamage : MonoBehaviour
{
    [Header("Звуки")]
    public AudioClip hitEnemyClip;
    public AudioClip throwClip;
    private AudioSource audioSource;

    [Header("Настройки урона")]
    public float damageThreshold = 3f; // Минимальная скорость
    public float damageMultiplier = 2f; // Множитель урона

    private Rigidbody rb;
    public float Weight { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Weight = rb.mass;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Если летим слишком медленно
        if (collision.relativeVelocity.magnitude < damageThreshold) return;

        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            // Формула урона
            float speed = collision.relativeVelocity.magnitude;
            float damage = speed * damageMultiplier * Weight;

            Vector3 hitDirection = collision.relativeVelocity.normalized;

            // Наносим урон
            enemy.TakeDamage(damage, hitDirection);

            Debug.Log($"Ящик весом {Weight} врезался на скорости {speed:F1} и нанес {damage:F1} урона!");

            if (audioSource != null && hitEnemyClip != null)
            {
                audioSource.pitch = 1f + Random.Range(-0.1f, 0.1f);
                audioSource.PlayOneShot(hitEnemyClip, 0.5f);
            }
        }
    }

    public void PlayThrowSound()
    {
        if (audioSource != null && throwClip != null)
        {
            audioSource.PlayOneShot(throwClip, 0.2f);
        }
    }
}