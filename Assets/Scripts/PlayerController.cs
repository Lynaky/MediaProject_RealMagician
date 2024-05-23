using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpellbookSystem spellbookSystem;
    private MagicCircleUI magicCircleUI;
    private MouseLook mouseLook;

    public GameObject damageAreaPrefab; // ������ ���� Prefab�� �Ҵ�
    public GameObject speedDebuffAreaPrefab; // �ӵ� ���� ���� Prefab�� �Ҵ�
    public GameObject healingAreaPrefab; // ü�� ȸ�� ���� Prefab�� �Ҵ�
    public Transform cameraTransform; // ī�޶� Transform�� �Ҵ�
    public LayerMask layerMask; // ������ ���̾ ����

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
        }
    }
}