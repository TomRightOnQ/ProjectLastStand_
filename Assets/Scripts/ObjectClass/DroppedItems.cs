using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using static WeaponConfigs;

// Dropped weapons
public class DroppedItems : DefaultObjects
{
    // Display
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI nameText;
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
    public long DroppedId { get { return droppedId; } set { droppedId = value; } }

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
        }
        else
        {
            weaponIndex = (int)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            viewID = photonView.ViewID;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Throw();
        // Generate a unique id
        long ticks = DateTime.UtcNow.Ticks - epochTicks;
        droppedId = (ticks << 8) | typeIndicator;
        int randomNumber = Random.Range(0, 1000);

        // Combine the random number with the id
        droppedId = (uint)((droppedId << 16) | ((uint)randomNumber << 8));
    }

    private void Update()
    {
        if (transform.position.y >= 0.01f)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        else
        {
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
            photonView.RPC("RPCSetUP", RpcTarget.Others, viewID, weaponData._name, weaponIndex);
        }
    }

    [PunRPC]
    public void RPCSetUP(int viewID, string weaponName, int _weaponIndex) 
    {
        WeaponConfig weaponData = WeaponConfigs.Instance._getWeaponConfig(weaponIndex);
        DroppedItems dropped = GameManager.Instance.GetDroppedItems(viewID);
        dropped.WeaponIndex = _weaponIndex;
        dropped.SetWeaponName(weaponName);
    }
    public void SetWeaponName(string weaponName)
    {
        nameText.text = weaponName;
    }
}
