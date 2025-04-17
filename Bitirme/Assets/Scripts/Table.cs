using UnityEngine;
using UnityEngine.Serialization;

public class Table : MonoBehaviour
{
    public GameObject currentItem;

    public void PlaceObject(GameObject objectToPlace)
    {
        currentItem = objectToPlace;
        currentItem.transform.position = this.transform.position + Vector3.up * 0.4f;
    }
    
}
