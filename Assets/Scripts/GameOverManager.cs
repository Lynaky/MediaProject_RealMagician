using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject clearUI; // 클리어 UI 추가
    public PlayerHealth playerHealth;
    public Button restartButton;

    void Start()
    {
        gameOverUI.SetActive(false); // 시작할 때 게임 오버 UI를 숨깁니다.
        clearUI.SetActive(false); // 시작할 때 클리어 UI를 숨깁니다.
        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (playerHealth.currentHealth <= 0 && !gameOverUI.activeInHierarchy)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // 게임 시간을 멈춥니다.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // 마우스 커서를 보이게 합니다.
    }

    public void GameClear()
    {
        clearUI.SetActive(true);
        Time.timeScale = 0f; // 게임 시간을 멈춥니다.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // 마우스 커서를 보이게 합니다.
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // 게임 시간을 다시 진행시킵니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬을 다시 로드합니다.
    }
}
