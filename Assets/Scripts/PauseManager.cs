using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public MonoBehaviour mouseLook;

    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver()) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (mouseLook) mouseLook.enabled = false;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (mouseLook) mouseLook.enabled = true;
    }

    public void BackToMainMenu()
    {
        // Обязательно возвращаем нормальное время!
        // Если этого не сделать, главное меню загрузится с timeScale = 0, и кнопки там не будут работать.
        Time.timeScale = 1f;

        // Загружаем сцену главного меню (убедись, что имя точно совпадает)
        SceneManager.LoadScene("MenuScene");
    }
}