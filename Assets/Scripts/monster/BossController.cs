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
    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        bossHealth = GetComponent<BossHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();

        // Rigidbody Constraints 설정
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

            // 애니메이션 상태 업데이트
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

        // 플레이어에게 데미지를 주는 로직 추가
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(attackInterval);
        isAttacking = false;
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }
}
