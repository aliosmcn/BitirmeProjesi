using UnityEngine;

public class Energy : MonoBehaviour
{
    #region Singleton
    private static Energy instance;

    public static Energy Instance
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
    
    
}
