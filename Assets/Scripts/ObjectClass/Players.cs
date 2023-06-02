using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using static WeaponConfigs;

// All players are a Players object
public class Players : Entities, IPunObservable
{
    private PrefabManager prefabReference;
    public static WeaponConfigs Instance;
    // Player 1 - 4
    [SerializeField] private int index = 0;
    [SerializeField] private float fortune = 1;
    [SerializeField] private bool isAlive = true;
    [SerializeField] private bool isReady = false;
    [SerializeField] private float timeForRevive = 15f;

    private bool armed = false;
    private const string PREFAB_LOC = "Prefabs/";

    // Weapons
    private List<Weapons> weapons = new List<Weapons>();

    // Effects collected
    [SerializeField] private Effects _effect;
    private List<int> effectHeld = new List<int>();
    public int MeleeCounts = 0;

    float reviveCount = 0;

    public List<Weapons> WeaponList { get { return weapons; } set { weapons = value; } }
    public int Index { get { return index; } set { index = value; } }
    public float Fortune { get { return fortune; } set { fortune = value; } }
    public bool Armed { get { return armed; } set { armed = value; } }
    public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
    public float TimeForRevive { get { return timeForRevive; } set { timeForRevive = value; } }
    public bool IsReady { get { return isReady; } set { isReady = value; } }

    // Locked effects
    private int[] IMMORTAL_C = new int[4] { 0, 100, 200, 300 };
    private bool imUnlocked = false;
    private int[] FLIGHT_C = new int[4] { 1, 101, 201, 301 };
    private bool flUnlocked = false;
    private int[] DAMAGE_C = new int[4] { 2, 102, 202, 302 };
    private bool daUnlocked = false;
    private int[] NOVA_C = new int[4] { 6, 106, 206, 306 };
    private bool noUnlocked = false;
    private int[] ASSASSI_C = new int[4] { 7, 107, 207, 307 };
    private bool asUnlocked = false;

