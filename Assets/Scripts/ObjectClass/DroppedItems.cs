using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using static WeaponConfigs;

// Dropped weapons
public class DroppedItems : Items
{
    // Display
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI nameText;

    // Data
    [SerializeField] private int weaponIndex;
    private int level;
    private int viewID;

    public int WeaponIndex { get { return weaponIndex; } set { weaponIndex = value; } }
    public int ViewID { get { return viewID; } }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // The order of writing and reading is really important
        // Not need to send or read position data, Other component is doing this.
        // ORDER:
        //      1. weaponIndex
        //      2. level

        if (stream.IsWriting)
        {
            stream.SendNext(weaponIndex);
            stream.SendNext(level);
        }
        else
        {
            weaponIndex = (int)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
        }
    }

    // Get ViewID
    private void Start()
    {
        if (PhotonNetwork.IsConnected) {
            PhotonView photonView = GetComponent<PhotonView>();
            viewID = photonView.ViewID;
        }
    }

    // Set up a weapon info
    public void SetUp()
    {
        WeaponConfig weaponData = WeaponConfigs.Instance._getWeaponConfig(weaponIndex);
        // set details
        nameText.text = weaponData._name;
        if (PhotonNetwork.IsConnected) {
            photonView.RPC("RPCSetUP", RpcTarget.Others, viewID);
        }
    }

    [PunRPC]
    public void RPCSetUP(int viewID) 
    {
        WeaponConfig weaponData = WeaponConfigs.Instance._getWeaponConfig(weaponIndex);
        DroppedItems dropped = GameManager.Instance.GetDroppedItems(viewID);
        // set details
        dropped.nameText.text = weaponData._name;
    }
}
