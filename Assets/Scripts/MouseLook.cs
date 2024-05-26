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
    private bool isEnabled = true; // 활성화 여부

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return; // 비활성화 상태에서는 업데이트하지 않음

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 35f); // x축 각도 제한
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None; // 마우스 커서 잠금 상태 변경
    }

    private void OnGUI()
    {
        if (!isEnabled) return; // 비활성화 상태에서는 조준 십자선 표시하지 않음

        float xMin = (Screen.width / 2) - (crosshairSize / 2);
        float yMin = (Screen.height / 2) - (crosshairSize / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshairSize, crosshairSize), crosshair);
    }
}
