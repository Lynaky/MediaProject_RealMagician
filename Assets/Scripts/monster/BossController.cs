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
    public float dashSpeedMultiplier = 3f; // 돌진 속도 배율
    public float minDashInterval = 5f; // 최소 돌진 간격
    public float maxDashInterval = 10f; // 최대 돌진 간격
    public float dashDuration = 1f; // 돌진 지속 시간

    private bool isAttacking = false;
    private bool isDashing = false;
    private Animator animator;
    private Rigidbody rb;
    private BossHealth bossHealth;
    private float originalMoveSpeed;
    public GameObject debuffEffect; // 디버프 이펙트 파티클 시스템

    private Coroutine speedDebuffCoroutine; // 현재 적용된 속도 디버프 코루틴

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        bossHealth = GetComponent<BossHealth>();
        originalMoveSpeed = moveSpeed;

        // Rigidbody Constraints 설정
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        // 디버프 이펙트 비활성화
        if (debuffEffect != null)
        {
            debuffEffect.SetActive(false);
        }

        StartCoroutine(DashRoutine()); // 돌진 패턴 시작
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

            // 애니메이션 상태 업데이트
            animator.SetBool("isWalking", distanceToPlayer > attackRange && !isDashing);
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isDashing", isDashing); // 돌진 애니메이션 상태 업데이트
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

        // 플레이어에게 데미지를 주는 로직 추가
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
            StopCoroutine(speedDebuffCoroutine); // 기존 디버프 코루틴 중지
        }
        speedDebuffCoroutine = StartCoroutine(SpeedDebuff(debuffAmount, duration));
    }

    private IEnumerator SpeedDebuff(float debuffAmount, float duration)
    {
        moveSpeed = originalMoveSpeed - debuffAmount; // 속도 디버프 갱신
        if (debuffEffect != null)
        {
            debuffEffect.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed; // 원래 속도로 복원
        if (debuffEffect != null)
        {
            debuffEffect.SetActive(false);
        }
        speedDebuffCoroutine = null; // 코루틴 참조 해제
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

        animator.SetBool("isDashing", true); // 돌진 애니메이션 시작

        while (Time.time < dashStartTime + dashDuration)
        {
            MoveTowardsPlayer(dashSpeed); // 플레이어를 추적하며 이동
            yield return null;
        }

        animator.SetBool("isDashing", false); // 돌진 애니메이션 종료

        isDashing = false;
    }
}