using System;
using UnityEngine;

public class CrowController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent onSwitchCrow;
    [SerializeField] private VoidEvent onSwitchCharacter;
    
    [Header("Movement Settings")]
    public float acceleration = 2f;
    public float deceleration = 2f;
    public float minFlightSpeed = 0f;
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
    public bool isFlying = false;
    public bool isCollided = false;
    private float targetBankAngle = 0f;
    private float targetPitchAngle = 0f;
    private float targetVerticalSpeed = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = crowModel.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        rb.useGravity = false;
        Cursor.lockState = CursorLockMode.Locked;
        onSwitchCrow.AddListener(OnSwitch);
    }

    private void OnDisable()
    {
        onSwitchCrow.RemoveListener(OnSwitch);
    }

    private void OnSwitch()
    {
        transform.rotation = Quaternion.identity;
        rb.linearVelocity = Vector3.zero;
        animator.SetTrigger("Hit");
    }

    void Update()
    {
        // Çarpışma sonrası W tuşu ile yeniden hareket
        if (isCollided && Input.GetKeyDown(KeyCode.W))
        {
            ResetAfterCollision();
        }

        // Başlangıç hareketi
        if (!isFlying && Input.GetKeyDown(KeyCode.Space))
        {
            StartFlying();
        }

        if (isFlying && !isCollided)
        {
            HandleMovement();
            HandleBanking();
            HandleVertical();
            UpdateModelRotation();
            
            // Yüksekliğe göre animasyon kontrolü
            UpdateFlightAnimation();
        }
    }

    void StartFlying()
    {
        rb.linearVelocity = transform.forward * minFlightSpeed;
        isFlying = true;
        animator.SetTrigger("Jump");
        animator.SetBool("Down", true);
    }

    void ResetAfterCollision()
    {
        isCollided = false;
        isFlying = true;
        rb.linearVelocity = transform.forward * minFlightSpeed;
        animator.SetTrigger("Jump");
        animator.SetBool("Down", true);
    }

    void UpdateFlightAnimation()
    {
        // Yüksekliğe göre animasyon kontrolü
        float verticalVelocity = rb.linearVelocity.y;
        
        if (verticalVelocity < -0.2f) // Belirgin şekilde aşağı iniyorsa
        {
            animator.SetBool("Down", true);
            animator.SetBool("Up", false);
        }
        else if (verticalVelocity > 0.2f) // Belirgin şekilde yukarı çıkıyorsa
        {
            animator.SetBool("Up", true);
            animator.SetBool("Down", false);
        }
        else // Düz uçuş veya minimal hareket
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
        }
    }

    void HandleMovement()
    {
        Vector3 forwardDirection = transform.forward;
        float currentForwardSpeed = Vector3.Dot(rb.linearVelocity, forwardDirection);

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(forwardDirection * acceleration, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-forwardDirection * deceleration, ForceMode.Acceleration);
        }

        rb.linearVelocity = forwardDirection * Mathf.Clamp(
            currentForwardSpeed,
            minFlightSpeed,
            maxFlightSpeed
        ) + new Vector3(0, rb.linearVelocity.y, 0);
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
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("carpti");
        isCollided = true;
        rb.linearVelocity = Vector3.zero;
        animator.SetTrigger("Hit");
        animator.SetBool("Down", false); 
    }
}