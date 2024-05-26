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
    public float dashSpeedMultiplier = 3f; // ���� �ӵ� ����
    public float minDashInterval = 5f; // �ּ� ���� ����
    public float maxDashInterval = 10f; // �ִ� ���� ����
    public float dashDuration = 1f; // ���� ���� �ð�

    private bool isAttacking = false;
    private bool isDashing = false;
    private Animator animator;
    private Rigidbody rb;
    private BossHealth bossHealth;
    private float originalMoveSpeed;
    public GameObject debuffEffect; // ����� ����Ʈ ��ƼŬ �ý���

    private Coroutine speedDebuffCoroutine; // ���� ����� �ӵ� ����� �ڷ�ƾ

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

        StartCoroutine(DashRoutine()); // ���� ���� ����
    }

    void Update()
    {
        if (player != null && bossHealth.IsAlive())
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > attackRange && !isDashing)
            {
                MoveTowardsPlayer(moveSpeed);
            }
            else if (!isAttacking && !isDashing)
            {
                StartCoroutine(AttackPlayer());
            }

            // �ִϸ��̼� ���� ������Ʈ
            animator.SetBool("isWalking", distanceToPlayer > attackRange && !isDashing);
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isDashing", isDashing); // ���� �ִϸ��̼� ���� ������Ʈ
        }
    }

    void MoveTowardsPlayer(float speed)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
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
        if (speedDebuffCoroutine != null)
        {
            StopCoroutine(speedDebuffCoroutine); // ���� ����� �ڷ�ƾ ����
        }
        speedDebuffCoroutine = StartCoroutine(SpeedDebuff(debuffAmount, duration));
    }

    private IEnumerator SpeedDebuff(float debuffAmount, float duration)
    {
        moveSpeed = originalMoveSpeed - debuffAmount; // �ӵ� ����� ����
        if (debuffEffect != null)
        {
            debuffEffect.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed; // ���� �ӵ��� ����
        if (debuffEffect != null)
        {
            debuffEffect.SetActive(false);
        }
        speedDebuffCoroutine = null; // �ڷ�ƾ ���� ����
    }

    private IEnumerator DashRoutine()
    {
        while (bossHealth.IsAlive())
        {
            float waitTime = Random.Range(minDashInterval, maxDashInterval);
            yield return new WaitForSeconds(waitTime);

            StartCoroutine(DashTowardsPlayer());
        }
    }

    private IEnumerator DashTowardsPlayer()
    {
        isDashing = true;
        float dashStartTime = Time.time;
        float dashSpeed = originalMoveSpeed * dashSpeedMultiplier;

        animator.SetBool("isDashing", true); // ���� �ִϸ��̼� ����

        while (Time.time < dashStartTime + dashDuration)
        {
            MoveTowardsPlayer(dashSpeed); // �÷��̾ �����ϸ� �̵�
            yield return null;
        }

        animator.SetBool("isDashing", false); // ���� �ִϸ��̼� ����

        isDashing = false;
    }
}