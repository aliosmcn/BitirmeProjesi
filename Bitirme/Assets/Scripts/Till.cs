using UnityEngine;

public class Till : MonoBehaviour
{
    [Header("Kasa Ayarları")]
    [SerializeField] private GameObject itemPrefab; // Inspector'dan atanacak prefab
    [SerializeField] private Transform spawnPoint; // Item'ın çıkacağı nokta

    public GameObject GetItemFromTill()
    {
        if (itemPrefab == null) return null;
        
        GameObject spawnedItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
        return spawnedItem;
    }
}
