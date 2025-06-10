using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 2f; // Artık normal değerlerle çalışacak (1-5 arası)
    [SerializeField] private Transform playerBody;

    private float _xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _xRotation = transform.localEulerAngles.x; // Mevcut rotasyonu koru
    }

    void Update()
    {
        // Fare girdisini al (deltaTime'siz)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Dikey bakış
        _xRotation = Mathf.Clamp(_xRotation - mouseY, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Yatay dönüş
        playerBody.Rotate(Vector3.up * mouseX);
    }
}