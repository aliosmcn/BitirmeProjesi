using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/ItemSOEvent", fileName = "newItemSOEvent")]
public class ItemSOEvent : ScriptableObject
{
    private Action<ItemSO> _action;

    public void AddListener(Action<ItemSO> action)
    {
        this._action += action;
    }

    public void RemoveListener(Action<ItemSO> action)
    {
        this._action -= action;
    }

    public void Raise(ItemSO value)
    {
        this._action?.Invoke(value);
    }
}