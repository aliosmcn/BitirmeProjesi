using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    #region Singleton
    private static GameUIController instance;

    public static GameUIController Instance
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
    [SerializeField] private ItemSOEvent onLookingItem;
    
    [Header("Directive")]
    [SerializeField] private GameObject bar;
    [SerializeField] private Image directiveImg;
    [SerializeField] private Text directiveText;
    
    public GameObject crosshair;

    
    private void OnEnable()
    {
        onLookingItem.AddListener(GetInteractive);
    }

    private void OnDisable()
    {
        onLookingItem.RemoveListener(GetInteractive);
    }

    private void Start()
    {
        bar.SetActive(false);
    }

    public void GetInteractive(ItemSO item)
    {
        if (item)
        {
            bar.SetActive(true);
            directiveText.text = item.ItemID;
            //directiveImg.sprite = item.ItemIcon;
        }
        else
        {
            bar.SetActive(false);
        }
    }

    public void SetInteractive(bool state, string text, Sprite img)
    {
        bar.SetActive(state);
        directiveImg.sprite = img;
        directiveText.text = text;
    }
    
}
