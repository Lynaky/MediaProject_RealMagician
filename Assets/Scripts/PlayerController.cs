using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpellbookSystem spellbookSystem;
    private MagicCircleUI magicCircleUI;
    private MouseLook mouseLook;

    public GameObject damageAreaPrefab; // 데미지 장판 Prefab을 할당
    public GameObject speedDebuffAreaPrefab; // 속도 감소 장판 Prefab을 할당
    public GameObject healingAreaPrefab; // 체력 회복 장판 Prefab을 할당
    public Transform cameraTransform; // 카메라 Transform을 할당
    public LayerMask layerMask; // 무시할 레이어를 설정

    private PlayerHealth playerHealth;

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
}