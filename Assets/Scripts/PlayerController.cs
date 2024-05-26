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

    public GameObject AttackShortEffect; // 공격 속성 근접 마법 이펙트 오브젝트
    public GameObject SpeedShortEffect; // 속도 속성 근접 마법 이펙트 오브젝트
    public GameObject HealthShortEffect; // 체력 속성 근접 마법 이펙트 오브젝트

    public Transform cameraTransform; // 카메라 Transform을 할당
    public LayerMask layerMask; // 무시할 레이어를 설정
    public LayerMask layerMask2; // 무시할 레이어를 설정
    public float buffDuration = 5f; // 버프 지속 시간
    public float speedBuffMultiplier = 1.5f; // 속도 버프 배율
    public float speedDebuffAmount = 10f; // 속도 디버프 양
    public float speedDebuffDuration = 3f; // 속도 디버프 지속 시간

    private PlayerHealth playerHealth;
    private Coroutine currentBuffCoroutine;
    private PlayerMovement playerMovement;

    private enum BuffType { None, Attack, Speed, Health }
    private BuffType activeBuff = BuffType.None;

    public AudioClip LongAttackSound; // 마법진 여는 사운드
    public AudioClip LongSpeedSound; // 점을 연결할 때 사운드
    public AudioClip LongHealthSound; // 점을 연결할 때 사운드
    public AudioClip BuffAttackSound; // 마법진 여는 사운드
    public AudioClip BuffSpeedSound; // 점을 연결할 때 사운드
    public AudioClip BuffHealthSound; // 점을 연결할 때 사운드
    public AudioClip ShortAttackSound; // 마법진 여는 사운드
    public AudioClip ShortSpeedSound; // 점을 연결할 때 사운드
    public AudioClip ShortHealthSound; // 점을 연결할 때 사운드
    private AudioSource audioSource; // 오디오 소스

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

        if (AttackShortEffect != null)
        {
            AttackShortEffect.SetActive(false); // 시작 시 근접 마법 이펙트 비활성화
        }

        if (SpeedShortEffect != null)
        {
            SpeedShortEffect.SetActive(false); // 시작 시 근접 마법 이펙트 비활성화
        }

        if (HealthShortEffect != null)
        {
            HealthShortEffect.SetActive(false); // 시작 시 근접 마법 이펙트 비활성화
        }

        audioSource = gameObject.AddComponent<AudioSource>(); // 오디오 소스 추가
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; // 게임 오버 또는 클리어 상태에서 업데이트 중지

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
            PlayLongAttackSound();
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
            PlayLongSpeedSound();
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
            PlayLongHealthSound();
        }
    }

    public void CastAttackSpellBuff()
    {
        Debug.Log("공격 속성 버프 마법 사용!");
        PlayBuffAttackSound();
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
        PlayBuffSpeedSound();
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
        PlayBuffHealthSound();
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

    public void CastAttackSpellShort()
    {
        Debug.Log("공격 속성 근접 마법 사용!");
        PlayShortAttackSound();

        if (AttackShortEffect != null)
        {
            AttackShortEffect.SetActive(true); // 근접 마법 이펙트 활성화
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6.5f, layerMask2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                BossHealth bossHealth = hitCollider.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(60); // 보스에게 데미지를 줌
                }
            }
        }

        StartCoroutine(DisableAttackShortEffect());
    }

    private IEnumerator DisableAttackShortEffect()
    {
        yield return new WaitForSeconds(1f); // 이펙트 지속 시간
        if (AttackShortEffect != null)
        {
            AttackShortEffect.SetActive(false); // 근접 마법 이펙트 비활성화
        }
    }

    public void CastSpeedSpellShort()
    {
        Debug.Log("속도 속성 근접 마법 사용!");
        PlayShortSpeedSound();

        if (SpeedShortEffect != null)
        {
            SpeedShortEffect.SetActive(true); // 근접 마법 이펙트 활성화
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6.5f, layerMask2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                BossHealth bossHealth = hitCollider.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(30); // 보스에게 데미지를 줌
                    BossController bossController = hitCollider.GetComponent<BossController>();
                    if (bossController != null)
                    {
                        bossController.ApplySpeedDebuff(speedDebuffAmount, speedDebuffDuration); // 보스에게 속도 디버프 적용
                    }
                }
            }
        }

        StartCoroutine(DisableSpeedShortEffect());
    }

    private IEnumerator DisableSpeedShortEffect()
    {
        yield return new WaitForSeconds(1f); // 이펙트 지속 시간
        if (SpeedShortEffect != null)
        {
            SpeedShortEffect.SetActive(false); // 근접 마법 이펙트 비활성화
        }
    }

    public void CastHealthSpellShort()
    {
        Debug.Log("체력 속성 근접 마법 사용!");
        PlayShortHealthSound();

        if (HealthShortEffect != null)
        {
            HealthShortEffect.SetActive(true); // 근접 마법 이펙트 활성화
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6.5f, layerMask2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                BossHealth bossHealth = hitCollider.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    float damage = 30; // 보스에게 줄 데미지
                    bossHealth.TakeDamage(damage); // 보스에게 데미지를 줌
                    playerHealth.Heal(damage * 0.5f); // 데미지의 절반만큼 플레이어를 회복
                }
            }
        }

        StartCoroutine(DisableHealthShortEffect());
    }

    private IEnumerator DisableHealthShortEffect()
    {
        yield return new WaitForSeconds(1f); // 이펙트 지속 시간
        if (HealthShortEffect != null)
        {
            HealthShortEffect.SetActive(false); // 근접 마법 이펙트 비활성화
        }
    }

    private void PlayLongAttackSound()
    {
        if (LongAttackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(LongAttackSound); 
        }
    }

    private void PlayLongSpeedSound()
    {
        if (LongSpeedSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(LongSpeedSound); 
        }
    }

    private void PlayLongHealthSound()
    {
        if (LongSpeedSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(LongHealthSound);
        }
    }

    private void PlayBuffHealthSound()
    {
        if (BuffHealthSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(BuffHealthSound);
        }
    }
    private void PlayBuffAttackSound()
    {
        if (BuffAttackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(BuffAttackSound);
        }
    }
    private void PlayBuffSpeedSound()
    {
        if (BuffSpeedSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(BuffSpeedSound);
        }
    }

    private void PlayShortAttackSound()
    {
        if (ShortAttackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(ShortAttackSound);
        }
    }
    private void PlayShortSpeedSound()
    {
        if (ShortSpeedSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(ShortSpeedSound);
        }
    }
    private void PlayShortHealthSound()
    {
        if (ShortHealthSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(ShortHealthSound);
        }
    }
}
