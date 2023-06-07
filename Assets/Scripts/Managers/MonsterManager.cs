using System.Collections;
using UnityEngine;
using Photon.Pun;
using static MonsterConfigs;
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    private float secondspassed = 1f;
    [SerializeField] private float difficulty = 1f;
    //we used to spawn 1 every 2.8 seconds
    private int playerCount = 1;
    private float spawncounter = 0f;
    [SerializeField] private float difficultyratio = 1f;
    private void Awake()
    {
        if (PhotonNetwork.IsConnected) {
            playerCount = PhotonNetwork.PlayerList.Length;
        }
        difficulty = 140;
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
            difficulty = 140f + (float)(System.Math.Pow(secondspassed, 1.2) / 4);
        }
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {

            yield return new WaitForSeconds(0.2f);
            spawncounter += (difficulty/5)*difficultyratio;

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
            if (spawncounter>=400){
                spawn(pos, 0);
                spawncounter -= 400;
            }
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
                int eliterandom = UnityEngine.Random.Range(1, 101);
                if (monsterData.id == 6)
                {
                    monster.SetLeviathan(monsterData);
                }
                else if (eliterandom>85)
                {
                    monster.SetEliteMonsters(monsterData);
                }
                else
                {
                    monster.SetMonsters(monsterData);
                    monster.UpdateHP();
                }
                
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

