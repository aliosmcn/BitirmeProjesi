using UnityEngine;

public class Book : MonoBehaviour
{
    #region Singleton
    private static Book instance;

    public static Book Instance
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

    [SerializeField] private GameObject bookUI;
    
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void NextPage()
    {
        animator.SetTrigger("Sonraki");
    }

    public void PreviousPage()
    {
        animator.SetTrigger("Onceki");
    }
    
}
