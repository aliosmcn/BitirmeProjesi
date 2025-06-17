using System;
using UnityEngine;

public class TillOrCircle : MonoBehaviour
{
    [SerializeField] private Sprite buttonSprite;
    
    [Header("ItemSO")]
    [SerializeField] private ItemSO tillName;
    [SerializeField] private ItemSO circleName;

    private GameObject heldObject;

    private void Update()
    {
        if (heldObject && Input.GetMouseButtonDown(1)) DropObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && heldObject == null)
        {
            heldObject = Instantiate(
                circleName.prefab,
                CrowController.Instance.pence.position,
                Quaternion.identity,
                CrowController.Instance.pence
            );
            SetUI(true);
            SetRbAndCollider(false);
        }
    }

    private void DropObject()
    {
        SetUI(false);
        SetRbAndCollider(true);
        heldObject.transform.SetParent(null);
        heldObject = null;
    }

    private void SetRbAndCollider(bool state)
    {
        if (heldObject.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = !state;
            rb.useGravity = state;
        }
        if(heldObject.TryGetComponent(out Collider coll))
            coll.isTrigger = !state;
    }

    private void SetUI(bool state)
    {
        GameUIController.Instance.SetInteractive(state, "DROP" , buttonSprite);
    }
}