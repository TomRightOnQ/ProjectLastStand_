using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConfigManager;
// Main Game Manager

public class GameManager : MonoBehaviour
{
    private bool isLoaded = false;
    private int maxColliders = 50;

    private static GameManager instance;
    public DataManager dataManager;
    public PrefabManager prefabManager;
    public MonsterManager monsterManager;
    public ConfigManager configManager;

    // Data
    private MonsterConfig[] monsterData;
    private WeaponConfig[] WeaponData;
    private List<DamageExplosion> damageExplosions = new List<DamageExplosion>();
    public List<DamageExplosion> DamageExplosions { get {return damageExplosions; } }
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
            GameObject configManagerObj = new GameObject("ConfigManager");
            configManager = configManagerObj.AddComponent<ConfigManager>();

            configManager.Load();

            GameObject monsterManagerObj = new GameObject("MonsterManager");
            monsterManager = monsterManagerObj.AddComponent<MonsterManager>();

            
            GameObject dataManagerObj = new GameObject("DataManager");
            dataManager = dataManagerObj.AddComponent<DataManager>();
            
            dataManager.initData(prefabManager);

            if (configManager && monsterManager && dataManager)
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
        // Prepare pools
        Projectiles[] projPoolA = GameManager.Instance.dataManager.GetProjs();
        Monsters[] monsterPoolA = GameManager.Instance.dataManager.GetMonsters();
        // Test Attack
        Players[] players = dataManager.GetPlayers();

        // AOE
        foreach (DamageExplosion explosion in damageExplosions)
        {
            // Early termination
            if (monsterPoolA.Length == 0)
            {
                return;
            }
            // create a list to store the colliders within the explosion range
            Collider[] colliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(explosion.position, explosion.damageRange, colliders);
            if (numColliders == 0) {
                continue;
            }
            // iterate over the colliders within range
            for (int i = 0; i < numColliders; i++)
            {
                Monsters monster = colliders[i].GetComponent<Monsters>();

                if (monster != null && monster.gameObject.activeSelf && monster.gameObject.CompareTag("Monster"))
                {
                    Debug.Log("Explosion damage taken");
                    monster.TakeDamage(explosion.damageValue);
                    GameManager.Instance.monsterManager.despawnCheck(monster);
                }
            }   
        }
        // Reset AOE positions
        damageExplosions.Clear();
    }
}

// Processing AOE damage
public class DamageExplosion
{
    public Vector3 position;
    public float damageRange;
    public float damageValue;

    public DamageExplosion(Vector3 pos, float range, float value)
    {
        position = pos;
        damageRange = range;
        damageValue = value;
    }
}
