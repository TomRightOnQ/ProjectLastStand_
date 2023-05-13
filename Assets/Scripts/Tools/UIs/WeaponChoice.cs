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
    private long droppedId;

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
        // Weapon hints
        if (weapon1Info.ID == weapon3Info.ID)
        {
            weapon1Hint.text = "Upgrade this weapon";
        }
        else
        {
            weapon1Hint.text = "Replace this weapon";
            weapon1Hint.color = Color.red;
        }
        if (weapon2Info.ID == weapon3Info.ID)
        {
            weapon2Hint.text = "Upgrade this weapon";
        }
        else
        {
            weapon2Hint.text = "Replace this weapon";
            weapon2Hint.color = Color.red;
        }
    }

    public void SetID(long id)
    {
        droppedId = id;
    }
    public long GetID()
    {
        return droppedId;
    }

    public void SetWeaponInfo(int slot, WeaponConfig weaponData)
    {
        weaponInfos[slot].SetWeaponInfo(slot, weaponData);
    }

    public void SetWeaponInfo(int slot, Weapons weapon)
    {
        weaponInfos[slot].SetWeaponInfo(slot, weapon);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
        chosenWeapon = -1;
        weapon1Info.CloseMenu();
        weapon2Info.CloseMenu();
        droppedId = 0; // Reset the droppedId
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
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
                photonView.RPC("RPCaddWeapons", RpcTarget.Others, playerViewID, chosenWeapon, weapon3Info.ID);
            }
            player.addWeapon(chosenWeapon, weapon3Info.ID);
            // Destroy the listing and dropped item with the same ID
            GameUI.Instance.RemoveDroppedItem(droppedId);
            chosenWeapon = -1;
            HidePanel();
        }
    }

    // Change Weapon
    [PunRPC]
    public void RPCaddWeapons(int playerViewID, int slot, int id)
    {
        Players player = GameManager.Instance.GetLocalPlayer(playerViewID);
        if (player == null)
        {
            return;
        }
        player.addWeapon(slot, id);
    }

    // Selection of weapon
    public void ChoseOne()
    {
        if (chosenWeapon == 0)
        {
            chosenWeapon = -1;
        }
        else
        {
            chosenWeapon = 0;
            confirmButton.gameObject.SetActive(true);
        }
    }

    public void ChoseTwo()
    {
        if (chosenWeapon == 1)
        {
            chosenWeapon = -1;
        }
        else
        {
            chosenWeapon = 1;
            confirmButton.gameObject.SetActive(true);
        }
    }
}
