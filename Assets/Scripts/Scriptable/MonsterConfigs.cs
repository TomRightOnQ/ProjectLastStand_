using UnityEngine;

// Hold the Configuration of all monsters
[CreateAssetMenu(menuName = "Configs/MonsterConfigs")]
public class MonsterConfigs : ScriptableSingleton<MonsterConfigs>
{
    public enum MonsterBehaviorType
    {
        Walker,
        Shooter
    }

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
        public MonsterBehaviorType behaviorType;
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
        exp = 4,
        behaviorType = MonsterBehaviorType.Walker
    };

    public static MonsterConfig MonsterShooter = new MonsterConfig
    {
        _name = "Monster Shooter",
        id = 1,
        hitPoints = 15,
        speed = 1.2f,
        defaultAttack = 5,
        defaultWeaponAttack = 1,
        defaultDefence = 4,
        defaultMagicDefence = 3,
        exp = 6,
        behaviorType = MonsterBehaviorType.Shooter
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
                monsterData = MonsterConfigs.MonsterShooter;
                break;
        }
        return monsterData;
    }

    public MonsterConfig getMonsterConfig()
    {
        int id = Random.Range(0,2);
        MonsterConfig monsterData = getMonsterConfig(id);
        return monsterData;
    }
}
