using UnityEngine;

public class CrowController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 2f;
    public float deceleration = 2f;
    public float minFlightSpeed = 0f; // Artık 0'a kadar düşebilir
    public float maxFlightSpeed = 7f;
    public float maxBankAngle = 45f;
    public float maxPitchAngle = 30f;
    public float bankSpeed = 5f;
    public float rotationSpeed = 100f;
    public float verticalSpeed = 3f;
    public float mouseYSensitivity = 0.1f;
    
    [Header("References")]
    public Transform crowModel;
    private Animator animator;
    
    private Rigidbody rb;
    private bool isFlying = false;
    private bool isCollided = false; // Çarpışma durumu
    private float targetBankAngle = 0f;
    private float targetPitchAngle = 0f;
    private float targetVerticalSpeed = 0f;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        animator = crowModel.gameObject.GetComponent<Animator>();
        rb.useGravity = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Çarpışma algılandığında
        isCollided = true;
        rb.linearVelocity = Vector3.zero;
        animator.SetTrigger("Hit");
    }

    void Update()
    {
        // Çarpışma sonrası W tuşu ile yeniden hareket
        if (isCollided && Input.GetKeyDown(KeyCode.W))
        {
            isCollided = false;
            isFlying = true;
            rb.linearVelocity = transform.forward * minFlightSpeed;
            animator.SetTrigger("Jump");
        }
        // Başlangıç hareketi
        if (!isFlying && !isCollided && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
            rb.linearVelocity = transform.forward * minFlightSpeed;
            isFlying = true;
        }

        if (isFlying && !isCollided)
        {
            HandleMovement();
            HandleBanking();
            HandleVertical();
            UpdateModelRotation();
        }
    }

    void HandleMovement()
    {
        Vector3 forwardDirection = transform.forward;
        float currentForwardSpeed = Vector3.Dot(rb.linearVelocity, forwardDirection);

        // Hız kontrolü (artık 0'a kadar düşebilir)
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(forwardDirection * acceleration, ForceMode.Acceleration);
            isCollided = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-forwardDirection * deceleration, ForceMode.Acceleration);
        }

        // Hız sınırlaması
        float newForwardSpeed = Mathf.Clamp(
            Vector3.Dot(rb.linearVelocity, forwardDirection),
            minFlightSpeed,
            maxFlightSpeed
        );

        rb.linearVelocity = forwardDirection * newForwardSpeed + new Vector3(0, rb.linearVelocity.y, 0);
    }

    void HandleBanking()
    {
        float mouseX = Input.GetAxis("Mouse X");
        targetBankAngle = -mouseX * maxBankAngle;
        transform.Rotate(0, mouseX * rotationSpeed * Time.deltaTime, 0);
    }

    void HandleVertical()
    {
        float mouseY = -Input.GetAxis("Mouse Y");
        targetPitchAngle = -mouseY * maxPitchAngle;
        targetVerticalSpeed = mouseY * verticalSpeed;
        
        float currentYVelocity = Mathf.Lerp(
            rb.linearVelocity.y,
            targetVerticalSpeed,
            Time.deltaTime * 5f
        );
        
        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            currentYVelocity,
            rb.linearVelocity.z
        );
    }

    void UpdateModelRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(
            targetPitchAngle,
            0,
            targetBankAngle
        );
        
        crowModel.localRotation = Quaternion.Slerp(
            crowModel.localRotation,
            targetRotation,
            bankSpeed * Time.deltaTime
        );
    }
}