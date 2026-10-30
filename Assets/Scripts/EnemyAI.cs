using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Характеристики")]
    public float moveSpeed = 3.5f;
    public float damage = 10f;
    public float fireRate = 1.5f; // Выстрел раз в 1.5 секунды

    [Header("Стрельба и Обзор")]
    public float detectionRange = 20f; // Радиус, в котором враг видит игрока
    public float shootingRange = 12f;  // С какого расстояния начинает стрелять
    public GameObject bulletPrefab;    // Префаб пули
    public Transform firePoint;        // Откуда вылетает пуля

    private NavMeshAgent agent;
    private Transform player;
    private float currentHealth;
    private float nextFireTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        // Ищем игрока на сцене по тегу
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return; // Если игрок умер, ничего не делаем

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Если игрок в зоне видимости
        if (distanceToPlayer <= detectionRange)
        {
            // Поворачиваемся в сторону игрока
            LookAtPlayer();

            // Если игрок далеко - идем к нему
            if (distanceToPlayer > shootingRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else // Если близко - останавливаемся и стреляем
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
            // Если игрока не видно - патрулируем (просто ходим по комнате)
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
        // Плавно поворачиваем тело к игроку (по горизонтали)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Не наклоняемся вверх/вниз
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // Создаем пулю и задаем ей направление в сторону игрока
            Vector3 direction = (player.position - firePoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

            // Если у пули есть Rigidbody, толкаем её вперед
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = direction * 20f; // Скорость пули 20
        }
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        // Ищем случайную точку рядом с врагом для блуждания
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
            return hit.position;

        return Vector3.zero;
    }
}