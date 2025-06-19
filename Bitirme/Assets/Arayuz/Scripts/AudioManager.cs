using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class MusicEntry
    {
        public string key;
        public AudioClip clip;
        [Range(0,1)] public float volume = 0.5f;
        public bool loop = true;
    }

    [System.Serializable]
    public class SFXEntry
    {
        public string key;
        public AudioClip clip;
        [Range(0,1)] public float volume = 1f;
        public bool is3D = true;
        public bool loop = false;      
        public float minDistance = 1f;
        public float maxDistance = 20f;
    }

    [Header("Music (2D)")]
    public AudioSource musicSource;
    public MusicEntry[] musicEntries;

    [Header("SFX")]
    public SFXEntry[] sfxEntries;

    Dictionary<string, MusicEntry> musicMap;
    Dictionary<string, SFXEntry> sfxMap;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicMap = new Dictionary<string, MusicEntry>();
        foreach (var e in musicEntries) if (e.clip != null) musicMap[e.key] = e;

        sfxMap = new Dictionary<string, SFXEntry>();
        foreach (var e in sfxEntries) if (e.clip != null) sfxMap[e.key] = e;
    }

    public void PlayMusic(string key)
    {
        if (!musicMap.TryGetValue(key, out var e)) return;
        if (musicSource.clip == e.clip) return;
        musicSource.clip = e.clip;
        musicSource.volume = e.volume;
        musicSource.loop = e.loop;
        musicSource.spatialBlend = 0f; // 2D müzik
        musicSource.Play();
    }

    public void PlaySFX(string key, GameObject caller)
    {
        if (caller == null || !sfxMap.TryGetValue(key, out var e)) return;

        var src = caller.AddComponent<AudioSource>();
        src.clip = e.clip;
        src.volume = e.volume;
        src.spatialBlend = e.is3D ? 1f : 0f;
        src.loop = e.loop;                        
        if (e.is3D)
        {
            src.rolloffMode = AudioRolloffMode.Linear;
            src.minDistance = e.minDistance;
            src.maxDistance = e.maxDistance;
        }
        src.Play();

        if (!e.loop)
            Destroy(src, e.clip.length + .1f);
    }
    
    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }

    public void StopSFX(string key, GameObject caller)
    {
        if (!sfxMap.ContainsKey(key) || caller == null) return;

        var clip = sfxMap[key].clip;
        foreach (var src in caller.GetComponents<AudioSource>())
        {
            if (src.clip == clip)
            {
                src.Stop();
                Destroy(src);
            }
        }
    }
}
