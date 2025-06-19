using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject creditsPanel;

    private void Start()
    {
        creditsPanel.SetActive(false);
        AudioManager.Instance.PlayMusic("MenuMusic");
    }

    private void Update()
    {
        if (!creditsPanel) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            creditsPanel.SetActive(false);
            AudioManager.Instance.PlaySFX("Button", gameObject);
        }
    }

    public void StartButton()
    {
        AudioManager.Instance.PlaySFX("Button", gameObject);
        AudioManager.Instance.StopMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreditsButton(bool state)
    {
        AudioManager.Instance.PlaySFX("Button", gameObject);
        creditsPanel.SetActive(state);
    }
    
    public void QuitButton()
    {
        AudioManager.Instance.PlaySFX("Button", gameObject);
        Application.Quit();
    }
}
