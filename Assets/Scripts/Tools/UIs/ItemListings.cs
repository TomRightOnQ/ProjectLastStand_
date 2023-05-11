using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using static WeaponConfigs;

// Item Listings
public class ItemListings : MonoBehaviourPunCallbacks
{
    // Display
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    // Data
    [SerializeField] private int weaponIndex;
    private int level;
    private int viewID;
    [SerializeField] private long droppedId;

    public long DroppedId { get { return droppedId; } set { droppedId = value; } }
    public int WeaponIndex { get { return weaponIndex; } set { weaponIndex = value; } }

    // Adding to the list

    // Set up an listing info
    public void SetUp()
    {
        WeaponConfig weaponData = WeaponConfigs.Instance._getWeaponConfig(weaponIndex);
        // set details
        nameText.text = weaponData._name;
    }
}
