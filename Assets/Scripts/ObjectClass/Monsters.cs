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
    private bool currentState;
    private Vector3 targetPosition;
    void Start()
    {
        gameObject.tag = "Monster";
        targetPosition = new Vector3(0, 0.1f, 0);
    }

    public float EXP
    {
        get { return exp; }
        set { exp = value; }
    }
    public int ID
    {
        get { return id; }
        set { id = value; }
    }

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
    }

    // HP Bar 
    public void UpdateHP()
    {
        hpS.maxValue = hitPoints;
        hpS.value = currentHitPoints;
    }
    // Update
    void Update()
    {
        UpdateHP();
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        direction.Normalize();
        transform.LookAt(targetPosition);
        transform.position += direction * speed * 4 * Time.deltaTime;
        GameManager.Instance.monsterManager.despawnCheck(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            Base _base = other.gameObject.GetComponent<Base>();
            if (_base != null) {
                _base.TakeDamage(defaultAttack * defaultWeaponAttack * 4);
            }
            currentHitPoints = -1;
        }
    }
}
