using System;
using UnityEngine;

public class CrowController : MonoBehaviour
{
    #region Singleton
    private static CrowController instance;

    public static CrowController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        animator = crowModel.GetComponent<Animator>();
    }

    #endregion Singleton
    
    [Header("Events")] 
    [SerializeField] private ItemSOEvent onLookingItem;
    [SerializeField] private VoidEvent onSwitchCrow;
    [SerializeField] private VoidEvent onSwitchCharacter;
    
    [Header("Movement Settings")]
    public float acceleration = 1f;
    public float deceleration = 1f;
    public float minFlightSpeed = 2f;
    public float maxFlightSpeed = 7f;
    public float maxBankAngle = 45f;
    public float maxPitchAngle = 40f;
    public float bankSpeed = 5f;
    public float rotationSpeed = 200f;
    public float verticalSpeed = 4f;
    public float mouseYSensitivity = 0.1f;
    public float verticalConstrain = 8f;
    
    [Header("References")]
    public Transform crowModel;
    public Transform pence;
    
    private Animator animator;
    private Rigidbody rb;
    [Header("Fly Settings")]
    public bool isFlying = false;
    public bool isCollided = false;
    private float targetBankAngle = 0f;
    private float targetPitchAngle = 0f;
    private float targetVerticalSpeed = 0f;

    private bool canPlaySFX = true;
    private void OnEnable()
    {
        onSwitchCrow.AddListener(OnSwitch);
    }

    private void OnDisable()
    {
        onSwitchCrow.RemoveListener(OnSwitch);
    }

    private void OnSwitch()
    {
        rb.useGravity = false;
        Cursor.lockState = CursorLockMode.Locked;
        isFlying = false;
        transform.rotation = Quaternion.identity;
        rb.linearVelocity = Vector3.zero;
        if (TryGetComponent(out Interactable interact)) onLookingItem.Raise(interact.itemData);
    }

    void Update()
    {
        if (isCollided && Input.GetKeyDown(KeyCode.Space))
        {
            onLookingItem.Raise(null);
            ResetAfterCollision();
        }

        if (!isFlying && Input.GetKeyDown(KeyCode.Space))
        {
            onLookingItem.Raise(null);
            StartFlying();
        }

        if (isFlying && !isCollided)
        {
            HandleMovement();
            HandleBanking();
            HandleVertical();
            UpdateModelRotation();
            
            UpdateFlightAnimation();
        }

        if (canPlaySFX && animator.GetBool("Up"))
        {
            AudioManager.Instance.PlaySFX("Kanat", gameObject);
            canPlaySFX = false;
        }
    }

    void StartFlying()
    {
        onLookingItem.Raise(null);
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
        float verticalVelocity = rb.linearVelocity.y;
        
        if (verticalVelocity < -0.2f) 
        {
            animator.SetBool("Down", true);
            animator.SetBool("Up", false);
            AudioManager.Instance.StopSFX("Kanat", gameObject);
            canPlaySFX = true;
        }
        else if (verticalVelocity > 0.2f) 
        {
            animator.SetBool("Up", true);
            animator.SetBool("Down", false);
        }
        else 
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
        if (transform.position.y >= verticalConstrain && Input.GetAxis("Mouse Y") < 0)
        {
            targetPitchAngle = 0;
            targetVerticalSpeed = 0;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            return;
        }

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
        if (collision.gameObject.CompareTag("item")) return;
        isCollided = true;
        rb.linearVelocity = Vector3.zero;
        animator.SetTrigger("Hit");
        animator.SetBool("Down", false); 
    }
}