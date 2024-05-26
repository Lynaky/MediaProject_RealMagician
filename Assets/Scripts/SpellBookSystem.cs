using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellbookType { None, Speed, Attack, Health }

public class SpellbookSystem : MonoBehaviour
{
    public SpellbookType currentSpellbook = SpellbookType.None;
    public PlayerController playerController;
    private MagicCircleUI magicCircleUI;

    public AudioClip switchSound; // ����å ����ġ ����
    private AudioSource audioSource; // ����� �ҽ�

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        magicCircleUI = GetComponentInChildren<MagicCircleUI>();
        audioSource = gameObject.AddComponent<AudioSource>(); // ����� �ҽ� �߰�
    }

    public void HandleSpellbookSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EquipSpellbook(SpellbookType.Attack);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            EquipSpellbook(SpellbookType.Speed);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            EquipSpellbook(SpellbookType.Health);
        }
    }

    private void EquipSpellbook(SpellbookType spellbookType)
    {
        currentSpellbook = spellbookType;
        Debug.Log("Equipped Spellbook: " + spellbookType);
        magicCircleUI.UpdateSelectedPointsColor(); // ����å�� ����� �� ���õ� ��ư ���� ������Ʈ
        PlaySwitchSound(); // ����å ����ġ ���� ���
    }

    private void PlaySwitchSound()
    {
        if (switchSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(switchSound); // ȿ���� ���
        }
    }

    public void CastSpellLong()
    {
        switch (currentSpellbook)
        {
            case SpellbookType.Attack:
                playerController.CastAttackSpellLong();
                break;
            case SpellbookType.Speed:
                playerController.CastSpeedSpellLong();
                break;
            case SpellbookType.Health:
                playerController.CastHealthSpellLong();
                break;
            default:
                Debug.Log("No spellbook equipped.");
                break;
        }
    }

    public void CastSpellBuff()
    {
        switch (currentSpellbook)
        {
            case SpellbookType.Attack:
                playerController.CastAttackSpellBuff();
                break;
            case SpellbookType.Speed:
                playerController.CastSpeedSpellBuff();
                break;
            case SpellbookType.Health:
                playerController.CastHealthSpellBuff();
                break;
            default:
                Debug.Log("No spellbook equipped.");
                break;
        }
    }

    public void CastSpellShort()
    {
        switch (currentSpellbook)
        {
            case SpellbookType.Attack:
                playerController.CastAttackSpellShort();
                break;
            case SpellbookType.Speed:
                playerController.CastSpeedSpellShort();
                break;
            case SpellbookType.Health:
                playerController.CastHealthSpellShort();
                break;
            default:
                Debug.Log("No spellbook equipped.");
                break;
        }
    }
}
