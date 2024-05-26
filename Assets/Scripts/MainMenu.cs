using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource; // ����� �ҽ�

    public void StartGame()
    {
        PlayClickSound();
        SceneManager.LoadScene("InGame"); // ���� ���� ��Ȯ�� �̸��� �Է��ϼ���
    }

    public void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
