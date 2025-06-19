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
    [SerializeField] private VoidEvent onOrderCorrect;  
    [SerializeField] private VoidEvent onOrderFail;     
    [SerializeField] private VoidEvent onReverseTime;   
    [SerializeField] private VoidEvent onClickGate;
    [SerializeField] private IntEvent onEnergyChanged;
    
    [SerializeField] private List<OrderSO> Orders;

    [SerializeField] private Transform customerPos;
    
    [HideInInspector] 
    public bool canReverseTime = false;
    public bool isDayStarted = false;

    private int remainingOrders;
    public OrderSO currentOrder;
    private GameObject newCustomer;
    
    
    private void OnEnable()
    {
        onDayStarted.AddListener(DayStarted);
        onDayFinished.AddListener(DayFinished);
        
        onReverseTime.AddListener(ReverseTime);
        onClickGate.AddListener(StartOrEnd);
    }

    private void OnDisable()
    {
        onDayStarted.RemoveListener(DayStarted);
        onDayFinished.RemoveListener(DayFinished);
        
        onReverseTime.RemoveListener(ReverseTime);
        onClickGate.RemoveListener(StartOrEnd);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            CorrectOrder();
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
        onEnergyChanged.Raise(20);
        if(DaySystem.Instance.canClose) return;
        
        if (newCustomer.TryGetComponent(out Animator animator))
        {
            Debug.Log("dogru siparis");
            animator.SetTrigger("Correct");
        }
        currentOrder = null;
        
        GameUIController.Instance.SetOrderText(currentOrder, false);
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
        
        onOrderFail.Raise();
        onEnergyChanged.Raise(-15);
        
        DaySystem.Instance.canDeliver = false;
        canReverseTime = true;
    }

    public void CheckOrder(Interactable result)
    {
        if (canReverseTime) return;
        if (currentOrder && result.itemData == currentOrder.OrderRecipe.result)
            CorrectOrder();
        else if (currentOrder && result.itemData != currentOrder.OrderRecipe.result)
            FailOrder();
        
    }

    private void CreateNewOrder()
    {
        if (currentOrder) return;
        if(newCustomer) Destroy(newCustomer);
        currentOrder = Orders[Random.Range(0, Orders.Count)];
        newCustomer = Instantiate(currentOrder.Hasta, customerPos.position, customerPos.rotation);
        GameUIController.Instance.SetOrderText(currentOrder, true);
    }
    
    public void ReverseTime()
    {
        if (!canReverseTime) return;
        if (newCustomer.TryGetComponent(out Animator animator))
        {
            animator.SetTrigger("Revive");
        }

        DaySystem.Instance.canDeliver = true;
        canReverseTime = false;
    }
    
}
