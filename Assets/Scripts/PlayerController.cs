using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpellbookSystem spellbookSystem;
    private MagicCircleUI magicCircleUI;
    private MouseLook mouseLook;

    public GameObject damageAreaPrefab; // ������ ���� Prefab�� �Ҵ�
    public Transform cameraTransform; // ī�޶� Transform�� �Ҵ�
    public LayerMask layerMask; // ������ ���̾ ����

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
        Debug.Log("�ӵ� �Ӽ� ���Ÿ� ���� ���!");
    }

    public void CastHealthSpellLong()
    {
        Debug.Log("ü�� �Ӽ� ���Ÿ� ���� ���!");
    }
}
