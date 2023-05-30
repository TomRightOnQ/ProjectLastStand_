using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

// Manage in-game UI status
public class GameUI : MonoBehaviourPunCallbacks
{
    public static GameUI Instance;
    // Player Info UIs
    [SerializeField] Slider HpS;
    [SerializeField] private WeaponInfo weapon1Info;
    [SerializeField] private WeaponInfo weapon2Info;
    [SerializeField] private Players player;
    [SerializeField] private GameObject content;

    public GameObject volObj;
    public GameObject playerUIObj;
    public TextMeshProUGUI countDown;

    private List<WeaponInfo> weaponInfos = new List<WeaponInfo>();
    private List<DroppedItems> droppedList;
    private List<ItemListings> itemList;

    public List<DroppedItems> DroppedList { get { return droppedList; } set { droppedList = value; } }
    public List<ItemListings> ItemList { get { return itemList; } set { itemList = value; } }
    public GameObject Content { get { return content; } }

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
        // Check for ItemListings without corresponding DroppedItems and remove them
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            ItemListings item = ItemList[i];
            DroppedItems droppedItem = DroppedList.Find(x => x.DroppedId == item.DroppedId);

            if (droppedItem == null)
            {
                ItemList.RemoveAt(i);
                if (WeaponChoice.Instance.GetID() == item.DroppedId) {
                    WeaponChoice.Instance.HidePanel();
                }
                Destroy(item.gameObject);
            }
        }
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

    // Delete a dropped item
    // Destroy both network and local
    public void RemoveDroppedItem(long targetId)
    {
        DroppedItems targetDropped = DroppedList.Find(x => x.DroppedId == targetId);
        if (targetDropped != null)
        {
            Player owner = targetDropped.photonView.Owner;
            if (!PhotonNetwork.IsConnected)
            {
                Destroy(targetDropped.gameObject);
            }
            else if (PhotonNetwork.IsConnected && owner != null)
            {
                if (targetDropped.photonView.IsMine) {
                    Debug.Log("Destroying dropped item -> Owner: " + targetDropped.name);
                    PhotonNetwork.Destroy(targetDropped.gameObject);
                }
                Debug.Log("Requesting destroy dropped item from owner: " + owner.NickName);
                photonView.RPC("RPCRequestDestroyDroppedItem", owner, targetDropped.ViewID);
            }
        }
    }

    [PunRPC]
    public void RPCRequestDestroyDroppedItem(int viewID)
    {
        DroppedItems targetDropped = GameManager.Instance.GetDroppedItems(viewID);
        if (targetDropped != null && targetDropped.photonView.IsMine)
        {
            Debug.Log("RPC: Received request to destroy dropped item: " + targetDropped.name);
            PhotonNetwork.Destroy(targetDropped.gameObject);
        }
        else
        {
            Debug.LogWarning("RPC: Invalid request to destroy dropped item with target ViewID: " + viewID);
        }
    }

    public void SetWeaponInfo(int slot, Weapons weapon) {
        weaponInfos[slot].SetWeaponInfo(slot, weapon);
    }
}
