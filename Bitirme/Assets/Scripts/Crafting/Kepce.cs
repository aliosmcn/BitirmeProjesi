using UnityEngine;

public class Kepce : MonoBehaviour
{
    #region Singleton
    private static Kepce instance;

    public static Kepce Instance
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

    
    public void OnAnimationEnd()
    {
        GetComponent<Animator>().SetBool("Mixing", false); 
        EventManager.OnMixed.Invoke();
    }
    
    
}
