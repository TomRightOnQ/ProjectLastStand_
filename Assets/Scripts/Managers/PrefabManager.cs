using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hold the Configuration of all monsters
[CreateAssetMenu(menuName = "Manager/PrefabManager")]
public class PrefabManager : ScriptableSingleton<PrefabManager>
{
    [SerializeField] private GameObject monsterPrefab; // Monsters
    [SerializeField] private GameObject projPrefab; // Projectiles
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject playerPrefab; // Players
    [SerializeField] private GameObject weaponPrefab; // Weapons
    [SerializeField] private GameObject damageNumberPrefab; // Damage Number system
    [SerializeField] private GameObject expAndLevels; // Exp System
    [SerializeField] private GameObject droppedWeapon; // Dropped items - weapon
    [SerializeField] private GameObject itemListing; // Items on the list right side
    [SerializeField] private GameObject explosionPrefab; // Explosions
    [SerializeField] private GameObject indicatorPrefab; // Player Indicator

    public GameObject MonsterPrefab { get { return monsterPrefab; } }
    public GameObject ProjPrefab { get { return projPrefab; } }
    public GameObject LaserPrefab { get { return laserPrefab; } }
    public GameObject PlayerPrefab { get { return playerPrefab; } }
    public GameObject WeaponPrefab { get { return weaponPrefab; } }
    public GameObject DamageNumberPrefab { get { return damageNumberPrefab; } }
    public GameObject ExpAndLevels { get { return expAndLevels; } }
    public GameObject DroppedWeapon { get { return droppedWeapon; } }
    public GameObject ItemListing { get { return itemListing; } }
    public GameObject ExplosionPrefab { get { return explosionPrefab; } }
    public GameObject IndicatorPrefab { get { return indicatorPrefab; } }
}
