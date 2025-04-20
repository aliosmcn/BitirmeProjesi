using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Global event manager for interaction events.
/// </summary>
public static class EventManager
{
    public static event Action<Interactable> OnPickUp;
    public static event Action<Interactable, Vector3> OnPlace;
    public static event Action<Interactable> OnDrop;

    public static void TriggerPickUp(Interactable obj)    => OnPickUp?.Invoke(obj);
    public static void TriggerPlace(Interactable obj, Vector3 pos) => OnPlace?.Invoke(obj, pos);
    public static void TriggerDrop(Interactable obj)      => OnDrop?.Invoke(obj);
}