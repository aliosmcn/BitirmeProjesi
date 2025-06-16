using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;

    public ItemSO ItemData
    {
        get
        {
            return itemData;
        }
    }
}
