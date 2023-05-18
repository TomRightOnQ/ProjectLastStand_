using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using static WeaponConfigs;

// All players are a Players object
public class Players : Entities, IPunObservable
{
    private PrefabManager prefabReference;
    public static WeaponConfigs Instance;
    // Player 1 - 4
    [SerializeField] private int index = 0;
    [SerializeField] private float fortune = 1;
    private bool armed = false;
    private const string PREFAB_LOC = "Prefabs/";
    // Levels
    // [SerializeField] private int level = 1;

    // Weapons
    private List<Weapons> weapons = new List<Weapons>();
    public List<Weapons> WeaponList { get { return weapons; } set { weapons = value; } }
    // private int weapon1Index = -1;
    // private int weapon2Index = -1;

    void Start()
    {
        gameObject.tag = "Player";
        Debug.Log("Ready");
    }

    // Sync
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
    }

    // Bars
    public void UpdateHP()
    {
        if (currentHitPoints >= hitPoints) {
            currentHitPoints = hitPoints;
        }
    }

    public int Index { get { return index; } set { index = value; } }

    public float Fortune { get { return fortune; } set { fortune = value; } }

    public bool Armed { get { return armed; } set { armed = value; } }

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
            weapons[slot].Upgrade(level);
        }
        if (PhotonNetwork.IsConnected && photonView.IsMine) {
            setWeaponPreview(slot);
        }
    }

    // Attack!
    public void fire() {
        foreach (Weapons weapon in weapons) {
            if (weapon == null) {
                return;
            }
            if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
            {
                weapon.Fire(index, GetAimDirection(), defaultAttack, defaultWeaponAttack);
            }
            else {
                int weaponViewID = -1;
                weaponViewID = weapon.photonView.ViewID;
                photonView.RPC("FireForPlayer", RpcTarget.MasterClient, weaponViewID, index, photonView.ViewID, GetAimDirection());
            }
            index += 1;
        }
    }

    private Vector3 GetAimDirection()
    {
        return transform.forward;
    }

    // For client firing, let master do it
    [PunRPC]
    private void FireForPlayer(int weaponViewID, int index, int playerViewID, Vector3 direction0, PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("FireForPlayer RPC called on non-master client");
            return;
        }
        Players player = GameManager.Instance.GetLocalPlayer(playerViewID);

        if (weaponViewID != -1)
        {
            PhotonView weapon1View = PhotonView.Find(weaponViewID);
            if (weapon1View != null)
            {
                Weapons weapon = weapon1View.GetComponent<Weapons>();
                if (weapon != null)
                {
                    weapon.Fire(index, direction0, defaultAttack, defaultWeaponAttack);
                }
            }
        }
    }

    // Set Weapon Info
    private void setWeaponPreview(int slot)
    {
        Debug.Log("Setting preview");
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

    private void Update()
    {
        if (!armed && weapons.Count >= 2) {
            addWeapon(0, -1, 0);
            addWeapon(1, -1, 0);
            armed = true;
        }
        currentHitPoints += 0.005f;
        UpdateHP();
    }
}
