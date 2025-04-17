using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform holdPoint;

    private Camera cam;
    private GameObject carriedObject;
    private Interactable currentOutline;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        bool hasHit = Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactDistance, interactableLayer);
        UpdateOutline(hasHit, hit);

        if (Input.GetKeyDown(KeyCode.E))
            HandleInteraction(hasHit, hit);
    }

    
    private void HandleInteraction(bool hasHit, RaycastHit hit)
    {
        if (!carriedObject)
        {
            if (!hasHit) return;

            if (hit.collider.CompareTag("item"))
                Pickup(hit.collider.gameObject);
            else if (hit.collider.TryGetComponent(out Table table))
                Pickup(table.currentItem);
        }
        else
        {
            if (hasHit && hit.collider.TryGetComponent(out Table table))
                PlaceOnTable(table);
            else
                Drop();
        }
    }

    private void Pickup(GameObject obj)
    {
        carriedObject = obj;
        carriedObject.TryGetComponent(out Collider coll); 
        coll.enabled = true;
        if (carriedObject.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = true;

        carriedObject.transform.SetParent(holdPoint);
        carriedObject.transform.localPosition = Vector3.zero;
        carriedObject.transform.localRotation = Quaternion.identity;
    }

    private void PlaceOnTable(Table table)
    {
        carriedObject.transform.SetParent(table.transform, worldPositionStays: true);
        carriedObject.TryGetComponent(out Collider coll); 
        coll.enabled = false;
        table.PlaceObject(carriedObject);
        ResetCarry();
    }

    private void Drop()
    {
        carriedObject.transform.SetParent(null);
        if (carriedObject.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = false;

        ResetCarry();
    }

    private void ResetCarry()
    {
        carriedObject.transform.rotation = Quaternion.identity;
        carriedObject = null;
    }

    #region Outline

    private void UpdateOutline(bool hasHit, RaycastHit hit)
    {
        if (hasHit && hit.collider.TryGetComponent<Interactable>(out var interactable))
        {
            if (currentOutline != interactable)
            {
                currentOutline = interactable;
                currentOutline.Highlight(true);
            }
        }
        else
        {
            ClearOutline();
        }
    }
    private void ClearOutline()
    {
        if (currentOutline)
        {
            currentOutline.Highlight(false);
            currentOutline = null;
        }
    }

    #endregion
    

    
}
