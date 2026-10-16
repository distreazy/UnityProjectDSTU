using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject mainMenuPanel;

    void Start()
    {
        // Меню активно
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}