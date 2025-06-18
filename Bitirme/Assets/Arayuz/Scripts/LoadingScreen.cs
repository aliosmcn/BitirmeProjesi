using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
    public Slider progressBar; 
    
    public string sceneName;

    void Start() {
        StartCoroutine(LoadNextSceneAsync(sceneName));
    }

    IEnumerator LoadNextSceneAsync(string scene) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false; 

        while (!asyncLoad.isDone) {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); 
            progressBar.value = progress;

            if (asyncLoad.progress >= 0.9f) {
                asyncLoad.allowSceneActivation = true; 
            }

            yield return null;
        }
    }
}