using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
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
    public const int PROJ_COUNT = 100;

    private float exp = 0;
    // Set up a reference sheet of objects
    public void initData(PrefabManager prefabReference)
    {
        PrefabReference = prefabReference;
        initPool();
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
        for (int i = 0; i < PLAYER_COUNT; i++)
        {
            GameObject playerObj = Instantiate(PrefabReference.playerPrefab, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
            playerObj.SetActive(true);
            playerList.Add(playerObj.GetComponent<Players>());
        }

        // Initialize monster pool
        for (int i = 0; i < MONSTER_COUNT; i++)
        {
            GameObject monsterObj = Instantiate(PrefabReference.monsterPrefab, dMpos, Quaternion.identity);
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
    }

    // Take an object from the pool and push it to the other
    public Projectiles TakeProjPool()
    {
        for (int i = 0; i < projPool.Count; i++)
        {
            if (!projPool[i].gameObject.activeSelf)
            {
                projPool[i].gameObject.SetActive(true);
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
                monsterPool[i].gameObject.SetActive(true);
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