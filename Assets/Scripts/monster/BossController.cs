using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 2f;
    public float attackInterval = 2f;
    public float attackDamage = 20f;

    private bool isAttacking = false;
    private Animator animator;
    private Rigidbody rb;
    private BossHealth bossHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        bossHealth = GetComponent<BossHealth>();

        // Rigidbody Constraints ����
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
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
        // �÷��̾�� �������� �ִ� ���� �߰� (��: �÷��̾��� Health ������Ʈ�� �����Ͽ� ������ ����)
        // player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

        yield return new WaitForSeconds(attackInterval);
        isAttacking = false;
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }
}
