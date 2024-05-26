using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject clearUI; // 클리어 UI 추가
    public PlayerHealth playerHealth;
    public Button restartButton;

    public AudioClip ClickSound;
    public AudioClip WinSound;
    public AudioClip LoseSound;
    private AudioSource audioSource; // 오디오 소스
    public AudioSource bgmSource; // 배경음악 오디오 소스

    void Start()
    {
        gameOverUI.SetActive(false); // 시작할 때 게임 오버 UI를 숨깁니다.
        clearUI.SetActive(false); // 시작할 때 클리어 UI를 숨깁니다.
        restartButton.onClick.AddListener(RestartGame);

        audioSource = gameObject.AddComponent<AudioSource>(); // 오디오 소스 추가
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
            bgmSource.Stop(); // 배경음악 멈추기
        }

        PlayLoseSound();
        Time.timeScale = 0f; // 게임 시간을 멈춥니다.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // 마우스 커서를 보이게 합니다.
    }

    public void GameClear()
    {
        clearUI.SetActive(true);
        if (bgmSource != null)
        {
            bgmSource.Stop(); // 배경음악 멈추기
        }


        PlayWinSound();
        Time.timeScale = 0f; // 게임 시간을 멈춥니다.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // 마우스 커서를 보이게 합니다.
    }

    public void RestartGame()
    {
        PlayClickSound();
        Time.timeScale = 1f; // 게임 시간을 다시 진행시킵니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬을 다시 로드합니다.
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
