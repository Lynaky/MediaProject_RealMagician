using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource; // 오디오 소스

    public void StartGame()
    {
        PlayClickSound();
        SceneManager.LoadScene("InGame"); // 게임 씬의 정확한 이름을 입력하세요
    }

    public void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
