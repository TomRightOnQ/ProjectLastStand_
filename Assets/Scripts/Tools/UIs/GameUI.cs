using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using static WeaponConfigs;

// Manage in-game UI status
public class GameUI : MonoBehaviourPunCallbacks
{
    public static GameUI Instance;
    // Player Info UIs
    [SerializeField] Slider HpS;
    [SerializeField] WeaponInfo weapon1Info;
    [SerializeField] WeaponInfo weapon2Info;
    private List<WeaponInfo> weaponInfos = new List<WeaponInfo>();
    [SerializeField] Players player;

    private List<DroppedItems> droppedList;
    private List<ItemListings> itemList;
    private List<int> itemIDs;
    public List<DroppedItems> DroppedList { get { return droppedList; } set { droppedList = value; } }
    public List<ItemListings> ItemList { get { return itemList; } set { itemList = value; } }

    bool found = false;

    private void Awake()
    {
        Instance = this;
        DroppedList = new List<DroppedItems>();
        ItemList = new List<ItemListings>();
    }

    private void Start()
    {
        weaponInfos.Add(weapon1Info);
        weaponInfos.Add(weapon2Info);
    }

    private void Update()
    {
        if (!found) {
            player = GameManager.Instance.GetLocalPlayer();
            if (player != null)
            {
                found = true;
            }
        }
        if (!found) return;
        HpS.maxValue = player.HitPoints;
        HpS.value = player.CurrentHitPoints;
    }

    // Item listing
    public void AddItem(ItemListings item) {
        itemList.Add(item);
    }
    public void RemoveDroppedItem(DroppedItems dropped)
    {
        if (DroppedList.Contains(dropped))
        {
            // Remove from DroppedList
            DroppedList.Remove(dropped);

            // Find the corresponding ItemListing
            ItemListings item = ItemList.Find(x => x.DroppedId == dropped.DroppedId);
            if (item != null)
            {
                // Remove from ItemList and destroy the game object
                ItemList.Remove(item);
                Destroy(item.gameObject);
            }
        }
    }

    public void SetWeaponInfo(int slot, Weapons weapon) {
        weaponInfos[slot].SetWeaponInfo(weapon);
    }
}
