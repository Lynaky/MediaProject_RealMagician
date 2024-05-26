using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject clearUI; // Ŭ���� UI �߰�
    public PlayerHealth playerHealth;
    public Button restartButton;

    public AudioClip ClickSound;
    public AudioClip WinSound;
    public AudioClip LoseSound;
    private AudioSource audioSource; // ����� �ҽ�
    public AudioSource bgmSource; // ������� ����� �ҽ�

    void Start()
    {
        gameOverUI.SetActive(false); // ������ �� ���� ���� UI�� ����ϴ�.
        clearUI.SetActive(false); // ������ �� Ŭ���� UI�� ����ϴ�.
        restartButton.onClick.AddListener(RestartGame);

        audioSource = gameObject.AddComponent<AudioSource>(); // ����� �ҽ� �߰�
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
        if (bgmSource != null)
        {
            bgmSource.Stop(); // ������� ���߱�
        }

        PlayLoseSound();
        Time.timeScale = 0f; // ���� �ð��� ����ϴ�.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // ���콺 Ŀ���� ���̰� �մϴ�.
    }

    public void GameClear()
    {
        clearUI.SetActive(true);
        if (bgmSource != null)
        {
            bgmSource.Stop(); // ������� ���߱�
        }


        PlayWinSound();
        Time.timeScale = 0f; // ���� �ð��� ����ϴ�.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // ���콺 Ŀ���� ���̰� �մϴ�.
    }

    public void RestartGame()
    {
        PlayClickSound();
        Time.timeScale = 1f; // ���� �ð��� �ٽ� �����ŵ�ϴ�.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� ���� �ٽ� �ε��մϴ�.
    }

    private void PlayWinSound()
    {
        if (WinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(WinSound);
        }
    }

    private void PlayLoseSound()
    {
        if (LoseSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(LoseSound);
        }
    }

    private void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(ClickSound);
        }
    }
}
