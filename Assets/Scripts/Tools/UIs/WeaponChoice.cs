using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using static WeaponConfigs;

// UI for weapon choice
public class WeaponChoice : MonoBehaviourPunCallbacks
{
    public static WeaponChoice Instance;

    [SerializeField] private WeaponInfo weapon1Info;
    [SerializeField] private WeaponInfo weapon2Info;
    [SerializeField] private WeaponInfo weapon3Info;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI weapon1Hint;
    [SerializeField] private TextMeshProUGUI weapon2Hint;
    private List<WeaponInfo> weaponInfos = new List<WeaponInfo>();
    [SerializeField] private int chosenWeapon = -1;
    private int levelAdder = 0;
    private long droppedId;
    [SerializeField] private bool isOpend = false;
    public bool IsOpened { get { return isOpend; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        weaponInfos.Add(weapon1Info);
        weaponInfos.Add(weapon2Info);
        weaponInfos.Add(weapon3Info);
        HidePanel();
    }

    private void Update()
    {
        if (chosenWeapon == -1)
        {
            confirmButton.gameObject.SetActive(false);
        }
        if (isOpend && Input.GetKeyDown(KeyCode.F)) 
        {
            HidePanel();
        }
    }

    // find if a weapon is the same type
    public int FindWeapon(WeaponConfig weaponData)
    {
        if (weapon1Info.ID == weaponData.id) {
            return 0;
        } else if (weapon2Info.ID == weaponData.id) {
            return 1;
        }
        return -1;
    }

    public void SetID(long id)
    {
        droppedId = id;
    }
    public long GetID()
    {
        return droppedId;
    }

    public void SetWeaponInfo(int slot, WeaponConfig weaponData, int _level)
    {
        weaponInfos[slot].SetWeaponInfo(slot, weaponData, _level);
    }

    public void SetWeaponInfo(int slot, Weapons weapon)
    {
        weaponInfos[slot].SetWeaponInfo(slot, weapon);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
        isOpend = false;
        chosenWeapon = -1;
        weapon1Info.CloseMenu();
        weapon2Info.CloseMenu();
        droppedId = 0; // Reset the droppedId
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        isOpend = true;
    }

    // Confirm
    public void ConfirmWeaponChoice()
    {
        if (chosenWeapon != -1)
        {
            // Get Players
            Players player = GameManager.Instance.GetLocalPlayer();
            if (PhotonNetwork.IsConnected) {
                int playerViewID = GameManager.Instance.dataManager.PlayerViewID;
                Debug.Log("Adding Weapons");
                photonView.RPC("RPCaddWeapons", RpcTarget.All, playerViewID, chosenWeapon, weapon3Info.ID, levelAdder);
            } 
            player.addWeapon(chosenWeapon, weapon3Info.ID, levelAdder);
            // Destroy the listing and dropped item with the same ID
            GameUI.Instance.RemoveDroppedItem(droppedId);
            chosenWeapon = -1;
            HidePanel();
        }
    }

    // Upgrade
    public void ConfirmWeaponChoice(int chosen, int id)
    {
        // Get Players
        Players player = GameManager.Instance.GetLocalPlayer();
        if (PhotonNetwork.IsConnected)
        {
            int playerViewID = GameManager.Instance.dataManager.PlayerViewID;
            Debug.Log("Adding Weapons");
            photonView.RPC("RPCaddWeapons", RpcTarget.All, playerViewID, chosen, id, 1);
        }
        player.addWeapon(chosen, id, 1);
        // Destroy the listing and dropped item with the same ID
        GameUI.Instance.RemoveDroppedItem(droppedId);
        HidePanel();
    }

    // Change Weapon
    [PunRPC]
    public void RPCaddWeapons(int playerViewID, int slot, int id, int level)
    {
        Players player = GameManager.Instance.GetLocalPlayer(playerViewID);
        if (player == null)
        {
            Debug.LogError("RPCaddWeapons: Player is null");
            return;
        }
        player.addWeapon(slot, id, level);
    }

    // Selection of weapon
    public void ChoseOne()
    {
        if (chosenWeapon == 0)
        {
            chosenWeapon = -1;
            levelAdder = -1;
        }
        else
        {
            chosenWeapon = 0;
            if (weapon1Info.ID == weapon3Info.ID)
            {
                levelAdder = 1;
            }
            else
            {
                levelAdder = -1;
            }
            confirmButton.gameObject.SetActive(true);
        }
    }

    public void ChoseTwo()
    {
        if (chosenWeapon == 1)
        {
            chosenWeapon = -1;
            levelAdder = -1;
        }
        else
        {
            chosenWeapon = 1;
            if (weapon2Info.ID == weapon3Info.ID)
            {
                levelAdder = 1;
            }
            else {
                levelAdder = -1;
            }
            confirmButton.gameObject.SetActive(true);
        }
    }
}
