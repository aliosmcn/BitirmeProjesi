using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
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

}
