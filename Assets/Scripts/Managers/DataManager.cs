using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
// All in-game DATA is stored here

public class DataManager : MonoBehaviourPunCallbacks
{
    public static DataManager Instance;
    public PrefabManager prefabManager;
    // Data sectors
    // Players
    private List<Players> playerList = new List<Players>();
    public const int PLAYER_COUNT = 4;
    // Monsters
    private List<Monsters> monsterPool = new List<Monsters>();
    public const int MONSTER_COUNT = 40;
    // Projectiles
    private List<Projectiles> projPool = new List<Projectiles>();
    public const int PROJ_COUNT = 100;
    // Weapons
    private List<Weapons> weaponsPool = new List<Weapons>();
    // Indicators
    private List<PlayerIndicator> indicatorPool = new List<PlayerIndicator>();

    private const string PREFAB_LOC = "Prefabs/";
    private int WEAPON_COUNT = 2;

    // For multiplayer
    private int playerViewID;
    public int PlayerViewID { get { return playerViewID; } set { playerViewID = value; } }

    // Set up a reference sheet of objects
    public void initData()
    {
        prefabManager = Resources.Load<PrefabManager>("PrefabManager");
        if (!PhotonNetwork.IsConnected) {
            initPoolLocal();
            return;
        }
        if (PhotonNetwork.IsMasterClient) {
            initPool();
        }
    }

    private void initPoolLocal()
    {
        Vector3 dPpos = new Vector3(-10f, -20f, -20f);
        Vector3 dMpos = new Vector3(10f, -20f, 20f);
        if (prefabManager == null)
        {
            Debug.LogError("Prefab reference is null!");
            return;
        }

        // Start the exp system
        GameObject expAndLevels = Instantiate(prefabManager.ExpAndLevels, new Vector3(0f, -20f, 0f), Quaternion.identity);
        expAndLevels.SetActive(true);

        // Place the players in the field
        for (int i = 0; i < 1; i++)
        {
            GameObject playerObj = Instantiate(prefabManager.PlayerPrefab, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
            playerObj.SetActive(true);
            playerList.Add(playerObj.GetComponent<Players>());
            playerObj.GetComponent<Players>().Index = i;
        }

        // Initialize monster pool
        for (int i = 0; i < MONSTER_COUNT; i++)
        {
            GameObject monsterObj = Instantiate(prefabManager.MonsterPrefab, dPpos, Quaternion.identity);
            monsterObj.SetActive(false);
            monsterPool.Add(monsterObj.GetComponent<Monsters>());
        }

        // Initialize projectile pool
        for (int i = 0; i < PROJ_COUNT; i++)
        {
            GameObject projObj = Instantiate(prefabManager.ProjPrefab, dPpos, Quaternion.identity);
            projObj.SetActive(false);
            projPool.Add(projObj.GetComponent<Projectiles>());
        }

        // Prepare weapons
        int numWeapons = WEAPON_COUNT;
        int onLeft = 1;
        for (int i = 0; i < numWeapons; i++)
        {
            GameObject weaponObj = Instantiate(prefabManager.WeaponPrefab, new Vector3(onLeft * -1.25f, 0.88f, 0.6f), Quaternion.Euler(0f, 0f, 0f));
            weaponObj.SetActive(true);
            Weapons weapon = weaponObj.GetComponent<Weapons>();
            weaponsPool.Add(weapon);
            playerList[0].WeaponList.Add(weapon);
            weapon.transform.SetParent(playerList[0].transform);
            onLeft *= -1;
        }
        Debug.Log("DataManager is Ready");
    }

    // Init pools
    private void initPool() 
    {
        Vector3 dPpos = new Vector3(-10f, -20f, -20f);
        Vector3 dMpos = new Vector3(10f, -20f, 20f);
        if (prefabManager == null)
        {
            Debug.LogError("Prefab reference is null");
            return;
        }

        // Start the exp system
        GameObject expAndLevels = PhotonNetwork.Instantiate(PREFAB_LOC + "UI&System/" + prefabManager.ExpAndLevels.name, new Vector3(0f, -20f, 0f), Quaternion.identity);
        expAndLevels.SetActive(true);

        // Place the players in the field
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject playerObj = PhotonNetwork.Instantiate(PREFAB_LOC + prefabManager.PlayerPrefab.name, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
            playerObj.SetActive(true); 
            playerList.Add(playerObj.GetComponent<Players>());
            playerObj.GetComponent<Players>().Index = i;
            // Assigning to players
            PhotonView photonView = playerObj.GetComponent<PhotonView>();
            photonView.TransferOwnership(PhotonNetwork.PlayerList[i]);
        }

        // Initialize monster pool
        for (int i = 0; i < MONSTER_COUNT; i++)
        {
            GameObject monsterObj = PhotonNetwork.Instantiate(PREFAB_LOC + prefabManager.MonsterPrefab.name, dPpos, Quaternion.identity);
            monsterObj.SetActive(false);
            monsterPool.Add(monsterObj.GetComponent<Monsters>());
        }

        // Initialize projectile pool
        for (int i = 0; i < PROJ_COUNT; i++)
        {
            GameObject projObj = PhotonNetwork.Instantiate(PREFAB_LOC + prefabManager.ProjPrefab.name, dPpos, Quaternion.identity);
            projObj.SetActive(false);
            projPool.Add(projObj.GetComponent<Projectiles>());
        }

        // Prepare weapons
        int numWeapons = WEAPON_COUNT * PhotonNetwork.PlayerList.Length;
        int onLeft = 1;
        for (int i = 0; i < numWeapons; i++)
        {
            GameObject weaponObj = PhotonNetwork.Instantiate(PREFAB_LOC + prefabManager.WeaponPrefab.name, new Vector3(onLeft * -1.25f, 0.88f, 0.6f), Quaternion.Euler(0f, 0f, 0f));
            weaponObj.SetActive(true);
            Weapons weapon = weaponObj.GetComponent<Weapons>();
            weaponsPool.Add(weapon);
            onLeft *= -1;
        }

        // Prepare indicators
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject indicatorObj = PhotonNetwork.Instantiate(PREFAB_LOC + "UI&System/" + prefabManager.IndicatorPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            PlayerIndicator playerIndicator = indicatorObj.GetComponent<PlayerIndicator>();
            indicatorPool.Add(playerIndicator);
            onLeft *= -1;
        }

        // Assigning
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            int playerViewID = playerList[i].photonView.ViewID;
            for (int j = 0; j < WEAPON_COUNT; j++)
            {
                int weaponViewID = weaponsPool[i * WEAPON_COUNT + j].photonView.ViewID;
                PhotonView weaponView = weaponsPool[i * WEAPON_COUNT + j].GetComponent<PhotonView>();
                weaponView.TransferOwnership(PhotonNetwork.PlayerList[i]);
                photonView.RPC("AddWeaponToPlayer", RpcTarget.AllBuffered, playerViewID, weaponViewID, j);
                photonView.RPC("assignViewID", PhotonNetwork.PlayerList[i], playerViewID);
            }
            PhotonView indicatorView = indicatorPool[i].GetComponent<PhotonView>();
            int indicatorViewID = indicatorView.ViewID;
            Debug.Log(PhotonNetwork.PlayerList[i]);
            indicatorView.TransferOwnership(PhotonNetwork.PlayerList[i]);
            photonView.RPC("AddIndicatorToPlayer", RpcTarget.AllBuffered, playerViewID, indicatorViewID);
            playerList[i].transform.position = new Vector3(Random.Range(-25f, 25f), 0.1f, (Random.Range(-25f, 25f)));
        }
        Debug.Log("DataManager is Ready");
    }

