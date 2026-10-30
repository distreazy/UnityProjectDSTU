using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Звуки")]
    public AudioClip shootClip;
    private AudioSource audioSource;

    [Header("Характеристики")]
    public float moveSpeed = 3.5f;
    public float damage = 10f;
    public float fireRate = 1.5f;

    [Header("Стрельба и Обзор")]
    public float detectionRange = 20f; // Радиус, в котором враг видит игрока
    public float shootingRange = 12f;  // С какого расстояния начинает стрелять
    public GameObject bulletPrefab;
    public Transform firePoint;

    private NavMeshAgent agent;
    private Transform player;
    private float currentHealth;
    private float nextFireTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        // Ищем игрока на сцене
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return; // Если игрок умер

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Если игрок в зоне
        if (distanceToPlayer <= detectionRange)
        {
            LookAtPlayer();

            // Если игрок далеко
            if (distanceToPlayer > shootingRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else // Если близко
            {
                agent.isStopped = true;
                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                }
            }
        }
        else
        {
            // Если игрока не видно
            agent.isStopped = false;
            if (!agent.hasPath || agent.remainingDistance < 0.5f)
            {
                Vector3 randomPoint = GetRandomPointOnNavMesh();
                if (randomPoint != Vector3.zero)
                    agent.SetDestination(randomPoint);
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            if (audioSource != null && shootClip != null)
            {
                audioSource.pitch = 1f + Random.Range(-0.15f, 0.15f);
                audioSource.PlayOneShot(shootClip, 0.1f);
            }

            // Создаем пулю
            Vector3 direction = (player.position - firePoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = direction * 20f;
        }
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
            return hit.position;

        return Vector3.zero;
    }
}