using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Events/OrderSOEvent", fileName = "newOrderSOEvent")]
public class OrderSOEvent : ScriptableObject
{
    private Action<OrderSO> _action;

    public void AddListener(Action<OrderSO> action)
    {
        this._action += action;
    }

    public void RemoveListener(Action<OrderSO> action)
    {
        this._action -= action;
    }

    public void Raise(OrderSO value)
    {
        this._action?.Invoke(value);
    }
}