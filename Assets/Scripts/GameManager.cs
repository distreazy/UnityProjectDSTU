using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Нужно для работы с текстом

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Синглтон, чтобы враги могли легко его найти

    [Header("Цели")]
    public int killsToWin = 15;
    private int currentKills = 0;
    private bool isGameOver = false;

    [Header("Ссылки на UI")]
    public TextMeshProUGUI killsText;
    public GameObject endGamePanel;
    public TextMeshProUGUI endGameText;

    [Header("Ссылки на Игрока")]
    public PlayerStats playerStats;

    void Awake()
    {
        // Настраиваем Синглтон
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        isGameOver = false;
        currentKills = 0;
        UpdateKillsUI();
        endGamePanel.SetActive(false); // Скрываем экран конца игры
    }

    void Update()
    {
        // Проверяем смерть игрока
        if (playerStats != null && playerStats.CurrentHealth <= 0 && !isGameOver)
        {
            LoseGame();
        }
    }

    // Вызывается из скрипта врага при смерти
    public void OnEnemyKilled()
    {
        if (isGameOver) return;

        currentKills++;
        UpdateKillsUI();

        if (currentKills >= killsToWin)
        {
            WinGame();
        }
    }

    void UpdateKillsUI()
    {
        if (killsText != null)
        {
            killsText.text = $"Убито: {currentKills} / {killsToWin}";
        }
    }

    void WinGame()
    {
        isGameOver = true;
        Time.timeScale = 0f; // Останавливаем игру
        endGamePanel.SetActive(true);
        endGameText.text = "VICTORY";
        endGameText.color = Color.green;

        // Освобождаем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LoseGame()
    {
        isGameOver = true;
        Time.timeScale = 0f; // Останавливаем игру
        endGamePanel.SetActive(true);
        endGameText.text = "YOU LOOSE";
        endGameText.color = Color.red;

        // Освобождаем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Метод для кнопки перезапуска
    public void RestartGame()
    {
        Time.timeScale = 1f; // Обязательно возвращаем время!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезагружаем текущую сцену
    }

    // Метод для кнопки выхода в главное меню
    public void GoToMainMenu()
    {
        // Обязательно возвращаем время в нормальное состояние!
        // Иначе главное меню загрузится с замороженным временем (timeScale = 0)
        Time.timeScale = 1f;

        // Загружаем сцену главного меню. 
        // Убедись, что сцена называется именно "MainMenu"
        SceneManager.LoadScene("MenuScene");
    }

    // Этот метод нужен, чтобы другие скрипты могли узнать, закончилась ли игра
    public bool IsGameOver()
    {
        return isGameOver;
    }
}