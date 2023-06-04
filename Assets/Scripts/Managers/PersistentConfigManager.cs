using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// Loading
public class PersistentConfigManager : MonoBehaviour
{
    private static PersistentConfigManager instance;
    private bool isLoaded = false;

    private string[] MUSIC_PATH = new string[3] { "Audio/Menu", "Audio/Loading", "Audio/Battle" };

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        UpgradeConfigs.Instance.InitEffect();
        AudioConfigs.Instance.InitMusic();
        yield return StartCoroutine(LoadMusicClips());
        isLoaded = true;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("MainMenu");
    }

    private IEnumerator LoadMusicClips()
    {

        AudioConfigs audioConfigs = AudioConfigs.Instance;

        // Load menu music clips
        for (int i = 0; i < audioConfigs.MenuMusicList.Count; i++)
        {
            AudioConfigs.MusicConfig musicConfig = audioConfigs.MenuMusicList[i];
            string path = Path.Combine(MUSIC_PATH[musicConfig.type], musicConfig.name + ".ogg");
            yield return StartCoroutine(LoadMusicClip(path, clip =>
            {
                musicConfig.clip = clip;
                audioConfigs.MenuMusicList[i] = musicConfig; // Update the struct in AudioConfigs
            }));
        }

        // Load loading music clips
        for (int i = 0; i < audioConfigs.LoadingMusicList.Count; i++)
        {
            AudioConfigs.MusicConfig musicConfig = audioConfigs.LoadingMusicList[i];
            string path = Path.Combine(MUSIC_PATH[musicConfig.type], musicConfig.name + ".ogg");
            yield return StartCoroutine(LoadMusicClip(path, clip =>
            {
                musicConfig.clip = clip;
                audioConfigs.LoadingMusicList[i] = musicConfig; // Update the struct in AudioConfigs
            }));
        }

        // Load battle music clips
        for (int i = 0; i < audioConfigs.BattleMusicList.Count; i++)
        {
            AudioConfigs.MusicConfig musicConfig = audioConfigs.BattleMusicList[i];
            string path = Path.Combine(MUSIC_PATH[musicConfig.type], musicConfig.name + ".ogg");
            yield return StartCoroutine(LoadMusicClip(path, clip =>
            {
                musicConfig.clip = clip;
                audioConfigs.BattleMusicList[i] = musicConfig; // Update the struct in AudioConfigs
            }));
        }
    }

    private IEnumerator LoadMusicClip(string path, System.Action<AudioClip> callback)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, path);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                callback?.Invoke(clip);
            }
            else
            {
                Debug.LogError("Failed to load music clip: " + path);
            }
        }
    }


    // Method to check if the loading process is complete
    public static bool IsLoaded()
    {
        return instance != null && instance.isLoaded;
    }
}
