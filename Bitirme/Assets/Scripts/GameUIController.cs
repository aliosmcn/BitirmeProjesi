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
    [SerializeField] private Image directiveImg;
    [SerializeField] private Text directiveText;
    
    [SerializeField] private GameObject kazanBar;
    [SerializeField] private GameObject crowBar;
    [SerializeField] private GameObject bookBar;
    [SerializeField] private GameObject openCloseBar;
    
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
            if (item.prefab && item.prefab.gameObject.CompareTag("item"))
            {
                interactiveBar.SetActive(true);
                directiveText.text = item.UIText;
                directiveImg.sprite = item.ItemIcon;
            }
            else
            {
                switch (item.ItemID)
                {
                    case "Kazan":
                        kazanBar.SetActive(true);
                        break;
                    case "Book":
                        bookBar.SetActive(true);
                        break;
                    case "Crow":
                        crowBar.SetActive(true);
                        break;
                    case "OpenClose":
                        openCloseBar.SetActive(true);
                        break;
                    default:
                        kazanBar.SetActive(false);
                        bookBar.SetActive(false);
                        crowBar.SetActive(false);
                        break;
                }
                
            }
        }
        else
        {
            interactiveBar.SetActive(false);
            kazanBar.SetActive(false);
            bookBar.SetActive(false);
            crowBar.SetActive(false);
            openCloseBar.SetActive(false);
        }
    }

    public void SetInteractive(bool state, string text, Sprite img)
    {
        if (img)
        {
            interactiveBar.SetActive(state);
            directiveImg.sprite = img;
            directiveText.text = text;
        }
        else
        {
            interactiveBar.SetActive(false);
        }
        
    }

    
}
