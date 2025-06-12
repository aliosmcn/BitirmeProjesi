using UnityEngine;

public class CrowController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 2f;
    public float deceleration = 2f;
    public float minFlightSpeed = 2f;
    public float maxFlightSpeed = 7f;
    public float maxBankAngle = 45f;
    public float bankSpeed = 5f;
    public float rotationSpeed = 100f;
    
    [Header("References")]
    public Transform crowModel;
    
    private Rigidbody rb;
    private bool isFlying = false;
    private float targetBankAngle = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!isFlying && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = transform.forward * minFlightSpeed;
            isFlying = true;
        }

        if (isFlying)
        {
            HandleMovement();
            HandleBanking();
        }
    }

    void HandleMovement()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 currentVelocity = rb.linearVelocity;
        float currentForwardSpeed = Vector3.Dot(currentVelocity, forwardDirection);

        // KESİNLİKLE GERİ GİTME ENGELLEYİCİ
        if (currentForwardSpeed <= 0)
        {
            rb.linearVelocity = forwardDirection * minFlightSpeed;
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (currentForwardSpeed < maxFlightSpeed)
            {
                rb.AddForce(forwardDirection * acceleration, ForceMode.Acceleration);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (currentForwardSpeed > minFlightSpeed)
            {
                rb.AddForce(-forwardDirection * deceleration, ForceMode.Acceleration);
            }
        }

        // Hızı her frame'de ileri yöne kilitle
        rb.linearVelocity = forwardDirection * Mathf.Clamp(
            Vector3.Dot(rb.linearVelocity, forwardDirection),
            minFlightSpeed,
            maxFlightSpeed
        );
    }

    void HandleBanking()
    {
        float mouseX = Input.GetAxis("Mouse X");
        targetBankAngle = -mouseX * maxBankAngle;
        crowModel.localRotation = Quaternion.Slerp(
            crowModel.localRotation,
            Quaternion.Euler(0, 0, targetBankAngle),
            bankSpeed * Time.deltaTime
        );
        transform.Rotate(0, mouseX * rotationSpeed * Time.deltaTime, 0);
    }
}