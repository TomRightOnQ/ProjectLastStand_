using System.Collections;
using System.Collections.Generic;
using System;
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
    private bool isFalling = true;
    private float throwForce = 5f;
    private Rigidbody rb;

    // Data
    [SerializeField] private int weaponIndex;
    private int level;
    private int viewID;
    [SerializeField] private long droppedId;

    private static readonly long epochTicks = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
    private static readonly long typeIndicator = 0L;

    public int WeaponIndex { get { return weaponIndex; } set { weaponIndex = value; } }
    public int ViewID { get { return viewID; } }
    public long DroppedId { get { return droppedId; } }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // The order of writing and reading is really important
        // Not need to send or read position data, Other component is doing this.
        // ORDER:
        //      1. weaponIndex
        //      2. level
        //      3. droppedId

        if (stream.IsWriting)
        {
            stream.SendNext(weaponIndex);
            stream.SendNext(level);
            stream.SendNext(droppedId);
        }
        else
        {
            weaponIndex = (int)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
            droppedId = (long)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (PhotonNetwork.IsConnected)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            viewID = photonView.ViewID;
        }
        Throw();
        // Generate a unique id
        long ticks = DateTime.UtcNow.Ticks - epochTicks;
        droppedId = (ticks << 8) | typeIndicator;
    }

    private void Update()
    {
        if (transform.position.y >= 0.01f)
        {
            isFalling = true;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        else
        {
            isFalling = false;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    // Throwing
    public void Throw()
    {
        rb.AddForce(transform.up * throwForce, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Players player = other.GetComponent<Players>();
            player.AddDroppedItem(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Players player = other.GetComponent<Players>();
            player.RemoveDroppedItem(this);
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
