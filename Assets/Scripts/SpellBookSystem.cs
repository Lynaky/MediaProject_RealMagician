using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellbookType { None, Speed, Attack, Health }

public class SpellbookSystem : MonoBehaviour
{
    public SpellbookType currentSpellbook = SpellbookType.None;
    public PlayerController playerController;
    private MagicCircleUI magicCircleUI;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        magicCircleUI = GetComponentInChildren<MagicCircleUI>();
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
}