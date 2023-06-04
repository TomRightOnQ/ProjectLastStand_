using System.Collections;
using UnityEngine;
using Photon.Pun;
using static MonsterConfigs;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    [SerializeField] private int difficulty = 1;
    private float spawning = 2.8f;

    public void Begin()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnCoroutine());
        }
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            float elapsed = 0f;

            while (elapsed < spawning)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Increase difficulty up to 4 over time
            if (difficulty < 400)
            {
                difficulty++;
            }

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

    // Check if a monster should get despawn
    public void despawnCheck(Monsters monster)
    {
        if (monster != null && monster.CurrentHitPoints <= 0.1)
        {
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

