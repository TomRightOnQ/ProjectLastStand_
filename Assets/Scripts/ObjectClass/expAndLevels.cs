using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

// Hold the exp and levels info
// Only one copy per game
public class ExpAndLevels : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float exp = 0;
    [SerializeField] private float expL = 10;
    [SerializeField] private int level = 1;
    [SerializeField] private Slider ExpS;
    [SerializeField] private TextMeshProUGUI levelText;
    private UpgradeMenu upgradeMenu;

    public float EXP { get { return exp; } set { exp = value; } }
    public float EXPL { get { return expL; } set { expL = value; } }
    public int Level { get { return level; } }

    private void Update()
    {
        ExpS.value = exp;
        ExpS.maxValue = expL;
        levelText.text = "Lv." + level;
        if (exp >= expL) {
            Upgrade();
        }
    }

    public void Upgrade()
    {
        UpgradeMenu upgradeMenu = FindObjectOfType<UpgradeMenu>();
        level += 1;
        exp = exp - expL;
        expL = 10 * (float)System.Math.Pow(1.1, level);
        if (!PhotonNetwork.IsConnected)
        {
            upgradeMenu.addPoints(1);
            // Animation of leveling up
            Players player = GameManager.Instance.dataManager.GetPlayers()[0];
            ParticleSystem particleSystem = player.gameObject.GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
        else {
            if (PhotonNetwork.IsMasterClient) {
                photonView.RPC("LevelUp", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void LevelUp()
    {
        // Notify the local UpgradeMenu that a level up occurred
        UpgradeMenu.Instance.addPoints(1);
        // Animation of leveling up
        Players[] players = FindObjectsOfType<Players>();
        foreach (Players player in players) {
            ParticleSystem particleSystem = player.gameObject.GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }

    // Sync
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        // The order of writing and reading is really important
        // Not need to send or read position data, Other component is doing this.
        // ORDER:
        //      1. exp
        //      2. level

        if (stream.IsWriting)
        {
            stream.SendNext(exp);
            stream.SendNext(level);

        }
        else
        {
            exp = (float)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
        }
    }
}
