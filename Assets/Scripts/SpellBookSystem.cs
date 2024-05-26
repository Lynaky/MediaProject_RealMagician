using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellbookType { None, Speed, Attack, Health }

public class SpellbookSystem : MonoBehaviour
{
    public SpellbookType currentSpellbook = SpellbookType.None;
    public PlayerController playerController;
    private MagicCircleUI magicCircleUI;

    public AudioClip switchSound; // 마법책 스위치 사운드
    private AudioSource audioSource; // 오디오 소스

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        magicCircleUI = GetComponentInChildren<MagicCircleUI>();
        audioSource = gameObject.AddComponent<AudioSource>(); // 오디오 소스 추가
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
        magicCircleUI.UpdateSelectedPointsColor(); // 마법책이 변경될 때 선택된 버튼 색상 업데이트
        PlaySwitchSound(); // 마법책 스위치 사운드 재생
    }

    private void PlaySwitchSound()
    {
        if (switchSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(switchSound); // 효과음 재생
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
