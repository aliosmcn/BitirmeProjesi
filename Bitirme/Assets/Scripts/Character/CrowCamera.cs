using UnityEngine;

public class SmoothBirdCamera : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 2f, -5f); 
    public float smoothness = 5f; 

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + target.rotation * offset;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
        
        transform.LookAt(target);
    }
}