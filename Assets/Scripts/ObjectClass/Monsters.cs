using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static ConfigManager;

// All Monsters are one class
public class Monsters : Entities
{
    void Start()
    {
        gameObject.tag = "Monster";
    }
    // Monster Stats
    [SerializeField] private float exp = 1;
    [SerializeField] private int id = 1;
    private bool currentState;

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
        name = MonsterConfigs.name;
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
    }
}
