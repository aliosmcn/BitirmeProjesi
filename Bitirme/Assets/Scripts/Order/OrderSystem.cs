using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    [HideInInspector] 
    public bool canReverseTime = false;
    public bool isDayStarted = false;

    private int remainingOrders;
    private OrderSO currentOrder;
    
    
    
    private void OnEnable()
    {
        onDayStarted.AddListener(DayStarted);
        onDayFinished.AddListener(DayFinished);
        
        onOrderCorrect.AddListener(CorrectOrder);
        onOrderFail.AddListener(FailOrder);
        onReverseTime.AddListener(ReverseTime);
        onClickGate.AddListener(StartOrEnd);
    }

    private void OnDisable()
    {
        onDayStarted.RemoveListener(DayStarted);
        onDayFinished.RemoveListener(DayFinished);
        
        onOrderCorrect.RemoveListener(CorrectOrder);
        onOrderFail.RemoveListener(FailOrder);
        onReverseTime.RemoveListener(ReverseTime);
        onClickGate.RemoveListener(StartOrEnd);
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
        CreateNewOrder();
        
    }

    private void DayFinished()
    {
        currentOrder = null;
        
        //gun sonu ui acilacak
    }

    private void CorrectOrder()
    {
        currentOrder = null;
        
        //hayat enerjisi artacak
        
        CreateNewOrder();
    }

    private void FailOrder()
    {
        //musteri bayilma animasyonu.
        //hayat enerjisi azalacak
        
        canReverseTime = true;
    }


    private void CreateNewOrder()
    {
        if (currentOrder) return;
        
        currentOrder = Orders[Random.Range(0, Orders.Count)];
        Instantiate(currentOrder, transform.position, Quaternion.identity);
        //musteri ui aktif olsun
    }
    
    private void ReverseTime()
    {
        
        canReverseTime = false;
    }
    
}
