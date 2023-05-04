using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static ConfigManager;

// Managing the spawning and despawning of monster entities
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    private int difficulty = 1;
    private float spawning = 1f;
    private bool cycleBegin = false;


    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    public void begin() {
        cycleBegin = true;
    }

    IEnumerator SpawnCoroutine()
    {
        float elapsed = 0f;
        while (true && cycleBegin)
        {
            elapsed += Time.deltaTime;
            float timePerSpawn = spawning / Mathf.Min(4, difficulty);
            if (elapsed >= timePerSpawn)
            {
                elapsed -= timePerSpawn;

                Vector3 pos = Vector3.zero;
                float distance = 140.0f;
                float distanceSqr = distance * distance;
                while (true)
                {
                    pos = new Vector3(Random.Range(-distance, distance), 0f, Random.Range(-distance, distance));
                    if ((pos - Vector3.zero).sqrMagnitude > distanceSqr)
                    {
                        break;
                    }
                }
                spawn(pos, 0);
            }
            // Increase difficulty up to 4 over time
            if (difficulty < 4)
            {
                difficulty = Mathf.FloorToInt(elapsed / 120) + 1;
            }
            yield return null;
        }
    }

    // Spawning
    public void spawn(Vector3 pos, int id)
    {
        // Get monster from pool
        Monsters monster = GameManager.Instance.dataManager.TakeMonsterPool();

        if (monster != null)
        {
            // Config the monster
            MonsterConfig[] monsterData = GameManager.Instance.configManager.getMonsters();
            monster.transform.position = pos;
            monster.SetMonsters(monsterData[id]);
            monster.UpdateHP();
        }
    }

    // Check if a mosnter should get despawn
    public void despawnCheck(Monsters monster) {
        if (monster != null && monster.CurrentHitPoints <= 0) {
            monster.Deactivate();
            GameManager.Instance.dataManager.RemoveDeactivatedMonster(monster);
        }
    }
}
