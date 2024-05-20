using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isAlive = true;
    private SkinnedMeshRenderer bossRenderer; // SkinnedMeshRenderer 사용
    public Material originalMaterial; // 원래 메터리얼
    public Material damageMaterial; // 피격용 메터리얼
    public float damageColorDuration = 0.5f; // 피격용 메터리얼 지속 시간
    private BossController bossController;
    public Slider healthSlider; // 보스 체력 슬라이더

    void Start()
    {
        currentHealth = maxHealth;
        bossController = GetComponent<BossController>();

        // 자식 오브젝트에서 SkinnedMeshRenderer 컴포넌트 찾기
        Transform mutantMeshTransform = transform.Find("MutantMesh");
        if (mutantMeshTransform != null)
        {
            bossRenderer = mutantMeshTransform.GetComponent<SkinnedMeshRenderer>();
            if (bossRenderer != null)
            {
                originalMaterial = bossRenderer.material;
            }
        }

        if (bossRenderer == null)
        {
            Debug.LogError("MutantMesh의 SkinnedMeshRenderer를 찾을 수 없습니다.");
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Boss Health: " + currentHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        StartCoroutine(FlashDamageMaterial());

        if (currentHealth <= 0f && isAlive)
        {
            Die();
        }
    }

    private IEnumerator FlashDamageMaterial()
    {
        if (bossRenderer != null && damageMaterial != null)
        {
            bossRenderer.material = damageMaterial; // 피격용 메터리얼로 변경
            yield return new WaitForSeconds(damageColorDuration); // 일정 시간 대기
            bossRenderer.material = originalMaterial; // 원래 메터리얼로 복원
        }
    }

    private void Die()
    {
        Debug.Log("Boss Died!");
        isAlive = false;
        // 사망 애니메이션 등 추가
        bossController.Die();
        Destroy(gameObject, 60f); // 사망 후 60초 뒤에 오브젝트 제거
    }
}
