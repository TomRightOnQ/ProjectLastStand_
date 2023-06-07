using System.Collections;
using UnityEngine;
using Photon.Pun;
using static MonsterConfigs;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    [SerializeField] private int difficulty = 1;
    private float spawning = 2.8f;
    private int playerCount = 1;
    private int spawncounter = 0;
    private void Awake()
    {
        if (PhotonNetwork.IsConnected) {
            playerCount = PhotonNetwork.PlayerList.Length;
        }
        difficulty = 1;
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
            yield return new WaitForSeconds(2.5f);
            if (difficulty < 400)
            {
                difficulty++;
            }
        }
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            float elapsed = 0f;

            while (elapsed < spawning / difficulty)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            spawncounter += difficulty;

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
    public void spawn(Vector3 pos, int id, int swarm = -1)
    {
        // Get monster from pool
        Monsters monster = GameManager.Instance.dataManager.TakeMonsterPool();

        if (monster != null)
        {
            if (swarm != -1) // check if it is a swarm monster, iterate if yes.
            {
                MonsterConfig monsterData = MonsterConfigs.Instance.getMonsterConfig(3);
                monster.transform.position = pos;
                monster.SetMonsters(monsterData);
                monster.UpdateHP();
                swarm += -1;
                Vector3 posToRight = pos + new Vector3(1.5f, 0f, 1f);
                if (swarm>-1){spawn(posToRight, id, swarm);}
            }
            
            else // normally Config the monster
            {
                MonsterConfig monsterData = MonsterConfigs.Instance.getMonsterConfig();
                monster.transform.position = pos;
                monster.SetMonsters(monsterData);
                monster.UpdateHP();
                if (monsterData.id == 3)
                {
                    spawn(pos, id, 4);
                }
            }
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

