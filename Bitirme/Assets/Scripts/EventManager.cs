using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Global event manager for interaction events.
/// </summary>
public static class EventManager
{
    public static Action<Interactable> OnPickUp;
    public static Action<Interactable, Vector3> OnPlace;
    public static Action<Interactable> OnDrop;

    public static Action OnMixing ,OnMixed;
}