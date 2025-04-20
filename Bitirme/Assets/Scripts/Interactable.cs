using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject previewObject;
    
    private Outline outline;
    private Canvas canvas;
    
    private void Awake()
    {
        outline = GetComponent<Outline>();
        canvas = GetComponentInChildren<Canvas>();
        OutlineCanvasState(false);
        HidePreview();
    }

    public void OutlineCanvasState(bool state)
    {
        outline.enabled = state;
        canvas.enabled = state;
    }


    public void ShowPreview(Vector3 worldPos)
    {
        previewObject.SetActive(true);
        previewObject.transform.position = worldPos;
        previewObject.transform.rotation = Quaternion.identity;
    }

    public void HidePreview()
    {
        previewObject.SetActive(false);
    }
}