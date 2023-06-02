using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;

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
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float globalTime = 0f;

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI weaponAttackText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private TextMeshProUGUI magicDefenceText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI critcalRateText;
    [SerializeField] private TextMeshProUGUI critcalDamageText;
    private const string HP = "HP: ";
    private const string ATK = "Attack: ";
    private const string MATK = "Magical Attack: ";
    private const string DEF = "Defense: ";
    private const string MDEF = "Magical Defense: ";
    private const string SPD = "Speed: ";
    private const string CRR = "Cri.Rate: ";
    private const string CRD = "Cri.DMG: ";

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
        StartCoroutine(UpdateTimerCoroutine());
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
        HpS.maxValue = PlayerListener.Instance.HitPoints;
        HpS.value = PlayerListener.Instance.CurrentHitPoints;
        UpdatePlayerStats();

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

    private IEnumerator UpdateTimerCoroutine()
    {
        while (true)
        {
            UpdateTime();
            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateTime()
    {
        globalTime = PlayerListener.Instance.GlobalTime;
        int hours = Mathf.FloorToInt(globalTime / 3600);
        int minutes = Mathf.FloorToInt((globalTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(globalTime % 60);

        string formattedTime = $"{hours:00}:{minutes:00}:{seconds:00}";
        timerText.text = formattedTime;
    }

    private void UpdatePlayerStats()
    {
        hpText.text = HP + Mathf.Ceil(PlayerListener.Instance.HitPoints).ToString();
        attackText.text = ATK + (PlayerListener.Instance.DefaultAttack * 100f).ToString() + "%";
        weaponAttackText.text = MATK + (PlayerListener.Instance.DefaultWeaponAttack * 100f).ToString() + "%";
        defenceText.text = DEF + Mathf.Ceil(PlayerListener.Instance.DefaultDefence).ToString();
        magicDefenceText.text = MDEF + (PlayerListener.Instance.DefaultMagicDefence * 100f).ToString() + "%";
        speedText.text = SPD + (PlayerListener.Instance.Speed * 100f).ToString() + "%";
        critcalRateText.text = CRR + (PlayerListener.Instance.CriticalRate * 100f).ToString() + "%";
        critcalDamageText.text = CRD + (PlayerListener.Instance.CriticalDamage * 100f).ToString() + "%";
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
