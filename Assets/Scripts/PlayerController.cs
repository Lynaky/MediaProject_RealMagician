using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private SpellbookSystem spellbookSystem;
    private MagicCircleUI magicCircleUI;
    private MouseLook mouseLook;

    public GameObject damageAreaPrefab; // 데미지 장판 Prefab을 할당
    public GameObject speedDebuffAreaPrefab; // 속도 감소 장판 Prefab을 할당
    public GameObject healingAreaPrefab; // 체력 회복 장판 Prefab을 할당
    public GameObject AttackbuffEffect; // 공격 버프 이펙트 오브젝트
    public GameObject SpeedbuffEffect; // 속도 버프 이펙트 오브젝트
    public GameObject HealthbuffEffect; // 체력 버프 이펙트 오브젝트
    public Transform cameraTransform; // 카메라 Transform을 할당
    public LayerMask layerMask; // 무시할 레이어를 설정
    public float buffDuration = 5f; // 버프 지속 시간
    public float speedBuffMultiplier = 1.5f; // 속도 버프 배율

    private PlayerHealth playerHealth;
    private Coroutine currentBuffCoroutine;
    private PlayerMovement playerMovement;

    private enum BuffType { None, Attack, Speed, Health }
    private BuffType activeBuff = BuffType.None;

    void Start()
    {
        spellbookSystem = GetComponent<SpellbookSystem>();
        if (spellbookSystem == null)
        {
            spellbookSystem = GetComponentInChildren<SpellbookSystem>();
        }

        magicCircleUI = GetComponent<MagicCircleUI>();
        if (magicCircleUI == null)
        {
            magicCircleUI = GetComponentInChildren<MagicCircleUI>();
        }

        mouseLook = GetComponent<MouseLook>();
        if (mouseLook == null)
        {
            mouseLook = GetComponentInChildren<MouseLook>();
        }

        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();

        if (AttackbuffEffect != null)
        {
            AttackbuffEffect.SetActive(false); // 시작 시 버프 이펙트 비활성화
        }

        if (SpeedbuffEffect != null)
        {
            SpeedbuffEffect.SetActive(false); // 시작 시 버프 이펙트 비활성화
        }

        if (HealthbuffEffect != null)
        {
            HealthbuffEffect.SetActive(false); // 시작 시 버프 이펙트 비활성화
        }
    }

    void Update()
    {
        if (spellbookSystem != null)
        {
            spellbookSystem.HandleSpellbookSwitch();
        }

        if (spellbookSystem != null && spellbookSystem.currentSpellbook != SpellbookType.None)
        {
            if (magicCircleUI != null)
            {
                magicCircleUI.HandleMagicCircle();
                mouseLook.SetEnabled(!magicCircleUI.IsDrawing()); // 마법진 UI 활성화 시 MouseLook 비활성화
            }
        }
    }

    public void CastAttackSpellLong()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 50f, layerMask))
        {
            Vector3 spawnPosition = hit.point;
            Instantiate(damageAreaPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("공격 속성 원거리 마법 사용!");
        }
    }

    public void CastSpeedSpellLong()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 50f, layerMask))
        {
            Vector3 spawnPosition = hit.point;
            GameObject speedDebuffArea = Instantiate(speedDebuffAreaPrefab, spawnPosition, Quaternion.identity);
            DamageArea damageArea = speedDebuffArea.GetComponent<DamageArea>();
            if (damageArea != null)
            {
                damageArea.isSpeedDebuff = true;
            }
            Debug.Log("속도 속성 원거리 마법 사용!");
        }
    }

    public void CastHealthSpellLong()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 50f, layerMask))
        {
            Vector3 spawnPosition = hit.point;
            GameObject healingArea = Instantiate(healingAreaPrefab, spawnPosition, Quaternion.identity);
            DamageArea damageArea = healingArea.GetComponent<DamageArea>();
            if (damageArea != null)
            {
                damageArea.isHealingSpell = true;
                damageArea.playerHealth = playerHealth;
            }
            Debug.Log("체력 속성 원거리 마법 사용!");
        }
    }

    public void CastAttackSpellBuff()
    {
        Debug.Log("공격 속성 버프 마법 사용!");
        if (activeBuff != BuffType.None)
        {
            StopActiveBuff();
        }
        currentBuffCoroutine = StartCoroutine(ApplyAttackBuff());
    }

    private IEnumerator ApplyAttackBuff()
    {
        activeBuff = BuffType.Attack;
        if (AttackbuffEffect != null)
        {
            AttackbuffEffect.SetActive(true); // 버프 이펙트 활성화
        }

        float originalDamageMultiplier = DamageArea.damageMultiplier; // DamageArea 스크립트의 damageMultiplier 변수를 사용한다고 가정
        DamageArea.damageMultiplier = 1.5f; // 데미지 1.5배 증가

        yield return new WaitForSeconds(buffDuration); // 버프 지속 시간 동안 대기

        DamageArea.damageMultiplier = originalDamageMultiplier; // 원래 데미지로 복원

        if (AttackbuffEffect != null)
        {
            AttackbuffEffect.SetActive(false); // 버프 이펙트 비활성화
        }

        activeBuff = BuffType.None;
        currentBuffCoroutine = null; // 코루틴 참조 해제
    }

    public void CastSpeedSpellBuff()
    {
        Debug.Log("속도 속성 버프 마법 사용!");
        if (activeBuff != BuffType.None)
        {
            StopActiveBuff();
        }
        currentBuffCoroutine = StartCoroutine(ApplySpeedBuff());
    }

    private IEnumerator ApplySpeedBuff()
    {
        activeBuff = BuffType.Speed;
        if (SpeedbuffEffect != null)
        {
            SpeedbuffEffect.SetActive(true); // 버프 이펙트 활성화
        }

        float originalSpeed = playerMovement.speed;
        playerMovement.speed *= speedBuffMultiplier; // 속도 증가

        yield return new WaitForSeconds(buffDuration); // 버프 지속 시간 동안 대기

        playerMovement.speed = originalSpeed; // 원래 속도로 복원

        if (SpeedbuffEffect != null)
        {
            SpeedbuffEffect.SetActive(false); // 버프 이펙트 비활성화
        }

        activeBuff = BuffType.None;
        currentBuffCoroutine = null; // 코루틴 참조 해제
    }

    public void CastHealthSpellBuff()
    {
        Debug.Log("체력 속성 버프 마법 사용!");
        if (activeBuff != BuffType.None)
        {
            StopActiveBuff();
        }
        currentBuffCoroutine = StartCoroutine(ApplyHealthBuff());
    }

    private IEnumerator ApplyHealthBuff()
    {
        activeBuff = BuffType.Health;
        if (HealthbuffEffect != null)
        {
            HealthbuffEffect.SetActive(true); // 버프 이펙트 활성화
        }

        for (int i = 0; i < 5; i++)
        {
            playerHealth.Heal(3);
            yield return new WaitForSeconds(1f); // 1초 간격으로 5번 회복
        }

        if (HealthbuffEffect != null)
        {
            HealthbuffEffect.SetActive(false); // 버프 이펙트 비활성화
        }

        activeBuff = BuffType.None;
        currentBuffCoroutine = null; // 코루틴 참조 해제
    }

    private void StopActiveBuff()
    {
        if (currentBuffCoroutine != null)
        {
            StopCoroutine(currentBuffCoroutine);

            // 현재 적용 중인 버프 이펙트를 비활성화
            if (AttackbuffEffect != null && activeBuff == BuffType.Attack)
            {
                AttackbuffEffect.SetActive(false);
            }

            if (SpeedbuffEffect != null && activeBuff == BuffType.Speed)
            {
                SpeedbuffEffect.SetActive(false);
            }

            if (HealthbuffEffect != null && activeBuff == BuffType.Health)
            {
                HealthbuffEffect.SetActive(false);
            }

            // 원래 상태로 복원
            if (activeBuff == BuffType.Attack)
            {
                DamageArea.damageMultiplier = 1f; // 원래 데미지로 복원
            }
            else if (activeBuff == BuffType.Speed)
            {
                playerMovement.speed = playerMovement.speed / speedBuffMultiplier; // 원래 속도로 복원
            }

            activeBuff = BuffType.None;
            currentBuffCoroutine = null;
        }
    }
}
