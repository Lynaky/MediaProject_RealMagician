using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public float damageDuration = 3f; // �������� �ִ� �ð�
    public float damageAmount = 10f; // ������ ��
    public float damageInterval = 1f; // �������� �ִ� ����
    public float lifetime = 5f; // ������ ������ �����ֱ�

    private void Start()
    {
        // ������ �� 5�� �Ŀ� ������Ʈ�� ����
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
                // enemy.GetComponent<EnemyHealth>().TakeDamage(damageAmount); // ���÷� EnemyHealth ������Ʈ�� �ִٰ� ����
            }
            elapsedTime += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
