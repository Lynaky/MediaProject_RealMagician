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

    // Start is called before the first frame update
    void Start()
    {
        // hide the cursor 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // vertical mouse movement rotates the camera around the x-axis
        xRotation -= mouseY;

        // Limit the xRotation to be within -30 and 30 degrees
        xRotation = Mathf.Clamp(xRotation, -26f, 30f);

        // rotates the camera along the x-axis
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // rotate the player body along the y-axis
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void OnGUI()
    {
        float xMin = (Screen.width / 2) - (crosshairSize / 2);
        float yMin = (Screen.height / 2) - (crosshairSize / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshairSize, crosshairSize), crosshair);
    }
}
