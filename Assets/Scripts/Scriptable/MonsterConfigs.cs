using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hold the Configuration of all monsters
[CreateAssetMenu(menuName = "Configs/MonsterConfigs")]
public class MonsterConfigs : ScriptableSingleton<MonsterConfigs>
{
    public struct MonsterConfig
    {
        public string _name;
        public int id;
        public int hitPoints;
        public float speed;
        public int defaultAttack;
        public int defaultWeaponAttack;
        public int defaultDefence;
        public int defaultMagicDefence;
        public int exp;
    }

    public static MonsterConfig SimpleMonster = new MonsterConfig
    {
        _name = "Simple Monster",
        id = 0,
        hitPoints = 10,
        speed = 1.5f,
        defaultAttack = 10,
        defaultWeaponAttack = 1,
        defaultDefence = 5,
        defaultMagicDefence = 2,
        exp = 2
    };

    public static MonsterConfig Goblin = new MonsterConfig
    {
        _name = "Goblin",
        id = 1,
        hitPoints = 75,
        speed = 2f,
        defaultAttack = 15,
        defaultWeaponAttack = 8,
        defaultDefence = 6,
        defaultMagicDefence = 3,
        exp = 2
    };

    public MonsterConfig getMonsterConfig(int id)
    {
        MonsterConfig monsterData = SimpleMonster;
        switch (id)
        {
            case 0:
                monsterData = MonsterConfigs.SimpleMonster;
                break;
            case 1:
                monsterData = MonsterConfigs.Goblin;
                break;
        }
        return monsterData;
    }
}
