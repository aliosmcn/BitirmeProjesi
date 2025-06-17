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
        canNext = true;
    }

    #endregion Singleton

    public bool canNext = true;
    public bool canPrevious = false;

    [SerializeField] private GameObject bookUI;
    
    private Animator animator;

    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void NextPage()
    {
        if (canNext)
            animator.SetTrigger("Sonraki");
    }

    public void PreviousPage()
    {
        if (canPrevious)
            animator.SetTrigger("Onceki");
    }

    //AnimationEvents
    public void SetCanNext()
    {
        canNext = !canNext;
    }
    public void SetCanPrevious()
    {
        canPrevious = !canPrevious;
    }
}
