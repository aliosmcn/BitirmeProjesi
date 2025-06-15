using UnityEngine;

public class SmoothBirdCamera : MonoBehaviour
{
    public Transform target; // Karga transformu
    public Vector3 offset = new Vector3(0, 2f, -5f); // Kamera konumu
    public float smoothness = 5f; // Yumuşaklık (5-10 arası ideal)

    void LateUpdate()
    {
        // 1. Hedef pozisyonu hesapla (kargayla birlikte dönecek şekilde)
        Vector3 targetPosition = target.position + target.rotation * offset;
        
        // 2. Yumuşak geçiş uygula
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
        
        // 3. Kargaya doğru bak
        transform.LookAt(target);
    }
}