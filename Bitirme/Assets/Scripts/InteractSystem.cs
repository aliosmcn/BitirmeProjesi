using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform holdPoint;

    [Header("HandleObject")]
    public GameObject carriedObject;
    private bool isCarrying = false;

    private void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!isCarrying)
            {
                TryPickup();
            }
            else
            {
                TryPlace();
            }
        }
    }

    private void TryPickup()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, 
            out RaycastHit hit, interactDistance, interactableLayer))
        {
            //DIREKT
            if (hit.collider.gameObject.CompareTag("item"))
            {
                CarryObject(hit.collider.gameObject);
            }
            
            //MASADAN
            if(hit.collider.TryGetComponent(out Table table))
            {
                carriedObject = table.itemPrefab;
            }
            
            //KASADAN
            /*
            if (hit.collider.gameObject.CompareTag("Kasa"))
            {
                //kasadan ürün al
            }*/
        }
    }

    private void TryPlace()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, 
            out RaycastHit hit, interactDistance, interactableLayer))
        {
            if(hit.collider.GetComponent<Table>())
            {
                PlaceOnTable(hit.transform);
            }
        }
        else
        {
            DropObject();
        }
    }

    private void CarryObject(GameObject obj)
    {
        carriedObject = obj;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        isCarrying = true;
    }

    private void PlaceOnTable(Transform table)
    {
        carriedObject.transform.SetParent(table);
        table.TryGetComponent(out Table table2);
        table2.PlaceObject(carriedObject);
        ResetObject();
    }

    private void DropObject()
    {
        carriedObject.transform.SetParent(null);
        carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        ResetObject();
    }


    private void ResetObject()
    {
        carriedObject.transform.localRotation = Quaternion.identity;
        carriedObject = null;
        isCarrying = false;
    }
}