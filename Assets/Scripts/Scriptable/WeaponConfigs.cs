using UnityEngine;

// Hold the Configuration of all weapons
[CreateAssetMenu(menuName = "Configs/WeaponConfigs")]
public class WeaponConfigs : ScriptableSingleton<WeaponConfigs>
{
    public struct WeaponConfig
    {
        public string _name;
        public int id;
        public int rating;
        public int type;
        public float attack;
        public float pen;
        public float life;
        public float cd;
        public bool selfDet;
        public float projectileSpeed;
        public float damageRange;
        public bool aoe;
        public string info;
        public string intro;
        public int hitAnim;
        public int fireAnim;
        public bool isMagic;
        public ArtConfigs.Artconfig mesh;
        public ArtConfigs.Artconfig projMesh;
        public int fireSFX;
        public int hitSFX;
    }

    public static WeaponConfig Pistol = new WeaponConfig
    {
        _name = "Pistol",
        id = 0,
        rating = 1,
        type = 0,
        attack = 3.5f,
        pen = 0.1f,
        life = 1,
        cd = 0.5f,
        selfDet = false,
        projectileSpeed = 10,
        aoe = false,
        damageRange = 0.3f,
        info = "Better than nothing...",
        intro = "A default weapon",
        hitAnim = 0,
        fireAnim = 0,
        isMagic = false,
        mesh = ArtConfigs.Artconfig.PistolMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
        fireSFX = 1,
        hitSFX = 0,
    };

    public static WeaponConfig Fork = new WeaponConfig
    {
        _name = "Fork",
        id = -2,
        rating = 1,
        type = 2,
        attack = 8,
        pen = 0.3f,
        life = 0.05f,
        cd = 0.5f,
        selfDet = false,
        projectileSpeed = 5,
        aoe = false,
        damageRange = 1.25f,
        info = "In theory, you can slain a dragon with it",
        intro = "A fork from Brandywine",
        hitAnim = 0,
        fireAnim = -1,
        isMagic = false,
        mesh = ArtConfigs.Artconfig.ForkMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
        fireSFX = 18,
        hitSFX = -1,
    };

    public static WeaponConfig LMG = new WeaponConfig
    {
        _name = "LMG",
        id = 1,
        rating = 1,
        type = 0,
        attack = 0.5f,
        pen = 0f,
        life = 1,
        cd = 0.1f,
        selfDet = false,
        projectileSpeed = 10,
        aoe = false,
        damageRange = 0.3f,
        info = "LMG! Mounted! and Loaded!",
        intro = "Light machine gun with great RPM",
        hitAnim = 0,
        fireAnim = 0,
        isMagic = false,
        mesh = ArtConfigs.Artconfig.LMGMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
        fireSFX = 2,
        hitSFX = -1,
    };

    public static WeaponConfig Wand = new WeaponConfig
    {
        _name = "Wand",
        id = 2,
        rating = 1,
        type = 0,
        attack = 3f,
        pen = 0.25f,
        life = 1,
        cd = 0.5f,
        selfDet = false,
        projectileSpeed = 4,
        aoe = true,
        damageRange = 0.75f,
        info = "Magic starts here",
        intro = "A normal magical weapon",
        hitAnim = 0,
        fireAnim = -1,
        isMagic = true,
        mesh = ArtConfigs.Artconfig.WandMesh,
        projMesh = ArtConfigs.Artconfig.WandProj,
        fireSFX = 5,
        hitSFX = 0,
    };

    public static WeaponConfig RPG = new WeaponConfig
    {
        _name = "RPG",
        id = 100,
        type = 0,
        rating = 2,
        attack = 12,
        pen = 0.5f,
        life = 3f,
        cd = 2f,
        selfDet = true,
        projectileSpeed = 5,
        aoe = true,
        damageRange = 1f,
        info = "R! P! G!",
        intro = "Explosive weapon that causes AOE damage",
        hitAnim = 0,
        fireAnim = 0,
        isMagic = false,
        mesh = ArtConfigs.Artconfig.RPGMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
        fireSFX = 7,
        hitSFX = 9,
    };

