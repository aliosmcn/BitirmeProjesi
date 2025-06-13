using System;
using UnityEngine;

public class CrowController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 2f;
    public float deceleration = 2f;
    public float minFlightSpeed = 2f;
    public float maxFlightSpeed = 7f;
    public float maxBankAngle = 45f;
    public float maxPitchAngle = 30f; // Added pitch angle for vertical movement
    public float bankSpeed = 5f;
    public float rotationSpeed = 100f;
    public float verticalSpeed = 3f;
    public float mouseYSensitivity = 0.1f;
    
    [Header("References")]
    public Transform crowModel;
    
    private Rigidbody rb;
    private bool isFlying = false;
    private float targetBankAngle = 0f;
    private float targetPitchAngle = 0f; // Added pitch angle target
    private float targetVerticalSpeed = 0f;

    private void OnEnable()
    {
        isFlying = false;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        isFlying = false; 
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
            HandleVertical();
            UpdateModelRotation(); // Combined rotation handling
        }
    }

    void HandleMovement()
    {
        Vector3 forwardDirection = transform.forward;
        float currentForwardSpeed = Vector3.Dot(rb.linearVelocity, forwardDirection);

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

        rb.linearVelocity = forwardDirection * Mathf.Clamp(
            currentForwardSpeed,
            minFlightSpeed,
            maxFlightSpeed
        ) + Vector3.up * rb.linearVelocity.y;
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
        
        // Set pitch angle based on vertical movement
        targetPitchAngle = -mouseY * maxPitchAngle;
        
        // Fare hareketine göre hedef hız belirle
        targetVerticalSpeed = mouseY * verticalSpeed;
        
        // Yumuşak geçiş için Lerp
        float currentYVelocity = Mathf.Lerp(
            rb.linearVelocity.y,
            targetVerticalSpeed,
            Time.deltaTime * 5f
        );
        
        // Sadece Y eksenini güncelle
        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            currentYVelocity,
            rb.linearVelocity.z
        );
    }

    void UpdateModelRotation()
    {
        // Combine banking and pitch rotations
        Quaternion targetRotation = Quaternion.Euler(
            targetPitchAngle, // Pitch (forward/backward tilt)
            0,
            targetBankAngle  // Bank (left/right tilt)
        );
        
        crowModel.localRotation = Quaternion.Slerp(
            crowModel.localRotation,
            targetRotation,
            bankSpeed * Time.deltaTime
        );
    }
}