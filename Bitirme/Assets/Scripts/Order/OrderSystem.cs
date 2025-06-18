using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderSystem : MonoBehaviour
{
    #region Singleton
    private static OrderSystem instance;

    public static OrderSystem Instance
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
    
    [Header("Events")]
    [SerializeField] private VoidEvent onDayStarted;    
    [SerializeField] private VoidEvent onDayFinished;   
    [SerializeField] private VoidEvent onOrderCorrect;  //teslim scriptinde raise olacak
    [SerializeField] private VoidEvent onOrderFail;     //teslim scriptinde raise olacak
    [SerializeField] private VoidEvent onReverseTime;   //cadi kuresinde raise olacak
    [SerializeField] private VoidEvent onClickGate;
    
    [SerializeField] private List<OrderSO> Orders;

    [SerializeField] private Transform customerPos;
    
    [HideInInspector] 
    public bool canReverseTime = false;
    public bool isDayStarted = false;

    private int remainingOrders;
    private OrderSO currentOrder;
    private GameObject newCustomer;
    
    
    private void OnEnable()
    {
        onDayStarted.AddListener(DayStarted);
        onDayFinished.AddListener(DayFinished);
        
       //onOrderCorrect.AddListener(CorrectOrder);
        onOrderFail.AddListener(FailOrder);
        onReverseTime.AddListener(ReverseTime);
        onClickGate.AddListener(StartOrEnd);
    }

    private void OnDisable()
    {
        onDayStarted.RemoveListener(DayStarted);
        onDayFinished.RemoveListener(DayFinished);
        
        //onOrderCorrect.RemoveListener(CorrectOrder);
        onOrderFail.RemoveListener(FailOrder);
        onReverseTime.RemoveListener(ReverseTime);
        onClickGate.RemoveListener(StartOrEnd);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CorrectOrder();
        }
    }

    private void StartOrEnd()
    {
        if (!isDayStarted)
        {
            DayStarted();
        }
        else if (isDayStarted && remainingOrders == 0)
        {
            DayFinished();
        }
    }

    private void DayStarted()
    {
        Invoke(nameof(CreateNewOrder), 2f);
    }

    private void DayFinished()
    {
        currentOrder = null;
        
        //gun sonu ui acilacak
    }

    private void CorrectOrder()
    {
        if(DaySystem.Instance.canClose) return;
        
        if (newCustomer.TryGetComponent(out Animator animator))
        {
            Debug.Log("dogru siparis");
            animator.SetTrigger("Correct");
        }
        currentOrder = null;
        
        //hayat enerjisi artacak
        onOrderCorrect.Raise();
        
        if (DaySystem.Instance.canClose == false)
            Invoke(nameof(CreateNewOrder), 2f);
    }

    private void FailOrder()
    {
        if (newCustomer.TryGetComponent(out Animator animator))
        {
            animator.SetTrigger("Fail");
        }
        
        //hayat enerjisi azalacak
        
        canReverseTime = true;
    }


    private void CreateNewOrder()
    {
        if (currentOrder) return;
        if(newCustomer) Destroy(newCustomer);
        currentOrder = Orders[Random.Range(0, Orders.Count)];
        newCustomer = Instantiate(currentOrder.Hasta, customerPos.position, customerPos.rotation);
        //musteri ui aktif olsuns
    }
    
    private void ReverseTime()
    {
        if (newCustomer.TryGetComponent(out Animator animator))
        {
            animator.SetTrigger("Revive");
        }
        
        canReverseTime = false;
    }
    
}
