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

    [SerializeField] public string UIText;
    [SerializeField] private Sprite UIImage;

    public Sprite ItemIcon
    {
        get
        {
            return UIImage;
        }
    }

    [SerializeField] public GameObject prefab;


}
