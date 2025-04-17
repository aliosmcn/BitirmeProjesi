using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject previewObject;
    
    private Canvas ui;
    private Outline outline;
    
    private void Start()
    {
        ui = GetComponentInChildren<Canvas>();
        outline = GetComponent<Outline>();
        outline.enabled = false;
        ui.enabled = false;
    }

    public void Highlight(bool state)
    {
        outline.enabled = state;
        ui.enabled = state;
    }

    public void ShowPreview(RaycastHit hit, bool state)
    {
        if (gameObject.CompareTag("item"))
        {
            previewObject.transform.position = hit.point;
            previewObject.transform.rotation = Quaternion.identity;
            previewObject.SetActive(state);
        }
    }

}
