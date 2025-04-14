using UnityEngine;

public class Table : MonoBehaviour
{
    public GameObject itemPrefab;

    public void PlaceObject(GameObject objectToPlace)
    {
        itemPrefab = objectToPlace;
        itemPrefab.transform.position = this.transform.position + Vector3.up * 0.4f;
    }
    
}
