using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject creditsPanel;

    private void Start()
    {
        creditsPanel.SetActive(false);
    }

    private void Update()
    {
        if (!creditsPanel) return;
        if (Input.GetKeyDown(KeyCode.Escape)) creditsPanel.SetActive(false);
    }

    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreditsButton(bool state)
    {
        creditsPanel.SetActive(state);
    }
    
    public void QuitButton()
    {
        Application.Quit();
    }
}