    public static WeaponConfig HeatLaser = new WeaponConfig
    {
        _name = "HeatLaser",
        id = 101,
        rating = 2,
        type = 1,
        attack = 0.75f,
        pen = 0.5f,
        life = 0.1f,
        cd = 0.07f,
        selfDet = true,
        projectileSpeed = 5,
        aoe = false,
        damageRange = 0.1f,
        info = "OVERLOAD!",
        intro = "Burning the first enemy with a beam of laser",
        hitAnim = 0,
        fireAnim = 0,
        isMagic = false,
        mesh = ArtConfigs.Artconfig.HeatLaserMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
        fireSFX = 16,
        hitSFX = -1,
    };

    public static WeaponConfig RubyLaser = new WeaponConfig
    {
        _name = "RubyLaser",
        id = 200,
        rating = 3,
        type = 1,
        attack = 11,
        pen = 0.5f,
        life = 0.3f,
        cd = 1,
        selfDet = true,
        projectileSpeed = 5,
        aoe = false,
        damageRange = 0.1f,
        info = "Charging...",
        intro = "Slow but destructive laser cannon",
        hitAnim = 0,
        fireAnim = 0,
        isMagic = false,
        mesh = ArtConfigs.Artconfig.LaserGunMesh,
        projMesh = ArtConfigs.Artconfig.DefaultProj,
        fireSFX = 15,
        hitSFX = -1,
    };

    public static WeaponConfig Kornet = new WeaponConfig
    {
        _name = "9M133",
        id = 300,
        rating = 4,
        type = 0,
        attack = 16,
        pen = 0.8f,
        life = 3f,
        cd = 2f,
        selfDet = true,
        projectileSpeed = 6,
        aoe = true,
        damageRange = 0.75f,
        info = "Nothing is unshakeable",
        intro = "Extremly strong missile",
        hitAnim = 0,
        fireAnim = 0,
        isMagic = true,
        mesh = ArtConfigs.Artconfig.KornetMesh,
        projMesh = ArtConfigs.Artconfig.KornetProjMesh,
        fireSFX = 8,
        hitSFX = 9,
    };

    public static WeaponConfig BloodSword = new WeaponConfig
    {
        _name = "Blood Sword",
        id = 102,
        rating = 2,
        type = 2,
        attack = 16,
        pen = 1f,
        life = 3f,
        cd = 0.65f,
        selfDet = true,
        projectileSpeed = 6,
        aoe = true,
        damageRange = 1.5f,
        info = "A sword formed by blood chunks",
        intro = "A strong melee weapon",
        hitAnim = -1,
        fireAnim = -1,
        isMagic = true,
        mesh = ArtConfigs.Artconfig.BloodSwordMesh,
        projMesh = ArtConfigs.Artconfig.BloodSwordMesh,
        fireSFX = 18,
        hitSFX = 9,
    };

    public static WeaponConfig EvilDagger = new WeaponConfig
    {
        _name = "Evil Dagger",
        id = 301,
        rating = 4,
        type = 2,
        attack = 7.5f,
        pen = 0.3f,
        life = 3f,
        cd = 0.2f,
        selfDet = true,
        projectileSpeed = 6,
        aoe = true,
        damageRange = 0.75f,
        info = "And what must we give it return?",
        intro = "Fast and lethal",
        hitAnim = -1,
        fireAnim = -1,
        isMagic = true,
        mesh = ArtConfigs.Artconfig.EvilDaggerMesh,
        projMesh = ArtConfigs.Artconfig.EvilDaggerMesh,
        fireSFX = 18,
        hitSFX = 9,
    };

