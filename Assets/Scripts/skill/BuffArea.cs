using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffArea : MonoBehaviour
{
    public float buffDuration = 5f; // ���� ���� �ð�
    public float lifetime = 10f; // ������ �����ֱ�
    public bool isAttackBuff; // ������ ���� ����
    public bool isSpeedBuff; // �ӵ��� ���� ����
    public bool isHealthBuff; // ü���� ���� ����
    public float buffAmount = 10f; // ���� ��
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
            // ��: �÷��̾��� ���ݷ��� ������Ŵ
            // player.attackDamage += buffAmount;
        }
        else if (isSpeedBuff)
        {
            // ��: �÷��̾��� �̵� �ӵ��� ������Ŵ
            // player.movementSpeed *= buffAmount;
        }
        else if (isHealthBuff)
        {
            // ��: �÷��̾��� ü���� ������Ŵ
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
            // ��: �÷��̾��� ���ݷ��� ������� �ǵ���
            // player.attackDamage -= buffAmount;
        }
        else if (isSpeedBuff)
        {
            // ��: �÷��̾��� �̵� �ӵ��� ������� �ǵ���
            // player.movementSpeed /= buffAmount;
        }
    }
}
