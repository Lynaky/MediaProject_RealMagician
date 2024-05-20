using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public float damageDuration = 3f; // 데미지를 주는 시간
    public float damageAmount = 10f; // 데미지 양
    public float damageInterval = 1f; // 데미지를 주는 간격
    public float lifetime = 5f; // 데미지 장판의 생명주기

    private void Start()
    {
        // 생성된 후 5초 후에 오브젝트를 제거
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            StartCoroutine(DamageOverTime(other));
        }
    }

    private IEnumerator DamageOverTime(Collider enemy)
    {
        float elapsedTime = 0f;
        while (elapsedTime < damageDuration)
        {
            if (enemy != null)
            {
                Debug.Log("Damaging enemy: " + enemy.name);
                if (enemy.CompareTag("Boss"))
                {
                    enemy.GetComponent<BossHealth>().TakeDamage(damageAmount);
                }
                // enemy.GetComponent<EnemyHealth>().TakeDamage(damageAmount); // 예시로 EnemyHealth 컴포넌트가 있다고 가정
            }
            elapsedTime += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
