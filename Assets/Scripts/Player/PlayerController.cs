using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Stamina")]
    [Tooltip("Сколько стамины тратится при беге")]
    public float staminaDrainPerSecond = 20f;

    [Header("References")]
    public PlayerStats playerStats;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Если игра на паузе
        if (Time.timeScale == 0f) return;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Ввод WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        // Нормализация вектора чтобы не было бага диагонального бега
        move = Vector3.ClampMagnitude(move, 1f);

        // Проверка бега
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool canRun = playerStats == null || playerStats.HasStamina;

        bool isRunning = wantsToRun && canRun && move.sqrMagnitude > 0.01f;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Движение
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Прыжок 
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            bool canJump = playerStats == null ||
                        playerStats.TrySpendStamina(playerStats.jumpStaminaCost);

            if (canJump)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Гравитация
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Тратим стамину при беге
        if (playerStats != null)
        {
            if (isRunning)
                playerStats.DrainStamina(staminaDrainPerSecond * Time.deltaTime);
            else
                playerStats.RegenerateStamina(Time.deltaTime);
        }
    }
}