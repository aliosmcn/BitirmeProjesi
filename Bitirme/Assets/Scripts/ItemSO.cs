using UnityEngine;
// ReSharper disable InconsistentNaming

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{

    [SerializeField] private string itemId;

    public string ItemID
    {
        get
        {
            return itemId;
        }
    }

    [SerializeField] public string itemName;
    [SerializeField] private Sprite itemIcon;

    public Sprite ItemIcon
    {
        get
        {
            return itemIcon;
        }
    }

    [SerializeField] public GameObject prefab;


}
