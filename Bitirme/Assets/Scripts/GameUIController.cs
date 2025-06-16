using System;
using UnityEngine;
using UnityEngine.Serialization;
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
    
    [FormerlySerializedAs("bar")]
    [Header("Directive")]
    [SerializeField] private GameObject interactiveBar;
    [SerializeField] private GameObject kazanBar;
    [SerializeField] private GameObject crowBar;
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
        interactiveBar.SetActive(false);
    }

    public void GetInteractive(ItemSO item)
    {
        if (item)
        {
            interactiveBar.SetActive(true);
            directiveText.text = item.ItemID;
            //directiveImg.sprite = item.ItemIcon;
        }
        else
        {
            interactiveBar.SetActive(false);
        }
    }

    public void SetInteractive(bool state, string text, Sprite img)
    {
        interactiveBar.SetActive(state);
        directiveImg.sprite = img;
        directiveText.text = text;
    }

    public void UpdateUI(string name, bool state)
    {
        switch (name)
        {
            case "kazan":
                kazanBar.SetActive(state);
                break;
            case "crow":
                crowBar.SetActive(state);
                break;
            default:
                break;
        }
    }
    
}
