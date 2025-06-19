using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    #region Singleton
    private static LoadingScreen instance;

    public static LoadingScreen Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
    
        instance = this;
        //DontDestroyOnLoad(gameObject);  
    }

    #endregion Singleton
    
    [Header("Settings")]
    public Slider progressBar;
    public float minLoadTime = 2f;      
    public float progressAccel = 0.9f;   
    public string nextSceneName;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "LoadingScreen")
        {
            StartCoroutine(LoadScene(nextSceneName));
        }
    } 

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        
        float timer = 0f;
        float progress = 0f;

        while (timer < minLoadTime || progress < 0.99f)
        {
            timer += Time.deltaTime;
            
            float realProgress = asyncLoad.progress / 0.9f;
            progress = Mathf.Lerp(progress, realProgress, progressAccel * Time.deltaTime);
            
            progressBar.value = progress;
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
    }
}