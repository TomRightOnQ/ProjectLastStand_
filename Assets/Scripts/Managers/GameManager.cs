using UnityEngine;
using Photon.Pun;
using static ConfigManager;
using static MonsterConfigs;
using System.Collections;
// Main Game Manager

public class GameManager : MonoBehaviourPunCallbacks
{
    private bool isLoaded = false;

    private static GameManager instance;
    public DataManager dataManager;
    public MonsterManager monsterManager;

    // Data
    private MonsterConfig[] monsterData;
    private WeaponConfig[] WeaponData;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Check repeated instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // init all manager objects
            instance = this;
            DontDestroyOnLoad(gameObject);
            GameObject monsterManagerObj = new GameObject("MonsterManager");
            monsterManager = monsterManagerObj.AddComponent<MonsterManager>();
            dataManager.initData();

            if (monsterManager && dataManager)
            {
                Debug.Log("All managers loaded");
                monsterManager.begin();
                isLoaded = true;
            }
        }
    }


    private void Start()
    {

    }

    void Update()
    {
        if (!isLoaded)
        {
            Debug.Log("Loading...");
            return;
        }
        Players[] players = dataManager.GetPlayers();
    }

    // Add exp to players
    public void AddExp(float amount)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient) {

            ExpAndLevels expObj = GameObject.FindObjectOfType<ExpAndLevels>();
            if (expObj != null)
            {
                expObj.EXP += amount;
            }
        }
    }

    // Looking for a player
    public Players GetLocalPlayer()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonView photonView = PhotonView.Find(GameManager.Instance.dataManager.PlayerViewID);
            if (photonView != null)
            {
                return photonView.GetComponent<Players>();
            }
            else
            {
                return null;
            }
        }
        else
        {
            return GameManager.Instance.dataManager.GetPlayers()[0];
        }
    }

    public Players GetLocalPlayer(int viewID)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonView photonView = PhotonView.Find(viewID);
            if (photonView != null)
            {
                return photonView.GetComponent<Players>();
            }
            else
            {
                return null;
            }
        }
        return null;
    }

    // Looking for a dropped weapon
    public DroppedItems GetDroppedItems(int viewID)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonView photonView = PhotonView.Find(viewID);
            if (photonView != null)
            {
                DroppedItems dropped = photonView.GetComponent<DroppedItems>();
                Debug.Log($"Found DroppedItems with viewID {viewID}: {dropped}");
                return dropped;
            }
            else
            {
                Debug.LogWarning($"PhotonView not found for viewID {viewID}");
                return null;
            }
        }
        Debug.LogWarning("Not connected to Photon network");
        return null;
    }
}
