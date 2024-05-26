using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public static float damageMultiplier = 1f; // damageMultiplier를 static 변수로 선언

    public float damageDuration = 3f; // 데미지를 주는 시간
    public float damageAmount = 10f; // 데미지 양
    public float damageInterval = 1f; // 데미지를 주는 간격
    public float lifetime = 5f; // 데미지 장판의 생명주기
    public bool isSpeedDebuff; // 속도 감소 스킬 여부
    public float speedDebuffAmount = 10f; // 속도 감소량
    public float speedDebuffDuration = 3f; // 속도 감소 지속 시간
    public bool isHealingSpell; // 체력 회복 스킬 여부
    public PlayerHealth playerHealth; // 플레이어의 체력

    private HashSet<Collider> affectedEnemies = new HashSet<Collider>(); // 이미 충돌한 적들을 저장하는 집합

    private void Start()
    {
        // 생성된 후 lifetime 시간 후에 오브젝트를 제거
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
                        bossHealth.TakeDamage(damageAmount * damageMultiplier); // 데미지에 multiplier 적용

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