using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

// Hold the Configuration of all upgrades
[CreateAssetMenu(menuName = "Configs/UpgradeConfigs")]
public class UpgradeConfigs : ScriptableSingleton<UpgradeConfigs>
{
    // Rating
    // 1 - 5 White, Green, Blue, Purple, Orange
    // Chance: 40:25:20:10:5
    private Dictionary<int, UpgradeConfig> upgradeConfigDictionary = new Dictionary<int, UpgradeConfig>();
    public bool ready = false;

    private const string FILE_NAME = "UpgradeConfigs.json";

    // Getters and Setters
    private const int WHITE_BEGIN = 0;
    private int WHITE_COUNT = 0;
    private const int GRTEE_BEGIN = 100;
    private int GREEN_COUNT = 0;
    private const int BLUE_BEGIN = 200;
    private int BLUE_COUNT = 0;
    private const int PURPLE_BEGIN = 300;
    private int PURPLE_COUNT = 0;
    private const int ORANGE_BEGIN = 400;
    private int ORANGE_COUNT = 0;

    public struct UpgradeConfig
    {
        public string _name; // Name of the buff
        public int id; // ID of the buff
        public int rating; // Rarity
        public int specialEffectIndex; // Special effects not yet implemented
        public string description; // A description
        public int level;
        public string mesh;
    }

    public void InitEffect()
    {
        string path = Path.Combine(Application.streamingAssetsPath, FILE_NAME);
        string json = File.ReadAllText(path);
        UpgradeConfig[] configs = JsonConvert.DeserializeObject<UpgradeConfig[]>(json);

        // Clear the existing dictionary
        upgradeConfigDictionary.Clear();

        // Populate the dictionary with UpgradeConfig objects
        foreach (UpgradeConfig config in configs)
        {
            upgradeConfigDictionary[config.id] = config;
        }

        // Update the count variables based on the loaded data
        WHITE_COUNT = configs.Count(c => c.rating == 1) - 1;
        GREEN_COUNT = configs.Count(c => c.rating == 2);
        BLUE_COUNT = configs.Count(c => c.rating == 3);
        PURPLE_COUNT = configs.Count(c => c.rating == 4) - 4;
        ORANGE_COUNT = configs.Count(c => c.rating == 5);
    }

    public void AddUpgradeConfig(UpgradeConfig config)
    {
        upgradeConfigDictionary.Add(config.id, config);
    }


    public UpgradeConfig _getUpgradeConfig(int id)
    {
        Debug.Log("Getting config with id " + id.ToString());
        upgradeConfigDictionary.TryGetValue(id, out UpgradeConfig upgradeConfig);
        return upgradeConfig;
    }


    public UpgradeConfig getUpgradeConfig()
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
