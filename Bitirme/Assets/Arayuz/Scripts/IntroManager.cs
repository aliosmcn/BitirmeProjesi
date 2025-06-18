using System;
using UnityEngine;
using UnityEngine.TerrainTools;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour {
    public VideoPlayer videoPlayer;
    public string nextSceneName;

    void Start() {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            OnVideoEnd(videoPlayer);
    }

    void OnVideoEnd(VideoPlayer vp) {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}