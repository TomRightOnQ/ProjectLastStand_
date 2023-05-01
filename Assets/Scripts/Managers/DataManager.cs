using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// All in-game DATA is stored here

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    private PrefabManager PrefabReference;
    // Data sectors
    // Players
    private List<Players> playerList = new List<Players>();
    public const int PLAYER_COUNT = 1;
    // Monsters
    private List<Monsters> monsterPool = new List<Monsters>();
    private List<Monsters> monsterPoolA = new List<Monsters>();
    public const int MONSTER_COUNT = 3;
    // Projectiles
    private List<Projectiles> projPool = new List<Projectiles>();
    private List<Projectiles> projPoolA = new List<Projectiles>();
    public const int PROJ_COUNT = 10;

    private const string PREFAB_LOC = "Prefabs/";
    private int WEAPON_COUNT = 2;
    private float exp = 0;
    // Set up a reference sheet of objects
    public void initData(PrefabManager prefabReference)
    {
        PrefabReference = prefabReference;
        if (PhotonNetwork.IsMasterClient) {
            initPool();
        }
    }

    // Init pools
    private void initPool() 
    {
        Vector3 dPpos = new Vector3(-10f, -10f, -10f);
        Vector3 dMpos = new Vector3(10f, -10f, 10f);
        if (PrefabReference == null)
        {
            Debug.LogError("Prefab reference is null in DataManager.Awake!");
            return;
        }

        // Place the players in the field
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject playerObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabReference.playerPrefab.name, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
            // GameObject playerObj = Instantiate(PrefabReference.playerPrefab, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
            playerObj.SetActive(true); 
            playerList.Add(playerObj.GetComponent<Players>());
            playerList[i].WeaponList = new Weapons[WEAPON_COUNT];
            for (int j = 0; j < WEAPON_COUNT; j++)
            {
                GameObject weaponObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabReference.weaponPrefab.name, new Vector3(0.15f, 0.1f, 0.1f), Quaternion.Euler(0f, 90f, 0f));
                weaponObj.SetActive(true);
                Weapons weapon = weaponObj.GetComponent<Weapons>();
                weapon.transform.parent = playerObj.transform;
                playerList[i].WeaponList[j] = weapon; // Add weapon to the player
            }

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

    // Synced up the projectiles


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