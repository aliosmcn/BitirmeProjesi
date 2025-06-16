using System;
using UnityEngine;

public class ReverseTime : MonoBehaviour
{
    #region Singleton
    private static ReverseTime instance;

    public static ReverseTime Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion Singleton
    
    

    private void OnEnable()
    {
        throw new NotImplementedException();
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
    }
}
