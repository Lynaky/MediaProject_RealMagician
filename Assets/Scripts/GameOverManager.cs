using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject clearUI; // Ŭ���� UI �߰�
    public PlayerHealth playerHealth;
    public Button restartButton;

    void Start()
    {
        gameOverUI.SetActive(false); // ������ �� ���� ���� UI�� ����ϴ�.
        clearUI.SetActive(false); // ������ �� Ŭ���� UI�� ����ϴ�.
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
        Time.timeScale = 0f; // ���� �ð��� ����ϴ�.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // ���콺 Ŀ���� ���̰� �մϴ�.
    }

    public void GameClear()
    {
        clearUI.SetActive(true);
        Time.timeScale = 0f; // ���� �ð��� ����ϴ�.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // ���콺 Ŀ���� ���̰� �մϴ�.
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // ���� �ð��� �ٽ� �����ŵ�ϴ�.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� ���� �ٽ� �ε��մϴ�.
    }
}