    [PunRPC]
    public void AddWeaponToPlayer(int playerViewID, int weaponViewID, int slotIndex)
    {
        Debug.Log("Adding weapon for playr " + playerViewID + " with weapon " + weaponViewID);
        PhotonView playerView = PhotonView.Find(playerViewID);
        PhotonView weaponView = PhotonView.Find(weaponViewID);

        if (playerView != null && weaponView != null)
        {
            Players player = playerView.GetComponent<Players>();
            Weapons weapon = weaponView.GetComponent<Weapons>();

            if (player != null && weapon != null)
            {
                player.WeaponList.Add(weapon);
                weapon.transform.SetParent(player.transform);
            }
        }
    }

    [PunRPC]
    public void AddIndicatorToPlayer(int playerViewID, int indicatorViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        PhotonView indicatorView = PhotonView.Find(indicatorViewID);

        if (playerView != null && indicatorView != null)
        {
            Players player = playerView.GetComponent<Players>();
            PlayerIndicator indicator = indicatorView.GetComponent<PlayerIndicator>();

            if (player != null && indicator != null)
            {
                indicator.transform.SetParent(player.transform);
                indicator.SetUp();
            }
        }
    }

    [PunRPC]
    // give the viewID to each player
    public void assignViewID(int viewID)
    {
        playerViewID = viewID;
    }

    // Take an object from the pool and push it to the other
    public Projectiles TakeProjPool()
    {
        for (int i = 0; i < projPool.Count; i++)
        {
            if (!projPool[i].gameObject.activeSelf)
            {
                return projPool[i];
            }
        }
        return null;
    }

    public Monsters TakeMonsterPool()
    {
        for (int i = 0; i < monsterPool.Count; i++)
        {
            if (!monsterPool[i].gameObject.activeSelf)
            {
                monsterPool[i].Activate();
                return monsterPool[i];
            }
        }
        return null;
    }

    // Getters for the pools
    public Players[] GetPlayers()
    {
        return playerList.ToArray(); ;
    }
    
    void Start()
    {

    }

    void Update()
    {

    }
}