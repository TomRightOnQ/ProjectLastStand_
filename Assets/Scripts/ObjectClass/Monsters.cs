using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
