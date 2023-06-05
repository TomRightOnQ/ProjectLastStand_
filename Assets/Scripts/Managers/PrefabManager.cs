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

    [SerializeField] private GameObject iceRingPrefab; // Rings
    [SerializeField] private GameObject fireRingPrefab;
    [SerializeField] private GameObject tacticalShieldRingPrefab;

    [SerializeField] private GameObject buffIcon; // Buff listing
    [SerializeField] private GameObject monsterResult; // Monster listing

    [SerializeField] private GameObject sfxPrefab;

    public GameObject MonsterPrefab => monsterPrefab;
    public GameObject ProjPrefab => projPrefab;
    public GameObject LaserPrefab => laserPrefab;
    public GameObject PlayerPrefab => playerPrefab;
    public GameObject WeaponPrefab => weaponPrefab;
    public GameObject DamageNumberPrefab => damageNumberPrefab;
    public GameObject ExpAndLevels => expAndLevels;
    public GameObject DroppedWeapon => droppedWeapon;
    public GameObject ItemListing => itemListing;
    public GameObject ExplosionPrefab => explosionPrefab;
    public GameObject IndicatorPrefab => indicatorPrefab;
    public GameObject IceRingPrefab => iceRingPrefab;
    public GameObject FireRingPrefab => fireRingPrefab;
    public GameObject TacticalShieldRingPrefab => tacticalShieldRingPrefab;
    public GameObject BuffIcon => buffIcon;
    public GameObject MonsterResult => monsterResult;
    public GameObject SfxPrefab => sfxPrefab;
}
