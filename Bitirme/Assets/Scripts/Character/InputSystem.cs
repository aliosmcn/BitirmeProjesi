using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [SerializeField] private VoidEvent onEPressed;
    [SerializeField] private VoidEvent onSpacePressed;
    [SerializeField] private VoidEvent onCtrlPressed;
    [SerializeField] private VoidEvent onLeftClickPressed;
    [SerializeField] private VoidEvent onRightClickPressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) onEPressed.Raise();
        if (Input.GetKeyDown(KeyCode.Space)) onSpacePressed.Raise();
        if (Input.GetKeyDown(KeyCode.LeftControl)) onCtrlPressed.Raise();
        if (Input.GetMouseButtonDown(0)) onLeftClickPressed.Raise();
        if (Input.GetMouseButtonDown(1)) onRightClickPressed.Raise();
    }
}
