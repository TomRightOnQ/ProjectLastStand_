using UnityEngine;

// Hold the Configuration of all monsters
[CreateAssetMenu(menuName = "Configs/MonsterConfigs")]
public class MonsterConfigs : ScriptableSingleton<MonsterConfigs>
{
    //private int totalMonstersSpawned = 0;

    public enum MonsterBehaviorType
    {
        Walker,
        Shooter,
        Hyperion,
        Anteater
    }

    public struct MonsterConfig
    {
        public string _name;
        public int id;
        public float hitPoints;
        public float speed;
        public float defaultAttack;
        public float defaultWeaponAttack;
        public float defaultDefence;
        public float defaultMagicDefence;
        public int exp;
        public MonsterBehaviorType behaviorType;
        public ArtConfigs.Artconfig mesh;
        public ArtConfigs.Artconfig projMesh;
    }

    public static MonsterConfig SimpleMonster = new MonsterConfig
    {
        _name = "Simple Monster",
        id = 0,
        hitPoints = 10,
        speed = 1.5f,
        defaultAttack = 10,
        defaultWeaponAttack = 1,
        defaultDefence = 0.5f,
        defaultMagicDefence = 2,
        exp = 3,
        behaviorType = MonsterBehaviorType.Walker,
        mesh = ArtConfigs.Artconfig.DefaultMonsterMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
    };

    public static MonsterConfig MonsterShooter = new MonsterConfig
    {
        _name = "Monster Shooter",
        id = 1,
        hitPoints = 15,
        speed = 1.2f,
        defaultAttack = 3,
        defaultWeaponAttack = 1,
        defaultDefence = 1f,
        defaultMagicDefence = 3,
        exp = 5,
        behaviorType = MonsterBehaviorType.Shooter,
        mesh = ArtConfigs.Artconfig.DefaultMonsterMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
    };

    public static MonsterConfig MageKiller = new MonsterConfig
    {
        _name = "Mage Killer",
        id = 2,
        hitPoints = 20,
        speed = 1.3f,
        defaultAttack = 6,
        defaultWeaponAttack = 1,
        defaultDefence = 1.5f,
        defaultMagicDefence = 15,
        exp = 8,
        behaviorType = MonsterBehaviorType.Shooter,
        mesh = ArtConfigs.Artconfig.MageKillerMonsterMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
    };  

    public static MonsterConfig SwarmMonster = new MonsterConfig
    {
        _name = "Swarm Monster",
        id = 3,
        hitPoints = 5,
        speed = 2.0f,
        defaultAttack = 2,
        defaultWeaponAttack = 1,
        defaultDefence = 0.4f,
        defaultMagicDefence = 2,
        exp = 2,
        behaviorType = MonsterBehaviorType.Walker,
        mesh = ArtConfigs.Artconfig.SwarmMonsterMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
    };

    public static MonsterConfig Zealot = new MonsterConfig
    {
        _name = "Zealot",
        id = 4,
        hitPoints = 20,
        speed = 2.5f,
        defaultAttack = 20,
        defaultWeaponAttack = 1,
        defaultDefence = 2f,
        defaultMagicDefence = 10,
        exp = 8,
        behaviorType = MonsterBehaviorType.Walker,
        mesh = ArtConfigs.Artconfig.ZealotMonsterMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
    };

    public static MonsterConfig Leviathan = new MonsterConfig
    {
        _name = "Leviathan",
        id = 5,
        hitPoints = 285,
        speed = 0.5f,
        defaultAttack = 50,
        defaultWeaponAttack = 1,
        defaultDefence = 7f,
        defaultMagicDefence = 20,
        exp = 26,
        behaviorType = MonsterBehaviorType.Walker,
        mesh = ArtConfigs.Artconfig.DefaultMonsterMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
    };

    public static MonsterConfig Hyperion = new MonsterConfig
    {
        _name = "Hyperion",
        id = 6,
        hitPoints = 1000,
        speed = 0.4f,
        defaultAttack = 30,
        defaultWeaponAttack = 1,
        defaultDefence = 6f,
        defaultMagicDefence = 1,
        exp = 90,
        behaviorType = MonsterBehaviorType.Hyperion,
        mesh = ArtConfigs.Artconfig.DefaultMonsterMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
    };

    public static MonsterConfig Anteater = new MonsterConfig
    {
        _name = "Anteater",
        id = 7,
        hitPoints = 1500,
        speed = 0.4f,
        defaultAttack = 30,
        defaultWeaponAttack = 1,
        defaultDefence = 10f,
        defaultMagicDefence = 1,
        exp = 90,
        behaviorType = MonsterBehaviorType.Anteater,
        mesh = ArtConfigs.Artconfig.DefaultMonsterMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
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
            case 2:
                monsterData = MonsterConfigs.MageKiller;
                break;
            case 3:
                monsterData = MonsterConfigs.SwarmMonster;
                break;
            case 4:
                monsterData = MonsterConfigs.Zealot;
                break;
            case 5:
                monsterData = MonsterConfigs.Leviathan;
                break;
            case 6:
                monsterData = MonsterConfigs.Hyperion;
                break;
            case 7:
                monsterData = MonsterConfigs.Anteater;
                break;
        }
        return monsterData;
    }

    public MonsterConfig getMonsterConfig()
    {
        // Array of monster IDs, where the index is the ID and the value is the weight
        // if (++totalMonstersSpawned % 60 == 0)
        // {
        //     return getMonsterConfig(6);
        // }
        int[] weights = { 37, 25, 15, 10, 8};
        int totalWeight = 0;
        foreach(int weight in weights) 
        {
            totalWeight += weight;
        }

        // Generate a random number between 0 and totalWeight
        int randomNumber = Random.Range(0, totalWeight);
        
        // Determine which monster ID this random number corresponds to
        int runningTotal = 0;
        int chosenID = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            runningTotal += weights[i];
            if (randomNumber < runningTotal)
            {
                chosenID = i;
                break;
            }
        }
        if (chosenID==3){
            Monsters monster = GameManager.Instance.dataManager.TakeMonsterPool();
        }
        return getMonsterConfig(chosenID);
    }

    public MonsterConfig _getMonsterConfig()
    {
        // Array of monster IDs, where the index is the ID and the value is the weight
        int[] weights = { 37, 25, 15, 10, 8, 5 };
        int totalWeight = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        // Generate a random number between 0 and totalWeight
        int randomNumber = Random.Range(0, totalWeight);

        // Determine which monster ID this random number corresponds to
        int runningTotal = 0;
        int chosenID = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            runningTotal += weights[i];
            if (randomNumber < runningTotal)
            {
                chosenID = i;
                break;
            }
        }
        if (chosenID == 3)
        {
            Monsters monster = GameManager.Instance.dataManager.TakeMonsterPool();
        }
        return getMonsterConfig(chosenID);
    }
}
