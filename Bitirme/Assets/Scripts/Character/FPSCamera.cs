using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform playerBody;

    private float _xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _xRotation = transform.localEulerAngles.x; 
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _xRotation = Mathf.Clamp(_xRotation - mouseY, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}