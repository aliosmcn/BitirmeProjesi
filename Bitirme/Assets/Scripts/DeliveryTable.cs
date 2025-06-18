using System.Collections;
using UnityEngine;

public class DeliveryTable : MonoBehaviour
{
    #region Singleton
    private static DeliveryTable instance;

    public static DeliveryTable Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion Singleton
    
    [Header("Events")]
    [SerializeField] private VoidEvent onOrderCorrect;
    [SerializeField] private VoidEvent onOrderFail;

    public Transform orderPos;

    public Interactable deliverObject;

    public void SetDeliveryObject(Interactable obj)
    {
        deliverObject = obj;
        deliverObject.gameObject.tag = "Untagged";
        OrderSystem.Instance.CheckOrder(deliverObject);
        StartCoroutine(DestroyDeliver());
    }

    private IEnumerator DestroyDeliver()
    {
        yield return new WaitForSeconds(1f);
        
        if (deliverObject != null)
        {
            Destroy(deliverObject.gameObject);
            deliverObject = null;
        }
    }
}
