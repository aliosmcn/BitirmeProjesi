using UnityEngine;

public class CrowCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    
    [Header("Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, -5f);
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float rotationSpeed = 3f;

    void LateUpdate()
    {
        if (target == null) return;
        
        // Pozisyon takibi
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
        // Rotasyon takibi (hedefe bakış)
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}