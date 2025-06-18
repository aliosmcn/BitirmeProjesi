using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/IntEvent", fileName = "newIntEvent")]

public class IntEvent : ScriptableObject
{
    private Action<int> _action;

    public void AddListener(Action<int> action)
    {
        this._action += action;
    }

    public void RemoveListener(Action<int> action)
    {
        this._action -= action;
    }

    public void Raise(int value)
    {
        this._action?.Invoke(value);
    }
}