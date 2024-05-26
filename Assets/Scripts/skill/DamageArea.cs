using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public static float damageMultiplier = 1f; // damageMultiplier�� static ������ ����

    public float damageDuration = 3f; // �������� �ִ� �ð�
    public float damageAmount = 10f; // ������ ��
    public float damageInterval = 1f; // �������� �ִ� ����
    public float lifetime = 5f; // ������ ������ �����ֱ�
    public bool isSpeedDebuff; // �ӵ� ���� ��ų ����
    public float speedDebuffAmount = 10f; // �ӵ� ���ҷ�
    public float speedDebuffDuration = 3f; // �ӵ� ���� ���� �ð�
    public bool isHealingSpell; // ü�� ȸ�� ��ų ����
    public PlayerHealth playerHealth; // �÷��̾��� ü��

    private HashSet<Collider> affectedEnemies = new HashSet<Collider>(); // �̹� �浹�� ������ �����ϴ� ����

    private void Start()
    {
        // ������ �� lifetime �ð� �Ŀ� ������Ʈ�� ����
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss") && !affectedEnemies.Contains(other))
        {
            affectedEnemies.Add(other);
            StartCoroutine(DamageOverTime(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (affectedEnemies.Contains(other))
        {
            affectedEnemies.Remove(other);
        }
    }

    private IEnumerator DamageOverTime(Collider enemy)
    {
        float elapsedTime = 0f;
        while (elapsedTime < damageDuration && affectedEnemies.Contains(enemy))
        {
            if (enemy != null)
            {
                Debug.Log("Damaging enemy: " + enemy.name);
                if (enemy.CompareTag("Boss"))
                {
                    BossHealth bossHealth = enemy.GetComponent<BossHealth>();
                    if (bossHealth != null)
                    {
                        bossHealth.TakeDamage(damageAmount * damageMultiplier); // �������� multiplier ����

                        if (isHealingSpell && playerHealth != null)
                        {
                            float healAmount = (damageAmount * damageMultiplier) / 2;
                            playerHealth.Heal(healAmount);
                        }
                    }

                    if (isSpeedDebuff)
                    {
                        BossController bossController = enemy.GetComponent<BossController>();
                        if (bossController != null)
                        {
                            bossController.ApplySpeedDebuff(speedDebuffAmount, speedDebuffDuration);
                        }
                    }
                }
            }
            elapsedTime += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }
    }
}