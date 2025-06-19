using System;
using UnityEngine;
using UnityEngine.UI;

public class DaySystem : MonoBehaviour
{
    #region Singleton
    private static DaySystem instance;

    public static DaySystem Instance
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

    [SerializeField] private Text remainingCustomerPanel;
    
    public DaySO[] days;
    public DaySO currentDay;
    private int currentIndex = 0;
    
    [Header("OrderEvents")] 
    [SerializeField] private VoidEvent onDayStarted;    
    [SerializeField] private VoidEvent onDayFinished;
    [SerializeField] private VoidEvent onOrderCorrect;

    [HideInInspector] public bool isOpen = false;
    [HideInInspector] public bool canDeliver = true;
    [HideInInspector] public bool canClose = false;
    
    public int correctCount = 0;

    [SerializeField] private Animator animator;

    private void Start()
    {
        remainingCustomerPanel.gameObject.SetActive(false);
        UpdateDay();
    }

    private void UpdateDay()
    {
        currentDay = days[currentIndex];
        currentIndex++;
    }
    
    private void OnEnable()
    {
        onOrderCorrect.AddListener(UpdateCanClose);
    }

    private void OnDisable()
    {
        onOrderCorrect.RemoveListener(UpdateCanClose);
    }
    
    public void OnOpen()
    {
        animator.SetTrigger("Open");
        remainingCustomerPanel.text = "Remanining Customer: " + (currentDay.CustomerCount - correctCount);
        remainingCustomerPanel.gameObject.SetActive(true);
        isOpen = true;
        canClose = false;
        onDayStarted.Raise();
    }

    public void OnClose()
    {
        if(!canClose) return;
        animator.SetTrigger("Close");
        remainingCustomerPanel.gameObject.SetActive(false);
        correctCount = 0;
        isOpen = false;
        UpdateDay();
        onDayFinished.Raise();
    }

    private void UpdateCanClose()
    {
        correctCount++;
        remainingCustomerPanel.text = "Remanining Customer: " + (currentDay.CustomerCount - correctCount);
        if (correctCount == currentDay.CustomerCount)
        {
            remainingCustomerPanel.text = "Remanining Customer: " + (currentDay.CustomerCount - correctCount) + " (Close the Shop)"; 
            canClose = true;
        }
            
    }
}
