using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSCharacter : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;


    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(walkSpeed * Time.deltaTime * move);

        controller.Move(velocity * Time.deltaTime);
    }
}