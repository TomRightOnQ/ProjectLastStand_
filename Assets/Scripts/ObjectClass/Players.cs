using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConfigManager;

// All players are a Players object
public class Players : Entities
{
    private PrefabManager prefabReference;
    // Player 1 - 4
    [SerializeField] private int index = 0;
    [SerializeField] private float fortune = 1;
    [SerializeField] private bool armed = false;

    // Weapons
    private int WEAPON_COUNT = 2;
    private Weapons[] weapons;
    void Start()
    {
        gameObject.tag = "Player";
        weapons = new Weapons[WEAPON_COUNT];
        prefabReference = GameManager.Instance.prefabManager;

        for (int i = 0; i < WEAPON_COUNT; i++) {
            GameObject weaponObj = Instantiate(prefabReference.weaponPrefab, new Vector3(0.15f, 0.1f, 0.1f), Quaternion.Euler(0f, 90f, 0f));
            weaponObj.SetActive(true);
            weapons[i] = weaponObj.GetComponent<Weapons>();
            weapons[i].transform.parent = transform;
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
        if (Input.GetMouseButtonDown(0))
        {
            fire();
        }
        if (!armed) {
            addWeapon(0, 0);
            addWeapon(1, 1);
            armed = true;
        }
    }
}
