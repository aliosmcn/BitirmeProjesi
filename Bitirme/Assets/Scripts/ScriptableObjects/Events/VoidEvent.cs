using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/VoidEvent", fileName = "newVoidEvent")]

public class VoidEvent : ScriptableObject
{
    private Action _action;

    public void AddListener(Action action)
    {
        this._action += action;
    }

    public void RemoveListener(Action action)
    {
        this._action -= action;
    }

    public void Raise()
    {
        this._action?.Invoke();
    }
}