using UnityEngine;

// Online game settings
[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableSingleton<GameSettings>
{
    [SerializeField] private string gameVersion = "Dev 0.1";
    [SerializeField] private string nickName = "Player";

    // Audio Settings
    [SerializeField] private float masterVol = 25f;
    [SerializeField] private float musicVol = 25f;
    [SerializeField] private float fxVol = 25f;

    public string GameVersion { get { return gameVersion; } set { gameVersion = value; } }
    public string NickName { get { return nickName + Random.Range(0, 9999); } set { nickName = value.ToString(); }}

    public float MasterVol => masterVol;
    public float MusicVol => musicVol;
    public float FxVol => fxVol;

    public void UpdateVol(float master, float music, float fx)
    {
        masterVol = master;
        musicVol = music;
        fxVol = fx;
    }
}
