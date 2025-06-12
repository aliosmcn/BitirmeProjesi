using System;
using UnityEngine;

public class RotateUI : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    private void LateUpdate() => transform.forward = cam.transform.forward;
}
