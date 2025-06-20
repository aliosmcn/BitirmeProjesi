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
    
    private Animator animator;

    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void NextPage()
    {
        if (canNext)
        {
            AudioManager.Instance.PlaySFX("Sayfa", gameObject);
            animator.SetTrigger("Sonraki");
        }
            
    }

    public void PreviousPage()
    {
        if (canPrevious)
        {
            AudioManager.Instance.PlaySFX("Sayfa", gameObject);
            animator.SetTrigger("Onceki");
        }
            
    }

    public void SetCanNext()
    {
        canNext = !canNext;
    }
    public void SetCanPrevious()
    {
        canPrevious = !canPrevious;
    }

    public void DefaultState()
    {
        animator.SetTrigger("BasaDon");
    }
}
