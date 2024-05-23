using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 30f;
    public float attackRange = 5f;
    public float attackInterval = 2.667f;
    public float attackDamage = 20f;

    private bool isAttacking = false;
    private Animator animator;
    private Rigidbody rb;
    private BossHealth bossHealth;
    private float originalMoveSpeed;
    public GameObject debuffEffect; // ����� ����Ʈ ��ƼŬ �ý���

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        bossHealth = GetComponent<BossHealth>();
        originalMoveSpeed = moveSpeed;

        // Rigidbody Constraints ����
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        // ����� ����Ʈ ��Ȱ��ȭ
        if (debuffEffect != null)
        {
            debuffEffect.SetActive(false);
        }
    }

    void Update()
    {
        if (player != null && bossHealth.IsAlive())
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }

            // �ִϸ��̼� ���� ������Ʈ
            animator.SetBool("isWalking", distanceToPlayer > attackRange);
            animator.SetBool("isAttacking", isAttacking);
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        transform.LookAt(player);
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        Debug.Log("Boss attacking player...");

        // �÷��̾�� �������� �ִ� ���� �߰�
        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

        yield return new WaitForSeconds(attackInterval);
        isAttacking = false;
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }

    public void ApplySpeedDebuff(float debuffAmount, float duration)
    {
        StartCoroutine(SpeedDebuff(debuffAmount, duration));
    }

    private IEnumerator SpeedDebuff(float debuffAmount, float duration)
    {
        moveSpeed -= debuffAmount;
        if (debuffEffect != null)
        {
            debuffEffect.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed;
        if (debuffEffect != null)
        {
            debuffEffect.SetActive(false);
        }
    }
}
