using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConfigManager;

// Managing the spawning and despawning of monster entities
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    //private int difficulty = 1;
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
        while (true && cycleBegin)
        {
            yield return new WaitForSeconds(spawning);

            Vector3 pos = new Vector3(Random.Range(-2, 2), 0.1f, (Random.Range(-2, 2)));
            // AOE Test
            //Vector3 pos = new Vector3(0, 0.1f, 0);
            spawn(pos, 0);
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
