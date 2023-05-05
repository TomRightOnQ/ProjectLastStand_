using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

// Load configuration files
public class ConfigManager : MonoBehaviour
{
    private WeaponConfig[] WeaponTemplate;
    private string weaponConfigName = "WeaponConfig.json";

    private bool loaded;

    // Data Struct Sector
    public struct WeaponConfig
    {
        public string name;
        public int id;
        public int type;
        public float attack;
        public float pen;
        public float life;
        public float cd;
        public bool selfDet;
        public float projectileSpeed;
        public float damageRange;
        public bool aoe;
    }

    // Loader
    public static ConfigManager Instance;
    public T LoadConfig<T>(string filename)
    {

        string path = Path.Combine(Application.streamingAssetsPath, "Config", filename);

        if (!File.Exists(path))
        {
            Debug.LogError($"Failed to load config file {filename}: file does not exist");
            return default;
        }

        string json = File.ReadAllText(path);
        T config = JsonConvert.DeserializeObject<T>(json);
        return config;
    }

    // Load Configurations at start
    public void Load()
    {
        WeaponTemplate = LoadConfig<WeaponConfig[]>(weaponConfigName);
        loaded = true;
        Debug.Log(WeaponTemplate);
    }

    // Get Weapons
    public WeaponConfig[] getWeapons() {
        return WeaponTemplate;
    }


    public bool ConfigLoaded() {
        return loaded;
    }
}
