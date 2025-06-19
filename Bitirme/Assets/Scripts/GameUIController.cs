using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        AudioManager.Instance.PlayMusic("GameMusic");
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
    [SerializeField] private GameObject reverseBar;
    [SerializeField] private GameObject deliverBar;
    [SerializeField] private GameObject orderTextBar;
    [SerializeField] private Text orderText;

    [SerializeField] private GameObject pausePanel;
    
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
        this.enabled = true;
        interactiveBar.SetActive(false);
        orderTextBar.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if(orderTextBar.activeSelf)
        {   
            orderTextBar.SetActive(false); 
            AudioManager.Instance.PlaySFX("Button", gameObject);
            return;
        }

        if (!pausePanel.activeSelf)
        {
            AudioManager.Instance.PlaySFX("Button", gameObject);
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        if (pausePanel.activeSelf) 
        {
            AudioManager.Instance.PlaySFX("Button", gameObject);
            pausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }
    }

    public void SetOrderText(OrderSO order, bool state)
    {
        if(order) orderText.text = order.orderText;
        orderTextBar.SetActive(state);
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
                    case "Reverse":
                        reverseBar.SetActive(true);
                        break;
                    case "Delivery":
                        deliverBar.SetActive(true);
                        break;
                    default:
                        kazanBar.SetActive(false);
                        bookBar.SetActive(false);
                        crowBar.SetActive(false);
                        openCloseBar.SetActive(false);
                        reverseBar.SetActive(false);
                        deliverBar.SetActive(false);
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
            reverseBar.SetActive(false);
            deliverBar.SetActive(false);
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

    public void MainMenuButton()
    {
        AudioManager.Instance.PlaySFX("Button", gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void ContinueButton()
    {
        AudioManager.Instance.PlaySFX("Button", gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausePanel.SetActive(false);
    }
}
