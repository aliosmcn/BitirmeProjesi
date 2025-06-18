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
        instance = this;
    }

    #endregion Singleton
    
    [Header("Settings")]
    public Slider progressBar;
    public float minLoadTime = 2f;       // Minimum gösterim süresi (saniye)
    public float progressAccel = 0.9f;   // Progress hız çarpanı (0.1 = yavaş, 1 = hızlı)
    public string nextSceneName;

    void Start() => StartCoroutine(LoadScene());

    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;
        
        float timer = 0f;
        float progress = 0f;

        while (timer < minLoadTime || progress < 0.99f)
        {
            timer += Time.deltaTime;
            
            // Gerçek ve yapay progress karışımı
            float realProgress = asyncLoad.progress / 0.9f;
            progress = Mathf.Lerp(progress, realProgress, progressAccel * Time.deltaTime);
            
            progressBar.value = progress;
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
    }
}