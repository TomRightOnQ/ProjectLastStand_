using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;

// Manage all audio files
[CreateAssetMenu(menuName = "Configs/AudioConfigs")]
public class AudioConfigs : ScriptableSingleton<AudioConfigs>
{
    // Music Type
    // 0: Menu
    // 1: Loading
    // 2: Battle;

    private const string MUSIC_FILE_NAME = "MusicConfigs.json";
    private string AUDIO_PATH = "StreamingAssets/Audio/";

    public List<MusicConfig> MenuMusicList = new List<MusicConfig>();
    public List<MusicConfig> LoadingMusicList = new List<MusicConfig>();
    public List<MusicConfig> BattleMusicList = new List<MusicConfig>();

    public struct MusicConfig
    {
        public int id;
        public string name;
        public int type;
        public AudioClip clip;
    }

    public void InitMusic()
    {
        string path = Path.Combine(Application.streamingAssetsPath, MUSIC_FILE_NAME);
        string json = File.ReadAllText(path);
        MusicConfig[] configs = JsonConvert.DeserializeObject<MusicConfig[]>(json);

        foreach (MusicConfig config in configs)
        {
            string musicPath = Path.Combine(AUDIO_PATH, config.name + ".ogg");
            switch (config.type)
            {
                case 0:
                    MenuMusicList.Add(config);
                    break;
                case 1:
                    LoadingMusicList.Add(config);
                    break;
                case 2:
                    BattleMusicList.Add(config);
                    break;
            }
        }
        Debug.Log("Music Load Ready");
    }

    // Randomly get a music with specifc type
    public MusicConfig GetMusic(int type)
    {
        List<MusicConfig> musicList = MenuMusicList;
        switch (type)
        {
            case 0:
                musicList = MenuMusicList;
                break;
            case 1:
                musicList = LoadingMusicList;
                break;
            case 2:
                musicList = BattleMusicList;
                break;
        }
        int randomIndex = UnityEngine.Random.Range(0, musicList.Count);
        return musicList[randomIndex];
    }
}
