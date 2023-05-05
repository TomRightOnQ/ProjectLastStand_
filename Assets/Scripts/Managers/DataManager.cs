using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// All in-game DATA is stored here

public class DataManager : MonoBehaviourPunCallbacks
{
    public static DataManager Instance;
    private PrefabManager PrefabReference;
    // Data sectors
    // Players
    private List<Players> playerList = new List<Players>();
    public const int PLAYER_COUNT = 4;
    // Monsters
    private List<Monsters> monsterPool = new List<Monsters>();
    private List<Monsters> monsterPoolA = new List<Monsters>();
    public const int MONSTER_COUNT = 40;
    // Projectiles
    private List<Projectiles> projPool = new List<Projectiles>();
    private List<Projectiles> projPoolA = new List<Projectiles>();
    public const int PROJ_COUNT = 100;
    // Weapons
    private List<Weapons> weaponsPool = new List<Weapons>();

    private const string PREFAB_LOC = "Prefabs/";
    private int WEAPON_COUNT = 2;
    private float exp = 0;
    // Set up a reference sheet of objects
    public void initData(PrefabManager prefabReference)
    {
        PrefabReference = prefabReference;
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
        if (PrefabReference == null)
        {
            Debug.LogError("Prefab reference is null in DataManager.Awake!");
            return;
        }

        // Place the players in the field
        for (int i = 0; i < 1; i++)
        {
            GameObject playerObj = Instantiate(PrefabReference.playerPrefab, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
            playerObj.SetActive(true);
            playerList.Add(playerObj.GetComponent<Players>());
            playerObj.GetComponent<Players>().Index = i;
        }

        // Initialize monster pool
        for (int i = 0; i < MONSTER_COUNT; i++)
        {
            GameObject monsterObj = Instantiate(PrefabReference.monsterPrefab, dPpos, Quaternion.identity);
            monsterObj.SetActive(false);
            monsterPool.Add(monsterObj.GetComponent<Monsters>());
        }

        // Initialize projectile pool
        for (int i = 0; i < PROJ_COUNT; i++)
        {
            GameObject projObj = Instantiate(PrefabReference.projPrefab, dPpos, Quaternion.identity);
            projObj.SetActive(false);
            projPool.Add(projObj.GetComponent<Projectiles>());
        }

        Debug.Log("DataManager is Ready");

        // Prepare weapons
        int numWeapons = WEAPON_COUNT;
        int onLeft = 1;
        for (int i = 0; i < numWeapons; i++)
        {
            GameObject weaponObj = Instantiate(PrefabReference.weaponPrefab, new Vector3(onLeft * -1.25f, 0.88f, 0.6f), Quaternion.Euler(0f, 0f, 0f));
            weaponObj.SetActive(true);
            Weapons weapon = weaponObj.GetComponent<Weapons>();
            weaponsPool.Add(weapon);
            playerList[0].WeaponList.Add(weapon);
            weapon.transform.SetParent(playerList[0].transform);
            onLeft *= -1;
        }
    }

    // Init pools
    private void initPool() 
    {
        Vector3 dPpos = new Vector3(-10f, -20f, -20f);
        Vector3 dMpos = new Vector3(10f, -20f, 20f);
        if (PrefabReference == null)
        {
            Debug.LogError("Prefab reference is null in DataManager.Awake!");
            return;
        }

        // Place the players in the field
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject playerObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabReference.playerPrefab.name, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
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
            GameObject monsterObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabReference.monsterPrefab.name, dPpos, Quaternion.identity);
            monsterObj.SetActive(false);
            monsterPool.Add(monsterObj.GetComponent<Monsters>());
        }

        // Initialize projectile pool
        for (int i = 0; i < PROJ_COUNT; i++)
        {
            GameObject projObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabReference.projPrefab.name, dPpos, Quaternion.identity);
            projObj.SetActive(false);
            projPool.Add(projObj.GetComponent<Projectiles>());
        }

        Debug.Log("DataManager is Ready");

        // Prepare weapons
        int numWeapons = WEAPON_COUNT * PhotonNetwork.PlayerList.Length;
        int onLeft = 1;
        for (int i = 0; i < numWeapons; i++)
        {
            GameObject weaponObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabReference.weaponPrefab.name, new Vector3(onLeft * -1.25f, 0.88f, 0.6f), Quaternion.Euler(0f, 0f, 0f));
            weaponObj.SetActive(true);
            Weapons weapon = weaponObj.GetComponent<Weapons>();
            weaponsPool.Add(weapon);
            onLeft *= -1;
        }

        // Assigning
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            for (int j = 0; j < WEAPON_COUNT; j++)
            {
                int playerViewID = playerList[i].photonView.ViewID;
                int weaponViewID = weaponsPool[i * WEAPON_COUNT + j].photonView.ViewID;
                PhotonView weaponView = weaponsPool[i * WEAPON_COUNT + j].GetComponent<PhotonView>();
                weaponView.TransferOwnership(PhotonNetwork.PlayerList[i]);
                photonView.RPC("AddWeaponToPlayer", RpcTarget.AllBuffered, playerViewID, weaponViewID, j);
            }
            playerList[i].transform.position = new Vector3(Random.Range(-25f, 25f), 0.1f, (Random.Range(-25f, 25f)));
        }
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

    // Take an object from the pool and push it to the other
    public Projectiles TakeProjPool()
    {
        for (int i = 0; i < projPool.Count; i++)
        {
            if (!projPool[i].gameObject.activeSelf)
            {
                projPoolA.Add(projPool[i]);
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
                monsterPoolA.Add(monsterPool[i]);
                return monsterPool[i];
            }
        }
        return null;
    }

    public void RemoveDeactivatedProj(Projectiles proj)
    {
        if (projPoolA.Contains(proj))
        {
            projPoolA.Remove(proj);
        }
    }

    public void RemoveDeactivatedMonster(Monsters monster)
    {
        if (monsterPoolA.Contains(monster))
        {
            monsterPoolA.Remove(monster);
        }
    }

    // Return the prefabs
    public PrefabManager GetPrefabReference()
    {
        return PrefabReference;
    }

    // Getters for the pools
    public Players[] GetPlayers()
    {
        return playerList.ToArray(); ;
    }

    public Projectiles[] GetProjs()
    {
        return projPoolA.ToArray(); ;
    }

    public Monsters[] GetMonsters()
    {
        return monsterPoolA.ToArray(); ;
    }
    
    // manage EXP
    public float EXP
    {
        get { return exp; }
        set { exp = value; }
    }

    public void addEXP(int x) {
        exp += x;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}