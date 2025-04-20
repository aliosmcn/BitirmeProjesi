using System;
using UnityEngine;

public class RotateUI : MonoBehaviour
{
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate() => transform.forward = cam.transform.forward;
}
