using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static ConfigManager;

// All players are a Players object
public class Players : Entities, IPunObservable
{
    private PrefabManager prefabReference;
    // Player 1 - 4
    [SerializeField] private int index = 0;
    [SerializeField] private float fortune = 1;
    private bool armed = false;
    private const string PREFAB_LOC = "Prefabs/";

    // Weapons
    private List<Weapons> weapons = new List<Weapons>();
    public List<Weapons> WeaponList { get { return weapons; } set { weapons = value; } }
    void Start()
    {
        gameObject.tag = "Player";
        prefabReference = GameManager.Instance.prefabManager;
        Debug.Log("Ready");
    }

    // Sync
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }

    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    public float Fortune
    {
        get { return fortune; }
        set { fortune = value; }
    }

    public bool Armed 
    {
        get { return armed; }
        set { armed = value; }
    }

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
        WeaponConfig[] weaponData = GameManager.Instance.configManager.getWeapons();
        weapons[slot].SetWeapons(weaponData[id]);
    }

    // Attack!
    public void fire() {
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            if (weapons[0] != null)
                weapons[0].Fire(transform.position, index, GetAimDirection(0));
            if (weapons[1] != null)
                weapons[1].Fire(transform.position, index, GetAimDirection(1));
        }
        else {
            // Run RPC to let master using weapon 0 and 1's View ID to fire
            int weapon1ViewID = -1;
            int weapon2ViewID = -1;
            if (weapons[0] != null)
                weapon1ViewID = weapons[0].photonView.ViewID;
            if (weapons[1] != null)
                weapon2ViewID = weapons[1].photonView.ViewID;
            photonView.RPC("FireForPlayer", RpcTarget.MasterClient, weapon1ViewID, weapon2ViewID, index, photonView.ViewID, GetAimDirection(0), GetAimDirection(1));
        }
    }

    private Vector3 GetAimDirection(int weaponIndex)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 groundPosition = hit.point;
            Vector3 weaponPosition = weapons[weaponIndex].transform.position;
            Vector3 direction = (groundPosition - weaponPosition).normalized;
            direction.y = 0;
            direction.Normalize();
            return direction;
        }
        return Vector3.zero;
    }

    // For client firing, let master do it
    [PunRPC]
    private void FireForPlayer(int weapon1ViewID, int weapon2ViewID, int index, int playerViewID, Vector3 direction0, Vector3 direction1, PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("FireForPlayer RPC called on non-master client");
            return;
        }
        PhotonView playerView = PhotonView.Find(playerViewID);
        Players player = playerView.GetComponent<Players>();

        if (weapon1ViewID != -1)
        {
            PhotonView weapon1View = PhotonView.Find(weapon1ViewID);
            if (weapon1View != null)
            {
                Weapons weapon = weapon1View.GetComponent<Weapons>();
                if (weapon != null)
                {
                    weapon.Fire(player.transform.position, index, direction0);
                }
            }
        }

        if (weapon2ViewID != -1)
        {
            PhotonView weapon2View = PhotonView.Find(weapon2ViewID);
            if (weapon2View != null)
            {
                Weapons weapon = weapon2View.GetComponent<Weapons>();
                if (weapon != null)
                {
                    weapon.Fire(player.transform.position, index, direction1);
                }
            }
        }
    }

    private void Update()
    {
        if (!armed) {
            addWeapon(0, 0);
            addWeapon(1, 1);
            print("Player " + photonView.ViewID + " is now with weapon " + weapons[0].photonView.ViewID + " and " + weapons[1].photonView.ViewID);
            armed = true;
        }
    }
}
