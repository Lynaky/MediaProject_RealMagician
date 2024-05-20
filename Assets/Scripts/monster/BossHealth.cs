using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isAlive = true;
    private SkinnedMeshRenderer bossRenderer; // SkinnedMeshRenderer ���
    public Material originalMaterial; // ���� ���͸���
    public Material damageMaterial; // �ǰݿ� ���͸���
    public float damageColorDuration = 0.5f; // �ǰݿ� ���͸��� ���� �ð�
    private BossController bossController;
    public Slider healthSlider; // ���� ü�� �����̴�

    void Start()
    {
        currentHealth = maxHealth;
        bossController = GetComponent<BossController>();

        // �ڽ� ������Ʈ���� SkinnedMeshRenderer ������Ʈ ã��
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
            Debug.LogError("MutantMesh�� SkinnedMeshRenderer�� ã�� �� �����ϴ�.");
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
            bossRenderer.material = damageMaterial; // �ǰݿ� ���͸���� ����
            yield return new WaitForSeconds(damageColorDuration); // ���� �ð� ���
            bossRenderer.material = originalMaterial; // ���� ���͸���� ����
        }
    }

    private void Die()
    {
        Debug.Log("Boss Died!");
        isAlive = false;
        // ��� �ִϸ��̼� �� �߰�
        bossController.Die();
        Destroy(gameObject, 60f); // ��� �� 60�� �ڿ� ������Ʈ ����
    }
}
