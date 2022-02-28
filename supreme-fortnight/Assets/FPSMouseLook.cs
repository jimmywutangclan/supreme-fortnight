using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMouseLook : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 5f;
    [SerializeField] bool invertY = false;

    float yaw = 0;
    float pitch = 0;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");

        // yaw
        yaw += moveX * mouseSensitivity;

        // pitch
        pitch += (invertY ? moveY : -moveY) * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -90, 90);

        transform.parent.rotation = Quaternion.Euler(0, yaw, 0);
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
