using UnityEngine;

public class Switcher : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEvent onSwitchCrow;
    [SerializeField] private VoidEvent onSwitchCharacter;
    
    [Header("InteractiveUI")]
    [SerializeField] private Sprite sprite;

    
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject crow;
    [SerializeField] private GameObject crowCam;
    [SerializeField] private float switchDistance = 5f;

    [SerializeField] private Transform crowTarget;
    [SerializeField] private Transform playerTarget;

    private bool isPlayerActive = true;
    
    

    void Start()
    {
        isPlayerActive = true;
        playerCam.SetActive(true);
        crow.SetActive(false);
    }

    void Update()
    {
        // Check if Tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Check distance between the switcher object and characters
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float distanceToCrow = Vector3.Distance(transform.position, crow.transform.position);

            // If player is active and within range, switch to crow
            if (isPlayerActive && distanceToPlayer <= switchDistance)
            {
                SwitchToCrow();
                onSwitchCrow.Raise();
            }
            // If crow is active and within range, switch to player
            else if (!isPlayerActive && distanceToCrow <= switchDistance)
            {
                SwitchToPlayer();
            }
        }
    }

    void SwitchToCrow()
    {
        SetActivePlayers(false);
        crow.transform.position = crowTarget.position; 
        isPlayerActive = false;
        
        GameUIController.Instance.SetInteractive(false, "SWITCH", sprite);
        GameUIController.Instance.crosshair.SetActive(false);
    }

    void SwitchToPlayer()
    {
        SetActivePlayers(true);
        player.transform.position = playerTarget.position; 
        isPlayerActive = true;
        GameUIController.Instance.SetInteractive(false, "SWITCH", sprite);
        GameUIController.Instance.crosshair.SetActive(true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, switchDistance);
    }

    void SetActivePlayers(bool state)
    {
        player.SetActive(state);
        playerCam.SetActive(state);
        crow.SetActive(!state);
        crowCam.SetActive(!state);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIController.Instance.SetInteractive(true, "SWITCH", sprite);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIController.Instance.SetInteractive(false, "SWITCH", sprite);
        }
    }
}