    // Sync
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
        if (stream.IsWriting)
        {
            stream.SendNext(index);
            stream.SendNext(isAlive);
            stream.SendNext(timeForRevive);
        }
        else
        {
            index = (int)stream.ReceiveNext();
            isAlive = (bool)stream.ReceiveNext();
            timeForRevive = (float)stream.ReceiveNext();
        }
    }

    void Awake()
    {
        gameObject.tag = "Player";
        _effect = GetComponent<Effects>();
        isAlive = true;
    }

    public void SetNova()
    {
        WeaponList[0].IsNova = true;
        WeaponList[1].IsNova = true;
    }

    public void EnhanceAoe()
    {
        WeaponList[0].DamageRangeMod += 0.25f;
        WeaponList[1].DamageRangeMod += 0.25f;
    }

    public void UpdateStats()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine) {
            return;
        }
        PlayerListener.Instance.UpdatePlayerStats(this);
    }

    public void UpdateEffects()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }
        PlayerListener.Instance.UpdatePlayerEffects(index);
    }

    public void UpdateHitPoints() 
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }
        PlayerListener.Instance.UpdatePlayerHP(currentHitPoints, hitPoints);
    }

    // Add effects
    public virtual void AddEffect(int index, int level)
    {
        _effect.SetUp(index, level);
    }

    // Effect check
    public virtual void AddToEffectList(int index)
    {
        UpdateStats();
        if (!effectHeld.Contains(index))
        {
            effectHeld.Add(index);
            UpdateEffects();
        }
    }

    public virtual bool CheckCondition(int[] arr)
    {
        foreach (int value in arr)
        {
            if (!effectHeld.Contains(value))
            {
                return false;
            }
        }
        return true;
    }

    public virtual bool CheckCondition(int id)
    {
        if (!effectHeld.Contains(id))
        {
            return false;
        }
        return true;
    }

    // Set prefabreference
    public void SetPrefabManager(PrefabManager prefabReference)
    {
        this.prefabReference = prefabReference;
    }
    /* Add a Weapon
    slot: 1 or 2, indicate the current slot
    id: weapon id
    return: add a Weapons type object instance to Weapons[] array */
    public void addWeapon(int slot, int id, int level) {
        WeaponConfig weaponData = WeaponConfigs.Instance._getWeaponConfig(id);
        if (level <= 0)
        {
            weapons[slot].SetWeapons(weaponData);
        }
        else {
            weapons[slot].Upgrade(1);
        }
        if (PhotonNetwork.IsConnected && photonView.IsMine) {
            setWeaponPreview(slot);
        } else if (!PhotonNetwork.IsConnected) {
            setWeaponPreview(slot);
        }
    }

    // Attack!
    public void fire(Vector3 targetPosition) {
        foreach (Weapons weapon in weapons) {
            if (weapon == null) {
                return;
            }
            weapon.transform.LookAt(new Vector3(targetPosition.x, weapon.transform.position.y, targetPosition.z));
            weapon.Fire(index, targetPosition, defaultAttack, defaultWeaponAttack, criticalRate, criticalDamage);
        }
    }

    // Set Weapon Info
    private void setWeaponPreview(int slot)
    {

        GameUI.Instance.SetWeaponInfo(slot, weapons[slot]);
        WeaponChoice.Instance.SetWeaponInfo(slot, weapons[slot]);
    }

    // Weapon Picked up
    public void AddDroppedItem(DroppedItems dropped)
    {
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            // Check if already recorded
            if (!GameUI.Instance.DroppedList.Contains(dropped))
            {
                GameUI.Instance.DroppedList.Add(dropped);
                GameObject itemObj = Instantiate(PrefabManager.Instance.ItemListing, Vector3.zero, Quaternion.identity);
                ItemListings itemListing = itemObj.GetComponent<ItemListings>();
                itemListing.DroppedId = dropped.DroppedId;
                itemListing.WeaponIndex = dropped.WeaponIndex;
                itemListing.SetUp();
                GameUI.Instance.AddItem(itemListing);
            }
        }
    }

    public void RemoveDroppedItem(DroppedItems dropped)
    {
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            // Check if already recorded
            if (GameUI.Instance.DroppedList.Contains(dropped))
            {
                GameUI.Instance.RemoveDroppedItem(dropped);
            }
        }
    }

    public void PlayerDead()
    {
        isAlive = false;
        GameUI.Instance.playerUIObj.SetActive(true);
        GameUI.Instance.volObj.SetActive(true);
        reviveCount = 0;
        transform.position = new Vector3(-20f, -40f, -20f);
    }

    public void PlayerAlive()
    {
        isAlive = true;
        currentHitPoints = HitPoints;
        GameUI.Instance.playerUIObj.SetActive(false);
        GameUI.Instance.volObj.SetActive(false);
        transform.position = new Vector3(Random.Range(-25f, 25f), 0.01f, (Random.Range(-25f, 25f)));
        
    }

    // Bars
    public void UpdateHP()
    {
        if (currentHitPoints >= hitPoints)
        {
            currentHitPoints = hitPoints;
        }
        UpdateHitPoints();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void LateUpdate()
    {
        if (isAlive)
        {
            checkFfect();
            if (!armed && weapons.Count >= 2)
            {
                addWeapon(0, -1, 0);
                addWeapon(1, -1, 0);
                armed = true;
            }
            if (currentHitPoints < hitPoints / 2)
            {
                currentHitPoints += 0.001f;
            }
            if (criticalRate > 1) {
                criticalRate = 1;
            }
            UpdateHP();
            criticalDamage = criticalMod * criticalBase;
            defaultAttack = DamageBase * DamageMod;
            defaultWeaponAttack = WeaponDamageBase * DamageMod;
            speed = SpeedBase * SpeedMod;
            UpdateStats();
            if (currentHitPoints <= 0)
            {
                if (PhotonNetwork.IsConnected && !photonView.IsMine) {
                    return;
                }
                PlayerDead();
            }
        }
        else {
            if (PhotonNetwork.IsConnected && !photonView.IsMine)
            {
                return;
            }
            if (reviveCount >= timeForRevive) {
                PlayerAlive();
            }
            reviveCount += Time.deltaTime;
            GameUI.Instance.countDown.text = Mathf.Ceil(timeForRevive - reviveCount).ToString();
        }
    }

    private void checkFfect()
    {
        if (!daUnlocked)
        {
            if (CheckCondition(DAMAGE_C))
            {
                daUnlocked = true;
                UpgradeConfigs.Instance.Unlock(400);
            }
        }

        if (!imUnlocked)
        {
            if (CheckCondition(IMMORTAL_C))
            {
                imUnlocked = true;
                UpgradeConfigs.Instance.Unlock(401);
            }
        }

        if (!flUnlocked)
        {
            if (CheckCondition(FLIGHT_C))
            {
                flUnlocked = true;
                UpgradeConfigs.Instance.Unlock(402);
            }
        }

        if (!noUnlocked)
        {
            if (CheckCondition(NOVA_C))
            {
                noUnlocked = true;
                UpgradeConfigs.Instance.Unlock(406);
            }
        }

        if (!asUnlocked)
        {
            if (CheckCondition(ASSASSI_C))
            {
                asUnlocked = true;
                UpgradeConfigs.Instance.Unlock(407);
            }
        }

        if (hitPoints <= 3)
        {
            UpgradeConfigs.Instance.Lock(105);
        }
        else {
            UpgradeConfigs.Instance.Unlock(105);
        }
    }
}
