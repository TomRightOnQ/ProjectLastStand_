using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static MonsterConfigs;


// All Monsters are one class
public class Monsters : Entities
{
    // Monster Stats
    [SerializeField] private float exp = 1;
    [SerializeField] private int id = 1;
    [SerializeField] private int type = 0;
    [SerializeField] private MonsterAI monsterAI;
    [SerializeField] private MonsterBehaviorType behaviorType;
    private float prevHP;

    public PrefabManager prefabManager;

    void Start()
    {
        gameObject.tag = "Monster";
        prefabManager = Resources.Load<PrefabManager>("PrefabManager");
    }

    public float EXP { get { return exp; } set { exp = value; } }
    public int ID { get { return id; } set { id = value; } }
    public MonsterBehaviorType BehaviorType { get { return behaviorType; } }

    // Sync
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {

        }
        else
        {
            UpdateHP();
        }
    }

    // Morph the Monster
    public void SetMonsters(MonsterConfig MonsterConfigs)
    {
        id = MonsterConfigs.id;
        name = MonsterConfigs._name;
        hitPoints = MonsterConfigs.hitPoints;
        currentHitPoints = hitPoints;
        speed = MonsterConfigs.speed;
        exp = MonsterConfigs.exp;
        defaultAttack = MonsterConfigs.defaultAttack;
        defaultWeaponAttack = MonsterConfigs.defaultWeaponAttack;
        defaultDefence = MonsterConfigs.defaultDefence;
        defaultMagicDefence = MonsterConfigs.defaultMagicDefence;
        prevHP = currentHitPoints;
        behaviorType = MonsterConfigs.behaviorType;
        monsterAI.SetUp();
    }

    // Taking Damage
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        GameObject damageNumberObj = Instantiate(prefabManager.DamageNumberPrefab, transform.position, Quaternion.identity);
        DamageNumber damageNumber = damageNumberObj.GetComponent<DamageNumber>();
        damageNumber.Init(damage, transform.position);
    }

    // HP Bar 
    public void UpdateHP()
    {
        if (prevHP != currentHitPoints) {
            float change = currentHitPoints - prevHP;

            prevHP = currentHitPoints;
        }
        hpS.maxValue = hitPoints;
        hpS.value = currentHitPoints;
    }

    // Update
    void Update()
    {
        UpdateHP();
        GameManager.Instance.monsterManager.despawnCheck(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            Base _base = other.gameObject.GetComponent<Base>();
            if (_base != null) {
                _base.TakeDamage(defaultAttack * defaultWeaponAttack);
            }
            GameManager.Instance.monsterManager.despawnForce(this);
        }
        else if (other.CompareTag("Player"))
        {
            Players _player = other.gameObject.GetComponent<Players>();
            if (_player != null)
            {
                _player.TakeDamage(defaultAttack * defaultWeaponAttack);
            }
            GameManager.Instance.monsterManager.despawnForce(this);
        }
    }

    // Remove AI
    public override void Deactivate()
    {
        monsterAI.RemoveAI();
        base.Deactivate();
    }
}
