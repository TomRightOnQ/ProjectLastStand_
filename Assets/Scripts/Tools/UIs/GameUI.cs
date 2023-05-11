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

    bool found = false;

    private void Awake()
    {
        Instance = this;
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

    public void setWeaponInfo(int slot, Weapons weapon) {
        weaponInfos[slot].SetWeaponInfo(weapon);
    }
}
