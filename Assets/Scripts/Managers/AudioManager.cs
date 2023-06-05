using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Photon.Pun;
using static AudioConfigs;
using System.Collections.Generic;

// Control the audio
public class AudioManager : MonoBehaviourPun
{
    [SerializeField] private AudioSource musicAudioSource;

    // Mixer groups
    [SerializeField] private AudioMixer masterMixer;
    private const string MASTER_VOL = "masterVolume";
    private const string MUSIC_VOL = "musicVolume";
    private const string FX_VOL = "fxVolume";

    private bool isChangingMusic = false;
    public static AudioManager Instance;

    private float prevMasterVol = 25;
    private float prevMusicVol = 25;
    private float prevFxVol = 25;

    private int currentSceneType = 1;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        musicAudioSource.Play();
    }

    private void ChangeMusic()
    {
        isChangingMusic = true;
        musicAudioSource.Stop();
        MusicConfig config = AudioConfigs.Instance.GetMusic(currentSceneType);
        musicAudioSource.clip = config.clip;
        musicAudioSource.Play();
        isChangingMusic = false;
    }

    private void Update()
    {
        int newSceneType = UpdateSceneType();
        if ((currentSceneType != newSceneType || !musicAudioSource.isPlaying) && !isChangingMusic)
        {
            currentSceneType = newSceneType;
            ChangeMusic();
        }
        UpdateVolume(GameSettings.Instance.MasterVol, ref prevMasterVol, MASTER_VOL);
        UpdateVolume(GameSettings.Instance.MusicVol, ref prevMusicVol, MUSIC_VOL);
        UpdateVolume(GameSettings.Instance.FxVol, ref prevFxVol, FX_VOL);
    }

    private int UpdateSceneType()
    {
        string name = SceneManager.GetActiveScene().name;
        switch (name)
        {
            case "Loading":
                return 1;
            case "MainMenu":
                return 0;
            case "GameMain":
                return 2;
            case "Scoreboard":
                return 0;
            case "MultiplayerLobby":
                return 0;
            case "Tutorial":
                return 2;
            default:
                return 0;
        }
    }

    private void UpdateVolume(float volume, ref float prevVolume, string mixerParam)
    {
        if (volume != prevVolume)
        {
            SetVolume(mixerParam, volume);
            prevVolume = volume;
        }
    }

    private void SetVolume(string mixerParam, float volume)
    {
        masterMixer.SetFloat(mixerParam, Mathf.Log10(volume) * 20f);
    }

    public void PlaySound(int id, Vector3 pos)
    {
        if (id < 0) {
            return;
        }
        SFXObjects sfx = GameManager.Instance.dataManager.TakeSfxPool();
        if (sfx == null)
        {
            return;
        }
        sfx.transform.position = pos;
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPCPlaySound", RpcTarget.Others, id, pos);
        }
        sfx.SetUp(id);
    }

    [PunRPC]
    public void RPCPlaySound(int id, Vector3 pos)
    {
        if (id < 0)
        {
            return;
        }
        SFXObjects sfx = GameManager.Instance.dataManager.TakeSfxPool();
        if (sfx == null)
        {
            return;
        }
        sfx.transform.localPosition = pos;
        sfx.SetUp(id);
    }
}
