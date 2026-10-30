using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        isGameOver = false;
        currentKills = 0;
        UpdateKillsUI();
        endGamePanel.SetActive(false);
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
        Time.timeScale = 0f;
        endGamePanel.SetActive(true);
        endGameText.text = "VICTORY";
        endGameText.color = Color.green;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LoseGame()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        endGamePanel.SetActive(true);
        endGameText.text = "YOU LOOSE";
        endGameText.color = Color.red;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuScene");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}