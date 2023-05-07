using System.Collections;
using System.Collections.Generic;
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
    }

    public static WeaponConfig Pistol = new WeaponConfig
    {
        _name = "Pistol",
        id = 0,
        rating = 1,
        type = 0,
        attack = 2,
        pen = 0.1f,
        life = 1,
        cd = 0.1f,
        selfDet = false,
        projectileSpeed = 10,
        aoe = false,
        damageRange = 0.1f,
        info = "Better than nothing...",
        intro = "A default weapon"
    };

    public static WeaponConfig RPG = new WeaponConfig
    {
        _name = "RPG",
        id = 1,
        type = 0,
        rating = 2,
        attack = 20,
        pen = 0.5f,
        life = 3,
        cd = 2,
        selfDet = true,
        projectileSpeed = 5,
        aoe = true,
        damageRange = 0.5f,
        info = "R! P! G!",
        intro = "Explosive weapon that causes AOE damage"
    };

    public static WeaponConfig HeatLaser = new WeaponConfig
    {
        _name = "HeatLaser",
        id = 2,
        rating = 1,
        type = 2,
        attack = 2,
        pen = 0.5f,
        life = 0.1f,
        cd = 0.05f,
        selfDet = true,
        projectileSpeed = 5,
        aoe = true,
        damageRange = 0.1f,
        info = "OVERLOAD!",
        intro = "Burning the first enemy with a beam of laser"
    };

    public static WeaponConfig RubyLaser = new WeaponConfig
    {
        _name = "RubyLaser",
        id = 3,
        rating = 2,
        type = 1,
        attack = 20,
        pen = 0.5f,
        life = 0.1f,
        cd = 2,
        selfDet = true,
        projectileSpeed = 5,
        aoe = true,
        damageRange = 0.1f,
        info = "Chaerging...",
        intro = "Slow but destructive laser cannon"
    };

    public WeaponConfig getWeaponConfig(int id)
    {
        WeaponConfig weaponData = Pistol;
        switch (id)
        {
            case 0:
                weaponData = WeaponConfigs.Pistol;
            break;
            case 1:
                weaponData = WeaponConfigs.RPG;
            break;
            case 2:
                weaponData = WeaponConfigs.HeatLaser;
            break;
            case 3:
                weaponData = WeaponConfigs.RubyLaser;
            break;
        }
        return weaponData;
    }
}