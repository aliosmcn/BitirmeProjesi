using System;
using UnityEngine;

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
    
    public DaySO[] days;
    public DaySO currentDay;
    private int currentIndex = 0;
    
    [Header("OrderEvents")] 
    [SerializeField] private VoidEvent onDayStarted;    
    [SerializeField] private VoidEvent onDayFinished;
    [SerializeField] private VoidEvent onOrderCorrect;

    [HideInInspector] public bool isOpen = false;
    [HideInInspector] public bool canClose = false;
    
    public int correctCount = 0;

    

    private void Start()
    {
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
        Debug.Log("Acildi");
        isOpen = true;
        canClose = false;
        onDayStarted.Raise();
    }

    public void OnClose()
    {
        Debug.Log("Kapandi");
        if(!canClose) return;
        correctCount = 0;
        isOpen = false;
        UpdateDay();
        onDayFinished.Raise();
    }

    private void UpdateCanClose()
    {
        correctCount++;
        if(correctCount == currentDay.CustomerCount)
            canClose = true;
    }
}
