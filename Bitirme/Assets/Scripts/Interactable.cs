using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject previewObject;
    
    private Outline outline;
    private Canvas canvas;
    
    private void Awake()
    {
        TryGetComponent(out Outline outl);
        if (outl) outline = outl;
        canvas = GetComponentInChildren<Canvas>();
        OutlineCanvasState(false);
        SetPreviewState(false);
    }

    public void OutlineCanvasState(bool state)
    {
        if (outline) outline.enabled = state;
        if (canvas) canvas.enabled = state;
    }


    public void SetPreviewState(bool state, Vector3 position = default)
    {
        if(!previewObject) return;
        
        previewObject.SetActive(state);
        if(state) previewObject.transform.SetPositionAndRotation(position, Quaternion.identity);
    }
}