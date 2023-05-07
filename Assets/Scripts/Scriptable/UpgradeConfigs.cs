using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hold the Configuration of all upgrades
[CreateAssetMenu(menuName = "Configs/UpgradeConfigs")]
public class UpgradeConfigs : ScriptableSingleton<UpgradeConfigs>
{
    // Rating
    // 1 - 5 White, Green, Blue, Purple, Orange
    // Chance: 40:25:20:10:5

    public struct UpgradeConfig
    {
        public string _name; // Name of the buff
        public int id; // ID of the buff
        public int rating; // Rarity
        public float hitPoints; // Add to the upper limit of the players HP
        public float speed; // Add to the player's speed modifier, 0.05 means increaae by 5%
        public float regen; // Heal the player instantly
        public float defaultAttack; // Add to the player's physical attack modifier
        public float defaultWeaponAttack; // Add to the player's magical attack modifier
        public float defaultDefence; // Add to the player's defence modifier
        public float defaultMagicDefence; // Add to the player's magical defence modifier
        public int specialEffectIndex; // Special effects not yet implemented
        public string description; // A description
    }

    // Data Scetion
    public static UpgradeConfig LifeBoost = new UpgradeConfig
    {
        _name = "Life Boost",
        id = 0,
        rating = 1,
        hitPoints = 2,
        speed = 0,
        regen = 0,
        defaultAttack = 0,
        defaultWeaponAttack = 0,
        defaultDefence = 0,
        defaultMagicDefence = 0,
        specialEffectIndex = -1,
        description = "Increase your HP by 2" 
    };

    public static UpgradeConfig AttackBoost = new UpgradeConfig
    {
        _name = "AP Boost",
        id = 1,
        rating = 2,
        hitPoints = 0,
        speed = 0,
        regen = 0,
        defaultAttack = 0.1f,
        defaultWeaponAttack = 0,
        defaultDefence = 0,
        defaultMagicDefence = 0,
        specialEffectIndex = -1,
        description = "Increase your physcial damage by 10%"
    };

    public static UpgradeConfig Regenration = new UpgradeConfig
    {
        _name = "Regenration",
        id = 2,
        rating = 1,
        hitPoints = 10,
        speed = 0,
        regen = 10,
        defaultAttack = 0,
        defaultWeaponAttack = 0,
        defaultDefence = 0,
        defaultMagicDefence = 0,
        specialEffectIndex = -1,
        description = "Heal 2 points of HP"
    };

    // Getters and Setters
    private const int WHITE_BEGIN = 0;
    private const int WHITE_COUNT = 2;
    private const int GRTEE_BEGIN = 100;
    private const int GREEN_COUNT = 1;
    private const int BLUE_BEGIN = 200;
    private const int BLUE_COUNT = 0;
    private const int PURPLE_BEGIN = 300;
    private const int PURPLE_COUNT = 0;
    private const int ORANGE_BEGIN = 400;
    private const int ORANGE_COUNT = 0;

    public UpgradeConfig _getUpgradeConfig(int id)
    {
        UpgradeConfig upgradeData = UpgradeConfigs.LifeBoost; // default config
        switch (id)
        {
            case 0:
                upgradeData = UpgradeConfigs.LifeBoost;
                break;
            case 100:
                upgradeData = UpgradeConfigs.AttackBoost;
                break;
            case 1:
                upgradeData = UpgradeConfigs.Regenration;
                break;
                // add more cases for other upgrade configs
        }
        return upgradeData;
    }

    public UpgradeConfig getUpgradeConfig()
    {
        // Select a random rarity level based on chances
        int rarityRoll = Random.Range(1, 101);
        int rarityLevel = 1; // default to white rarity

        if (rarityRoll > 40)
        {
            rarityLevel = 2; // green rarity
        }
  
        else if (rarityRoll > 65)
        {
            rarityLevel = 3; // blue rarity
        }
        else if (rarityRoll > 85)
        {
            rarityLevel = 4; // purple rarity
        }
        else if (rarityRoll > 95)
        {
            rarityLevel = 5; // orange rarity
        }

        // Get a random index for the rarity level
        int begin = GetIndexBegin(rarityLevel);
        int end = GetIndexEnd(rarityLevel) + 1;
        int index = Random.Range(begin, end);
        // Get the upgrade config based on the selected index
        UpgradeConfig config = _getUpgradeConfig(index);
        return config;
    }

    private int GetIndexBegin(int rarityLevel)
    {
        switch (rarityLevel)
        {
            case 1:
                return WHITE_BEGIN;
            case 2:
                return GRTEE_BEGIN;
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
                return GRTEE_BEGIN + GREEN_COUNT - 1;
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