    public static WeaponConfig Keris = new WeaponConfig
    {
        _name = "Keris",
        id = 201,
        rating = 3,
        type = 2,
        attack = 7,
        pen = 0.5f,
        life = 3f,
        cd = 0.25f,
        selfDet = true,
        projectileSpeed = 6,
        aoe = true,
        damageRange = 1.7f,
        info = "Hold your mouse button and enjoy",
        intro = "A sword with weird shape",
        hitAnim = -1,
        fireAnim = -1,
        isMagic = false,
        mesh = ArtConfigs.Artconfig.KerisMesh,
        projMesh = ArtConfigs.Artconfig.KerisMesh,
        fireSFX = 18,
        hitSFX = 9,
    };

    // Getters and Setters
    private const int WHITE_BEGIN = 0;
    private const int WHITE_COUNT = 3;
    private const int GREEN_BEGIN = 100;
    private const int GREEN_COUNT = 3;
    private const int BLUE_BEGIN = 200;
    private const int BLUE_COUNT = 2;
    private const int PURPLE_BEGIN = 300;
    private const int PURPLE_COUNT = 2;
    private const int ORANGE_BEGIN = 400;
    private const int ORANGE_COUNT = 1;

    public WeaponConfig _getWeaponConfig(int id)
	{
        switch (id)
        {
            case -2:
                return WeaponConfigs.Fork;
            case 0:
                return WeaponConfigs.Pistol;
            case 1:
                return WeaponConfigs.LMG;
            case 2:
                return WeaponConfigs.Wand;
            case 100:
                return WeaponConfigs.RPG;
			case 200:
                return WeaponConfigs.RubyLaser;
			case 101:
                return WeaponConfigs.HeatLaser;
            case 300:
                return WeaponConfigs.Kornet;
            case 301:
                return WeaponConfigs.EvilDagger;
            case 102:
                return WeaponConfigs.BloodSword;
            case 201:
                return WeaponConfigs.Keris;
            default:
                return WeaponConfigs.Pistol;
        }
    }

    public WeaponConfig getWeaponConfig()
    {
        // Select a random rarity level based on chances
        int rarityRoll = Random.Range(1, 101);
        int rarityLevel = 1; // default to white rarity


        if (rarityRoll > 95)
        {
            rarityLevel = 5; // orange rarity
        }
        if (rarityRoll > 85 && rarityRoll <= 95)
        {
            rarityLevel = 4; // purple rarity
        }
        if (rarityRoll > 65 && rarityRoll <= 85)
        {
            rarityLevel = 3; // blue rarity
        }
        if (rarityRoll > 40 && rarityRoll <= 65)
        {
            rarityLevel = 2; // green rarity
        }

        // Get a random index for the rarity level
        int begin = GetIndexBegin(rarityLevel);
        int end = GetIndexEnd(rarityLevel) + 1;
        int index = Random.Range(begin, end);
        // Get the upgrade config based on the selected index
        WeaponConfig config = _getWeaponConfig(index);
        return config;
    }

    private int GetIndexBegin(int rarityLevel)
    {
        switch (rarityLevel)
        {
            case 1:
                return WHITE_BEGIN;
            case 2:
                return GREEN_BEGIN;
            case 3:
                return BLUE_BEGIN;
            case 4:
                return PURPLE_BEGIN;
            case 5:
                return ORANGE_BEGIN;
            default:
                return WHITE_BEGIN;
        }
    }

    private int GetIndexEnd(int rarityLevel)
    {
        switch (rarityLevel)
        {
            case 1:
                return WHITE_BEGIN + WHITE_COUNT - 1;
            case 2:
                return GREEN_BEGIN + GREEN_COUNT - 1;
            case 3:
                return BLUE_BEGIN + BLUE_COUNT - 1;
            case 4:
                return PURPLE_BEGIN + PURPLE_COUNT - 1;
            case 5:
                return ORANGE_BEGIN + ORANGE_COUNT - 1;
            default:
                return WHITE_BEGIN + WHITE_COUNT - 1;
        }
    }
}
