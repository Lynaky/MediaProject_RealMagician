using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private SpellbookSystem spellbookSystem;
    private MagicCircleUI magicCircleUI;
    private MouseLook mouseLook;

    public GameObject damageAreaPrefab; // ������ ���� Prefab�� �Ҵ�
    public GameObject speedDebuffAreaPrefab; // �ӵ� ���� ���� Prefab�� �Ҵ�
    public GameObject healingAreaPrefab; // ü�� ȸ�� ���� Prefab�� �Ҵ�

    public GameObject AttackbuffEffect; // ���� ���� ����Ʈ ������Ʈ
    public GameObject SpeedbuffEffect; // �ӵ� ���� ����Ʈ ������Ʈ
    public GameObject HealthbuffEffect; // ü�� ���� ����Ʈ ������Ʈ

    public GameObject AttackShortEffect; // ���� �Ӽ� ���� ���� ����Ʈ ������Ʈ
    public GameObject SpeedShortEffect; // �ӵ� �Ӽ� ���� ���� ����Ʈ ������Ʈ
    public GameObject HealthShortEffect; // ü�� �Ӽ� ���� ���� ����Ʈ ������Ʈ

    public Transform cameraTransform; // ī�޶� Transform�� �Ҵ�
    public LayerMask layerMask; // ������ ���̾ ����
    public LayerMask layerMask2; // ������ ���̾ ����
    public float buffDuration = 5f; // ���� ���� �ð�
    public float speedBuffMultiplier = 1.5f; // �ӵ� ���� ����
    public float speedDebuffAmount = 10f; // �ӵ� ����� ��
    public float speedDebuffDuration = 3f; // �ӵ� ����� ���� �ð�

    private PlayerHealth playerHealth;
    private Coroutine currentBuffCoroutine;
    private PlayerMovement playerMovement;

    private enum BuffType { None, Attack, Speed, Health }
    private BuffType activeBuff = BuffType.None;

    public AudioClip LongAttackSound; // ������ ���� ����
    public AudioClip LongSpeedSound; // ���� ������ �� ����
    public AudioClip LongHealthSound; // ���� ������ �� ����
    public AudioClip BuffAttackSound; // ������ ���� ����
    public AudioClip BuffSpeedSound; // ���� ������ �� ����
    public AudioClip BuffHealthSound; // ���� ������ �� ����
    public AudioClip ShortAttackSound; // ������ ���� ����
    public AudioClip ShortSpeedSound; // ���� ������ �� ����
    public AudioClip ShortHealthSound; // ���� ������ �� ����
    private AudioSource audioSource; // ����� �ҽ�

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
            AttackbuffEffect.SetActive(false); // ���� �� ���� ����Ʈ ��Ȱ��ȭ
        }

        if (SpeedbuffEffect != null)
        {
            SpeedbuffEffect.SetActive(false); // ���� �� ���� ����Ʈ ��Ȱ��ȭ
        }

        if (HealthbuffEffect != null)
        {
            HealthbuffEffect.SetActive(false); // ���� �� ���� ����Ʈ ��Ȱ��ȭ
        }

        if (AttackShortEffect != null)
        {
            AttackShortEffect.SetActive(false); // ���� �� ���� ���� ����Ʈ ��Ȱ��ȭ
        }

        if (SpeedShortEffect != null)
        {
            SpeedShortEffect.SetActive(false); // ���� �� ���� ���� ����Ʈ ��Ȱ��ȭ
        }

        if (HealthShortEffect != null)
        {
            HealthShortEffect.SetActive(false); // ���� �� ���� ���� ����Ʈ ��Ȱ��ȭ
        }

        audioSource = gameObject.AddComponent<AudioSource>(); // ����� �ҽ� �߰�
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; // ���� ���� �Ǵ� Ŭ���� ���¿��� ������Ʈ ����

        if (spellbookSystem != null)
        {
            spellbookSystem.HandleSpellbookSwitch();
        }

        if (spellbookSystem != null && spellbookSystem.currentSpellbook != SpellbookType.None)
        {
            if (magicCircleUI != null)
            {
                magicCircleUI.HandleMagicCircle();
                mouseLook.SetEnabled(!magicCircleUI.IsDrawing()); // ������ UI Ȱ��ȭ �� MouseLook ��Ȱ��ȭ
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
            Debug.Log("���� �Ӽ� ���Ÿ� ���� ���!");
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
            Debug.Log("�ӵ� �Ӽ� ���Ÿ� ���� ���!"); 
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
            Debug.Log("ü�� �Ӽ� ���Ÿ� ���� ���!");
            PlayLongHealthSound();
        }
    }

    public void CastAttackSpellBuff()
    {
        Debug.Log("���� �Ӽ� ���� ���� ���!");
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
            AttackbuffEffect.SetActive(true); // ���� ����Ʈ Ȱ��ȭ
        }

        float originalDamageMultiplier = DamageArea.damageMultiplier; // DamageArea ��ũ��Ʈ�� damageMultiplier ������ ����Ѵٰ� ����
        DamageArea.damageMultiplier = 1.5f; // ������ 1.5�� ����

        yield return new WaitForSeconds(buffDuration); // ���� ���� �ð� ���� ���

        DamageArea.damageMultiplier = originalDamageMultiplier; // ���� �������� ����

        if (AttackbuffEffect != null)
        {
            AttackbuffEffect.SetActive(false); // ���� ����Ʈ ��Ȱ��ȭ
        }

        activeBuff = BuffType.None;
        currentBuffCoroutine = null; // �ڷ�ƾ ���� ����
    }

    public void CastSpeedSpellBuff()
    {
        Debug.Log("�ӵ� �Ӽ� ���� ���� ���!");
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
            SpeedbuffEffect.SetActive(true); // ���� ����Ʈ Ȱ��ȭ
        }

        float originalSpeed = playerMovement.speed;
        playerMovement.speed *= speedBuffMultiplier; // �ӵ� ����

        yield return new WaitForSeconds(buffDuration); // ���� ���� �ð� ���� ���

        playerMovement.speed = originalSpeed; // ���� �ӵ��� ����

        if (SpeedbuffEffect != null)
        {
            SpeedbuffEffect.SetActive(false); // ���� ����Ʈ ��Ȱ��ȭ
        }

        activeBuff = BuffType.None;
        currentBuffCoroutine = null; // �ڷ�ƾ ���� ����
    }

    public void CastHealthSpellBuff()
    {
        Debug.Log("ü�� �Ӽ� ���� ���� ���!");
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
            HealthbuffEffect.SetActive(true); // ���� ����Ʈ Ȱ��ȭ
        }

        for (int i = 0; i < 5; i++)
        {
            playerHealth.Heal(3);
            yield return new WaitForSeconds(1f); // 1�� �������� 5�� ȸ��
        }

        if (HealthbuffEffect != null)
        {
            HealthbuffEffect.SetActive(false); // ���� ����Ʈ ��Ȱ��ȭ
        }

        activeBuff = BuffType.None;
        currentBuffCoroutine = null; // �ڷ�ƾ ���� ����
    }

    private void StopActiveBuff()
    {
        if (currentBuffCoroutine != null)
        {
            StopCoroutine(currentBuffCoroutine);

            // ���� ���� ���� ���� ����Ʈ�� ��Ȱ��ȭ
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

            // ���� ���·� ����
            if (activeBuff == BuffType.Attack)
            {
                DamageArea.damageMultiplier = 1f; // ���� �������� ����
            }
            else if (activeBuff == BuffType.Speed)
            {
                playerMovement.speed = playerMovement.speed / speedBuffMultiplier; // ���� �ӵ��� ����
            }

            activeBuff = BuffType.None;
            currentBuffCoroutine = null;
        }
    }

    public void CastAttackSpellShort()
    {
        Debug.Log("���� �Ӽ� ���� ���� ���!");
        PlayShortAttackSound();

        if (AttackShortEffect != null)
        {
            AttackShortEffect.SetActive(true); // ���� ���� ����Ʈ Ȱ��ȭ
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6.5f, layerMask2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                BossHealth bossHealth = hitCollider.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(60); // �������� �������� ��
                }
            }
        }

        StartCoroutine(DisableAttackShortEffect());
    }

    private IEnumerator DisableAttackShortEffect()
    {
        yield return new WaitForSeconds(1f); // ����Ʈ ���� �ð�
        if (AttackShortEffect != null)
        {
            AttackShortEffect.SetActive(false); // ���� ���� ����Ʈ ��Ȱ��ȭ
        }
    }

    public void CastSpeedSpellShort()
    {
        Debug.Log("�ӵ� �Ӽ� ���� ���� ���!");
        PlayShortSpeedSound();

        if (SpeedShortEffect != null)
        {
            SpeedShortEffect.SetActive(true); // ���� ���� ����Ʈ Ȱ��ȭ
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6.5f, layerMask2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                BossHealth bossHealth = hitCollider.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(30); // �������� �������� ��
                    BossController bossController = hitCollider.GetComponent<BossController>();
                    if (bossController != null)
                    {
                        bossController.ApplySpeedDebuff(speedDebuffAmount, speedDebuffDuration); // �������� �ӵ� ����� ����
                    }
                }
            }
        }

        StartCoroutine(DisableSpeedShortEffect());
    }

    private IEnumerator DisableSpeedShortEffect()
    {
        yield return new WaitForSeconds(1f); // ����Ʈ ���� �ð�
        if (SpeedShortEffect != null)
        {
            SpeedShortEffect.SetActive(false); // ���� ���� ����Ʈ ��Ȱ��ȭ
        }
    }

    public void CastHealthSpellShort()
    {
        Debug.Log("ü�� �Ӽ� ���� ���� ���!");
        PlayShortHealthSound();

        if (HealthShortEffect != null)
        {
            HealthShortEffect.SetActive(true); // ���� ���� ����Ʈ Ȱ��ȭ
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6.5f, layerMask2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss"))
            {
                BossHealth bossHealth = hitCollider.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    float damage = 30; // �������� �� ������
                    bossHealth.TakeDamage(damage); // �������� �������� ��
                    playerHealth.Heal(damage * 0.5f); // �������� ���ݸ�ŭ �÷��̾ ȸ��
                }
            }
        }

        StartCoroutine(DisableHealthShortEffect());
    }

    private IEnumerator DisableHealthShortEffect()
    {
        yield return new WaitForSeconds(1f); // ����Ʈ ���� �ð�
        if (HealthShortEffect != null)
        {
            HealthShortEffect.SetActive(false); // ���� ���� ����Ʈ ��Ȱ��ȭ
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
