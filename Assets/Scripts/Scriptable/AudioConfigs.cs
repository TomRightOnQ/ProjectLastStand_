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

    // SoundSFXRegion
    [SerializeField] private AudioClip buttonSFX;
    [SerializeField] private AudioClip gunFireSFX;
    [SerializeField] private AudioClip gunFire2SFX;
    [SerializeField] private AudioClip gunFire3SFX;
    [SerializeField] private AudioClip gunSelect;
    [SerializeField] private AudioClip magicFireSFX;
    [SerializeField] private AudioClip magicFire2SFX;
    [SerializeField] private AudioClip missileFireSFX;
    [SerializeField] private AudioClip missileFire2SFX;
    [SerializeField] private AudioClip explosionSFX;
    [SerializeField] private AudioClip explosion2SFX;
    [SerializeField] private AudioClip explosion3SFX;
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip death2SFX;
    [SerializeField] private AudioClip laserSFX;
    [SerializeField] private AudioClip burningSFX;
    [SerializeField] private AudioClip burning2SFX;
    [SerializeField] private AudioClip meleeSFX;

    public AudioClip ButtonSFX => buttonSFX;
    public AudioClip GunFireSFX => gunFireSFX;
    public AudioClip GunFire2SFX => gunFire2SFX;
    public AudioClip GunFire3SFX => gunFire3SFX;
    public AudioClip GunSelect => gunSelect;
    public AudioClip MagicFireSFX => magicFireSFX;
    public AudioClip MagicFire2SFX => magicFire2SFX;
    public AudioClip MissileFireSFX => missileFireSFX;
    public AudioClip MissileFire2SFX => missileFire2SFX;
    public AudioClip ExplosionSFX => explosionSFX;
    public AudioClip Explosion2SFX => explosion2SFX;
    public AudioClip Explosion3SFX => explosion3SFX;
    public AudioClip HitSFX => hitSFX;
    public AudioClip DeathSFX => deathSFX;
    public AudioClip Death2SFX => death2SFX;
    public AudioClip LaserSFX => laserSFX;
    public AudioClip BurningSFX => burningSFX;
    public AudioClip Burning2SFX => burning2SFX;
    public AudioClip MeleeSFX => meleeSFX;

    public AudioClip GetAudio(int id)
    {
        switch (id)
        {
            case 0:
                return ButtonSFX;
            case 1:
                return GunFireSFX;
            case 2:
                return GunFire2SFX;
            case 3:
                return GunFire3SFX;
            case 4:
                return GunSelect;
            case 5:
                return MagicFireSFX;
            case 6:
                return MagicFire2SFX;
            case 7:
                return MissileFireSFX;
            case 8:
                return MissileFire2SFX;
            case 9:
                return ExplosionSFX;
            case 10:
                return Explosion2SFX;
            case 11:
                return Explosion3SFX;
            case 12:
                return HitSFX;
            case 13:
                return DeathSFX;
            case 14:
                return Death2SFX;
            case 15:
                return LaserSFX;
            case 16:
                return BurningSFX;
            case 17:
                return Burning2SFX;
            case 18:
                return MeleeSFX;
            default:
                return null;
        }
    }
}
