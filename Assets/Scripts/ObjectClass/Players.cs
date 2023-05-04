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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 groundPosition = hit.point;

            // Calculate the midpoint between the two weapons
            Vector3 weaponMidpoint = Vector3.Lerp(weapons[0].transform.position, weapons[1].transform.position, 0.5f);

            // Calculate the direction from the midpoint to the ground position
            Vector3 direction = (groundPosition - weaponMidpoint).normalized;
            direction.y = 0;
            direction.Normalize();

            return direction;
        }
        return Vector3.zero;
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
            int randomInt1 = Random.Range(0, 3);
            int randomInt2 = Random.Range(0, 3);
            addWeapon(0, randomInt1);
            addWeapon(1, randomInt2);
            print("Player " + photonView.ViewID + " is now with weapon " + weapons[0].photonView.ViewID + " and " + weapons[1].photonView.ViewID);
            armed = true;
        }
    }
}
