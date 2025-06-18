using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject previewObject;
    
    private Outline outline;

    
    [SerializeField] public ItemSO itemData;

    public ItemSO ItemData
    {
        get
        {
            return itemData;
        }
    }


    private void Awake()
    {
        if (CompareTag("item"))
        {
            TryGetComponent(out Outline outl);
            if (outl) outline = outl;
            OutlineState(false);
            SetPreviewState(false);
        }
    }

    public void OutlineState(bool state)
    {
        if (outline) outline.enabled = state;
    }


    public void SetPreviewState(bool state, Vector3 position = default)
    {
        if(!previewObject) return;
        
        previewObject.SetActive(state);
        if(state) previewObject.transform.SetPositionAndRotation(position, Quaternion.identity);
    }
}