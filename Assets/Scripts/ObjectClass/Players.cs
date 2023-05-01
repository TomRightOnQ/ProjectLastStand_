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
    private int WEAPON_COUNT = 2;
    private Weapons[] weapons;
    public Weapons[] WeaponList { get { return weapons; } set { weapons = value; } }
    void Start()
    {
        gameObject.tag = "Player";
        prefabReference = GameManager.Instance.prefabManager;
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
        weapons[0].Fire(transform.position, index);
        weapons[1].Fire(transform.position, index);
    }

    private void Update()
    {
        Debug.Log(weapons[0]);
        if (!armed) {
            addWeapon(0, 0);
            addWeapon(1, 1);
            armed = true;
        }
    }
}
