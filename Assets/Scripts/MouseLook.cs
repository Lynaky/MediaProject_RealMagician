using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 600f;
    public Transform playerBody;
    public Texture2D crosshair; // aiming
    public int crosshairSize = 50;
    private float xRotation = 0f;
    private bool isEnabled = true; // Ȱ��ȭ ����

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return; // ��Ȱ��ȭ ���¿����� ������Ʈ���� ����

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 35f); // x�� ���� ����
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None; // ���콺 Ŀ�� ��� ���� ����
    }

    private void OnGUI()
    {
        if (!isEnabled) return; // ��Ȱ��ȭ ���¿����� ���� ���ڼ� ǥ������ ����

        float xMin = (Screen.width / 2) - (crosshairSize / 2);
        float yMin = (Screen.height / 2) - (crosshairSize / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshairSize, crosshairSize), crosshair);
    }
}
