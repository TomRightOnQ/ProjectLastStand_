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
    private List<WeaponInfo> weaponInfos = new List<WeaponInfo>();
    [SerializeField] private int chosenWeapon = -1;

    // Connect with the dropped items
    [SerializeField] private long droppedId;

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
        if (chosenWeapon == -1) {
            confirmButton.gameObject.SetActive(false);
        }
    }

    public void SetID(long id) {
        droppedId = id;
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
            // Destroy the listing and dropped item with the same ID
            GameUI.Instance.RemoveDroppedItem(droppedId);

            chosenWeapon = -1;
            HidePanel();
        }
    }

    // Selection of weapon
    public void ChoseOne() {
        if (chosenWeapon == 1)
        {
            chosenWeapon = -1;
        }
        else {
            chosenWeapon = 1;
            confirmButton.gameObject.SetActive(true);
        }
    }

    public void ChoseTwo()
    {
        if (chosenWeapon == 2)
        {
            chosenWeapon = -1;
        }
        else
        {
            chosenWeapon = 2;
            confirmButton.gameObject.SetActive(true);
        }
    }
}
