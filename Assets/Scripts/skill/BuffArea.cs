using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffArea : MonoBehaviour
{
    public float buffDuration = 5f; // 버프 지속 시간
    public float lifetime = 10f; // 장판의 생명주기
    public bool isAttackBuff; // 공격형 버프 여부
    public bool isSpeedBuff; // 속도형 버프 여부
    public bool isHealthBuff; // 체력형 버프 여부
    public float buffAmount = 10f; // 버프 양
    private List<PlayerController> playersInArea = new List<PlayerController>();

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null && !playersInArea.Contains(playerController))
            {
                playersInArea.Add(playerController);
                ApplyBuff(playerController);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null && playersInArea.Contains(playerController))
            {
                playersInArea.Remove(playerController);
                RemoveBuff(playerController);
            }
        }
    }

    private void ApplyBuff(PlayerController player)
    {
        if (isAttackBuff)
        {
            // 예: 플레이어의 공격력을 증가시킴
            // player.attackDamage += buffAmount;
        }
        else if (isSpeedBuff)
        {
            // 예: 플레이어의 이동 속도를 증가시킴
            // player.movementSpeed *= buffAmount;
        }
        else if (isHealthBuff)
        {
            // 예: 플레이어의 체력을 증가시킴
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(buffAmount);
            }
        }
    }

    private void RemoveBuff(PlayerController player)
    {
        if (isAttackBuff)
        {
            // 예: 플레이어의 공격력을 원래대로 되돌림
            // player.attackDamage -= buffAmount;
        }
        else if (isSpeedBuff)
        {
            // 예: 플레이어의 이동 속도를 원래대로 되돌림
            // player.movementSpeed /= buffAmount;
        }
    }
}
