using System.Collections;
using UnityEngine;
using Photon.Pun;
using static MonsterConfigs;

// Managing the spawning and despawning of monster entities
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    [SerializeField] private int difficulty = 1;
    private float spawning = 2.8f;
    private bool cycleBegin = false;

    void Start()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
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
                float distance = 100.0f;
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
            if (difficulty < 8)
            {
                difficulty = Mathf.FloorToInt(elapsed / 30) + 1;
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
            MonsterConfig monsterData = MonsterConfigs.Instance.getMonsterConfig();
            monster.transform.position = pos;
            monster.SetMonsters(monsterData);
            monster.UpdateHP();
        }
    }

    // Check if a mosnter should get despawn
    public void despawnCheck(Monsters monster) {
        if (monster != null && monster.CurrentHitPoints <= 0.1) {
            // Add experience points to players
            GameManager.Instance.AddExp(monster.EXP);
            monster.Deactivate();
        }
    }

    // Kill a monster
    public void despawnForce(Monsters monster)
    {
        if (monster != null)
        {
            monster.CurrentHitPoints = 0;
            monster.Deactivate();
        }
    }
}
