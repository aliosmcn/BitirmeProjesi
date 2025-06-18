using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject creditsPanel;
    
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
