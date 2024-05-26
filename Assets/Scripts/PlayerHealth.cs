using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider; // UI �����̴��� ���� ü���� �ð������� ǥ��

    public List<SkinnedMeshRenderer> playerRenderers = new List<SkinnedMeshRenderer>(); // ���� SkinnedMeshRenderer�� ���� ����Ʈ
    public Material damageMaterial; // �ǰݿ� ���͸���
    public float damageColorDuration = 0.5f; // �ǰݿ� ���͸��� ���� �ð�

    private Dictionary<SkinnedMeshRenderer, Material> originalMaterials = new Dictionary<SkinnedMeshRenderer, Material>(); // ���� ���͸����� �����ϴ� ��ųʸ�
    private Animator animator;
    
    public AudioClip HittedSound; // ���� ������ �� ����
    private AudioSource audioSource; // ����� �ҽ�
    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // �� SkinnedMeshRenderer�� ���� ���͸��� ����
        foreach (var renderer in playerRenderers)
        {
            if (renderer != null)
            {
                originalMaterials[renderer] = renderer.material;
            }
        }

        if (playerRenderers.Count == 0)
        {
            Debug.LogError("PlayerMesh�� SkinnedMeshRenderer�� ã�� �� �����ϴ�.");
        }

        animator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>(); // ����� �ҽ� �߰�
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        Debug.Log("Player Health: " + currentHealth);
        PlayHittedSound();

        StartCoroutine(FlashDamageMaterial());

        if (currentHealth <= 0f)
        {
            StartCoroutine(Die()); // �ڷ�ƾ���� ����
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // ü���� �ִ� ü������ ����
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        Debug.Log("Player Healed: " + healAmount + ", Current Health: " + currentHealth);
    }

    private IEnumerator FlashDamageMaterial()
    {
        // �� SkinnedMeshRenderer�� ���͸����� �ǰݿ� ���͸���� ����
        foreach (var renderer in playerRenderers)
        {
            if (renderer != null)
            {
                renderer.material = damageMaterial;
            }
        }

        yield return new WaitForSeconds(damageColorDuration); // ���� �ð� ���

        // �� SkinnedMeshRenderer�� ���͸����� ���� ���͸���� ����
        foreach (var renderer in playerRenderers)
        {
            if (renderer != null && originalMaterials.ContainsKey(renderer))
            {
                renderer.material = originalMaterials[renderer];
            }
        }
    }

    private IEnumerator Die()
    {
        Debug.Log("Player Died!");
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(3.667f); // �״� �ִϸ��̼� ��� �ð��� Ȯ��

        // ���� ���� ó�� �߰�
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.GameOver();
        }

        // �÷��̾� ��� ���� �߰� (��: ���� ���� ȭ�� ǥ�� ��)
    }

    private void PlayHittedSound()
    {
        if (HittedSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(HittedSound);
        }
    }
}
