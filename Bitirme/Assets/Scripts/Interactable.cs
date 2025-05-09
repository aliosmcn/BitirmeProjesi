using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject previewObject;
    
    private Outline outline;
    private Canvas canvas;
    
    [SerializeField] private ItemSO itemData;

    public ItemSO ItemData
    {
        get
        {
            return itemData;
        }
    }
    
    private void Awake()
    {
        outline = GetComponent<Outline>();
        canvas = GetComponentInChildren<Canvas>();
        OutlineCanvasState(false);
        SetPreviewState(false);
    }

    public void OutlineCanvasState(bool state)
    {
        outline.enabled = state;
        canvas.enabled = state;
    }


    public void SetPreviewState(bool state, Vector3 position = default)
    {
        if(!previewObject) return;
        
        previewObject.SetActive(state);
        if(state) previewObject.transform.SetPositionAndRotation(position, Quaternion.identity);
    }
}