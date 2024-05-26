using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider; // UI 슬라이더를 통해 체력을 시각적으로 표시

    public List<SkinnedMeshRenderer> playerRenderers = new List<SkinnedMeshRenderer>(); // 여러 SkinnedMeshRenderer를 담을 리스트
    public Material damageMaterial; // 피격용 메터리얼
    public float damageColorDuration = 0.5f; // 피격용 메터리얼 지속 시간

    private Dictionary<SkinnedMeshRenderer, Material> originalMaterials = new Dictionary<SkinnedMeshRenderer, Material>(); // 원래 메터리얼을 저장하는 딕셔너리
    private Animator animator;
    
    public AudioClip HittedSound; // 점을 연결할 때 사운드
    private AudioSource audioSource; // 오디오 소스
    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // 각 SkinnedMeshRenderer의 원래 메터리얼 저장
        foreach (var renderer in playerRenderers)
        {
            if (renderer != null)
            {
                originalMaterials[renderer] = renderer.material;
            }
        }

        if (playerRenderers.Count == 0)
        {
            Debug.LogError("PlayerMesh의 SkinnedMeshRenderer를 찾을 수 없습니다.");
        }

        animator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>(); // 오디오 소스 추가
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
            StartCoroutine(Die()); // 코루틴으로 변경
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // 체력을 최대 체력으로 제한
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        Debug.Log("Player Healed: " + healAmount + ", Current Health: " + currentHealth);
    }

    private IEnumerator FlashDamageMaterial()
    {
        // 각 SkinnedMeshRenderer의 메터리얼을 피격용 메터리얼로 변경
        foreach (var renderer in playerRenderers)
        {
            if (renderer != null)
            {
                renderer.material = damageMaterial;
            }
        }

        yield return new WaitForSeconds(damageColorDuration); // 일정 시간 대기

        // 각 SkinnedMeshRenderer의 메터리얼을 원래 메터리얼로 복원
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

        yield return new WaitForSeconds(3.667f); // 죽는 애니메이션 재생 시간을 확보

        // 게임 오버 처리 추가
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.GameOver();
        }

        // 플레이어 사망 로직 추가 (예: 게임 오버 화면 표시 등)
    }

    private void PlayHittedSound()
    {
        if (HittedSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(HittedSound);
        }
    }
}
