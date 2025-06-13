using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("InteractiveUI")]
    [SerializeField] private Sprite sprite;

    
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject crow;
    [SerializeField] private float switchDistance = 5f;

    private bool isPlayerActive = true;
    
    

    void Start()
    {
        isPlayerActive = true;
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
        player.SetActive(false);
        crow.SetActive(true);
        crow.transform.position = player.transform.position; 
        isPlayerActive = false;
        GameUIController.Instance.SetInteractive(false, "Switch", sprite);
        GameUIController.Instance.crosshair.SetActive(false);
    }

    void SwitchToPlayer()
    {
        crow.SetActive(false);
        player.SetActive(true);
        player.transform.position = crow.transform.position; 
        isPlayerActive = true;
        GameUIController.Instance.SetInteractive(false, "Switch", sprite);
        GameUIController.Instance.crosshair.SetActive(true);
    }

    // Optional: Visualize the switch distance in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, switchDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIController.Instance.SetInteractive(true, "Switch", sprite);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIController.Instance.SetInteractive(false, "Switch", sprite);
        }
    }
}