using UnityEngine;

public class RotateUI : MonoBehaviour
{
    void LateUpdate() => transform.forward = Camera.main.transform.forward;
}
