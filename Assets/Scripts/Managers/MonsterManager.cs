using System.Collections;
using UnityEngine;
using Photon.Pun;
using static MonsterConfigs;
using System;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    private float secondspassed = 1f;
    [SerializeField] private float difficulty = 1f;
    [SerializeField] private float modifier = 1f;
    private float C = 4.0f / (float)Math.Log(401);
    //we used to spawn 1 every 2.8 seconds
    private int playerCount = 1;
    [SerializeField] private float spawncounter = 0f;
    [SerializeField] private float bosscounter = 0f;
    private int bosscycle = 0;
    private bool bossexist = false;
    [SerializeField] private float difficultyratio = 1f;
    private const string PREFAB_LOC = "Prefabs/";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (PhotonNetwork.IsConnected) {
            playerCount = PhotonNetwork.PlayerList.Length;
        }
        difficultyratio += (float)((playerCount - 1) * 0.8);
    }


    public void Begin()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnCoroutine());
            StartCoroutine(diffUp());
        }
    }

    public void End()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            StopCoroutine(SpawnCoroutine());
            StopCoroutine(diffUp());
        }
    }

    private IEnumerator diffUp() 
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            secondspassed++;
            difficulty = 1f + (float)(System.Math.Pow(secondspassed, 1.2) / 4);
        }
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            CalculateCounter();
            Vector3 pos = Vector3.zero;
            float distance = 100.0f;
            float distanceSqr = distance * distance;

            while (true)
            {
                pos = new Vector3(UnityEngine.Random.Range(-distance, distance), 0f, UnityEngine.Random.Range(-distance, distance));
                if ((pos - Vector3.zero).sqrMagnitude > distanceSqr)
                {
                    break;
                }
            }
            if (spawncounter>=400){
                spawn(pos, 0);
                spawncounter -= 400;
            }
            if ((bosscounter>=40000) && !bossexist)
            {
                bossexist = true;
                if (bosscycle == 0)
                {
                    spawnHyperion(pos);
                    bosscycle = 1;
                }
                else if (bosscycle == 1)
                {
                    spawnAnteater(pos);
                    bosscycle = 2;
                }
                else if (bosscycle == 2)
                {
                    spawnLeviathan(pos);
                    bosscycle = 0;
                }
                bosscounter -= 50000;
            }
        }
    }

    public void CalculateCounter()
    {
        float maxDifficulty = 400f;
        float maxModifier = 400f;
        float minModifier = 1f;

        float normalizedDifficulty = (float)difficulty / maxDifficulty;
        float scalingFactor = maxModifier - minModifier;

        float m = 0.145f;
        float b = 22f;

        spawncounter += minModifier + scalingFactor * (float)Math.Pow(normalizedDifficulty, 2) * difficultyratio + b;
        bosscounter += (m * difficulty + b) * difficultyratio;
    }

    public float CalculateModifier()
    {
        float maxDifficulty = 400f;
        float maxModifier = 4f;
        float minModifier = 1f;

        float normalizedDifficulty = (float)difficulty / maxDifficulty;
        float scalingFactor = maxModifier - minModifier;

        return minModifier + scalingFactor * (float)Math.Pow(normalizedDifficulty, 2);
    }

    // Spawning
    public void spawn(Vector3 pos, int id, int swarm = -1)
    {
        // Get monster from pool
        Monsters monster = GameManager.Instance.dataManager.TakeMonsterPool();
        modifier = CalculateModifier();
        if (monster != null)
        {
            if (swarm != -1) // check if it is a swarm monster, iterate if yes.
            {
                MonsterConfig monsterData = MonsterConfigs.Instance.getMonsterConfig(3);
                monster.transform.position = pos;
                monster.SetMonsters(monsterData, modifier);
                monster.UpdateHP();
                swarm += -1;
                Vector3 posToRight = pos + new Vector3(1.5f, 0f, 1f);
                if (swarm>-1){spawn(posToRight, id, swarm);}
            }
            
            else // normally Config the monster
            {
                MonsterConfig monsterData = MonsterConfigs.Instance.getMonsterConfig();
                monster.transform.position = pos;
                int eliterandom = UnityEngine.Random.Range(1, 101);
                if (eliterandom>85)
                {
                    monster.SetEliteMonsters(monsterData, modifier);
                }
                else
                {
                    monster.SetMonsters(monsterData, modifier);
                    monster.UpdateHP();
                }
                
                if (monsterData.id == 3)
                {
                    spawn(pos, id, 4);
                }
            }
        }
    }

    public void spawnLeviathan(Vector3 pos)
    {
        MonsterConfig config = MonsterConfigs.Instance._getMonsterConfig();
        if (PhotonNetwork.IsConnected)
        {
            GameObject LeviathanObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.LeviathanPrefab.name, pos, Quaternion.identity);
            LeviathanObj.GetComponent<Leviathan>().SetMonsters(config, 1);
            LeviathanObj.GetComponent<Leviathan>().SetLe(config, modifier);
        }
        else 
        {
            GameObject LeviathanObj = Instantiate(PrefabManager.Instance.LeviathanPrefab, pos, Quaternion.identity);
            LeviathanObj.GetComponent<Leviathan>().SetMonsters(config, 1);
            LeviathanObj.GetComponent<Leviathan>().SetLe(config, modifier);
        }
        
    }

    public void spawnHyperion(Vector3 pos)
    {
        MonsterConfigs.MonsterConfig config = MonsterConfigs.Instance.getMonsterConfig(6);
        if (PhotonNetwork.IsConnected)
        {
            string prefabName = PREFAB_LOC + PrefabManager.Instance.HyperionPrefab.name;
            GameObject HyperionObj = PhotonNetwork.Instantiate(prefabName, pos, Quaternion.identity);
            HyperionObj.transform.position = new Vector3(pos.x, 10f, pos.z);
            HyperionObj.GetComponent<Hyperion>().SetHyperion(config, modifier);
        }
        else
        {
            GameObject HyperionObj = Instantiate(PrefabManager.Instance.HyperionPrefab, pos, Quaternion.identity);
            HyperionObj.transform.position = new Vector3(pos.x, 10f, pos.z);
            HyperionObj.GetComponent<Hyperion>().SetHyperion(config, modifier);
        }
    }

    public void spawnAnteater(Vector3 pos)
    {
        MonsterConfigs.MonsterConfig config = MonsterConfigs.Instance.getMonsterConfig(7);
        if (PhotonNetwork.IsConnected)
        {
            GameObject AnteaterionObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.AnteaterPrefab.name, pos, Quaternion.identity);
            AnteaterionObj.GetComponent<Anteater>().SetAnteater(config, modifier);
        }
        else 
        {
            GameObject AnteaterionObj = Instantiate(PrefabManager.Instance.AnteaterPrefab, pos, Quaternion.identity);
            AnteaterionObj.GetComponent<Anteater>().SetAnteater(config, modifier);
        }
    }

    // Check if a monster should get despawn
    public void despawnCheck(Monsters monster)
    {
        if (monster != null && monster.CurrentHitPoints <= 0.1)
        {
            // Add experience points to players
            GameManager.Instance.AddExp(monster.EXP/modifier);
            if (monster.IsBoss) 
            {
                bossexist = false;
            }
            monster.Deactivate();
        }
    }

    // Kill a monster
    public void despawnForce(Monsters monster)
    {
        if (monster != null)
        {
            if (monster.IsBoss)
            {
                bossexist = false;
            }
            monster.CurrentHitPoints = 0;
            monster.Deactivate();
        }
    }
}

