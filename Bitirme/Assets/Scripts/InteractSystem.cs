using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public class InteractSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rayDistance = 2.2f;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float moveTime = 0.2f;
    [SerializeField] private LayerMask interactableLayer; // Added layer mask for more efficient raycasting

    private Camera cam;
    private Interactable currentHover;
    private Interactable holdObject;
    private Coroutine currentRoutine; 

    void Awake() => cam = Camera.main;

    void Update()
    {
        HandleRaycast();
        HandleInput();
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
                if (currentHover) currentHover.OutlineCanvasState(false);
                currentHover = hover;
                if (currentHover) currentHover.OutlineCanvasState(true);
            }

            if (holdObject) holdObject.SetPreviewState(true, hit.point); }
        else
        {
            if (currentHover)
            {
                currentHover.OutlineCanvasState(false);
                currentHover = null;
            }
            
            if (holdObject) holdObject.SetPreviewState(false);
        }
    }

    private void HandleInput()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        if (currentRoutine != null) return;

        if (holdObject)
        {
            holdObject.SetPreviewState(false);

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.TryGetComponent(out Kazan kazan) && kazan.alabiliyorMu)
                {
                    currentRoutine = StartCoroutine(MoveObjectRoutine(holdObject, Kazan.Instance.GetTransform().transform.position, false));
                    Kazan.Instance.PlaceObject(holdObject);
                    return;
                }
                currentRoutine = StartCoroutine(MoveObjectRoutine(holdObject, hit.point, false));  //PLACE
            }
            else
                Drop();
        }
        else if (currentHover)
        {
            currentRoutine = StartCoroutine(MoveObjectRoutine(currentHover, holdPoint.position, true)); //PICK
        }
    }

    //Smooth Pick / Place
    private IEnumerator MoveObjectRoutine(Interactable obj, Vector3 targetPos, bool pickup)
    {
        if (currentHover) currentHover.OutlineCanvasState(false);
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

    //Drop
    private void Drop()
    {
        holdObject.transform.SetParent(null);
        holdObject.transform.rotation = Quaternion.identity;
        SetPhysicsState(holdObject, true);
        holdObject.SetPreviewState(false);
        holdObject = null;
    }
    
    //HoldObject Fizikleri
    private void SetPhysicsState(Interactable obj, bool state)
    {
        if (obj.TryGetComponent(out Collider c)) c.enabled = state;
        if (obj.TryGetComponent(out Rigidbody r)) r.isKinematic = !state;
    }
    
    //YOK OLURSA
    private void OnDisable()
    {
        if (currentHover) currentHover.OutlineCanvasState(false);
        if (holdObject) Drop();
        if (currentRoutine != null) StopCoroutine(currentRoutine);
    }
}