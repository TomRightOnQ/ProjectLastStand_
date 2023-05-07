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
    [SerializeField] private int level = 1;

    // Weapons
    private List<Weapons> weapons = new List<Weapons>();
    public List<Weapons> WeaponList { get { return weapons; } set { weapons = value; } }

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
        hpS.maxValue = hitPoints;
        hpS.value = currentHitPoints;
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
    public void addWeapon(int slot, int id) {
        WeaponConfig weaponData = WeaponConfigs.Instance._getWeaponConfig(id);
        weapons[slot].SetWeapons(weaponData);
    }

    // Attack!
    public void fire() {
        foreach (Weapons weapon in weapons) {
            if (weapon == null) {
                return;
            }
            if (weapon.Type == 0) {
                if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
                {
                    weapon.Fire(index, GetAimDirection());
                }
                else {
                    int weaponViewID = -1;
                    weaponViewID = weapon.photonView.ViewID;
                    photonView.RPC("FireForPlayer", RpcTarget.MasterClient, weaponViewID, index, photonView.ViewID, GetAimDirection());
                }
            } else if (weapon.Type == 1 || weapon.Type == 2) {
                weapon.Fire(index, GetAimDirection());
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
        PhotonView playerView = PhotonView.Find(playerViewID);
        Players player = playerView.GetComponent<Players>();

        if (weaponViewID != -1)
        {
            PhotonView weapon1View = PhotonView.Find(weaponViewID);
            if (weapon1View != null)
            {
                Weapons weapon = weapon1View.GetComponent<Weapons>();
                if (weapon != null)
                {
                    weapon.Fire(index, direction0);
                }
            }
        }
    }

    private void Update()
    {
        if (!armed && weapons.Count >= 2) {
            addWeapon(0, 0);
            addWeapon(1, 0);
            armed = true;
        }
        UpdateHP();
    }
}
