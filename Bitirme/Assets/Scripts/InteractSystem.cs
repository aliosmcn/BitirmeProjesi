using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class InteractSystem : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent onEPressed;
    [SerializeField] private VoidEvent onSpacePressed;
    [SerializeField] private VoidEvent onCtrlPressed;
    [SerializeField] private VoidEvent onRightClickPressed;
    [SerializeField] private VoidEvent onLeftClickPressed;
    [SerializeField] private ItemSOEvent onLookingItem;
    [SerializeField] private IntEvent onEnergyChanged;

    
    [Header("Settings")]
    [SerializeField] private float rayDistance = 2.2f;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float moveTime = 0.2f;
    [SerializeField] private LayerMask interactableLayer; // Added layer mask for more efficient raycasting

    private Camera cam;
    private Interactable currentHover;
    private Interactable holdObject;
    private Coroutine currentRoutine;

    private void OnEnable()
    {
        onEPressed.AddListener(E_Button);
        onLeftClickPressed.AddListener(LeftClick);
        onRightClickPressed.AddListener(RightClick);
    }
    
    private void OnDisable()
    {
        onEPressed.RemoveListener(E_Button);
        onLeftClickPressed.RemoveListener(LeftClick);
        onRightClickPressed.RemoveListener(RightClick);
        
        if (currentHover) currentHover.OutlineState(false);
        if (currentRoutine != null) StopCoroutine(currentRoutine);
    }

    void Awake() => cam = Camera.main;

    void Update()
    {
        HandleRaycast();
    }

    private void HandleRaycast()
    {
        if (currentRoutine != null) return;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 
                rayDistance, interactableLayer))
        {
            hit.collider.TryGetComponent(out Interactable hover);
        
            if (hover != currentHover)
            {
                if (currentHover) currentHover.OutlineState(false);
                onLookingItem.Raise(null);
                currentHover = hover;
                if (currentHover) currentHover.OutlineState(true);
                if (hover)
                    onLookingItem.Raise(currentHover.ItemData);
            }

            if (holdObject) holdObject.SetPreviewState(true, hit.point);
        }
        else
        {
            if (currentHover)
            {
                currentHover.OutlineState(false);
                currentHover = null;
                onLookingItem.Raise(null);
            }
        
            if (holdObject) holdObject.SetPreviewState(false);
        }
    }

    private void E_Button()
    {
        if (currentRoutine != null) return;
        
        //elimizde obje var birakacagiz
        if (holdObject)
        {
            holdObject.SetPreviewState(false);
            
            //birakacak yer var
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.TryGetComponent(out DeliveryTable table) && DaySystem.Instance.isOpen && holdObject.itemData.isPotion && DaySystem.Instance.canDeliver)
                {
                    if (!table.deliverObject) currentRoutine = StartCoroutine(MoveObjectRoutine(holdObject, table.orderPos.position, false));
                    DeliveryTable.Instance.SetDeliveryObject(holdObject);
                    return;
                }
                if (hit.collider.TryGetComponent(out Kazan kazan) && kazan.alabiliyorMu && Kepce.Instance.TryGetComponent(out Animator animator) && !animator.GetBool("Mixing"))
                {
                    currentRoutine = StartCoroutine(MoveObjectRoutine(holdObject,
                    Kazan.Instance.GetTransform().transform.position, false));
                    Kazan.Instance.PlaceObject(holdObject);
                    return;
                }
                if (kazan && !kazan.alabiliyorMu || kazan && kazan.alabiliyorMu && Kepce.Instance.TryGetComponent(out Animator animator2) && animator2.GetBool("Mixing")) 
                {
                    //(KAZAN DOLU || KULLANIMDA)
                    Drop();
                    return;
                }

                if (hit.collider.gameObject.CompareTag("Table"))
                    return;
                currentRoutine = StartCoroutine(MoveObjectRoutine(holdObject, hit.point, false)); //PLACE
            }
            else
                Drop();

        }
        else if (currentHover && currentHover.gameObject.CompareTag("item"))
        {
            currentRoutine = StartCoroutine(MoveObjectRoutine(currentHover, holdPoint.position, true)); //PICK
        }
        else if (currentHover && currentHover.TryGetComponent(out TillOrCircle till))
        {
            GameObject newObject = Instantiate(currentHover.itemData.prefab, till.transform.position, Quaternion.identity);
            newObject.TryGetComponent(out Interactable child);
            currentRoutine = StartCoroutine(MoveObjectRoutine(child, holdPoint.position, true));
            onEnergyChanged.Raise(-5);
            //KASADAN ALINIYOR
        }
    }

    private void LeftClick()
    {
        
        if (currentRoutine != null) return;
        if (holdObject) return;
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Book")) //Kitap
            {
                Book.Instance.NextPage();
            }
            else if (hit.collider.TryGetComponent(out Kazan kazan) && Kepce.Instance.TryGetComponent(out Animator animator) && !animator.GetBool("Mixing")) // Kazan
            {
                Kazan.Instance.Mix();
            }
            else if (hit.collider.TryGetComponent(out DaySystem openClose)) //OpenClose
            {
                if (!openClose.isOpen)
                {
                    openClose.OnOpen();
                }
                else
                {
                    if(openClose.canClose)
                        openClose.OnClose();
                }
            }
            else if (hit.collider.CompareTag("Kure"))
            {
                OrderSystem.Instance.ReverseTime();
                AudioManager.Instance.PlaySFX("geriAl", gameObject);
            }
            
        }
    }

    private void RightClick()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Book"))
            {
                Book.Instance.PreviousPage();
            }
            else if (hit.collider.TryGetComponent(out Kazan kazan))
            {
                if (currentRoutine != null) return;
                if (holdObject) return;
                if (Kepce.Instance.TryGetComponent(out Animator animator) && animator.GetBool("Mixing")) return;
                Kazan.Instance.ClearItems();
            }
        }
    }

    private IEnumerator MoveObjectRoutine(Interactable obj, Vector3 targetPos, bool pickup)
    {
        if (currentHover) currentHover.OutlineState(false);
        obj.SetPreviewState(false);
        
        SetPhysicsState(obj, false);
        
        Vector3 startPos = obj.transform.position;
        Quaternion startRot = obj.transform.rotation;
        Quaternion targetRot = pickup ? holdPoint.rotation : Quaternion.identity;
        
        if (!pickup) 
            obj.transform.SetParent(null);
            
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            float easedT = Mathf.SmoothStep(0, 1, Mathf.Clamp01(t));
            
            obj.transform.position = Vector3.Lerp(startPos, targetPos, easedT);
            obj.transform.rotation = Quaternion.Slerp(startRot, targetRot, easedT);
            yield return null;
        }

        obj.transform.position = targetPos;
        obj.transform.rotation = targetRot;
        
        if (pickup)
        {
            obj.transform.SetParent(holdPoint);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            holdObject = obj;
        }
        else
        {
            SetPhysicsState(obj, true);
            holdObject = null;
        }
        
        currentRoutine = null;
    }
    
    private void Drop()
    {
        holdObject.transform.SetParent(null);
        holdObject.transform.rotation = Quaternion.identity;
        SetPhysicsState(holdObject, true);
        holdObject.SetPreviewState(false);
        holdObject = null;
    }

    private void SetPhysicsState(Interactable obj, bool state)
    {
        if (obj.TryGetComponent(out Collider c)) c.enabled = state;
        if (obj.TryGetComponent(out Rigidbody r)) r.isKinematic = !state;
    }
    
}

